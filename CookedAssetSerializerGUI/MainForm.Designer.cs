using CookedAssetSerializer;
using CookedAssetSerializerGUI.Properties;
using ExtendedTreeView;

namespace CookedAssetSerializerGUI;

partial class MainForm {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    


    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tTipTree = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cntxtMainStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearAllPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restorePathsToDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseSerializationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelSerializationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeParseDir = new ExtendedTreeView.ExTreeView();
            this.cntxtTreeParse = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbAuthors = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbRun = new System.Windows.Forms.TabPage();
            this.btnPrsTree = new System.Windows.Forms.Button();
            this.chkAutoLoad = new System.Windows.Forms.CheckBox();
            this.chkDumNativ = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.rtxtInfoDir = new System.Windows.Forms.RichTextBox();
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.rtxtParseDir = new System.Windows.Forms.RichTextBox();
            this.rtxtContentDir = new System.Windows.Forms.RichTextBox();
            this.rtxtJSONDir = new System.Windows.Forms.RichTextBox();
            this.rtxtCookedDir = new System.Windows.Forms.RichTextBox();
            this.btnSelectContentDir = new System.Windows.Forms.Button();
            this.btnInfoDir = new System.Windows.Forms.Button();
            this.btnOpenAllTypes = new System.Windows.Forms.Button();
            this.btnOpenAssetTypes = new System.Windows.Forms.Button();
            this.btnSelectParseDir = new System.Windows.Forms.Button();
            this.btnOpenLogs = new System.Windows.Forms.Button();
            this.btnSelectJSONDir = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSerializeAssets = new System.Windows.Forms.Button();
            this.btnSelectCookedDir = new System.Windows.Forms.Button();
            this.btnMoveCookedAssets = new System.Windows.Forms.Button();
            this.btnScanAssets = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRefreshAssets = new System.Windows.Forms.CheckBox();
            this.cbUEVersion = new System.Windows.Forms.ComboBox();
            this.tbSettings = new System.Windows.Forms.TabPage();
            this.chkSettDNS = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbAssetsToDelete = new System.Windows.Forms.ListBox();
            this.chkDummyWithProps = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbDummyAssets = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAssetsToSkipSerialization = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rtxtCookedAssets = new System.Windows.Forms.RichTextBox();
            this.rtxtSimpleAssets = new System.Windows.Forms.RichTextBox();
            this.rtxtCircularDependancy = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbNativSett = new System.Windows.Forms.TabPage();
            this.btnDfltGamCnfg = new System.Windows.Forms.Button();
            this.rtxtDfltGamCnfg = new System.Windows.Forms.RichTextBox();
            this.rtxtCXXDir = new System.Windows.Forms.RichTextBox();
            this.rtxtNativAssets = new System.Windows.Forms.RichTextBox();
            this.btnCXXDir = new System.Windows.Forms.Button();
            this.lbNativMode = new System.Windows.Forms.Label();
            this.cbNativMethod = new System.Windows.Forms.ComboBox();
            this.chkUserEnumStruct = new System.Windows.Forms.CheckBox();
            this.lbNativAssets = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.cntxtMainStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.cntxtTreeParse.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbRun.SuspendLayout();
            this.tbSettings.SuspendLayout();
            this.tbNativSett.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.ContextMenuStrip = this.cntxtMainStrip;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1232, 717);
            this.flowLayoutPanel1.TabIndex = 40;
            // 
            // cntxtMainStrip
            // 
            this.cntxtMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllPathsToolStripMenuItem,
            this.restorePathsToDefaultsToolStripMenuItem,
            this.pauseSerializationToolStripMenuItem,
            this.cancelSerializationToolStripMenuItem,
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem,
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem});
            this.cntxtMainStrip.Name = "cntxtMainStrip";
            this.cntxtMainStrip.Size = new System.Drawing.Size(277, 136);
            this.cntxtMainStrip.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMainStrip_Opening);
            // 
            // clearAllPathsToolStripMenuItem
            // 
            this.clearAllPathsToolStripMenuItem.Name = "clearAllPathsToolStripMenuItem";
            this.clearAllPathsToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.clearAllPathsToolStripMenuItem.Text = "Clear all paths";
            this.clearAllPathsToolStripMenuItem.Click += new System.EventHandler(this.clearAllPathsToolStripMenuItem_Click);
            // 
            // restorePathsToDefaultsToolStripMenuItem
            // 
            this.restorePathsToDefaultsToolStripMenuItem.Name = "restorePathsToDefaultsToolStripMenuItem";
            this.restorePathsToDefaultsToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.restorePathsToDefaultsToolStripMenuItem.Text = "Restore paths to defaults";
            this.restorePathsToDefaultsToolStripMenuItem.Click += new System.EventHandler(this.restorePathsToDefaultsToolStripMenuItem_Click);
            // 
            // pauseSerializationToolStripMenuItem
            // 
            this.pauseSerializationToolStripMenuItem.Name = "pauseSerializationToolStripMenuItem";
            this.pauseSerializationToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.pauseSerializationToolStripMenuItem.Text = "Pause Serialization";
            this.pauseSerializationToolStripMenuItem.Click += new System.EventHandler(this.cancelSerializationToolStripMenuItem_Click);
            // 
            // cancelSerializationToolStripMenuItem
            // 
            this.cancelSerializationToolStripMenuItem.Name = "cancelSerializationToolStripMenuItem";
            this.cancelSerializationToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.cancelSerializationToolStripMenuItem.Text = "Cancel Serialization";
            this.cancelSerializationToolStripMenuItem.Click += new System.EventHandler(this.cancelSerializationToolStripMenuItem_Click_1);
            // 
            // restoreAllSettingsToDefaultthisTabToolStripMenuItem
            // 
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Name = "restoreAllSettingsToDefaultthisTabToolStripMenuItem";
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Text = "Restore all settings to default (this tab)";
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSettingsToDefaultthisTabToolStripMenuItem_Click);
            // 
            // restoreAllSettingsToDefaultallTabsToolStripMenuItem
            // 
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Name = "restoreAllSettingsToDefaultallTabsToolStripMenuItem";
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Text = "Restore all settings to default (all tabs)";
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSettingsToDefaultallTabsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.treeParseDir);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 711);
            this.panel1.TabIndex = 0;
            // 
            // treeParseDir
            // 
            this.treeParseDir.CheckBoxes = true;
            this.treeParseDir.ContextMenuStrip = this.cntxtTreeParse;
            this.treeParseDir.Location = new System.Drawing.Point(0, 7);
            this.treeParseDir.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.treeParseDir.Name = "treeParseDir";
            this.treeParseDir.Size = new System.Drawing.Size(247, 704);
            this.treeParseDir.TabIndex = 2;
            this.treeParseDir.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeParseDir_AfterCheck);
            this.treeParseDir.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeParseDir_BeforeExpand);
            this.treeParseDir.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeParseDir_MouseDown);
            this.treeParseDir.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeParseDir_MouseMove);
            // 
            // cntxtTreeParse
            // 
            this.cntxtTreeParse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.refreshAllToolStripMenuItem});
            this.cntxtTreeParse.Name = "cntxtTreeParse";
            this.cntxtTreeParse.Size = new System.Drawing.Size(137, 70);
            this.cntxtTreeParse.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtTreeParse_Opening);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // refreshAllToolStripMenuItem
            // 
            this.refreshAllToolStripMenuItem.Name = "refreshAllToolStripMenuItem";
            this.refreshAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.refreshAllToolStripMenuItem.Text = "Refresh All";
            this.refreshAllToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.lbAuthors);
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(259, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 711);
            this.panel2.TabIndex = 1;
            // 
            // lbAuthors
            // 
            this.lbAuthors.AutoSize = true;
            this.lbAuthors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbAuthors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAuthors.Location = new System.Drawing.Point(289, 1);
            this.lbAuthors.Name = "lbAuthors";
            this.lbAuthors.Size = new System.Drawing.Size(393, 15);
            this.lbAuthors.TabIndex = 40;
            this.lbAuthors.Text = "Written by LongerWarrior, Buckminsterfullerene and atenfyr (UAAPI)";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbRun);
            this.tabControl1.Controls.Add(this.tbSettings);
            this.tabControl1.Controls.Add(this.tbNativSett);
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(964, 709);
            this.tabControl1.TabIndex = 39;
            // 
            // tbRun
            // 
            this.tbRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tbRun.Controls.Add(this.btnPrsTree);
            this.tbRun.Controls.Add(this.chkAutoLoad);
            this.tbRun.Controls.Add(this.chkDumNativ);
            this.tbRun.Controls.Add(this.label2);
            this.tbRun.Controls.Add(this.btnLoadConfig);
            this.tbRun.Controls.Add(this.btnSaveConfig);
            this.tbRun.Controls.Add(this.rtxtInfoDir);
            this.tbRun.Controls.Add(this.rtxtOutput);
            this.tbRun.Controls.Add(this.rtxtParseDir);
            this.tbRun.Controls.Add(this.rtxtContentDir);
            this.tbRun.Controls.Add(this.rtxtJSONDir);
            this.tbRun.Controls.Add(this.rtxtCookedDir);
            this.tbRun.Controls.Add(this.btnSelectContentDir);
            this.tbRun.Controls.Add(this.btnInfoDir);
            this.tbRun.Controls.Add(this.btnOpenAllTypes);
            this.tbRun.Controls.Add(this.btnOpenAssetTypes);
            this.tbRun.Controls.Add(this.btnSelectParseDir);
            this.tbRun.Controls.Add(this.btnOpenLogs);
            this.tbRun.Controls.Add(this.btnSelectJSONDir);
            this.tbRun.Controls.Add(this.lblProgress);
            this.tbRun.Controls.Add(this.btnClearLogs);
            this.tbRun.Controls.Add(this.btnSerializeAssets);
            this.tbRun.Controls.Add(this.btnSelectCookedDir);
            this.tbRun.Controls.Add(this.btnMoveCookedAssets);
            this.tbRun.Controls.Add(this.btnScanAssets);
            this.tbRun.Controls.Add(this.label1);
            this.tbRun.Controls.Add(this.chkRefreshAssets);
            this.tbRun.Controls.Add(this.cbUEVersion);
            this.tbRun.Location = new System.Drawing.Point(4, 24);
            this.tbRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbRun.Name = "tbRun";
            this.tbRun.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbRun.Size = new System.Drawing.Size(956, 681);
            this.tbRun.TabIndex = 0;
            this.tbRun.Text = "Run";
            // 
            // btnPrsTree
            // 
            this.btnPrsTree.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnPrsTree.FlatAppearance.BorderSize = 2;
            this.btnPrsTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrsTree.Font = new System.Drawing.Font("Yu Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPrsTree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnPrsTree.Location = new System.Drawing.Point(13, 50);
            this.btnPrsTree.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrsTree.Name = "btnPrsTree";
            this.btnPrsTree.Size = new System.Drawing.Size(35, 30);
            this.btnPrsTree.TabIndex = 40;
            this.btnPrsTree.Text = "<<";
            this.btnPrsTree.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrsTree.UseVisualStyleBackColor = true;
            this.btnPrsTree.Click += new System.EventHandler(this.GatherCheckedFiles);
            // 
            // chkAutoLoad
            // 
            this.chkAutoLoad.AutoSize = true;
            this.chkAutoLoad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkAutoLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkAutoLoad.Location = new System.Drawing.Point(626, 245);
            this.chkAutoLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAutoLoad.Name = "chkAutoLoad";
            this.chkAutoLoad.Size = new System.Drawing.Size(245, 25);
            this.chkAutoLoad.TabIndex = 39;
            this.chkAutoLoad.Text = "Auto Load Config on Launch";
            this.chkAutoLoad.UseVisualStyleBackColor = true;
            this.chkAutoLoad.CheckedChanged += new System.EventHandler(this.chkAutoLoad_CheckedChanged);
            // 
            // chkDumNativ
            // 
            this.chkDumNativ.AutoSize = true;
            this.chkDumNativ.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkDumNativ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkDumNativ.Location = new System.Drawing.Point(311, 201);
            this.chkDumNativ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDumNativ.Name = "chkDumNativ";
            this.chkDumNativ.Size = new System.Drawing.Size(164, 25);
            this.chkDumNativ.TabIndex = 38;
            this.chkDumNativ.Text = "Uses Nativ. Assets";
            this.chkDumNativ.UseVisualStyleBackColor = true;
            this.chkDumNativ.CheckedChanged += new System.EventHandler(this.chkDumNativ_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label2.Location = new System.Drawing.Point(437, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 30);
            this.label2.TabIndex = 33;
            this.label2.Text = "Output";
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnLoadConfig.FlatAppearance.BorderSize = 2;
            this.btnLoadConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadConfig.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLoadConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnLoadConfig.Location = new System.Drawing.Point(511, 211);
            this.btnLoadConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(198, 30);
            this.btnLoadConfig.TabIndex = 29;
            this.btnLoadConfig.Text = "Load Config Settings";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSaveConfig.FlatAppearance.BorderSize = 2;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveConfig.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSaveConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSaveConfig.Location = new System.Drawing.Point(739, 211);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(198, 30);
            this.btnSaveConfig.TabIndex = 30;
            this.btnSaveConfig.Text = "Save Config Settings";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // rtxtInfoDir
            // 
            this.rtxtInfoDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtInfoDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtInfoDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtInfoDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtInfoDir.Location = new System.Drawing.Point(150, 158);
            this.rtxtInfoDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtInfoDir.Multiline = false;
            this.rtxtInfoDir.Name = "rtxtInfoDir";
            this.rtxtInfoDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtInfoDir.Size = new System.Drawing.Size(787, 30);
            this.rtxtInfoDir.TabIndex = 37;
            this.rtxtInfoDir.Text = "C:\\ExamplePath\\Info";
            this.rtxtInfoDir.Enter += new System.EventHandler(this.rtxtInfoDir_Enter);
            this.rtxtInfoDir.Leave += new System.EventHandler(this.rtxtInfoDir_Leave);
            // 
            // rtxtOutput
            // 
            this.rtxtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtOutput.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtOutput.Location = new System.Drawing.Point(16, 342);
            this.rtxtOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.ReadOnly = true;
            this.rtxtOutput.Size = new System.Drawing.Size(925, 284);
            this.rtxtOutput.TabIndex = 23;
            this.rtxtOutput.Text = "";
            this.rtxtOutput.WordWrap = false;
            // 
            // rtxtParseDir
            // 
            this.rtxtParseDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtParseDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtParseDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtParseDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtParseDir.Location = new System.Drawing.Point(150, 50);
            this.rtxtParseDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtParseDir.Multiline = false;
            this.rtxtParseDir.Name = "rtxtParseDir";
            this.rtxtParseDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtParseDir.Size = new System.Drawing.Size(787, 30);
            this.rtxtParseDir.TabIndex = 33;
            this.rtxtParseDir.Text = "C:\\ExamplePath\\Content\\Data";
            this.rtxtParseDir.Enter += new System.EventHandler(this.rtxtParseDir_Enter);
            this.rtxtParseDir.Leave += new System.EventHandler(this.rtxtParseDir_Leave);
            // 
            // rtxtContentDir
            // 
            this.rtxtContentDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtContentDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtContentDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtContentDir.Location = new System.Drawing.Point(150, 14);
            this.rtxtContentDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtContentDir.Multiline = false;
            this.rtxtContentDir.Name = "rtxtContentDir";
            this.rtxtContentDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtContentDir.Size = new System.Drawing.Size(787, 30);
            this.rtxtContentDir.TabIndex = 1;
            this.rtxtContentDir.Text = "C:\\ExamplePath\\Content";
            this.rtxtContentDir.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateContentDir);
            // 
            // rtxtJSONDir
            // 
            this.rtxtJSONDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtJSONDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtJSONDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtJSONDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtJSONDir.Location = new System.Drawing.Point(150, 86);
            this.rtxtJSONDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtJSONDir.Multiline = false;
            this.rtxtJSONDir.Name = "rtxtJSONDir";
            this.rtxtJSONDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtJSONDir.Size = new System.Drawing.Size(787, 30);
            this.rtxtJSONDir.TabIndex = 3;
            this.rtxtJSONDir.Text = "C:\\ExamplePath\\AssetDump";
            this.rtxtJSONDir.Enter += new System.EventHandler(this.rtxtJSONDir_Enter);
            this.rtxtJSONDir.Leave += new System.EventHandler(this.rtxtJSONDir_Leave);
            // 
            // rtxtCookedDir
            // 
            this.rtxtCookedDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCookedDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCookedDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCookedDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCookedDir.Location = new System.Drawing.Point(150, 122);
            this.rtxtCookedDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtCookedDir.Multiline = false;
            this.rtxtCookedDir.Name = "rtxtCookedDir";
            this.rtxtCookedDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtCookedDir.Size = new System.Drawing.Size(787, 30);
            this.rtxtCookedDir.TabIndex = 5;
            this.rtxtCookedDir.Text = "C:\\ExamplePath\\Cooked";
            this.rtxtCookedDir.Enter += new System.EventHandler(this.rtxtCookedDir_Enter);
            this.rtxtCookedDir.Leave += new System.EventHandler(this.rtxtCookedDir_Leave);
            // 
            // btnSelectContentDir
            // 
            this.btnSelectContentDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectContentDir.FlatAppearance.BorderSize = 2;
            this.btnSelectContentDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectContentDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectContentDir.Location = new System.Drawing.Point(13, 14);
            this.btnSelectContentDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectContentDir.Name = "btnSelectContentDir";
            this.btnSelectContentDir.Size = new System.Drawing.Size(122, 30);
            this.btnSelectContentDir.TabIndex = 0;
            this.btnSelectContentDir.Text = "Content Dir";
            this.btnSelectContentDir.UseVisualStyleBackColor = true;
            this.btnSelectContentDir.Click += new System.EventHandler(this.btnSelectContentDir_Click);
            // 
            // btnInfoDir
            // 
            this.btnInfoDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnInfoDir.FlatAppearance.BorderSize = 2;
            this.btnInfoDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfoDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnInfoDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnInfoDir.Location = new System.Drawing.Point(13, 158);
            this.btnInfoDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnInfoDir.Name = "btnInfoDir";
            this.btnInfoDir.Size = new System.Drawing.Size(122, 30);
            this.btnInfoDir.TabIndex = 36;
            this.btnInfoDir.Text = "Info Dir";
            this.btnInfoDir.UseVisualStyleBackColor = true;
            this.btnInfoDir.Click += new System.EventHandler(this.btnInfoDir_Click);
            // 
            // btnOpenAllTypes
            // 
            this.btnOpenAllTypes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenAllTypes.FlatAppearance.BorderSize = 2;
            this.btnOpenAllTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAllTypes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAllTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenAllTypes.Location = new System.Drawing.Point(16, 637);
            this.btnOpenAllTypes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenAllTypes.Name = "btnOpenAllTypes";
            this.btnOpenAllTypes.Size = new System.Drawing.Size(160, 30);
            this.btnOpenAllTypes.TabIndex = 25;
            this.btnOpenAllTypes.Text = "Open AllTypes";
            this.btnOpenAllTypes.UseVisualStyleBackColor = true;
            this.btnOpenAllTypes.Click += new System.EventHandler(this.btnOpenAllTypes_Click);
            // 
            // btnOpenAssetTypes
            // 
            this.btnOpenAssetTypes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenAssetTypes.FlatAppearance.BorderSize = 2;
            this.btnOpenAssetTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAssetTypes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAssetTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenAssetTypes.Location = new System.Drawing.Point(197, 637);
            this.btnOpenAssetTypes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenAssetTypes.Name = "btnOpenAssetTypes";
            this.btnOpenAssetTypes.Size = new System.Drawing.Size(177, 30);
            this.btnOpenAssetTypes.TabIndex = 24;
            this.btnOpenAssetTypes.Text = "Open AssetTypes";
            this.btnOpenAssetTypes.UseVisualStyleBackColor = true;
            this.btnOpenAssetTypes.Click += new System.EventHandler(this.btnOpenAssetTypes_Click);
            // 
            // btnSelectParseDir
            // 
            this.btnSelectParseDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectParseDir.FlatAppearance.BorderSize = 2;
            this.btnSelectParseDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectParseDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectParseDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectParseDir.Location = new System.Drawing.Point(43, 50);
            this.btnSelectParseDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectParseDir.Name = "btnSelectParseDir";
            this.btnSelectParseDir.Size = new System.Drawing.Size(92, 30);
            this.btnSelectParseDir.TabIndex = 32;
            this.btnSelectParseDir.Text = "Parse Dir";
            this.btnSelectParseDir.UseVisualStyleBackColor = true;
            this.btnSelectParseDir.Click += new System.EventHandler(this.btnSelectParseDir_Click);
            // 
            // btnOpenLogs
            // 
            this.btnOpenLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenLogs.FlatAppearance.BorderSize = 2;
            this.btnOpenLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLogs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenLogs.Location = new System.Drawing.Point(677, 637);
            this.btnOpenLogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenLogs.Name = "btnOpenLogs";
            this.btnOpenLogs.Size = new System.Drawing.Size(121, 30);
            this.btnOpenLogs.TabIndex = 26;
            this.btnOpenLogs.Text = "Open Logs";
            this.btnOpenLogs.UseVisualStyleBackColor = true;
            this.btnOpenLogs.Click += new System.EventHandler(this.btnOpenLogs_Click);
            // 
            // btnSelectJSONDir
            // 
            this.btnSelectJSONDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectJSONDir.FlatAppearance.BorderSize = 2;
            this.btnSelectJSONDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectJSONDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectJSONDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectJSONDir.Location = new System.Drawing.Point(13, 86);
            this.btnSelectJSONDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectJSONDir.Name = "btnSelectJSONDir";
            this.btnSelectJSONDir.Size = new System.Drawing.Size(122, 30);
            this.btnSelectJSONDir.TabIndex = 2;
            this.btnSelectJSONDir.Text = "Dump Dir";
            this.btnSelectJSONDir.UseVisualStyleBackColor = true;
            this.btnSelectJSONDir.Click += new System.EventHandler(this.btnSelectJSONDir_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lblProgress.Location = new System.Drawing.Point(708, 275);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(233, 30);
            this.lblProgress.TabIndex = 31;
            this.lblProgress.Text = "0/0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnClearLogs.FlatAppearance.BorderSize = 2;
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClearLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnClearLogs.Location = new System.Drawing.Point(820, 637);
            this.btnClearLogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(121, 30);
            this.btnClearLogs.TabIndex = 28;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnSerializeAssets
            // 
            this.btnSerializeAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSerializeAssets.FlatAppearance.BorderSize = 2;
            this.btnSerializeAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerializeAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSerializeAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSerializeAssets.Location = new System.Drawing.Point(472, 275);
            this.btnSerializeAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSerializeAssets.Name = "btnSerializeAssets";
            this.btnSerializeAssets.Size = new System.Drawing.Size(198, 30);
            this.btnSerializeAssets.TabIndex = 18;
            this.btnSerializeAssets.Text = "Serialize Assets";
            this.btnSerializeAssets.UseVisualStyleBackColor = true;
            this.btnSerializeAssets.Click += new System.EventHandler(this.btnSerializeAssets_Click);
            // 
            // btnSelectCookedDir
            // 
            this.btnSelectCookedDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectCookedDir.FlatAppearance.BorderSize = 2;
            this.btnSelectCookedDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCookedDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectCookedDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectCookedDir.Location = new System.Drawing.Point(13, 122);
            this.btnSelectCookedDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectCookedDir.Name = "btnSelectCookedDir";
            this.btnSelectCookedDir.Size = new System.Drawing.Size(122, 30);
            this.btnSelectCookedDir.TabIndex = 4;
            this.btnSelectCookedDir.Text = "Cooked Dir";
            this.btnSelectCookedDir.UseVisualStyleBackColor = true;
            this.btnSelectCookedDir.Click += new System.EventHandler(this.btnSelectOutputDir_Click);
            // 
            // btnMoveCookedAssets
            // 
            this.btnMoveCookedAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnMoveCookedAssets.FlatAppearance.BorderSize = 2;
            this.btnMoveCookedAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveCookedAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnMoveCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnMoveCookedAssets.Location = new System.Drawing.Point(242, 275);
            this.btnMoveCookedAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMoveCookedAssets.Name = "btnMoveCookedAssets";
            this.btnMoveCookedAssets.Size = new System.Drawing.Size(198, 30);
            this.btnMoveCookedAssets.TabIndex = 17;
            this.btnMoveCookedAssets.Text = "Move Cooked Assets";
            this.btnMoveCookedAssets.UseVisualStyleBackColor = true;
            this.btnMoveCookedAssets.Click += new System.EventHandler(this.btnMoveCookedAssets_Click);
            // 
            // btnScanAssets
            // 
            this.btnScanAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnScanAssets.FlatAppearance.BorderSize = 2;
            this.btnScanAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnScanAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnScanAssets.Location = new System.Drawing.Point(15, 275);
            this.btnScanAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnScanAssets.Name = "btnScanAssets";
            this.btnScanAssets.Size = new System.Drawing.Size(198, 30);
            this.btnScanAssets.TabIndex = 16;
            this.btnScanAssets.Text = "Scan Assets";
            this.btnScanAssets.UseVisualStyleBackColor = true;
            this.btnScanAssets.Click += new System.EventHandler(this.btnScanAssets_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label1.Location = new System.Drawing.Point(19, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "UE Version:";
            // 
            // chkRefreshAssets
            // 
            this.chkRefreshAssets.AutoSize = true;
            this.chkRefreshAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkRefreshAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkRefreshAssets.Location = new System.Drawing.Point(311, 230);
            this.chkRefreshAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkRefreshAssets.Name = "chkRefreshAssets";
            this.chkRefreshAssets.Size = new System.Drawing.Size(137, 25);
            this.chkRefreshAssets.TabIndex = 8;
            this.chkRefreshAssets.Text = "Refresh Assets";
            this.chkRefreshAssets.UseVisualStyleBackColor = true;
            // 
            // cbUEVersion
            // 
            this.cbUEVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.cbUEVersion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbUEVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbUEVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.cbUEVersion.FormattingEnabled = true;
            this.cbUEVersion.Location = new System.Drawing.Point(134, 211);
            this.cbUEVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbUEVersion.Name = "cbUEVersion";
            this.cbUEVersion.Size = new System.Drawing.Size(156, 29);
            this.cbUEVersion.TabIndex = 7;
            // 
            // tbSettings
            // 
            this.tbSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tbSettings.Controls.Add(this.chkSettDNS);
            this.tbSettings.Controls.Add(this.label7);
            this.tbSettings.Controls.Add(this.lbAssetsToDelete);
            this.tbSettings.Controls.Add(this.chkDummyWithProps);
            this.tbSettings.Controls.Add(this.label9);
            this.tbSettings.Controls.Add(this.lbDummyAssets);
            this.tbSettings.Controls.Add(this.label3);
            this.tbSettings.Controls.Add(this.lbAssetsToSkipSerialization);
            this.tbSettings.Controls.Add(this.label5);
            this.tbSettings.Controls.Add(this.label4);
            this.tbSettings.Controls.Add(this.rtxtCookedAssets);
            this.tbSettings.Controls.Add(this.rtxtSimpleAssets);
            this.tbSettings.Controls.Add(this.rtxtCircularDependancy);
            this.tbSettings.Controls.Add(this.label6);
            this.tbSettings.Location = new System.Drawing.Point(4, 24);
            this.tbSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbSettings.Name = "tbSettings";
            this.tbSettings.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbSettings.Size = new System.Drawing.Size(956, 681);
            this.tbSettings.TabIndex = 1;
            this.tbSettings.Text = "Settings";
            // 
            // chkSettDNS
            // 
            this.chkSettDNS.AutoSize = true;
            this.chkSettDNS.Location = new System.Drawing.Point(656, 654);
            this.chkSettDNS.Name = "chkSettDNS";
            this.chkSettDNS.Size = new System.Drawing.Size(215, 19);
            this.chkSettDNS.TabIndex = 41;
            this.chkSettDNS.Text = "Do Not Show Save Prompt on Close";
            this.chkSettDNS.UseVisualStyleBackColor = true;
            this.chkSettDNS.CheckedChanged += new System.EventHandler(this.chkSettDNS_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label7.Location = new System.Drawing.Point(616, 485);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(263, 30);
            this.label7.TabIndex = 39;
            this.label7.Text = "Existing assets to delete";
            // 
            // lbAssetsToDelete
            // 
            this.lbAssetsToDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbAssetsToDelete.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAssetsToDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lbAssetsToDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAssetsToDelete.FormattingEnabled = true;
            this.lbAssetsToDelete.ItemHeight = 21;
            this.lbAssetsToDelete.Location = new System.Drawing.Point(584, 517);
            this.lbAssetsToDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbAssetsToDelete.Name = "lbAssetsToDelete";
            this.lbAssetsToDelete.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToDelete.Size = new System.Drawing.Size(354, 126);
            this.lbAssetsToDelete.TabIndex = 40;
            // 
            // chkDummyWithProps
            // 
            this.chkDummyWithProps.AutoSize = true;
            this.chkDummyWithProps.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkDummyWithProps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkDummyWithProps.Location = new System.Drawing.Point(658, 459);
            this.chkDummyWithProps.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDummyWithProps.Name = "chkDummyWithProps";
            this.chkDummyWithProps.Size = new System.Drawing.Size(213, 25);
            this.chkDummyWithProps.TabIndex = 38;
            this.chkDummyWithProps.Text = "Dummy With Properties";
            this.chkDummyWithProps.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label9.Location = new System.Drawing.Point(658, 234);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(192, 30);
            this.label9.TabIndex = 34;
            this.label9.Text = "Assets to dummy";
            // 
            // lbDummyAssets
            // 
            this.lbDummyAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbDummyAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbDummyAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lbDummyAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbDummyAssets.FormattingEnabled = true;
            this.lbDummyAssets.ItemHeight = 21;
            this.lbDummyAssets.Location = new System.Drawing.Point(584, 263);
            this.lbDummyAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbDummyAssets.Name = "lbDummyAssets";
            this.lbDummyAssets.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbDummyAssets.Size = new System.Drawing.Size(354, 189);
            this.lbDummyAssets.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label3.Location = new System.Drawing.Point(624, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 30);
            this.label3.TabIndex = 10;
            this.label3.Text = "Assets to skip serializing";
            // 
            // lbAssetsToSkipSerialization
            // 
            this.lbAssetsToSkipSerialization.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbAssetsToSkipSerialization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAssetsToSkipSerialization.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lbAssetsToSkipSerialization.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAssetsToSkipSerialization.FormattingEnabled = true;
            this.lbAssetsToSkipSerialization.ItemHeight = 21;
            this.lbAssetsToSkipSerialization.Location = new System.Drawing.Point(584, 33);
            this.lbAssetsToSkipSerialization.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbAssetsToSkipSerialization.Name = "lbAssetsToSkipSerialization";
            this.lbAssetsToSkipSerialization.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToSkipSerialization.Size = new System.Drawing.Size(354, 189);
            this.lbAssetsToSkipSerialization.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label5.Location = new System.Drawing.Point(191, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 30);
            this.label5.TabIndex = 14;
            this.label5.Text = "Simple assets";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label4.Location = new System.Drawing.Point(147, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(244, 30);
            this.label4.TabIndex = 12;
            this.label4.Text = "Cooked assets to copy";
            // 
            // rtxtCookedAssets
            // 
            this.rtxtCookedAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCookedAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCookedAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCookedAssets.Location = new System.Drawing.Point(15, 263);
            this.rtxtCookedAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtCookedAssets.Name = "rtxtCookedAssets";
            this.rtxtCookedAssets.Size = new System.Drawing.Size(542, 189);
            this.rtxtCookedAssets.TabIndex = 13;
            this.rtxtCookedAssets.Text = resources.GetString("rtxtCookedAssets.Text");
            // 
            // rtxtSimpleAssets
            // 
            this.rtxtSimpleAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtSimpleAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtSimpleAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtSimpleAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtSimpleAssets.Location = new System.Drawing.Point(14, 33);
            this.rtxtSimpleAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtSimpleAssets.Name = "rtxtSimpleAssets";
            this.rtxtSimpleAssets.Size = new System.Drawing.Size(543, 189);
            this.rtxtSimpleAssets.TabIndex = 15;
            this.rtxtSimpleAssets.Text = resources.GetString("rtxtSimpleAssets.Text");
            this.rtxtSimpleAssets.WordWrap = false;
            // 
            // rtxtCircularDependancy
            // 
            this.rtxtCircularDependancy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCircularDependancy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCircularDependancy.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCircularDependancy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCircularDependancy.Location = new System.Drawing.Point(17, 517);
            this.rtxtCircularDependancy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtCircularDependancy.Name = "rtxtCircularDependancy";
            this.rtxtCircularDependancy.Size = new System.Drawing.Size(543, 131);
            this.rtxtCircularDependancy.TabIndex = 20;
            this.rtxtCircularDependancy.Text = "/Script/Engine.SoundClass\n/Script/Engine.SoundSubmix\n/Script/Engine.EndpointSubmi" +
    "x";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label6.Location = new System.Drawing.Point(85, 485);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(365, 30);
            this.label6.TabIndex = 19;
            this.label6.Text = "Assets with a circular dependancy";
            // 
            // tbNativSett
            // 
            this.tbNativSett.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tbNativSett.Controls.Add(this.btnDfltGamCnfg);
            this.tbNativSett.Controls.Add(this.rtxtDfltGamCnfg);
            this.tbNativSett.Controls.Add(this.rtxtCXXDir);
            this.tbNativSett.Controls.Add(this.rtxtNativAssets);
            this.tbNativSett.Controls.Add(this.btnCXXDir);
            this.tbNativSett.Controls.Add(this.lbNativMode);
            this.tbNativSett.Controls.Add(this.cbNativMethod);
            this.tbNativSett.Controls.Add(this.chkUserEnumStruct);
            this.tbNativSett.Controls.Add(this.lbNativAssets);
            this.tbNativSett.Location = new System.Drawing.Point(4, 24);
            this.tbNativSett.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbNativSett.Name = "tbNativSett";
            this.tbNativSett.Padding = new System.Windows.Forms.Padding(3);
            this.tbNativSett.Size = new System.Drawing.Size(956, 681);
            this.tbNativSett.TabIndex = 2;
            this.tbNativSett.Text = "Nativ. Asset Settings";
            // 
            // btnDfltGamCnfg
            // 
            this.btnDfltGamCnfg.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnDfltGamCnfg.FlatAppearance.BorderSize = 2;
            this.btnDfltGamCnfg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDfltGamCnfg.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDfltGamCnfg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnDfltGamCnfg.Location = new System.Drawing.Point(14, 16);
            this.btnDfltGamCnfg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDfltGamCnfg.Name = "btnDfltGamCnfg";
            this.btnDfltGamCnfg.Size = new System.Drawing.Size(122, 30);
            this.btnDfltGamCnfg.TabIndex = 23;
            this.btnDfltGamCnfg.Text = "Game .ini";
            this.btnDfltGamCnfg.UseVisualStyleBackColor = true;
            this.btnDfltGamCnfg.Click += new System.EventHandler(this.btnDfltGamCnfg_Click);
            // 
            // rtxtDfltGamCnfg
            // 
            this.rtxtDfltGamCnfg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtDfltGamCnfg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtDfltGamCnfg.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtDfltGamCnfg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtDfltGamCnfg.Location = new System.Drawing.Point(151, 16);
            this.rtxtDfltGamCnfg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtDfltGamCnfg.Multiline = false;
            this.rtxtDfltGamCnfg.Name = "rtxtDfltGamCnfg";
            this.rtxtDfltGamCnfg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtDfltGamCnfg.Size = new System.Drawing.Size(787, 30);
            this.rtxtDfltGamCnfg.TabIndex = 24;
            this.rtxtDfltGamCnfg.Text = "C:\\ExamplePath\\DefaultGame.ini";
            this.rtxtDfltGamCnfg.TextChanged += new System.EventHandler(this.rtxtDfltGamCnfg_TextChanged);
            // 
            // rtxtCXXDir
            // 
            this.rtxtCXXDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCXXDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCXXDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCXXDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCXXDir.Location = new System.Drawing.Point(151, 61);
            this.rtxtCXXDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtCXXDir.Multiline = false;
            this.rtxtCXXDir.Name = "rtxtCXXDir";
            this.rtxtCXXDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtCXXDir.Size = new System.Drawing.Size(787, 30);
            this.rtxtCXXDir.TabIndex = 22;
            this.rtxtCXXDir.Text = "C:\\ExamplePath\\CXXHeaderDump";
            this.rtxtCXXDir.TextChanged += new System.EventHandler(this.rtxtCXXDir_TextChanged);
            // 
            // rtxtNativAssets
            // 
            this.rtxtNativAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtNativAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtNativAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtNativAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtNativAssets.Location = new System.Drawing.Point(213, 163);
            this.rtxtNativAssets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxtNativAssets.Name = "rtxtNativAssets";
            this.rtxtNativAssets.Size = new System.Drawing.Size(543, 189);
            this.rtxtNativAssets.TabIndex = 17;
            this.rtxtNativAssets.Text = "Copy/Paste List of Nativized Assets from DefaultGame.ini or Select DefaultGame.in" +
    "i Above";
            // 
            // btnCXXDir
            // 
            this.btnCXXDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnCXXDir.FlatAppearance.BorderSize = 2;
            this.btnCXXDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCXXDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCXXDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnCXXDir.Location = new System.Drawing.Point(14, 61);
            this.btnCXXDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCXXDir.Name = "btnCXXDir";
            this.btnCXXDir.Size = new System.Drawing.Size(122, 30);
            this.btnCXXDir.TabIndex = 21;
            this.btnCXXDir.Text = "CXX Hdr Dir";
            this.btnCXXDir.UseVisualStyleBackColor = true;
            this.btnCXXDir.Click += new System.EventHandler(this.btnCXXDir_Click);
            // 
            // lbNativMode
            // 
            this.lbNativMode.AutoSize = true;
            this.lbNativMode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbNativMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbNativMode.Location = new System.Drawing.Point(218, 363);
            this.lbNativMode.Name = "lbNativMode";
            this.lbNativMode.Size = new System.Drawing.Size(157, 21);
            this.lbNativMode.TabIndex = 19;
            this.lbNativMode.Text = "Nativization Mode:";
            // 
            // cbNativMethod
            // 
            this.cbNativMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.cbNativMethod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbNativMethod.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbNativMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.cbNativMethod.FormattingEnabled = true;
            this.cbNativMethod.Location = new System.Drawing.Point(378, 357);
            this.cbNativMethod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbNativMethod.Name = "cbNativMethod";
            this.cbNativMethod.Size = new System.Drawing.Size(156, 29);
            this.cbNativMethod.TabIndex = 20;
            // 
            // chkUserEnumStruct
            // 
            this.chkUserEnumStruct.AutoSize = true;
            this.chkUserEnumStruct.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkUserEnumStruct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkUserEnumStruct.Location = new System.Drawing.Point(218, 409);
            this.chkUserEnumStruct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkUserEnumStruct.Name = "chkUserEnumStruct";
            this.chkUserEnumStruct.Size = new System.Drawing.Size(325, 25);
            this.chkUserEnumStruct.TabIndex = 18;
            this.chkUserEnumStruct.Text = "Create User Defined Enums and Structs";
            this.chkUserEnumStruct.UseVisualStyleBackColor = true;
            // 
            // lbNativAssets
            // 
            this.lbNativAssets.AutoSize = true;
            this.lbNativAssets.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbNativAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbNativAssets.Location = new System.Drawing.Point(336, 133);
            this.lbNativAssets.Name = "lbNativAssets";
            this.lbNativAssets.Size = new System.Drawing.Size(286, 30);
            this.lbNativAssets.TabIndex = 16;
            this.lbNativAssets.Text = "Nativized Blueprint Assets";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1204, 722);
            this.Controls.Add(this.flowLayoutPanel1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cooked Asset Serializer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.cntxtMainStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.cntxtTreeParse.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tbRun.ResumeLayout(false);
            this.tbRun.PerformLayout();
            this.tbSettings.ResumeLayout(false);
            this.tbSettings.PerformLayout();
            this.tbNativSett.ResumeLayout(false);
            this.tbNativSett.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private ToolTip tTipTree;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel panel1;
    private ExTreeView treeParseDir;
    private Panel panel2;
    private Label lbAuthors;
    private TabControl tabControl1;
    private TabPage tbRun;
    private Button btnPrsTree;
    private CheckBox chkAutoLoad;
    private CheckBox chkDumNativ;
    private Label label2;
    private Button btnLoadConfig;
    private Button btnSaveConfig;
    private RichTextBox rtxtInfoDir;
    private RichTextBox rtxtOutput;
    private RichTextBox rtxtParseDir;
    private RichTextBox rtxtContentDir;
    private RichTextBox rtxtJSONDir;
    private RichTextBox rtxtCookedDir;
    private Button btnSelectContentDir;
    private Button btnInfoDir;
    private Button btnOpenAllTypes;
    private Button btnOpenAssetTypes;
    private Button btnSelectParseDir;
    private Button btnOpenLogs;
    private Button btnSelectJSONDir;
    private Label lblProgress;
    private Button btnClearLogs;
    private Button btnSerializeAssets;
    private Button btnSelectCookedDir;
    private Button btnMoveCookedAssets;
    private Button btnScanAssets;
    private Label label1;
    private CheckBox chkRefreshAssets;
    private ComboBox cbUEVersion;
    private TabPage tbSettings;
    private Label label7;
    private ListBox lbAssetsToDelete;
    private CheckBox chkDummyWithProps;
    private Label label9;
    private ListBox lbDummyAssets;
    private Label label3;
    private ListBox lbAssetsToSkipSerialization;
    private Label label5;
    private Label label4;
    private RichTextBox rtxtCookedAssets;
    private RichTextBox rtxtSimpleAssets;
    private RichTextBox rtxtCircularDependancy;
    private Label label6;
    private TabPage tbNativSett;
    private Button btnDfltGamCnfg;
    private RichTextBox rtxtDfltGamCnfg;
    private RichTextBox rtxtCXXDir;
    private RichTextBox rtxtNativAssets;
    private Button btnCXXDir;
    private Label lbNativMode;
    private ComboBox cbNativMethod;
    private CheckBox chkUserEnumStruct;
    private Label lbNativAssets;
    private CheckBox chkSettDNS;
    private ContextMenuStrip cntxtTreeParse;
    private ToolStripMenuItem expandAllToolStripMenuItem;
    private ToolStripMenuItem collapseAllToolStripMenuItem;
    private ToolStripMenuItem refreshAllToolStripMenuItem;
    private ContextMenuStrip cntxtMainStrip;
    private ToolStripMenuItem clearAllPathsToolStripMenuItem;
    private ToolStripMenuItem restorePathsToDefaultsToolStripMenuItem;
    private ToolStripMenuItem pauseSerializationToolStripMenuItem;
    private ToolStripMenuItem cancelSerializationToolStripMenuItem;
    private ToolStripMenuItem restoreAllSettingsToDefaultthisTabToolStripMenuItem;
    private ToolStripMenuItem restoreAllSettingsToDefaultallTabsToolStripMenuItem;
}
