namespace OPLServer
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbEnableLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLogFilter = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbLogInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLogWarn = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLogError = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLogCritical = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLogTrace = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLogDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLogVerbose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAutoScroll = new System.Windows.Forms.ToolStripButton();
            this.tsbServerState = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClearLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tstbPort = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(890, 307);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Event";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Source";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Message";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEnableLog,
            this.toolStripSeparator2,
            this.tsbLogFilter,
            this.toolStripSeparator1,
            this.tsbAutoScroll,
            this.tsbServerState,
            this.toolStripSeparator3,
            this.tsbClearLog,
            this.toolStripSeparator4,
            this.tsbAbout,
            this.toolStripSeparator5,
            this.tstbPort,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(890, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbEnableLog
            // 
            this.tsbEnableLog.Checked = true;
            this.tsbEnableLog.CheckOnClick = true;
            this.tsbEnableLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbEnableLog.Image = ((System.Drawing.Image)(resources.GetObject("tsbEnableLog.Image")));
            this.tsbEnableLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEnableLog.Name = "tsbEnableLog";
            this.tsbEnableLog.Size = new System.Drawing.Size(85, 22);
            this.tsbEnableLog.Text = "Enable Log";
            this.tsbEnableLog.CheckedChanged += new System.EventHandler(this.tsbEnableLog_CheckedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbLogFilter
            // 
            this.tsbLogFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLogInfo,
            this.tsbLogWarn,
            this.tsbLogError,
            this.tsbLogCritical,
            this.tsbLogTrace,
            this.tsbLogDebug,
            this.tsbLogVerbose});
            this.tsbLogFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsbLogFilter.Image")));
            this.tsbLogFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLogFilter.Name = "tsbLogFilter";
            this.tsbLogFilter.Size = new System.Drawing.Size(86, 22);
            this.tsbLogFilter.Text = "Log filter";
            // 
            // tsbLogInfo
            // 
            this.tsbLogInfo.Checked = true;
            this.tsbLogInfo.CheckOnClick = true;
            this.tsbLogInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbLogInfo.Name = "tsbLogInfo";
            this.tsbLogInfo.Size = new System.Drawing.Size(137, 22);
            this.tsbLogInfo.Text = "Information";
            this.tsbLogInfo.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbLogWarn
            // 
            this.tsbLogWarn.Checked = true;
            this.tsbLogWarn.CheckOnClick = true;
            this.tsbLogWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbLogWarn.Name = "tsbLogWarn";
            this.tsbLogWarn.Size = new System.Drawing.Size(137, 22);
            this.tsbLogWarn.Text = "Warning";
            this.tsbLogWarn.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbLogError
            // 
            this.tsbLogError.Checked = true;
            this.tsbLogError.CheckOnClick = true;
            this.tsbLogError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbLogError.Name = "tsbLogError";
            this.tsbLogError.Size = new System.Drawing.Size(137, 22);
            this.tsbLogError.Text = "Error";
            this.tsbLogError.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbLogCritical
            // 
            this.tsbLogCritical.Checked = true;
            this.tsbLogCritical.CheckOnClick = true;
            this.tsbLogCritical.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbLogCritical.Name = "tsbLogCritical";
            this.tsbLogCritical.Size = new System.Drawing.Size(137, 22);
            this.tsbLogCritical.Text = "Critical";
            this.tsbLogCritical.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbLogTrace
            // 
            this.tsbLogTrace.Checked = true;
            this.tsbLogTrace.CheckOnClick = true;
            this.tsbLogTrace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbLogTrace.Name = "tsbLogTrace";
            this.tsbLogTrace.Size = new System.Drawing.Size(137, 22);
            this.tsbLogTrace.Text = "Trace";
            this.tsbLogTrace.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbLogDebug
            // 
            this.tsbLogDebug.CheckOnClick = true;
            this.tsbLogDebug.Name = "tsbLogDebug";
            this.tsbLogDebug.Size = new System.Drawing.Size(137, 22);
            this.tsbLogDebug.Text = "Debug";
            this.tsbLogDebug.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbLogVerbose
            // 
            this.tsbLogVerbose.CheckOnClick = true;
            this.tsbLogVerbose.Name = "tsbLogVerbose";
            this.tsbLogVerbose.Size = new System.Drawing.Size(137, 22);
            this.tsbLogVerbose.Text = "Verbose";
            this.tsbLogVerbose.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAutoScroll
            // 
            this.tsbAutoScroll.CheckOnClick = true;
            this.tsbAutoScroll.Image = ((System.Drawing.Image)(resources.GetObject("tsbAutoScroll.Image")));
            this.tsbAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAutoScroll.Name = "tsbAutoScroll";
            this.tsbAutoScroll.Size = new System.Drawing.Size(110, 22);
            this.tsbAutoScroll.Text = "Log Auto-Scroll";
            this.tsbAutoScroll.CheckedChanged += new System.EventHandler(this.tsbSettingChanged_CheckedChanged);
            // 
            // tsbServerState
            // 
            this.tsbServerState.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbServerState.CheckOnClick = true;
            this.tsbServerState.Image = global::OPLServer.Properties.Resources.start;
            this.tsbServerState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbServerState.Name = "tsbServerState";
            this.tsbServerState.Size = new System.Drawing.Size(194, 22);
            this.tsbServerState.Text = "Server is stopped (press to start)";
            this.tsbServerState.CheckedChanged += new System.EventHandler(this.tsbServerState_CheckedChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClearLog
            // 
            this.tsbClearLog.Image = global::OPLServer.Properties.Resources.clear;
            this.tsbClearLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearLog.Name = "tsbClearLog";
            this.tsbClearLog.Size = new System.Drawing.Size(77, 22);
            this.tsbClearLog.Text = "Clear Log";
            this.tsbClearLog.Click += new System.EventHandler(this.tsbClearLog_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAbout
            // 
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAbout.Image = ((System.Drawing.Image)(resources.GetObject("tsbAbout.Image")));
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(44, 22);
            this.tsbAbout.Text = "About";
            this.tsbAbout.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tstbPort
            // 
            this.tstbPort.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tstbPort.BackColor = System.Drawing.Color.White;
            this.tstbPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstbPort.Name = "tstbPort";
            this.tstbPort.Size = new System.Drawing.Size(40, 25);
            this.tstbPort.Text = "1024";
            this.tstbPort.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tstbPort.ToolTipText = "Server Port";
            this.tstbPort.Leave += new System.EventHandler(this.tstbPort_Leave);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "Port:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 332);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "OPL Server by MaRioLo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbAutoScroll;
        private System.Windows.Forms.ToolStripSplitButton tsbLogFilter;
        private System.Windows.Forms.ToolStripMenuItem tsbLogInfo;
        private System.Windows.Forms.ToolStripMenuItem tsbLogWarn;
        private System.Windows.Forms.ToolStripMenuItem tsbLogError;
        private System.Windows.Forms.ToolStripMenuItem tsbLogCritical;
        private System.Windows.Forms.ToolStripMenuItem tsbLogTrace;
        private System.Windows.Forms.ToolStripMenuItem tsbLogDebug;
        private System.Windows.Forms.ToolStripMenuItem tsbLogVerbose;
        private System.Windows.Forms.ToolStripButton tsbServerState;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbEnableLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbClearLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripTextBox tstbPort;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}

