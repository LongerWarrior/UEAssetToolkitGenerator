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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbAuthors = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbRun = new System.Windows.Forms.TabPage();
            this.btnSelectDfltGamCnfg = new System.Windows.Forms.Button();
            this.rtxtDfltGamCnfg = new System.Windows.Forms.RichTextBox();
            this.rtxtAR = new System.Windows.Forms.RichTextBox();
            this.rtxtLogDir = new System.Windows.Forms.RichTextBox();
            this.chkSettDNS = new System.Windows.Forms.CheckBox();
            this.btnSelectAR = new System.Windows.Forms.Button();
            this.btnLogDir = new System.Windows.Forms.Button();
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.rtxtContentDir = new System.Windows.Forms.RichTextBox();
            this.rtxtJSONDir = new System.Windows.Forms.RichTextBox();
            this.chkAutoLoad = new System.Windows.Forms.CheckBox();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnSelectContentDir = new System.Windows.Forms.Button();
            this.btnOpenAllTypes = new System.Windows.Forms.Button();
            this.btnOpenAssetTypes = new System.Windows.Forms.Button();
            this.btnOpenLogs = new System.Windows.Forms.Button();
            this.btnSelectJSONDir = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSerializeAssets = new System.Windows.Forms.Button();
            this.btnScanAssets = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRefreshAssets = new System.Windows.Forms.CheckBox();
            this.cbUEVersion = new System.Windows.Forms.ComboBox();
            this.tbSerialSettings = new System.Windows.Forms.TabPage();
            this.rtxtSimpleAssets = new System.Windows.Forms.RichTextBox();
            this.chkDummyWithProps = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbDummyAssets = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAssetsToSkipSerialization = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rtxtCircularDependancy = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabCpyDlt = new System.Windows.Forms.TabPage();
            this.btnDeleteCookedAssets = new System.Windows.Forms.Button();
            this.btnCopyCookedAssets = new System.Windows.Forms.Button();
            this.rtxtMoveFrom = new System.Windows.Forms.RichTextBox();
            this.btnMoveFromDir = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbAssetsToDelete = new System.Windows.Forms.ListBox();
            this.btnMoveCookedAssets = new System.Windows.Forms.Button();
            this.rtxtMoveTo = new System.Windows.Forms.RichTextBox();
            this.btnSelectCookedDir = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rtxtCookedAssets = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.treeParseDir = new ExtendedTreeView.ExTreeView();
            this.cntxtTreeParse = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSerializeNatives = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.cntxtMainStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbRun.SuspendLayout();
            this.tbSerialSettings.SuspendLayout();
            this.tabCpyDlt.SuspendLayout();
            this.panel4.SuspendLayout();
            this.cntxtTreeParse.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.ContextMenuStrip = this.cntxtMainStrip;
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1406, 949);
            this.flowLayoutPanel1.TabIndex = 40;
            // 
            // cntxtMainStrip
            // 
            this.cntxtMainStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cntxtMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllPathsToolStripMenuItem,
            this.restorePathsToDefaultsToolStripMenuItem,
            this.pauseSerializationToolStripMenuItem,
            this.cancelSerializationToolStripMenuItem,
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem,
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem});
            this.cntxtMainStrip.Name = "cntxtMainStrip";
            this.cntxtMainStrip.Size = new System.Drawing.Size(336, 148);
            this.cntxtMainStrip.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMainStrip_Opening);
            // 
            // clearAllPathsToolStripMenuItem
            // 
            this.clearAllPathsToolStripMenuItem.Name = "clearAllPathsToolStripMenuItem";
            this.clearAllPathsToolStripMenuItem.Size = new System.Drawing.Size(335, 24);
            this.clearAllPathsToolStripMenuItem.Text = "Clear all paths";
            this.clearAllPathsToolStripMenuItem.Click += new System.EventHandler(this.clearAllPathsToolStripMenuItem_Click);
            // 
            // restorePathsToDefaultsToolStripMenuItem
            // 
            this.restorePathsToDefaultsToolStripMenuItem.Name = "restorePathsToDefaultsToolStripMenuItem";
            this.restorePathsToDefaultsToolStripMenuItem.Size = new System.Drawing.Size(335, 24);
            this.restorePathsToDefaultsToolStripMenuItem.Text = "Restore paths to defaults";
            this.restorePathsToDefaultsToolStripMenuItem.Click += new System.EventHandler(this.restorePathsToDefaultsToolStripMenuItem_Click);
            // 
            // pauseSerializationToolStripMenuItem
            // 
            this.pauseSerializationToolStripMenuItem.Name = "pauseSerializationToolStripMenuItem";
            this.pauseSerializationToolStripMenuItem.Size = new System.Drawing.Size(335, 24);
            this.pauseSerializationToolStripMenuItem.Text = "Pause Serialization";
            this.pauseSerializationToolStripMenuItem.Click += new System.EventHandler(this.cancelSerializationToolStripMenuItem_Click);
            // 
            // cancelSerializationToolStripMenuItem
            // 
            this.cancelSerializationToolStripMenuItem.Name = "cancelSerializationToolStripMenuItem";
            this.cancelSerializationToolStripMenuItem.Size = new System.Drawing.Size(335, 24);
            this.cancelSerializationToolStripMenuItem.Text = "Cancel Serialization";
            this.cancelSerializationToolStripMenuItem.Click += new System.EventHandler(this.cancelSerializationToolStripMenuItem_Click_1);
            // 
            // restoreAllSettingsToDefaultthisTabToolStripMenuItem
            // 
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Name = "restoreAllSettingsToDefaultthisTabToolStripMenuItem";
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Size = new System.Drawing.Size(335, 24);
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Text = "Restore all settings to default (this tab)";
            this.restoreAllSettingsToDefaultthisTabToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSettingsToDefaultthisTabToolStripMenuItem_Click);
            // 
            // restoreAllSettingsToDefaultallTabsToolStripMenuItem
            // 
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Name = "restoreAllSettingsToDefaultallTabsToolStripMenuItem";
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Size = new System.Drawing.Size(335, 24);
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Text = "Restore all settings to default (all tabs)";
            this.restoreAllSettingsToDefaultallTabsToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSettingsToDefaultallTabsToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.lbAuthors);
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1052, 941);
            this.panel2.TabIndex = 1;
            // 
            // lbAuthors
            // 
            this.lbAuthors.AutoSize = true;
            this.lbAuthors.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbAuthors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbAuthors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAuthors.Location = new System.Drawing.Point(350, 0);
            this.lbAuthors.Name = "lbAuthors";
            this.lbAuthors.Size = new System.Drawing.Size(702, 20);
            this.lbAuthors.TabIndex = 40;
            this.lbAuthors.Text = "Written by LongerWarrior, Buckminsterfullerene and Narknon. Based on CUE4Parse an" +
    "d UAssetAPI";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbRun);
            this.tabControl1.Controls.Add(this.tbSerialSettings);
            this.tabControl1.Controls.Add(this.tabCpyDlt);
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1046, 938);
            this.tabControl1.TabIndex = 39;
            // 
            // tbRun
            // 
            this.tbRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tbRun.Controls.Add(this.btnSerializeNatives);
            this.tbRun.Controls.Add(this.btnSelectDfltGamCnfg);
            this.tbRun.Controls.Add(this.rtxtDfltGamCnfg);
            this.tbRun.Controls.Add(this.rtxtAR);
            this.tbRun.Controls.Add(this.rtxtLogDir);
            this.tbRun.Controls.Add(this.chkSettDNS);
            this.tbRun.Controls.Add(this.btnSelectAR);
            this.tbRun.Controls.Add(this.btnLogDir);
            this.tbRun.Controls.Add(this.rtxtOutput);
            this.tbRun.Controls.Add(this.rtxtContentDir);
            this.tbRun.Controls.Add(this.rtxtJSONDir);
            this.tbRun.Controls.Add(this.chkAutoLoad);
            this.tbRun.Controls.Add(this.btnLoadConfig);
            this.tbRun.Controls.Add(this.btnSaveConfig);
            this.tbRun.Controls.Add(this.btnSelectContentDir);
            this.tbRun.Controls.Add(this.btnOpenAllTypes);
            this.tbRun.Controls.Add(this.btnOpenAssetTypes);
            this.tbRun.Controls.Add(this.btnOpenLogs);
            this.tbRun.Controls.Add(this.btnSelectJSONDir);
            this.tbRun.Controls.Add(this.lblProgress);
            this.tbRun.Controls.Add(this.btnClearLogs);
            this.tbRun.Controls.Add(this.btnSerializeAssets);
            this.tbRun.Controls.Add(this.btnScanAssets);
            this.tbRun.Controls.Add(this.label1);
            this.tbRun.Controls.Add(this.chkRefreshAssets);
            this.tbRun.Controls.Add(this.cbUEVersion);
            this.tbRun.Location = new System.Drawing.Point(4, 29);
            this.tbRun.Name = "tbRun";
            this.tbRun.Padding = new System.Windows.Forms.Padding(3);
            this.tbRun.Size = new System.Drawing.Size(1038, 905);
            this.tbRun.TabIndex = 0;
            this.tbRun.Text = "Run";
            // 
            // btnSelectDfltGamCnfg
            // 
            this.btnSelectDfltGamCnfg.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectDfltGamCnfg.FlatAppearance.BorderSize = 2;
            this.btnSelectDfltGamCnfg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectDfltGamCnfg.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectDfltGamCnfg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectDfltGamCnfg.Location = new System.Drawing.Point(15, 108);
            this.btnSelectDfltGamCnfg.Name = "btnSelectDfltGamCnfg";
            this.btnSelectDfltGamCnfg.Size = new System.Drawing.Size(139, 40);
            this.btnSelectDfltGamCnfg.TabIndex = 23;
            this.btnSelectDfltGamCnfg.Text = "Game .ini";
            this.btnSelectDfltGamCnfg.UseVisualStyleBackColor = true;
            this.btnSelectDfltGamCnfg.Click += new System.EventHandler(this.btnDfltGamCnfg_Click);
            // 
            // rtxtDfltGamCnfg
            // 
            this.rtxtDfltGamCnfg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtDfltGamCnfg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtDfltGamCnfg.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtDfltGamCnfg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtDfltGamCnfg.Location = new System.Drawing.Point(171, 108);
            this.rtxtDfltGamCnfg.Multiline = false;
            this.rtxtDfltGamCnfg.Name = "rtxtDfltGamCnfg";
            this.rtxtDfltGamCnfg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtDfltGamCnfg.Size = new System.Drawing.Size(857, 40);
            this.rtxtDfltGamCnfg.TabIndex = 24;
            this.rtxtDfltGamCnfg.Text = "C:\\ExamplePath\\DefaultGame.ini";
            this.rtxtDfltGamCnfg.TextChanged += new System.EventHandler(this.rtxtDfltGamCnfg_TextChanged);
            // 
            // rtxtAR
            // 
            this.rtxtAR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtAR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtAR.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtAR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtAR.Location = new System.Drawing.Point(171, 62);
            this.rtxtAR.Multiline = false;
            this.rtxtAR.Name = "rtxtAR";
            this.rtxtAR.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtAR.Size = new System.Drawing.Size(857, 40);
            this.rtxtAR.TabIndex = 43;
            this.rtxtAR.Text = "C:\\ExamplePath\\AssetRegistry.bin";
            // 
            // rtxtLogDir
            // 
            this.rtxtLogDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtLogDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtLogDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtLogDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtLogDir.Location = new System.Drawing.Point(171, 200);
            this.rtxtLogDir.Multiline = false;
            this.rtxtLogDir.Name = "rtxtLogDir";
            this.rtxtLogDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtLogDir.Size = new System.Drawing.Size(857, 40);
            this.rtxtLogDir.TabIndex = 37;
            this.rtxtLogDir.Text = "C:\\ExamplePath\\Logs";
            this.rtxtLogDir.Enter += new System.EventHandler(this.rtxtInfoDir_Enter);
            this.rtxtLogDir.Leave += new System.EventHandler(this.rtxtInfoDir_Leave);
            // 
            // chkSettDNS
            // 
            this.chkSettDNS.AutoSize = true;
            this.chkSettDNS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkSettDNS.Location = new System.Drawing.Point(15, 810);
            this.chkSettDNS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkSettDNS.Name = "chkSettDNS";
            this.chkSettDNS.Size = new System.Drawing.Size(372, 32);
            this.chkSettDNS.TabIndex = 41;
            this.chkSettDNS.Text = "Do Not Show Save Prompt on Close";
            this.chkSettDNS.UseVisualStyleBackColor = true;
            this.chkSettDNS.CheckedChanged += new System.EventHandler(this.chkSettDNS_CheckedChanged);
            // 
            // btnSelectAR
            // 
            this.btnSelectAR.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectAR.FlatAppearance.BorderSize = 2;
            this.btnSelectAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAR.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectAR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectAR.Location = new System.Drawing.Point(15, 62);
            this.btnSelectAR.Name = "btnSelectAR";
            this.btnSelectAR.Size = new System.Drawing.Size(139, 40);
            this.btnSelectAR.TabIndex = 42;
            this.btnSelectAR.Text = "Asset Reg";
            this.btnSelectAR.UseVisualStyleBackColor = true;
            this.btnSelectAR.Click += new System.EventHandler(this.btnAR_Click);
            // 
            // btnLogDir
            // 
            this.btnLogDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnLogDir.FlatAppearance.BorderSize = 2;
            this.btnLogDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnLogDir.Location = new System.Drawing.Point(15, 200);
            this.btnLogDir.Name = "btnLogDir";
            this.btnLogDir.Size = new System.Drawing.Size(139, 40);
            this.btnLogDir.TabIndex = 36;
            this.btnLogDir.Text = "Logs Dir";
            this.btnLogDir.UseVisualStyleBackColor = true;
            this.btnLogDir.Click += new System.EventHandler(this.btnInfoDir_Click);
            // 
            // rtxtOutput
            // 
            this.rtxtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtOutput.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtOutput.Location = new System.Drawing.Point(15, 344);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.ReadOnly = true;
            this.rtxtOutput.Size = new System.Drawing.Size(1013, 421);
            this.rtxtOutput.TabIndex = 23;
            this.rtxtOutput.Text = "";
            this.rtxtOutput.WordWrap = false;
            // 
            // rtxtContentDir
            // 
            this.rtxtContentDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtContentDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtContentDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtContentDir.Location = new System.Drawing.Point(171, 16);
            this.rtxtContentDir.Multiline = false;
            this.rtxtContentDir.Name = "rtxtContentDir";
            this.rtxtContentDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtContentDir.Size = new System.Drawing.Size(857, 40);
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
            this.rtxtJSONDir.Location = new System.Drawing.Point(171, 154);
            this.rtxtJSONDir.Multiline = false;
            this.rtxtJSONDir.Name = "rtxtJSONDir";
            this.rtxtJSONDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtJSONDir.Size = new System.Drawing.Size(857, 40);
            this.rtxtJSONDir.TabIndex = 3;
            this.rtxtJSONDir.Text = "C:\\ExamplePath\\AssetDump";
            this.rtxtJSONDir.Enter += new System.EventHandler(this.rtxtJSONDir_Enter);
            this.rtxtJSONDir.Leave += new System.EventHandler(this.rtxtJSONDir_Leave);
            // 
            // chkAutoLoad
            // 
            this.chkAutoLoad.AutoSize = true;
            this.chkAutoLoad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkAutoLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkAutoLoad.Location = new System.Drawing.Point(15, 771);
            this.chkAutoLoad.Name = "chkAutoLoad";
            this.chkAutoLoad.Size = new System.Drawing.Size(303, 32);
            this.chkAutoLoad.TabIndex = 39;
            this.chkAutoLoad.Text = "Auto Load Profile on Launch";
            this.chkAutoLoad.UseVisualStyleBackColor = true;
            this.chkAutoLoad.CheckedChanged += new System.EventHandler(this.chkAutoLoad_CheckedChanged);
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnLoadConfig.FlatAppearance.BorderSize = 2;
            this.btnLoadConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadConfig.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLoadConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnLoadConfig.Location = new System.Drawing.Point(487, 788);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(190, 40);
            this.btnLoadConfig.TabIndex = 29;
            this.btnLoadConfig.Text = "Load Profile";
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
            this.btnSaveConfig.Location = new System.Drawing.Point(487, 849);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(190, 40);
            this.btnSaveConfig.TabIndex = 30;
            this.btnSaveConfig.Text = "Save Profile";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnSelectContentDir
            // 
            this.btnSelectContentDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectContentDir.FlatAppearance.BorderSize = 2;
            this.btnSelectContentDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectContentDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectContentDir.Location = new System.Drawing.Point(15, 16);
            this.btnSelectContentDir.Name = "btnSelectContentDir";
            this.btnSelectContentDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectContentDir.TabIndex = 0;
            this.btnSelectContentDir.Text = "Content Dir";
            this.btnSelectContentDir.UseVisualStyleBackColor = true;
            this.btnSelectContentDir.Click += new System.EventHandler(this.btnSelectContentDir_Click);
            // 
            // btnOpenAllTypes
            // 
            this.btnOpenAllTypes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenAllTypes.FlatAppearance.BorderSize = 2;
            this.btnOpenAllTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAllTypes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAllTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenAllTypes.Location = new System.Drawing.Point(15, 849);
            this.btnOpenAllTypes.Name = "btnOpenAllTypes";
            this.btnOpenAllTypes.Size = new System.Drawing.Size(183, 40);
            this.btnOpenAllTypes.TabIndex = 25;
            this.btnOpenAllTypes.Text = "Open Asset List";
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
            this.btnOpenAssetTypes.Location = new System.Drawing.Point(225, 849);
            this.btnOpenAssetTypes.Name = "btnOpenAssetTypes";
            this.btnOpenAssetTypes.Size = new System.Drawing.Size(202, 40);
            this.btnOpenAssetTypes.TabIndex = 24;
            this.btnOpenAssetTypes.Text = "Open Asset Types";
            this.btnOpenAssetTypes.UseVisualStyleBackColor = true;
            this.btnOpenAssetTypes.Click += new System.EventHandler(this.btnOpenAssetTypes_Click);
            // 
            // btnOpenLogs
            // 
            this.btnOpenLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenLogs.FlatAppearance.BorderSize = 2;
            this.btnOpenLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLogs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenLogs.Location = new System.Drawing.Point(724, 849);
            this.btnOpenLogs.Name = "btnOpenLogs";
            this.btnOpenLogs.Size = new System.Drawing.Size(138, 40);
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
            this.btnSelectJSONDir.Location = new System.Drawing.Point(15, 154);
            this.btnSelectJSONDir.Name = "btnSelectJSONDir";
            this.btnSelectJSONDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectJSONDir.TabIndex = 2;
            this.btnSelectJSONDir.Text = "Result Dir";
            this.btnSelectJSONDir.UseVisualStyleBackColor = true;
            this.btnSelectJSONDir.Click += new System.EventHandler(this.btnSelectJSONDir_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lblProgress.Location = new System.Drawing.Point(678, 293);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(283, 40);
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
            this.btnClearLogs.Location = new System.Drawing.Point(890, 849);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(138, 40);
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
            this.btnSerializeAssets.Location = new System.Drawing.Point(698, 246);
            this.btnSerializeAssets.Name = "btnSerializeAssets";
            this.btnSerializeAssets.Size = new System.Drawing.Size(246, 40);
            this.btnSerializeAssets.TabIndex = 18;
            this.btnSerializeAssets.Text = "Serialize Assets";
            this.btnSerializeAssets.UseVisualStyleBackColor = true;
            this.btnSerializeAssets.Click += new System.EventHandler(this.btnSerializeAssets_Click);
            // 
            // btnScanAssets
            // 
            this.btnScanAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnScanAssets.FlatAppearance.BorderSize = 2;
            this.btnScanAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnScanAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnScanAssets.Location = new System.Drawing.Point(403, 246);
            this.btnScanAssets.Name = "btnScanAssets";
            this.btnScanAssets.Size = new System.Drawing.Size(246, 40);
            this.btnScanAssets.TabIndex = 16;
            this.btnScanAssets.Text = "Scan Asset Registry";
            this.btnScanAssets.UseVisualStyleBackColor = true;
            this.btnScanAssets.Click += new System.EventHandler(this.btnScanAssets_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label1.Location = new System.Drawing.Point(27, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 28);
            this.label1.TabIndex = 6;
            this.label1.Text = "UE Version:";
            // 
            // chkRefreshAssets
            // 
            this.chkRefreshAssets.AutoSize = true;
            this.chkRefreshAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkRefreshAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkRefreshAssets.Location = new System.Drawing.Point(177, 301);
            this.chkRefreshAssets.Name = "chkRefreshAssets";
            this.chkRefreshAssets.Size = new System.Drawing.Size(172, 32);
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
            this.cbUEVersion.Location = new System.Drawing.Point(171, 246);
            this.cbUEVersion.Name = "cbUEVersion";
            this.cbUEVersion.Size = new System.Drawing.Size(178, 36);
            this.cbUEVersion.TabIndex = 7;
            // 
            // tbSerialSettings
            // 
            this.tbSerialSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tbSerialSettings.Controls.Add(this.rtxtSimpleAssets);
            this.tbSerialSettings.Controls.Add(this.chkDummyWithProps);
            this.tbSerialSettings.Controls.Add(this.label9);
            this.tbSerialSettings.Controls.Add(this.lbDummyAssets);
            this.tbSerialSettings.Controls.Add(this.label3);
            this.tbSerialSettings.Controls.Add(this.lbAssetsToSkipSerialization);
            this.tbSerialSettings.Controls.Add(this.label5);
            this.tbSerialSettings.Controls.Add(this.rtxtCircularDependancy);
            this.tbSerialSettings.Controls.Add(this.label6);
            this.tbSerialSettings.Location = new System.Drawing.Point(4, 29);
            this.tbSerialSettings.Name = "tbSerialSettings";
            this.tbSerialSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tbSerialSettings.Size = new System.Drawing.Size(1038, 905);
            this.tbSerialSettings.TabIndex = 1;
            this.tbSerialSettings.Text = "Serialization Settings";
            // 
            // rtxtSimpleAssets
            // 
            this.rtxtSimpleAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtSimpleAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtSimpleAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtSimpleAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtSimpleAssets.Location = new System.Drawing.Point(16, 46);
            this.rtxtSimpleAssets.Name = "rtxtSimpleAssets";
            this.rtxtSimpleAssets.Size = new System.Drawing.Size(514, 392);
            this.rtxtSimpleAssets.TabIndex = 15;
            this.rtxtSimpleAssets.Text = resources.GetString("rtxtSimpleAssets.Text");
            this.rtxtSimpleAssets.WordWrap = false;
            // 
            // chkDummyWithProps
            // 
            this.chkDummyWithProps.AutoSize = true;
            this.chkDummyWithProps.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkDummyWithProps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkDummyWithProps.Location = new System.Drawing.Point(673, 852);
            this.chkDummyWithProps.Name = "chkDummyWithProps";
            this.chkDummyWithProps.Size = new System.Drawing.Size(264, 32);
            this.chkDummyWithProps.TabIndex = 38;
            this.chkDummyWithProps.Text = "Dummy With Properties";
            this.chkDummyWithProps.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label9.Location = new System.Drawing.Point(640, 460);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(326, 38);
            this.label9.TabIndex = 34;
            this.label9.Text = "Assets Types to dummy";
            // 
            // lbDummyAssets
            // 
            this.lbDummyAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbDummyAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbDummyAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lbDummyAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbDummyAssets.FormattingEnabled = true;
            this.lbDummyAssets.ItemHeight = 28;
            this.lbDummyAssets.Location = new System.Drawing.Point(555, 503);
            this.lbDummyAssets.Name = "lbDummyAssets";
            this.lbDummyAssets.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbDummyAssets.Size = new System.Drawing.Size(458, 336);
            this.lbDummyAssets.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label3.Location = new System.Drawing.Point(581, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(418, 38);
            this.label3.TabIndex = 10;
            this.label3.Text = "Assets Types to skip serializing";
            // 
            // lbAssetsToSkipSerialization
            // 
            this.lbAssetsToSkipSerialization.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbAssetsToSkipSerialization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAssetsToSkipSerialization.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lbAssetsToSkipSerialization.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAssetsToSkipSerialization.FormattingEnabled = true;
            this.lbAssetsToSkipSerialization.ItemHeight = 28;
            this.lbAssetsToSkipSerialization.Location = new System.Drawing.Point(555, 46);
            this.lbAssetsToSkipSerialization.Name = "lbAssetsToSkipSerialization";
            this.lbAssetsToSkipSerialization.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToSkipSerialization.Size = new System.Drawing.Size(458, 392);
            this.lbAssetsToSkipSerialization.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label5.Location = new System.Drawing.Point(178, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 38);
            this.label5.TabIndex = 14;
            this.label5.Text = "Simple assets";
            // 
            // rtxtCircularDependancy
            // 
            this.rtxtCircularDependancy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCircularDependancy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCircularDependancy.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCircularDependancy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCircularDependancy.Location = new System.Drawing.Point(16, 503);
            this.rtxtCircularDependancy.Name = "rtxtCircularDependancy";
            this.rtxtCircularDependancy.Size = new System.Drawing.Size(514, 381);
            this.rtxtCircularDependancy.TabIndex = 20;
            this.rtxtCircularDependancy.Text = "/Script/Engine.SoundClass\n/Script/Engine.SoundSubmix\n/Script/Engine.EndpointSubmi" +
    "x";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label6.Location = new System.Drawing.Point(62, 460);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(458, 38);
            this.label6.TabIndex = 19;
            this.label6.Text = "Assets with a circular dependancy";
            // 
            // tabCpyDlt
            // 
            this.tabCpyDlt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tabCpyDlt.Controls.Add(this.btnDeleteCookedAssets);
            this.tabCpyDlt.Controls.Add(this.btnCopyCookedAssets);
            this.tabCpyDlt.Controls.Add(this.rtxtMoveFrom);
            this.tabCpyDlt.Controls.Add(this.btnMoveFromDir);
            this.tabCpyDlt.Controls.Add(this.label8);
            this.tabCpyDlt.Controls.Add(this.label7);
            this.tabCpyDlt.Controls.Add(this.lbAssetsToDelete);
            this.tabCpyDlt.Controls.Add(this.btnMoveCookedAssets);
            this.tabCpyDlt.Controls.Add(this.rtxtMoveTo);
            this.tabCpyDlt.Controls.Add(this.btnSelectCookedDir);
            this.tabCpyDlt.Controls.Add(this.label4);
            this.tabCpyDlt.Controls.Add(this.rtxtCookedAssets);
            this.tabCpyDlt.Location = new System.Drawing.Point(4, 29);
            this.tabCpyDlt.Name = "tabCpyDlt";
            this.tabCpyDlt.Padding = new System.Windows.Forms.Padding(3);
            this.tabCpyDlt.Size = new System.Drawing.Size(1038, 905);
            this.tabCpyDlt.TabIndex = 4;
            this.tabCpyDlt.Text = "Asset Utilities";
            // 
            // btnDeleteCookedAssets
            // 
            this.btnDeleteCookedAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnDeleteCookedAssets.FlatAppearance.BorderSize = 2;
            this.btnDeleteCookedAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCookedAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDeleteCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnDeleteCookedAssets.Location = new System.Drawing.Point(754, 788);
            this.btnDeleteCookedAssets.Name = "btnDeleteCookedAssets";
            this.btnDeleteCookedAssets.Size = new System.Drawing.Size(226, 40);
            this.btnDeleteCookedAssets.TabIndex = 45;
            this.btnDeleteCookedAssets.Text = "Delete";
            this.btnDeleteCookedAssets.UseVisualStyleBackColor = true;
            // 
            // btnCopyCookedAssets
            // 
            this.btnCopyCookedAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnCopyCookedAssets.FlatAppearance.BorderSize = 2;
            this.btnCopyCookedAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyCookedAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCopyCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnCopyCookedAssets.Location = new System.Drawing.Point(410, 788);
            this.btnCopyCookedAssets.Name = "btnCopyCookedAssets";
            this.btnCopyCookedAssets.Size = new System.Drawing.Size(226, 40);
            this.btnCopyCookedAssets.TabIndex = 44;
            this.btnCopyCookedAssets.Text = "Copy";
            this.btnCopyCookedAssets.UseVisualStyleBackColor = true;
            // 
            // rtxtMoveFrom
            // 
            this.rtxtMoveFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtMoveFrom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtMoveFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtMoveFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtMoveFrom.Location = new System.Drawing.Point(220, 104);
            this.rtxtMoveFrom.Multiline = false;
            this.rtxtMoveFrom.Name = "rtxtMoveFrom";
            this.rtxtMoveFrom.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtMoveFrom.Size = new System.Drawing.Size(760, 40);
            this.rtxtMoveFrom.TabIndex = 43;
            this.rtxtMoveFrom.Text = "C:\\ExamplePath\\OriginalDir";
            // 
            // btnMoveFromDir
            // 
            this.btnMoveFromDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnMoveFromDir.FlatAppearance.BorderSize = 2;
            this.btnMoveFromDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveFromDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnMoveFromDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnMoveFromDir.Location = new System.Drawing.Point(59, 104);
            this.btnMoveFromDir.Name = "btnMoveFromDir";
            this.btnMoveFromDir.Size = new System.Drawing.Size(139, 40);
            this.btnMoveFromDir.TabIndex = 42;
            this.btnMoveFromDir.Text = "Move From Dir";
            this.btnMoveFromDir.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label8.Location = new System.Drawing.Point(133, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(290, 38);
            this.label8.TabIndex = 41;
            this.label8.Text = "Asset types to delete";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label7.Location = new System.Drawing.Point(334, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(398, 38);
            this.label7.TabIndex = 39;
            this.label7.Text = "Copy, Delete, or Move Assets";
            // 
            // lbAssetsToDelete
            // 
            this.lbAssetsToDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbAssetsToDelete.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAssetsToDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.lbAssetsToDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAssetsToDelete.FormattingEnabled = true;
            this.lbAssetsToDelete.ItemHeight = 28;
            this.lbAssetsToDelete.Location = new System.Drawing.Point(59, 267);
            this.lbAssetsToDelete.Name = "lbAssetsToDelete";
            this.lbAssetsToDelete.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToDelete.Size = new System.Drawing.Size(446, 504);
            this.lbAssetsToDelete.TabIndex = 40;
            // 
            // btnMoveCookedAssets
            // 
            this.btnMoveCookedAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnMoveCookedAssets.FlatAppearance.BorderSize = 2;
            this.btnMoveCookedAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveCookedAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnMoveCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnMoveCookedAssets.Location = new System.Drawing.Point(59, 788);
            this.btnMoveCookedAssets.Name = "btnMoveCookedAssets";
            this.btnMoveCookedAssets.Size = new System.Drawing.Size(226, 40);
            this.btnMoveCookedAssets.TabIndex = 17;
            this.btnMoveCookedAssets.Text = "Move";
            this.btnMoveCookedAssets.UseVisualStyleBackColor = true;
            this.btnMoveCookedAssets.Click += new System.EventHandler(this.btnMoveCookedAssets_Click);
            // 
            // rtxtMoveTo
            // 
            this.rtxtMoveTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtMoveTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtMoveTo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtMoveTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtMoveTo.Location = new System.Drawing.Point(220, 163);
            this.rtxtMoveTo.Multiline = false;
            this.rtxtMoveTo.Name = "rtxtMoveTo";
            this.rtxtMoveTo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtMoveTo.Size = new System.Drawing.Size(760, 40);
            this.rtxtMoveTo.TabIndex = 5;
            this.rtxtMoveTo.Text = "C:\\ExamplePath\\MoveToDir";
            this.rtxtMoveTo.Enter += new System.EventHandler(this.rtxtCookedDir_Enter);
            this.rtxtMoveTo.Leave += new System.EventHandler(this.rtxtCookedDir_Leave);
            // 
            // btnSelectCookedDir
            // 
            this.btnSelectCookedDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectCookedDir.FlatAppearance.BorderSize = 2;
            this.btnSelectCookedDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCookedDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectCookedDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectCookedDir.Location = new System.Drawing.Point(59, 163);
            this.btnSelectCookedDir.Name = "btnSelectCookedDir";
            this.btnSelectCookedDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectCookedDir.TabIndex = 4;
            this.btnSelectCookedDir.Text = "Move To Dir";
            this.btnSelectCookedDir.UseVisualStyleBackColor = true;
            this.btnSelectCookedDir.Click += new System.EventHandler(this.btnSelectOutputDir_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label4.Location = new System.Drawing.Point(573, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(373, 38);
            this.label4.TabIndex = 12;
            this.label4.Text = "Cooked asset types to copy";
            // 
            // rtxtCookedAssets
            // 
            this.rtxtCookedAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCookedAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCookedAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCookedAssets.Location = new System.Drawing.Point(534, 267);
            this.rtxtCookedAssets.Name = "rtxtCookedAssets";
            this.rtxtCookedAssets.Size = new System.Drawing.Size(446, 504);
            this.rtxtCookedAssets.TabIndex = 13;
            this.rtxtCookedAssets.Text = resources.GetString("rtxtCookedAssets.Text");
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button4);
            this.panel4.Controls.Add(this.treeParseDir);
            this.panel4.Location = new System.Drawing.Point(1061, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 938);
            this.panel4.TabIndex = 3;
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.button4.FlatAppearance.BorderSize = 2;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.button4.Location = new System.Drawing.Point(123, 898);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 40);
            this.button4.TabIndex = 46;
            this.button4.Text = "Refresh";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // treeParseDir
            // 
            this.treeParseDir.CheckBoxes = true;
            this.treeParseDir.ContextMenuStrip = this.cntxtTreeParse;
            this.treeParseDir.Location = new System.Drawing.Point(3, 29);
            this.treeParseDir.Margin = new System.Windows.Forms.Padding(3, 4, 0, 4);
            this.treeParseDir.Name = "treeParseDir";
            this.treeParseDir.ShowNodeToolTips = true;
            this.treeParseDir.Size = new System.Drawing.Size(339, 862);
            this.treeParseDir.TabIndex = 2;
            this.treeParseDir.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeParseDir_AfterCheck);
            this.treeParseDir.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeParseDir_BeforeExpand);
            this.treeParseDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeParseDir_KeyDown);
            this.treeParseDir.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeParseDir_MouseDown);
            this.treeParseDir.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeParseDir_MouseMove);
            // 
            // cntxtTreeParse
            // 
            this.cntxtTreeParse.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cntxtTreeParse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.copyPathToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.refreshAllToolStripMenuItem});
            this.cntxtTreeParse.Name = "cntxtTreeParse";
            this.cntxtTreeParse.Size = new System.Drawing.Size(147, 124);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // copyPathToolStripMenuItem
            // 
            this.copyPathToolStripMenuItem.Name = "copyPathToolStripMenuItem";
            this.copyPathToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.copyPathToolStripMenuItem.Text = "&Copy path";
            this.copyPathToolStripMenuItem.Click += new System.EventHandler(this.copyPathToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.expandAllToolStripMenuItem.Text = "&Expand";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.collapseAllToolStripMenuItem.Text = "Collap&se";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // refreshAllToolStripMenuItem
            // 
            this.refreshAllToolStripMenuItem.Name = "refreshAllToolStripMenuItem";
            this.refreshAllToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.refreshAllToolStripMenuItem.Text = "&Refresh";
            this.refreshAllToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // btnSerializeNatives
            // 
            this.btnSerializeNatives.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSerializeNatives.FlatAppearance.BorderSize = 2;
            this.btnSerializeNatives.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerializeNatives.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSerializeNatives.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSerializeNatives.Location = new System.Drawing.Point(403, 293);
            this.btnSerializeNatives.Name = "btnSerializeNatives";
            this.btnSerializeNatives.Size = new System.Drawing.Size(246, 40);
            this.btnSerializeNatives.TabIndex = 44;
            this.btnSerializeNatives.Text = "Serialize Native Assets";
            this.btnSerializeNatives.UseVisualStyleBackColor = true;
            this.btnSerializeNatives.Click += new System.EventHandler(this.btnSerializeNatives_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1418, 955);
            this.Controls.Add(this.flowLayoutPanel1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cooked Asset Serializer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.cntxtMainStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tbRun.ResumeLayout(false);
            this.tbRun.PerformLayout();
            this.tbSerialSettings.ResumeLayout(false);
            this.tbSerialSettings.PerformLayout();
            this.tabCpyDlt.ResumeLayout(false);
            this.tabCpyDlt.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.cntxtTreeParse.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private ToolTip tTipTree;
    private FlowLayoutPanel flowLayoutPanel1;
    private ExTreeView treeParseDir;
    private Panel panel2;
    private Label lbAuthors;
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
    private ToolStripMenuItem copyPathToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private Panel panel4;
    private TabControl tabControl1;
    private TabPage tbRun;
    private RichTextBox rtxtLogDir;
    private RichTextBox rtxtOutput;
    private RichTextBox rtxtContentDir;
    private RichTextBox rtxtJSONDir;
    private RichTextBox rtxtMoveTo;
    private CheckBox chkAutoLoad;
    private Button btnLoadConfig;
    private Button btnSaveConfig;
    private Button btnSelectContentDir;
    private Button btnLogDir;
    private Button btnOpenAllTypes;
    private Button btnOpenAssetTypes;
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
    private TabPage tbSerialSettings;
    private CheckBox chkSettDNS;
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
    private Button btnSelectDfltGamCnfg;
    private RichTextBox rtxtDfltGamCnfg;
    private TabPage tabCpyDlt;
    private Label label8;
    private Button btnDeleteCookedAssets;
    private Button btnCopyCookedAssets;
    private RichTextBox rtxtMoveFrom;
    private Button btnMoveFromDir;
    private Button button4;
    private RichTextBox rtxtAR;
    private Button btnSelectAR;
    private Button btnSerializeNatives;
}
