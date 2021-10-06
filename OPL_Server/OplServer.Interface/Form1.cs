using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;
using OplServer.Interface.Properties;
using SMBLibrary;
using SMBLibrary.Authentication.GSSAPI;
using SMBLibrary.Authentication.NTLM;
using SMBLibrary.Server;
using SMBLibrary.Win32;
using Utilities;

namespace OplServer.Interface
{
    public partial class Form1 : Form
    {
        public delegate void addLog(string a, string b, string c, string d);

        private readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly NTLMAuthenticationProviderBase authenticationMechanism;
        public bool isLoadingSettings;
        private readonly LogWriter m_logWriter;
        private readonly SMBLibrary.Server.SMBServer m_server;
        private readonly IPAddress serverAddress = IPAddress.Any;
        private readonly SMBTransportType transportType = SMBTransportType.DirectTCPTransport;
        private readonly UserCollection users = new UserCollection();
        
        private int ServerPort { get; set; }

        public Form1()
        {
            InitializeComponent();

            if (!Directory.Exists(AppPath + "PS2")) Directory.CreateDirectory(AppPath + "PS2");

            users.Add("Guest", "");
            users.Add("Guest", "Guest");
            authenticationMechanism = new IndependentNTLMAuthenticationProvider(users.GetUserPassword);

            var sharesSettings = new List<ShareSettings>();
            var itemtoshare = new ShareSettings("PS2", AppPath + "PS2", new List<string> { "Guest" },
                new List<string> { "Guest" });
            sharesSettings.Add(itemtoshare);

            var shares = new SMBShareCollection();
            foreach (ShareSettings shareSettings in sharesSettings)
            {
                FileSystemShare share = InitializeShare(shareSettings);
                shares.Add(share);
            }

            var securityProvider = new GSSProvider(authenticationMechanism);
            m_server = new SMBLibrary.Server.SMBServer(shares, securityProvider);

            loadSettings();

            m_logWriter = new LogWriter();
            if (tsbEnableLog.Checked) m_server.LogEntryAdded += m_server_LogEntryAdded;

            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg.ToUpper() == "/NOLOG")
                {
                    addLogList(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Information", "Commandline", "/NOLOG");
                    tsbEnableLog.Checked = false;
                }

                if (arg.ToUpper() == "/START")
                {
                    addLogList(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Information", "Commandline", "/START");
                    tsbServerState.Checked = true;
                    //tsbServerState_CheckedChanged(null, null);
                }
            }
        }

        private void loadSettings()
        {
            isLoadingSettings = true;
            if (getSetting("ServerPort") != "") tstbPort.Text = getSetting("ServerPort");
            if (getSetting("EnableLog") == "1")
                tsbEnableLog.Checked = true;
            else
                tsbEnableLog.Checked = false;
            if (getSetting("AutoScroll") == "1")
                tsbAutoScroll.Checked = true;
            else
                tsbAutoScroll.Checked = false;
            if (getSetting("LogCritical") == "1")
                tsbLogCritical.Checked = true;
            else
                tsbLogCritical.Checked = false;
            if (getSetting("LogDebug") == "1")
                tsbLogDebug.Checked = true;
            else
                tsbLogDebug.Checked = false;
            if (getSetting("LogError") == "1")
                tsbLogError.Checked = true;
            else
                tsbLogError.Checked = false;
            if (getSetting("LogInfo") == "1")
                tsbLogInfo.Checked = true;
            else
                tsbLogInfo.Checked = false;
            if (getSetting("LogTrace") == "1")
                tsbLogTrace.Checked = true;
            else
                tsbLogTrace.Checked = false;
            if (getSetting("LogVerbose") == "1")
                tsbLogVerbose.Checked = true;
            else
                tsbLogVerbose.Checked = false;
            if (getSetting("LogWarn") == "1")
                tsbLogWarn.Checked = true;
            else
                tsbLogWarn.Checked = false;
            isLoadingSettings = false;
        }

        private void saveSettings()
        {
            if (isLoadingSettings) return;

            setSetting("ServerPort", tstbPort.Text);
            setSetting("EnableLog", tsbEnableLog.Checked ? "1" : "0");
            setSetting("AutoScroll", tsbAutoScroll.Checked ? "1" : "0");
            setSetting("LogCritical", tsbLogCritical.Checked ? "1" : "0");
            setSetting("LogDebug", tsbLogDebug.Checked ? "1" : "0");
            setSetting("LogError", tsbLogError.Checked ? "1" : "0");
            setSetting("LogInfo", tsbLogInfo.Checked ? "1" : "0");
            setSetting("LogTrace", tsbLogTrace.Checked ? "1" : "0");
            setSetting("LogVerbose", tsbLogVerbose.Checked ? "1" : "0");
            setSetting("LogWarn", tsbLogWarn.Checked ? "1" : "0");
        }

        private void m_server_LogEntryAdded(object sender, LogEntry e)
        {
            if (e.Severity == Severity.Critical && tsbLogCritical.Checked == false) return;
            if (e.Severity == Severity.Debug && tsbLogDebug.Checked == false) return;
            if (e.Severity == Severity.Error && tsbLogError.Checked == false) return;
            if (e.Severity == Severity.Information && tsbLogInfo.Checked == false) return;
            if (e.Severity == Severity.Trace && tsbLogTrace.Checked == false) return;
            if (e.Severity == Severity.Verbose && tsbLogVerbose.Checked == false) return;
            if (e.Severity == Severity.Warning && tsbLogWarn.Checked == false) return;

            addLogList(e.Time.ToString("yyyy-MM-dd HH:mm:ss"), e.Severity.ToString(), e.Source, e.Message);
        }

        public void addLogList(string time, string seve, string source, string messg)
        {
            if (listView1.InvokeRequired)
            {
                addLog tmplog = addLogList;
                Invoke(tmplog, time, seve, source, messg);
            }
            else
            {
                var item = new ListViewItem(time);
                item.SubItems.Add(seve);
                item.SubItems.Add(source);
                item.SubItems.Add(messg);

                listView1.Items.Add(item);
                if (tsbAutoScroll.Checked) item.EnsureVisible();
            }
        }

        public static FileSystemShare InitializeShare(ShareSettings shareSettings)
        {
            string shareName = shareSettings.ShareName;
            string sharePath = shareSettings.SharePath;
            List<string> readAccess = shareSettings.ReadAccess;
            List<string> writeAccess = shareSettings.WriteAccess;
            var share = new FileSystemShare(shareName, new NTDirectoryFileSystem(sharePath));
            share.AccessRequested += delegate(object sender, AccessRequestArgs args)
            {
                bool hasReadAccess = Contains(readAccess, "Users") || Contains(readAccess, args.UserName);
                bool hasWriteAccess = Contains(writeAccess, "Users") || Contains(writeAccess, args.UserName);
                if (args.RequestedAccess == FileAccess.Read)
                    args.Allow = hasReadAccess;
                else if (args.RequestedAccess == FileAccess.Write)
                    args.Allow = hasWriteAccess;
                else // FileAccess.ReadWrite
                    args.Allow = hasReadAccess && hasWriteAccess;
            };
            return share;
        }

        public static bool Contains(List<string> list, string value)
        {
            return IndexOf(list, value) >= 0;
        }

        public static int IndexOf(List<string> list, string value)
        {
            for (var index = 0; index < list.Count; index++)
                if (string.Equals(list[index], value, StringComparison.OrdinalIgnoreCase))
                    return index;
            return -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form1_Resize(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_server.Stop();
            m_logWriter.CloseLogFile();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = 130;
            listView1.Columns[1].Width = 100;
            listView1.Columns[2].Width = 80;
            listView1.Columns[3].Width = Width - 50 - listView1.Columns[0].Width - listView1.Columns[1].Width -
                                         listView1.Columns[2].Width;
        }

        private void tsbServerState_CheckedChanged(object sender, EventArgs e)
        {
            tstbPort_Leave(sender, e);

            if (tsbServerState.Checked)
            {
                try
                {
                    m_server.Start(
                        serverAddress,
                        transportType,
                        true,
                        false,
                        false,
                        null,
                        ServerPort);
                }
                catch (Exception ex)
                {
                    tsbServerState.Checked = false;
                    tsbServerState.Image = Resources.start;
                    tsbServerState.Text = "Server is stopped (press to start)";
                    tstbPort.Enabled = true;
                    MessageBox.Show(ex.Message, "Error");
                    return;
                }

                m_logWriter.CloseLogFile();
                tsbServerState.Image = Resources.stop;
                tsbServerState.Text = "Server is running (press to stop)";
                tstbPort.Enabled = false;
            }
            else
            {
                m_server.Stop();
                m_logWriter.CloseLogFile();
                tsbServerState.Image = Resources.start;
                tsbServerState.Text = "Server is stopped (press to start)";
                tstbPort.Enabled = true;
            }

            saveSettings();
        }

        private void tsbEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            if (tsbEnableLog.Checked)
            {
                m_server.LogEntryAdded += m_server_LogEntryAdded;
                listView1.Enabled = true;
            }
            else
            {
                m_server.LogEntryAdded -= m_server_LogEntryAdded;
                listView1.Enabled = false;
            }

            saveSettings();
        }

        private void tsbClearLog_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var tmpFrm = new frmAbout();
            tmpFrm.ShowDialog(this);
        }

        private void tstbPort_Leave(object sender, EventArgs e)
        {
            int finalport;

            if (int.TryParse(tstbPort.Text, out finalport))
            {
                if (finalport > 0 && finalport < 1025)
                {
                    ServerPort = finalport;
                    tstbPort.Text = finalport.ToString();
                    saveSettings();
                }
                else
                {
                    MessageBox.Show("The server port has to be set between 1 and 1024", "Server port range",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tstbPort.Text = "1024";
                    tstbPort.Focus();
                }
            }
            else
            {
                MessageBox.Show("The server port has to be set between 1 and 1024", "Server port range",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tstbPort.Text = "1024";
                tstbPort.Focus();
            }
        }

        private string getSetting(string key)
        {
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
            }

            return "";
        }

        private void setSetting(string key, string value)
        {
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                    settings.Add(key, value);
                else
                    settings[key].Value = value;
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
            }
        }

        private void tsbSettingChanged_CheckedChanged(object sender, EventArgs e)
        {
            saveSettings();
        }
    }
}