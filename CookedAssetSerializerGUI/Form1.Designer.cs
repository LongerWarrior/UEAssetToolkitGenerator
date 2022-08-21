namespace CookedAssetSerializerGUI;

partial class Form1 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSelectContentDir = new System.Windows.Forms.Button();
            this.rtxtContentDir = new System.Windows.Forms.RichTextBox();
            this.rtxtJSONDir = new System.Windows.Forms.RichTextBox();
            this.btnSelectJSONDir = new System.Windows.Forms.Button();
            this.rtxtOutputDir = new System.Windows.Forms.RichTextBox();
            this.btnSelectOutputDir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUEVersion = new System.Windows.Forms.ComboBox();
            this.chkRefreshAssets = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAssetsToSkipSerialization = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rtxtCookedAssets = new System.Windows.Forms.RichTextBox();
            this.rtxtSimpleAssets = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnScanAssets = new System.Windows.Forms.Button();
            this.btnMoveCookedAssets = new System.Windows.Forms.Button();
            this.btnSerializeAssets = new System.Windows.Forms.Button();
            this.rtxtCircularDependancy = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.btnOpenAssetTypes = new System.Windows.Forms.Button();
            this.btnOpenAllTypes = new System.Windows.Forms.Button();
            this.btnOpenLogs = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.rtxtParseDir = new System.Windows.Forms.RichTextBox();
            this.btnSelectParseDir = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbRun = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxtInfoDir = new System.Windows.Forms.RichTextBox();
            this.btnInfoDir = new System.Windows.Forms.Button();
            this.tbSettings = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.lbAssetsToDelete = new System.Windows.Forms.ListBox();
            this.chkDummyWithProps = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbDummyAssets = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tbRun.SuspendLayout();
            this.tbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectContentDir
            // 
            this.btnSelectContentDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectContentDir.FlatAppearance.BorderSize = 2;
            this.btnSelectContentDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectContentDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectContentDir.Location = new System.Drawing.Point(15, 18);
            this.btnSelectContentDir.Name = "btnSelectContentDir";
            this.btnSelectContentDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectContentDir.TabIndex = 0;
            this.btnSelectContentDir.Text = "Content Dir";
            this.btnSelectContentDir.UseVisualStyleBackColor = true;
            this.btnSelectContentDir.Click += new System.EventHandler(this.btnSelectContentDir_Click);
            // 
            // rtxtContentDir
            // 
            this.rtxtContentDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtContentDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtContentDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtContentDir.Location = new System.Drawing.Point(171, 18);
            this.rtxtContentDir.Multiline = false;
            this.rtxtContentDir.Name = "rtxtContentDir";
            this.rtxtContentDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtContentDir.Size = new System.Drawing.Size(899, 40);
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
            this.rtxtJSONDir.Location = new System.Drawing.Point(171, 114);
            this.rtxtJSONDir.Multiline = false;
            this.rtxtJSONDir.Name = "rtxtJSONDir";
            this.rtxtJSONDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtJSONDir.Size = new System.Drawing.Size(899, 40);
            this.rtxtJSONDir.TabIndex = 3;
            this.rtxtJSONDir.Text = "C:\\ExamplePath\\AssetDump";
            this.rtxtJSONDir.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateDir);
            // 
            // btnSelectJSONDir
            // 
            this.btnSelectJSONDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectJSONDir.FlatAppearance.BorderSize = 2;
            this.btnSelectJSONDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectJSONDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectJSONDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectJSONDir.Location = new System.Drawing.Point(15, 114);
            this.btnSelectJSONDir.Name = "btnSelectJSONDir";
            this.btnSelectJSONDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectJSONDir.TabIndex = 2;
            this.btnSelectJSONDir.Text = "Dump Dir";
            this.btnSelectJSONDir.UseVisualStyleBackColor = true;
            this.btnSelectJSONDir.Click += new System.EventHandler(this.btnSelectJSONDir_Click);
            // 
            // rtxtOutputDir
            // 
            this.rtxtOutputDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtOutputDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtOutputDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtOutputDir.Location = new System.Drawing.Point(171, 162);
            this.rtxtOutputDir.Multiline = false;
            this.rtxtOutputDir.Name = "rtxtOutputDir";
            this.rtxtOutputDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtOutputDir.Size = new System.Drawing.Size(899, 40);
            this.rtxtOutputDir.TabIndex = 5;
            this.rtxtOutputDir.Text = "C:\\ExamplePath\\Cooked";
            this.rtxtOutputDir.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateDir);
            // 
            // btnSelectOutputDir
            // 
            this.btnSelectOutputDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectOutputDir.FlatAppearance.BorderSize = 2;
            this.btnSelectOutputDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectOutputDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectOutputDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectOutputDir.Location = new System.Drawing.Point(15, 162);
            this.btnSelectOutputDir.Name = "btnSelectOutputDir";
            this.btnSelectOutputDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectOutputDir.TabIndex = 4;
            this.btnSelectOutputDir.Text = "Cooked Dir";
            this.btnSelectOutputDir.UseVisualStyleBackColor = true;
            this.btnSelectOutputDir.Click += new System.EventHandler(this.btnSelectOutputDir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label1.Location = new System.Drawing.Point(36, 269);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 28);
            this.label1.TabIndex = 6;
            this.label1.Text = "UE Version:";
            // 
            // cbUEVersion
            // 
            this.cbUEVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.cbUEVersion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbUEVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbUEVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.cbUEVersion.FormattingEnabled = true;
            this.cbUEVersion.Location = new System.Drawing.Point(171, 267);
            this.cbUEVersion.Name = "cbUEVersion";
            this.cbUEVersion.Size = new System.Drawing.Size(178, 36);
            this.cbUEVersion.TabIndex = 7;
            // 
            // chkRefreshAssets
            // 
            this.chkRefreshAssets.AutoSize = true;
            this.chkRefreshAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkRefreshAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkRefreshAssets.Location = new System.Drawing.Point(387, 269);
            this.chkRefreshAssets.Name = "chkRefreshAssets";
            this.chkRefreshAssets.Size = new System.Drawing.Size(172, 32);
            this.chkRefreshAssets.TabIndex = 8;
            this.chkRefreshAssets.Text = "Refresh Assets";
            this.chkRefreshAssets.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label3.Location = new System.Drawing.Point(710, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 38);
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
            this.lbAssetsToSkipSerialization.ItemHeight = 28;
            this.lbAssetsToSkipSerialization.Location = new System.Drawing.Point(667, 44);
            this.lbAssetsToSkipSerialization.Name = "lbAssetsToSkipSerialization";
            this.lbAssetsToSkipSerialization.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToSkipSerialization.Size = new System.Drawing.Size(404, 252);
            this.lbAssetsToSkipSerialization.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label4.Location = new System.Drawing.Point(164, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 38);
            this.label4.TabIndex = 12;
            this.label4.Text = "Cooked assets to copy";
            // 
            // rtxtCookedAssets
            // 
            this.rtxtCookedAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCookedAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCookedAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCookedAssets.Location = new System.Drawing.Point(17, 351);
            this.rtxtCookedAssets.Name = "rtxtCookedAssets";
            this.rtxtCookedAssets.Size = new System.Drawing.Size(620, 252);
            this.rtxtCookedAssets.TabIndex = 13;
            this.rtxtCookedAssets.Text = resources.GetString("rtxtCookedAssets.Text");
            // 
            // rtxtSimpleAssets
            // 
            this.rtxtSimpleAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtSimpleAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtSimpleAssets.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtSimpleAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtSimpleAssets.Location = new System.Drawing.Point(16, 44);
            this.rtxtSimpleAssets.Name = "rtxtSimpleAssets";
            this.rtxtSimpleAssets.Size = new System.Drawing.Size(621, 252);
            this.rtxtSimpleAssets.TabIndex = 15;
            this.rtxtSimpleAssets.Text = resources.GetString("rtxtSimpleAssets.Text");
            this.rtxtSimpleAssets.WordWrap = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label5.Location = new System.Drawing.Point(215, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 38);
            this.label5.TabIndex = 14;
            this.label5.Text = "Simple assets";
            // 
            // btnScanAssets
            // 
            this.btnScanAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnScanAssets.FlatAppearance.BorderSize = 2;
            this.btnScanAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnScanAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnScanAssets.Location = new System.Drawing.Point(13, 329);
            this.btnScanAssets.Name = "btnScanAssets";
            this.btnScanAssets.Size = new System.Drawing.Size(226, 40);
            this.btnScanAssets.TabIndex = 16;
            this.btnScanAssets.Text = "Scan Assets";
            this.btnScanAssets.UseVisualStyleBackColor = true;
            this.btnScanAssets.Click += new System.EventHandler(this.btnScanAssets_Click);
            // 
            // btnMoveCookedAssets
            // 
            this.btnMoveCookedAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnMoveCookedAssets.FlatAppearance.BorderSize = 2;
            this.btnMoveCookedAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveCookedAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnMoveCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnMoveCookedAssets.Location = new System.Drawing.Point(272, 329);
            this.btnMoveCookedAssets.Name = "btnMoveCookedAssets";
            this.btnMoveCookedAssets.Size = new System.Drawing.Size(226, 40);
            this.btnMoveCookedAssets.TabIndex = 17;
            this.btnMoveCookedAssets.Text = "Move Cooked Assets";
            this.btnMoveCookedAssets.UseVisualStyleBackColor = true;
            this.btnMoveCookedAssets.Click += new System.EventHandler(this.btnMoveCookedAssets_Click);
            // 
            // btnSerializeAssets
            // 
            this.btnSerializeAssets.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSerializeAssets.FlatAppearance.BorderSize = 2;
            this.btnSerializeAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerializeAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSerializeAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSerializeAssets.Location = new System.Drawing.Point(535, 329);
            this.btnSerializeAssets.Name = "btnSerializeAssets";
            this.btnSerializeAssets.Size = new System.Drawing.Size(226, 40);
            this.btnSerializeAssets.TabIndex = 18;
            this.btnSerializeAssets.Text = "Serialize Assets";
            this.btnSerializeAssets.UseVisualStyleBackColor = true;
            this.btnSerializeAssets.Click += new System.EventHandler(this.btnSerializeAssets_Click);
            // 
            // rtxtCircularDependancy
            // 
            this.rtxtCircularDependancy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCircularDependancy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCircularDependancy.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtCircularDependancy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCircularDependancy.Location = new System.Drawing.Point(19, 689);
            this.rtxtCircularDependancy.Name = "rtxtCircularDependancy";
            this.rtxtCircularDependancy.Size = new System.Drawing.Size(621, 196);
            this.rtxtCircularDependancy.TabIndex = 20;
            this.rtxtCircularDependancy.Text = "/Script/Engine.SoundClass\n/Script/Engine.SoundSubmix\n/Script/Engine.EndpointSubmi" +
    "x";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label6.Location = new System.Drawing.Point(94, 644);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(458, 38);
            this.label6.TabIndex = 19;
            this.label6.Text = "Assets with a circular dependancy";
            // 
            // rtxtOutput
            // 
            this.rtxtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtOutput.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtOutput.Location = new System.Drawing.Point(17, 425);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.ReadOnly = true;
            this.rtxtOutput.Size = new System.Drawing.Size(1057, 409);
            this.rtxtOutput.TabIndex = 23;
            this.rtxtOutput.Text = "";
            this.rtxtOutput.WordWrap = false;
            // 
            // btnOpenAssetTypes
            // 
            this.btnOpenAssetTypes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenAssetTypes.FlatAppearance.BorderSize = 2;
            this.btnOpenAssetTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAssetTypes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAssetTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenAssetTypes.Location = new System.Drawing.Point(224, 849);
            this.btnOpenAssetTypes.Name = "btnOpenAssetTypes";
            this.btnOpenAssetTypes.Size = new System.Drawing.Size(202, 40);
            this.btnOpenAssetTypes.TabIndex = 24;
            this.btnOpenAssetTypes.Text = "Open AssetTypes";
            this.btnOpenAssetTypes.UseVisualStyleBackColor = true;
            this.btnOpenAssetTypes.Click += new System.EventHandler(this.btnOpenAssetTypes_Click);
            // 
            // btnOpenAllTypes
            // 
            this.btnOpenAllTypes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenAllTypes.FlatAppearance.BorderSize = 2;
            this.btnOpenAllTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAllTypes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAllTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenAllTypes.Location = new System.Drawing.Point(17, 849);
            this.btnOpenAllTypes.Name = "btnOpenAllTypes";
            this.btnOpenAllTypes.Size = new System.Drawing.Size(183, 40);
            this.btnOpenAllTypes.TabIndex = 25;
            this.btnOpenAllTypes.Text = "Open AllTypes";
            this.btnOpenAllTypes.UseVisualStyleBackColor = true;
            this.btnOpenAllTypes.Click += new System.EventHandler(this.btnOpenAllTypes_Click);
            // 
            // btnOpenLogs
            // 
            this.btnOpenLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenLogs.FlatAppearance.BorderSize = 2;
            this.btnOpenLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLogs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenLogs.Location = new System.Drawing.Point(773, 849);
            this.btnOpenLogs.Name = "btnOpenLogs";
            this.btnOpenLogs.Size = new System.Drawing.Size(138, 40);
            this.btnOpenLogs.TabIndex = 26;
            this.btnOpenLogs.Text = "Open Logs";
            this.btnOpenLogs.UseVisualStyleBackColor = true;
            this.btnOpenLogs.Click += new System.EventHandler(this.btnOpenLogs_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnClearLogs.FlatAppearance.BorderSize = 2;
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClearLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnClearLogs.Location = new System.Drawing.Point(936, 849);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(138, 40);
            this.btnClearLogs.TabIndex = 28;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnLoadConfig.FlatAppearance.BorderSize = 2;
            this.btnLoadConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadConfig.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLoadConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnLoadConfig.Location = new System.Drawing.Point(584, 263);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(226, 40);
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
            this.btnSaveConfig.Location = new System.Drawing.Point(845, 263);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(226, 40);
            this.btnSaveConfig.TabIndex = 30;
            this.btnSaveConfig.Text = "Save Config Settings";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lblProgress.Location = new System.Drawing.Point(804, 329);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(266, 40);
            this.lblProgress.TabIndex = 31;
            this.lblProgress.Text = "0/0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtxtParseDir
            // 
            this.rtxtParseDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtParseDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtParseDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtParseDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtParseDir.Location = new System.Drawing.Point(171, 66);
            this.rtxtParseDir.Multiline = false;
            this.rtxtParseDir.Name = "rtxtParseDir";
            this.rtxtParseDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtParseDir.Size = new System.Drawing.Size(899, 40);
            this.rtxtParseDir.TabIndex = 33;
            this.rtxtParseDir.Text = "C:\\ExamplePath\\Content\\Data";
            this.rtxtParseDir.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateParseDir);
            // 
            // btnSelectParseDir
            // 
            this.btnSelectParseDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectParseDir.FlatAppearance.BorderSize = 2;
            this.btnSelectParseDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectParseDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectParseDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectParseDir.Location = new System.Drawing.Point(15, 66);
            this.btnSelectParseDir.Name = "btnSelectParseDir";
            this.btnSelectParseDir.Size = new System.Drawing.Size(139, 40);
            this.btnSelectParseDir.TabIndex = 32;
            this.btnSelectParseDir.Text = "Parse Dir";
            this.btnSelectParseDir.UseVisualStyleBackColor = true;
            this.btnSelectParseDir.Click += new System.EventHandler(this.btnSelectParseDir_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbRun);
            this.tabControl1.Controls.Add(this.tbSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1102, 942);
            this.tabControl1.TabIndex = 34;
            // 
            // tbRun
            // 
            this.tbRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.tbRun.Controls.Add(this.label2);
            this.tbRun.Controls.Add(this.btnLoadConfig);
            this.tbRun.Controls.Add(this.btnSaveConfig);
            this.tbRun.Controls.Add(this.rtxtInfoDir);
            this.tbRun.Controls.Add(this.rtxtOutput);
            this.tbRun.Controls.Add(this.btnSelectContentDir);
            this.tbRun.Controls.Add(this.btnInfoDir);
            this.tbRun.Controls.Add(this.btnOpenAllTypes);
            this.tbRun.Controls.Add(this.rtxtParseDir);
            this.tbRun.Controls.Add(this.btnOpenAssetTypes);
            this.tbRun.Controls.Add(this.btnSelectParseDir);
            this.tbRun.Controls.Add(this.btnOpenLogs);
            this.tbRun.Controls.Add(this.btnSelectJSONDir);
            this.tbRun.Controls.Add(this.lblProgress);
            this.tbRun.Controls.Add(this.rtxtContentDir);
            this.tbRun.Controls.Add(this.btnClearLogs);
            this.tbRun.Controls.Add(this.rtxtJSONDir);
            this.tbRun.Controls.Add(this.btnSerializeAssets);
            this.tbRun.Controls.Add(this.btnSelectOutputDir);
            this.tbRun.Controls.Add(this.btnMoveCookedAssets);
            this.tbRun.Controls.Add(this.rtxtOutputDir);
            this.tbRun.Controls.Add(this.btnScanAssets);
            this.tbRun.Controls.Add(this.label1);
            this.tbRun.Controls.Add(this.chkRefreshAssets);
            this.tbRun.Controls.Add(this.cbUEVersion);
            this.tbRun.Location = new System.Drawing.Point(4, 29);
            this.tbRun.Name = "tbRun";
            this.tbRun.Padding = new System.Windows.Forms.Padding(3);
            this.tbRun.Size = new System.Drawing.Size(1094, 909);
            this.tbRun.TabIndex = 0;
            this.tbRun.Text = "Run";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label2.Location = new System.Drawing.Point(484, 377);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 38);
            this.label2.TabIndex = 33;
            this.label2.Text = "Output";
            // 
            // rtxtInfoDir
            // 
            this.rtxtInfoDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtInfoDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtInfoDir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.rtxtInfoDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtInfoDir.Location = new System.Drawing.Point(171, 211);
            this.rtxtInfoDir.Multiline = false;
            this.rtxtInfoDir.Name = "rtxtInfoDir";
            this.rtxtInfoDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtInfoDir.Size = new System.Drawing.Size(899, 40);
            this.rtxtInfoDir.TabIndex = 37;
            this.rtxtInfoDir.Text = "C:\\ExamplePath\\Info";
            // 
            // btnInfoDir
            // 
            this.btnInfoDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnInfoDir.FlatAppearance.BorderSize = 2;
            this.btnInfoDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfoDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnInfoDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnInfoDir.Location = new System.Drawing.Point(15, 211);
            this.btnInfoDir.Name = "btnInfoDir";
            this.btnInfoDir.Size = new System.Drawing.Size(139, 40);
            this.btnInfoDir.TabIndex = 36;
            this.btnInfoDir.Text = "Info Dir";
            this.btnInfoDir.UseVisualStyleBackColor = true;
            this.btnInfoDir.Click += new System.EventHandler(this.btnInfoDir_Click);
            // 
            // tbSettings
            // 
            this.tbSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
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
            this.tbSettings.Controls.Add(this.label6);
            this.tbSettings.Controls.Add(this.rtxtCircularDependancy);
            this.tbSettings.Location = new System.Drawing.Point(4, 29);
            this.tbSettings.Name = "tbSettings";
            this.tbSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tbSettings.Size = new System.Drawing.Size(1094, 909);
            this.tbSettings.TabIndex = 1;
            this.tbSettings.Text = "Settings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label7.Location = new System.Drawing.Point(701, 644);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(330, 38);
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
            this.lbAssetsToDelete.ItemHeight = 28;
            this.lbAssetsToDelete.Location = new System.Drawing.Point(667, 689);
            this.lbAssetsToDelete.Name = "lbAssetsToDelete";
            this.lbAssetsToDelete.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToDelete.Size = new System.Drawing.Size(404, 196);
            this.lbAssetsToDelete.TabIndex = 40;
            // 
            // chkDummyWithProps
            // 
            this.chkDummyWithProps.AutoSize = true;
            this.chkDummyWithProps.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkDummyWithProps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkDummyWithProps.Location = new System.Drawing.Point(749, 609);
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
            this.label9.Location = new System.Drawing.Point(749, 310);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(244, 38);
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
            this.lbDummyAssets.ItemHeight = 28;
            this.lbDummyAssets.Location = new System.Drawing.Point(667, 351);
            this.lbDummyAssets.Name = "lbDummyAssets";
            this.lbDummyAssets.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbDummyAssets.Size = new System.Drawing.Size(404, 252);
            this.lbDummyAssets.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label8.Location = new System.Drawing.Point(329, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(495, 20);
            this.label8.TabIndex = 38;
            this.label8.Text = "Written by LongerWarrior, Buckminsterfullerene and atenfyr (UAAPI)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1126, 968);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tabControl1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.Name = "Form1";
            this.Text = "Cooked Asset Serializer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.tabControl1.ResumeLayout(false);
            this.tbRun.ResumeLayout(false);
            this.tbRun.PerformLayout();
            this.tbSettings.ResumeLayout(false);
            this.tbSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private Button btnSelectContentDir;
    private RichTextBox rtxtContentDir;
    private RichTextBox rtxtJSONDir;
    private Button btnSelectJSONDir;
    private RichTextBox rtxtOutputDir;
    private Button btnSelectOutputDir;
    private Label label1;
    private ComboBox cbUEVersion;
    private CheckBox chkRefreshAssets;
    private Label label3;
    private ListBox lbAssetsToSkipSerialization;
    private Label label4;
    private RichTextBox rtxtCookedAssets;
    private RichTextBox rtxtSimpleAssets;
    private Label label5;
    private Button btnScanAssets;
    private Button btnMoveCookedAssets;
    private Button btnSerializeAssets;
    private RichTextBox rtxtCircularDependancy;
    private Label label6;
    private RichTextBox rtxtOutput;
    private Button btnOpenAssetTypes;
    private Button btnOpenAllTypes;
    private Button btnOpenLogs;
    private Button btnClearLogs;
    private Button btnLoadConfig;
    private Button btnSaveConfig;
    private Label lblProgress;
    private RichTextBox rtxtParseDir;
    private Button btnSelectParseDir;
    private TabControl tabControl1;
    private TabPage tbRun;
    private TabPage tbSettings;
    private RichTextBox rtxtInfoDir;
    private Button btnInfoDir;
    private Label label9;
    private ListBox lbDummyAssets;
    private CheckBox chkDummyWithProps;
    private Label label2;
    private Label label7;
    private ListBox lbAssetsToDelete;
    private Label label8;
}
