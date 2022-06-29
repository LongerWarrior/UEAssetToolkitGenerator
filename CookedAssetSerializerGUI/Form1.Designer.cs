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
            this.label2 = new System.Windows.Forms.Label();
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
            this.label8 = new System.Windows.Forms.Label();
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOpenAssetTypes = new System.Windows.Forms.Button();
            this.btnOpenAllTypes = new System.Windows.Forms.Button();
            this.btnOpenLogs = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // btnSelectContentDir
            //
            this.btnSelectContentDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectContentDir.FlatAppearance.BorderSize = 2;
            this.btnSelectContentDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectContentDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectContentDir.Location = new System.Drawing.Point(35, 77);
            this.btnSelectContentDir.Name = "btnSelectContentDir";
            this.btnSelectContentDir.Size = new System.Drawing.Size(140, 40);
            this.btnSelectContentDir.TabIndex = 0;
            this.btnSelectContentDir.Text = "Content Dir";
            this.btnSelectContentDir.UseVisualStyleBackColor = true;
            this.btnSelectContentDir.Click += new System.EventHandler(this.btnSelectContentDir_Click);
            //
            // rtxtContentDir
            //
            this.rtxtContentDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtContentDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtContentDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtContentDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtContentDir.Location = new System.Drawing.Point(191, 77);
            this.rtxtContentDir.Multiline = false;
            this.rtxtContentDir.Name = "rtxtContentDir";
            this.rtxtContentDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtContentDir.Size = new System.Drawing.Size(630, 40);
            this.rtxtContentDir.TabIndex = 1;
            this.rtxtContentDir.Text = "C:\\ExamplePath\\Content";
            this.rtxtContentDir.TextChanged += new System.EventHandler(this.rtxtContentDir_TextChanged);
            //
            // rtxtJSONDir
            //
            this.rtxtJSONDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtJSONDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtJSONDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtJSONDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtJSONDir.Location = new System.Drawing.Point(191, 138);
            this.rtxtJSONDir.Multiline = false;
            this.rtxtJSONDir.Name = "rtxtJSONDir";
            this.rtxtJSONDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtJSONDir.Size = new System.Drawing.Size(630, 40);
            this.rtxtJSONDir.TabIndex = 3;
            this.rtxtJSONDir.Text = "C:\\ExamplePath\\JSON";
            this.rtxtJSONDir.TextChanged += new System.EventHandler(this.rtxtJSONDir_TextChanged);
            //
            // btnSelectJSONDir
            //
            this.btnSelectJSONDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectJSONDir.FlatAppearance.BorderSize = 2;
            this.btnSelectJSONDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectJSONDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectJSONDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectJSONDir.Location = new System.Drawing.Point(35, 138);
            this.btnSelectJSONDir.Name = "btnSelectJSONDir";
            this.btnSelectJSONDir.Size = new System.Drawing.Size(140, 40);
            this.btnSelectJSONDir.TabIndex = 2;
            this.btnSelectJSONDir.Text = "JSON Dir";
            this.btnSelectJSONDir.UseVisualStyleBackColor = true;
            this.btnSelectJSONDir.Click += new System.EventHandler(this.btnSelectJSONDir_Click);
            //
            // rtxtOutputDir
            //
            this.rtxtOutputDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtOutputDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtOutputDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtOutputDir.Location = new System.Drawing.Point(191, 199);
            this.rtxtOutputDir.Multiline = false;
            this.rtxtOutputDir.Name = "rtxtOutputDir";
            this.rtxtOutputDir.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtOutputDir.Size = new System.Drawing.Size(630, 40);
            this.rtxtOutputDir.TabIndex = 5;
            this.rtxtOutputDir.Text = "C:\\ExamplePath\\Output";
            this.rtxtOutputDir.TextChanged += new System.EventHandler(this.rtxtOutputDir_TextChanged);
            //
            // btnSelectOutputDir
            //
            this.btnSelectOutputDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnSelectOutputDir.FlatAppearance.BorderSize = 2;
            this.btnSelectOutputDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectOutputDir.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectOutputDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnSelectOutputDir.Location = new System.Drawing.Point(35, 199);
            this.btnSelectOutputDir.Name = "btnSelectOutputDir";
            this.btnSelectOutputDir.Size = new System.Drawing.Size(140, 40);
            this.btnSelectOutputDir.TabIndex = 4;
            this.btnSelectOutputDir.Text = "Output Dir";
            this.btnSelectOutputDir.UseVisualStyleBackColor = true;
            this.btnSelectOutputDir.Click += new System.EventHandler(this.btnSelectOutputDir_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label1.Location = new System.Drawing.Point(35, 262);
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
            this.cbUEVersion.Location = new System.Drawing.Point(191, 262);
            this.cbUEVersion.Name = "cbUEVersion";
            this.cbUEVersion.Size = new System.Drawing.Size(323, 36);
            this.cbUEVersion.TabIndex = 7;
            this.cbUEVersion.SelectedIndexChanged += new System.EventHandler(this.cbUEVersion_SelectedIndexChanged);
            //
            // chkRefreshAssets
            //
            this.chkRefreshAssets.AutoSize = true;
            this.chkRefreshAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkRefreshAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.chkRefreshAssets.Location = new System.Drawing.Point(605, 607);
            this.chkRefreshAssets.Name = "chkRefreshAssets";
            this.chkRefreshAssets.Size = new System.Drawing.Size(172, 32);
            this.chkRefreshAssets.TabIndex = 8;
            this.chkRefreshAssets.Text = "Refresh Assets";
            this.chkRefreshAssets.UseVisualStyleBackColor = true;
            this.chkRefreshAssets.CheckedChanged += new System.EventHandler(this.chkRefreshAssets_CheckedChanged);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label2.Location = new System.Drawing.Point(390, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 38);
            this.label2.TabIndex = 9;
            this.label2.Text = "Settings";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label3.Location = new System.Drawing.Point(118, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 38);
            this.label3.TabIndex = 10;
            this.label3.Text = "Assets to skip serializing";
            //
            // lbAssetsToSkipSerialization
            //
            this.lbAssetsToSkipSerialization.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.lbAssetsToSkipSerialization.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAssetsToSkipSerialization.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbAssetsToSkipSerialization.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.lbAssetsToSkipSerialization.FormattingEnabled = true;
            this.lbAssetsToSkipSerialization.ItemHeight = 28;
            this.lbAssetsToSkipSerialization.Location = new System.Drawing.Point(32, 373);
            this.lbAssetsToSkipSerialization.Name = "lbAssetsToSkipSerialization";
            this.lbAssetsToSkipSerialization.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAssetsToSkipSerialization.Size = new System.Drawing.Size(482, 336);
            this.lbAssetsToSkipSerialization.TabIndex = 11;
            this.lbAssetsToSkipSerialization.SelectedIndexChanged += new System.EventHandler(this.lbAssetsToSkipSerialization_SelectedIndexChanged);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label4.Location = new System.Drawing.Point(969, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 38);
            this.label4.TabIndex = 12;
            this.label4.Text = "Cooked assets to copy";
            //
            // rtxtCookedAssets
            //
            this.rtxtCookedAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtCookedAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCookedAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtCookedAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCookedAssets.Location = new System.Drawing.Point(864, 359);
            this.rtxtCookedAssets.Name = "rtxtCookedAssets";
            this.rtxtCookedAssets.Size = new System.Drawing.Size(521, 166);
            this.rtxtCookedAssets.TabIndex = 13;
            this.rtxtCookedAssets.Text = resources.GetString("rtxtCookedAssets.Text");
            this.rtxtCookedAssets.TextChanged += new System.EventHandler(this.rtxtCookedAssets_TextChanged);
            //
            // rtxtSimpleAssets
            //
            this.rtxtSimpleAssets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtSimpleAssets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtSimpleAssets.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtSimpleAssets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtSimpleAssets.Location = new System.Drawing.Point(864, 77);
            this.rtxtSimpleAssets.Name = "rtxtSimpleAssets";
            this.rtxtSimpleAssets.Size = new System.Drawing.Size(521, 210);
            this.rtxtSimpleAssets.TabIndex = 15;
            this.rtxtSimpleAssets.Text = resources.GetString("rtxtSimpleAssets.Text");
            this.rtxtSimpleAssets.WordWrap = false;
            this.rtxtSimpleAssets.TextChanged += new System.EventHandler(this.rtxtSimpleAssets_TextChanged);
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label5.Location = new System.Drawing.Point(1024, 23);
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
            this.btnScanAssets.Location = new System.Drawing.Point(572, 388);
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
            this.btnMoveCookedAssets.Location = new System.Drawing.Point(572, 466);
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
            this.btnSerializeAssets.Location = new System.Drawing.Point(572, 542);
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
            this.rtxtCircularDependancy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtCircularDependancy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtCircularDependancy.Location = new System.Drawing.Point(864, 597);
            this.rtxtCircularDependancy.Name = "rtxtCircularDependancy";
            this.rtxtCircularDependancy.Size = new System.Drawing.Size(521, 113);
            this.rtxtCircularDependancy.TabIndex = 20;
            this.rtxtCircularDependancy.Text = "/Script/Engine.SoundClass\n/Script/Engine.SoundSubmix\n/Script/Engine.EndpointSubmi" +
    "x";
            this.rtxtCircularDependancy.TextChanged += new System.EventHandler(this.rtxtCircularDependancy_TextChanged);
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label6.Location = new System.Drawing.Point(900, 547);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(458, 38);
            this.label6.TabIndex = 19;
            this.label6.Text = "Assets with a circular dependancy";
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label8.Location = new System.Drawing.Point(625, 681);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 38);
            this.label8.TabIndex = 22;
            this.label8.Text = "Output";
            //
            // rtxtOutput
            //
            this.rtxtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.rtxtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtOutput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rtxtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.rtxtOutput.Location = new System.Drawing.Point(32, 734);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.ReadOnly = true;
            this.rtxtOutput.Size = new System.Drawing.Size(1353, 204);
            this.rtxtOutput.TabIndex = 23;
            this.rtxtOutput.Text = "";
            this.rtxtOutput.WordWrap = false;
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.label7.Location = new System.Drawing.Point(648, 318);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 38);
            this.label7.TabIndex = 21;
            this.label7.Text = "Run";
            //
            // btnOpenAssetTypes
            //
            this.btnOpenAssetTypes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(43)))));
            this.btnOpenAssetTypes.FlatAppearance.BorderSize = 2;
            this.btnOpenAssetTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAssetTypes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAssetTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.btnOpenAssetTypes.Location = new System.Drawing.Point(236, 960);
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
            this.btnOpenAllTypes.Location = new System.Drawing.Point(35, 960);
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
            this.btnOpenLogs.Location = new System.Drawing.Point(1247, 960);
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
            this.btnClearLogs.Location = new System.Drawing.Point(1093, 960);
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
            this.btnLoadConfig.Location = new System.Drawing.Point(529, 960);
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
            this.btnSaveConfig.Location = new System.Drawing.Point(777, 960);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(226, 40);
            this.btnSaveConfig.TabIndex = 30;
            this.btnSaveConfig.Text = "Save Config Settings";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1425, 1033);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.btnLoadConfig);
            this.Controls.Add(this.btnClearLogs);
            this.Controls.Add(this.btnOpenLogs);
            this.Controls.Add(this.btnOpenAllTypes);
            this.Controls.Add(this.btnOpenAssetTypes);
            this.Controls.Add(this.rtxtOutput);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.rtxtCircularDependancy);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSerializeAssets);
            this.Controls.Add(this.btnMoveCookedAssets);
            this.Controls.Add(this.btnScanAssets);
            this.Controls.Add(this.rtxtSimpleAssets);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rtxtCookedAssets);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbAssetsToSkipSerialization);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkRefreshAssets);
            this.Controls.Add(this.cbUEVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtxtOutputDir);
            this.Controls.Add(this.btnSelectOutputDir);
            this.Controls.Add(this.rtxtJSONDir);
            this.Controls.Add(this.btnSelectJSONDir);
            this.Controls.Add(this.rtxtContentDir);
            this.Controls.Add(this.btnSelectContentDir);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.Name = "Form1";
            this.Text = "Cooked Asset Serializer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
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
    private Label label2;
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
    private Label label8;
    private RichTextBox rtxtOutput;
    private Label label7;
    private Button btnOpenAssetTypes;
    private Button btnOpenAllTypes;
    private Button btnOpenLogs;
    private Button btnClearLogs;
    private Button btnLoadConfig;
    private Button btnSaveConfig;
}
