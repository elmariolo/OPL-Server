using SMBLibrary;
using SMBLibrary.Authentication.GSSAPI;
using SMBLibrary.Authentication.NTLM;
using SMBLibrary.Server;
using SMBLibrary.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Utilities;

namespace OPLServer
{
    public partial class Form1 : Form
    {
        #region SMBServer variables

        private SMBServer m_server;
        private IPAddress serverAddress = IPAddress.Any;
        private SMBTransportType transportType = SMBTransportType.DirectTCPTransport;
        private NTLMAuthenticationProviderBase authenticationMechanism;
        private UserCollection users = new UserCollection();
        private LogWriter m_logWriter;
        
        #endregion
        
        #region App variables

        private static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public delegate void addLog(string a, string b, string c, string d);

        private bool allowshowdisplay = true;
        public bool isLoadingSettings = false;
        public bool silentMode = false;
        public string sharePath = AppPath + "PS2";
        public int serverPort = 1024;
        public bool logEnabled = true;
        public bool autoScrollLog = false;
        public bool logLvlCritical = true;
        public bool logLvlDebug = false;
        public bool logLvlError = true;
        public bool logLvlInfo = true;
        public bool logLvlTrace = true;
        public bool logLvlVerbose = false;
        public bool logLvlWarn = true;
        
        #endregion

        #region Form Methods and GUI

        public Form1()
        {
            InitializeComponent();

            loadSettings();

            if (!Directory.Exists(sharePath))
            {
                Directory.CreateDirectory(sharePath);
            }

            mainInit();

            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (existArg("/NOLOG", args))
                {
                    enableLog(false);
                }

                if (existArg("/START", args))
                {
                    tsbServerState.Checked = true;
                }

                if (existArg("/SILENT", args))
                {
                    silentMode = true;
                }

                if (existArg("/HIDE", args))
                {
                    allowshowdisplay = false;
                    tsbToTray_Click(null, null);
                }
            }

            enableLog(logEnabled);
        }

        private bool existArg(string arg, string[] args)
        {
            foreach (string strarg in args)
            {
                if (arg.ToUpper() == strarg.ToUpper()) return true;
            }

            return false;
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
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
            lstvwLog.Columns[0].Width = 130;
            lstvwLog.Columns[1].Width = 100;
            lstvwLog.Columns[2].Width = 80;
            lstvwLog.Columns[3].Width = (this.Width - 50) - lstvwLog.Columns[0].Width - lstvwLog.Columns[1].Width - lstvwLog.Columns[2].Width;
        }

        public void addLogList(string time, string seve, string source, string messg)
        {
            if (!logEnabled) return;

            if (lstvwLog.InvokeRequired)
            {
                addLog tmplog = new addLog(addLogList);
                this.Invoke(tmplog, time, seve, source, messg);
            }
            else
            {
                ListViewItem item = new ListViewItem(time);
                item.SubItems.Add(seve);
                item.SubItems.Add(source);
                item.SubItems.Add(messg);

                lstvwLog.Items.Add(item);
                if (autoScrollLog) item.EnsureVisible();
            }
        }

        private void tsbToTray_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            if (!silentMode)
            {
                if (tsbServerState.Checked)
                    notifyIcon1.ShowBalloonTip(1, "OPL Server", "Server is RUNNING on port " + serverPort, ToolTipIcon.None);
                else
                    notifyIcon1.ShowBalloonTip(1, "OPL Server", "Server is STOPPED", ToolTipIcon.None);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            allowshowdisplay = true;
            this.Show();
            notifyIcon1.Visible = false;
        }

        #endregion

        #region SMBServer Methods

        void mainInit()
        {
            users.Add("Guest", "");
            users.Add("Guest", "Guest");
            authenticationMechanism = new IndependentNTLMAuthenticationProvider(users.GetUserPassword);

            List<ShareSettings> sharesSettings = new List<ShareSettings>();
            ShareSettings itemtoshare = new ShareSettings("PS2", sharePath, new List<string>() { "Guest" }, new List<string>() { "Guest" });
            sharesSettings.Add(itemtoshare);

            SMBShareCollection shares = new SMBShareCollection();
            foreach (ShareSettings shareSettings in sharesSettings)
            {
                FileSystemShare share = InitializeShare(shareSettings);
                shares.Add(share);
            }

            GSSProvider securityProvider = new GSSProvider(authenticationMechanism);
            m_server = new SMBLibrary.Server.SMBServer(shares, securityProvider);

            m_logWriter = new LogWriter();
        }

        void setServerPort(int servPort)
        {
            SMBLibrary.Client.SMB1Client.DirectTCPPort = servPort;
            SMBLibrary.Client.SMB2Client.DirectTCPPort = servPort;
            m_server.DirectTCPPort = servPort;
        }

        void serverStart()
        {
            enableLog(false);

            mainInit();

            enableLog(logEnabled);

            setServerPort(serverPort);

            m_logWriter.CloseLogFile();

            try
            {
                m_server.Start(serverAddress, transportType, true, false);
                addLogList(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Information", "Server", "Server started at port " + serverPort);
            }
            catch (Exception ex)
            {
                addLogList(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Error", "Server", "Can't start server at port " + serverPort);
                m_logWriter.CloseLogFile();
                m_server.Stop();
            }
        }

        void serverStop()
        {
            try
            {
                m_server.Stop();
                addLogList(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Information", "Server", "Server stop");
            }
            catch (Exception ex)
            {
                addLogList(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Error", "Server", "Error trying to stop server");
                MessageBox.Show(ex.Message, "Error trying to stop server");
            }
        }

        void enableLog(bool val)
        {
            if (val)
            {
                try
                {
                    m_server.LogEntryAdded += m_server_LogEntryAdded;
                }
                catch { }
                lstvwLog.Enabled = true;
            }
            else
            {
                try
                {
                    m_server.LogEntryAdded -= m_server_LogEntryAdded;
                }
                catch { }
                lstvwLog.Enabled = false;
            }
        }

        void m_server_LogEntryAdded(object sender, LogEntry e)
        {
            if (e.Severity == Severity.Critical && logLvlCritical == false) return;
            if (e.Severity == Severity.Debug && logLvlDebug == false) return;
            if (e.Severity == Severity.Error && logLvlError == false) return;
            if (e.Severity == Severity.Information && logLvlInfo == false) return;
            if (e.Severity == Severity.Trace && logLvlTrace == false) return;
            if (e.Severity == Severity.Verbose && logLvlVerbose == false) return;
            if (e.Severity == Severity.Warning && logLvlWarn == false) return;

            addLogList(e.Time.ToString("yyyy-MM-dd HH:mm:ss"),e.Severity.ToString(),e.Source,e.Message);
        }        

        public static FileSystemShare InitializeShare(ShareSettings shareSettings)
        {
            string shareName = shareSettings.ShareName;
            string sharePath = shareSettings.SharePath;
            List<string> readAccess = shareSettings.ReadAccess;
            List<string> writeAccess = shareSettings.WriteAccess;
            FileSystemShare share = new FileSystemShare(shareName, new NTDirectoryFileSystem(sharePath));
            share.AccessRequested += delegate(object sender, AccessRequestArgs args)
            {
                bool hasReadAccess = Contains(readAccess, "Users") || Contains(readAccess, args.UserName);
                bool hasWriteAccess = Contains(writeAccess, "Users") || Contains(writeAccess, args.UserName);
                if (args.RequestedAccess == FileAccess.Read)
                {
                    args.Allow = hasReadAccess;
                }
                else if (args.RequestedAccess == FileAccess.Write)
                {
                    args.Allow = hasWriteAccess;
                }
                else // FileAccess.ReadWrite
                {
                    args.Allow = hasReadAccess && hasWriteAccess;
                }
            };
            return share;
        }

        public static bool Contains(List<string> list, string value)
        {
            return (IndexOf(list, value) >= 0);
        }

        public static int IndexOf(List<string> list, string value)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (string.Equals(list[index], value, StringComparison.OrdinalIgnoreCase))
                {
                    return index;
                }
            }
            return -1;
        }

        #endregion

        #region Toolbar Events

        private void tsbAutoScroll_CheckStateChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            autoScrollLog = tsbAutoScroll.Checked;
            saveSettings();
        }

        private void tsbServerState_CheckedChanged(object sender, EventArgs e)
        {
            if (tsbServerState.Checked)
            {
                try
                {
                    serverStart();
                }
                catch (Exception ex)
                {
                    tsbServerState.Checked = false;
                    tsbServerState.Image = Properties.Resources.start;
                    tsbServerState.Text = "Server is stopped (press to start)";
                    tstbPort.Enabled = true;
                    tsbSelectFolder.Enabled = true;
                    MessageBox.Show(ex.Message, "Error trying to start server");
                    return;
                }
                
                tsbServerState.Image = Properties.Resources.stop;
                tsbServerState.Text = "Server is running (press to stop)";
                tstbPort.Enabled = false;
                tsbSelectFolder.Enabled = false;
            }
            else
            {
                serverStop();
                tsbServerState.Image = Properties.Resources.start;
                tsbServerState.Text = "Server is stopped (press to start)";
                tstbPort.Enabled = true;
                tsbSelectFolder.Enabled = true;
            }
        }

        private void tsbEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logEnabled = tsbEnableLog.Checked;
            enableLog(logEnabled);
            saveSettings();            
        }

        private void tsbClearLog_Click(object sender, EventArgs e)
        {
            lstvwLog.Items.Clear();
        }

        private void tstbPort_Leave(object sender, EventArgs e)
        {
            int finalport;

            if (int.TryParse(tstbPort.Text, out finalport))
            {
                if (finalport > 0 && finalport <= 65535)
                {
                    setServerPort(finalport);
                    tstbPort.Text = finalport.ToString();
                    serverPort = finalport;
                    saveSettings();
                }
                else
                {
                    MessageBox.Show("The server port has to be a number between 1 and 65535", "Server port range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tstbPort.Text = "1024";
                    tstbPort.Focus();
                }
            }
            else
            {
                MessageBox.Show("The server port has to be a number between 1 and 65535", "Server port range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tstbPort.Text = "1024";
                tstbPort.Focus();
            }
        }

        private void tsbLogInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlInfo = tsbLogInfo.Checked;
            saveSettings();
        }

        private void tsbLogWarn_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlWarn = tsbLogWarn.Checked;
            saveSettings();
        }

        private void tsbLogError_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlError = tsbLogError.Checked;
            saveSettings();
        }

        private void tsbLogCritical_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlCritical = tsbLogCritical.Checked;
            saveSettings();
        }

        private void tsbLogTrace_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlTrace = tsbLogTrace.Checked;
            saveSettings();
        }

        private void tsbLogDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlDebug = tsbLogDebug.Checked;
            saveSettings();
        }

        private void tsbLogVerbose_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingSettings) return;
            logLvlVerbose = tsbLogVerbose.Checked;
            saveSettings();
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            frmAbout tmpFrm = new frmAbout();
            tmpFrm.ShowDialog(this);
        }

        private void tsbSelectFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    sharePath = fbd.SelectedPath;
                    saveSettings();
                    tsslblStatus.Text = "Share Folder: " + sharePath;
                }
            }
        }

        #endregion

        #region Settings Functions

        private void loadSettings()
        {
            isLoadingSettings = true;

            if (getSetting("SharePath") != "") sharePath = getSetting("SharePath");
            tsslblStatus.Text = "Share Folder: " + sharePath;

            if (getSetting("ServerPort") != "") serverPort = int.Parse(getSetting("ServerPort"));
            tstbPort.Text = serverPort.ToString();

            if (getSetting("EnableLog") == "1") { logEnabled = true; } else { logEnabled = false; }
            tsbEnableLog.Checked = logEnabled;

            if (getSetting("AutoScroll") == "1") { autoScrollLog = true; } else { autoScrollLog = false; }
            tsbAutoScroll.Checked = autoScrollLog;

            if (getSetting("LogCritical") == "1") { logLvlCritical = true; } else { logLvlCritical = false; }
            tsbLogCritical.Checked = logLvlCritical;

            if (getSetting("LogDebug") == "1") { logLvlDebug = true; } else { logLvlDebug = false; }
            tsbLogDebug.Checked = logLvlDebug;

            if (getSetting("LogError") == "1") { logLvlError = true; } else { logLvlError = false; }
            tsbLogError.Checked = logLvlError;

            if (getSetting("LogInfo") == "1") { logLvlInfo = true; } else { logLvlInfo = false; }
            tsbLogInfo.Checked = logLvlInfo;

            if (getSetting("LogTrace") == "1") { logLvlTrace = true; } else { logLvlTrace = false; }
            tsbLogTrace.Checked = logLvlTrace;

            if (getSetting("LogVerbose") == "1") { logLvlVerbose = true; } else { logLvlVerbose = false; }
            tsbLogVerbose.Checked = logLvlVerbose;

            if (getSetting("LogWarn") == "1") { logLvlWarn = true; } else { logLvlWarn = false; }
            tsbLogWarn.Checked = logLvlWarn;

            isLoadingSettings = false;
        }

        private void saveSettings()
        {
            if (isLoadingSettings) return;

            setSetting("SharePath", sharePath);
            setSetting("ServerPort", serverPort.ToString());
            setSetting("EnableLog", logEnabled ? "1" : "0");
            setSetting("AutoScroll", autoScrollLog ? "1" : "0");
            setSetting("LogCritical", logLvlCritical ? "1" : "0");
            setSetting("LogDebug", logLvlDebug ? "1" : "0");
            setSetting("LogError", logLvlError ? "1" : "0");
            setSetting("LogInfo", logLvlInfo ? "1" : "0");
            setSetting("LogTrace", logLvlTrace ? "1" : "0");
            setSetting("LogVerbose", logLvlVerbose ? "1" : "0");
            setSetting("LogWarn", logLvlWarn ? "1" : "0");
        }

        private string getSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                return result;
            }
            catch (ConfigurationErrorsException) { }
            return "";
        }

        private void setSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException){}
        }

        #endregion

    }
}
