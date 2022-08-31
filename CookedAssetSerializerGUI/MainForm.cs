using System.Diagnostics;
using System.Runtime.InteropServices;
using CookedAssetSerializer;
using CookedAssetSerializer.NativizedAssets;
using CookedAssetSerializerGUI.Properties;
using ExtendedTreeView;
using NativizedAssets;
using Newtonsoft.Json;
using UAssetAPI;

namespace CookedAssetSerializerGUI;

public partial class MainForm : Form
{
    public MainForm() {
        Singleton.Init(new Dictionary<string, bool> 
        { 
            ["StaticMesh.KeepMobileMinLODSettingOnDesktop"] = false,
            ["SkeletalMesh.KeepMobileMinLODSettingOnDesktop"] = false
        });

        InitializeComponent();
        SetupForm();
        SetupGlobals();

        

        //new GenerateBPY();

        Task.Run(EventLoop);
    }

    #region Vars

    private delegate void SafeCallDelegate();

    private delegate void SafeCallDelegateRichText(string text, RichTextBox rtxt);

    private delegate void SafeCallDelegateText(string text);

    private CookedAssetSerializer.System system;

    private JSONSettings jsonsettings;

    private volatile bool isRunning;
    
    private object boolLock = new object();

    private string lastValidContentDir = string.Empty;

    private readonly object[] versionOptionsKeys =
    {
        "Unknown version",
        "4.0",
        "4.1",
        "4.2",
        "4.3",
        "4.4",
        "4.5",
        "4.6",
        "4.7",
        "4.8",
        "4.9",
        "4.10",
        "4.11",
        "4.12",
        "4.13",
        "4.14",
        "4.15",
        "4.16",
        "4.17",
        "4.18",
        "4.19",
        "4.20",
        "4.21",
        "4.22",
        "4.23",
        "4.24",
        "4.25",
        "4.26",
        "4.27"
    };

    private readonly UE4Version[] versionOptionsValues =
    {
        UE4Version.UNKNOWN,
        UE4Version.VER_UE4_0,
        UE4Version.VER_UE4_1,
        UE4Version.VER_UE4_2,
        UE4Version.VER_UE4_3,
        UE4Version.VER_UE4_4,
        UE4Version.VER_UE4_5,
        UE4Version.VER_UE4_6,
        UE4Version.VER_UE4_7,
        UE4Version.VER_UE4_8,
        UE4Version.VER_UE4_9,
        UE4Version.VER_UE4_10,
        UE4Version.VER_UE4_11,
        UE4Version.VER_UE4_12,
        UE4Version.VER_UE4_13,
        UE4Version.VER_UE4_14,
        UE4Version.VER_UE4_15,
        UE4Version.VER_UE4_16,
        UE4Version.VER_UE4_17,
        UE4Version.VER_UE4_18,
        UE4Version.VER_UE4_19,
        UE4Version.VER_UE4_20,
        UE4Version.VER_UE4_21,
        UE4Version.VER_UE4_22,
        UE4Version.VER_UE4_23,
        UE4Version.VER_UE4_24,
        UE4Version.VER_UE4_25,
        UE4Version.VER_UE4_26,
        UE4Version.VER_UE4_27
    };



    private readonly object[] nativeMethodKeys =
    {
        "Disabled",
        "Inclusive",
        "Exclusive"
    };

    private readonly ENativizationMethod[] nativMethodValues =
    {
        ENativizationMethod.Disabled,
        ENativizationMethod.Inclusive,
        ENativizationMethod.Exclusive
    };

    #endregion

    #region CustomFormSetup

    private const int HT_CAPTION = 0x2;
    private const int WM_NCLBUTTONDOWN = 0x00A1;

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern bool ReleaseCapture();

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        var rct = DisplayRectangle;
        if (!rct.Contains(e.Location)) return;
        ReleaseCapture();
        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }

    #endregion



    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.WindowsShutDown) return;

        if (AppSettings.Default.bDNSSave == false)
        {
            var dialog = new ChkBoxDialog2Bool("Save?", "Do you want to save before exiting?", "Auto Load Last Used Config on Launch?", "Do Not Show Again.");
            if (AppSettings.Default.bAutoUseLastCfg == true)
            {
                dialog.b1Dialog = true;
            }
            var result = dialog.ShowDialog();

            AppSettings.Default.bAutoUseLastCfg = dialog.b1Dialog;
            AppSettings.Default.bDNSSave = dialog.b2Dialog;
            AppSettings.Default.Save();


            if (result == DialogResult.Yes)
            {
                SetupGlobals();
                SaveJSONSettings();
            }

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }

    private async Task EventLoop()
    {
        while (true)
        {
            lock(boolLock) if (isRunning)
            {
                if (system.GetAssetTotal() != 0) 
                {
                    ProgressText(system.GetAssetCount() + " / " + system.GetAssetTotal());
                }
            }
            await Task.Delay(50);
            
            // Do this in order to make sure that the correct total is outputted when the isRunning is set to false
            // and the delay is not enough to make the total outputted
            if ((system.GetAssetTotal() == system.GetAssetCount()) && system.GetAssetTotal() != 0)
            {
                ProgressText(system.GetAssetCount() + " / " + system.GetAssetTotal());
            }
        }
    }


    


    private void SetupAssetsListBox(List<EAssetType> assets, ListBox assetBox)
    {
        assetBox.DataSource = Enum.GetValues(typeof(EAssetType));
        var hasBP = false;
        foreach (var asset in assets)
        {
            assetBox.SetSelected(assetBox.FindString(asset.ToString()), true);
            if (asset == EAssetType.Blueprint) hasBP = true;
        }

        // For some stupid reason, the first item in the lb is always enabled, which in this case, is the Blueprint,
        // which is the absolute worst time for this """feature""" to happen
        assetBox.SetSelected(0, hasBP);
    }

    private void SetupForm()
    {
        /*rtxtContentDir.Text = Environment.CurrentDirectory;
        lastValidContentDir = Environment.CurrentDirectory;
        rtxtParseDir.Text = Environment.CurrentDirectory;
        rtxtJSONDir.Text = Environment.CurrentDirectory;
        rtxtCookedDir.Text = Environment.CurrentDirectory;
        rtxtInfoDir.Text = Environment.CurrentDirectory;*/

        cbUEVersion.Items.AddRange(versionOptionsKeys);
        cbUEVersion.SelectedIndex = 28; // This is a dumb thing to do, but oh well

        cbNativMethod.Items.AddRange(nativeMethodKeys);
        cbNativMethod.SelectedIndex = 0;

        List<EAssetType> defaultSkipAssets = new()
        {
            EAssetType.SkeletalMesh
        };
        SetupAssetsListBox(defaultSkipAssets, lbAssetsToSkipSerialization);
        List<EAssetType> defaultDummyAssets = new()
        {
            EAssetType.AnimSequence,
            EAssetType.AnimMontage,
            EAssetType.CameraAnim,
            EAssetType.LandscapeGrassType,
            EAssetType.MediaPlayer,
            EAssetType.MediaTexture
        };
        SetupAssetsListBox(defaultDummyAssets, lbDummyAssets);
        SetupAssetsListBox(new List<EAssetType>(), lbAssetsToDelete);
    }

    private void SetDialogDirectories()
    {

    }


    private void rtxtCookedDir_Leave(object sender, EventArgs e)
    {
        if (rtxtCookedDir.Text.Length == 0)
        {
            rtxtCookedDir.Text = "C:\\ExamplePath\\Cooked";
            rtxtCookedDir.ForeColor = SystemColors.GrayText;
        }
    }

    private void rtxtCookedDir_Enter(object sender, EventArgs e)
    {
        if (rtxtCookedDir.Text == "C:\\ExamplePath\\Cooked")
        {
            rtxtCookedDir.Text = "";
            rtxtCookedDir.ForeColor = SystemColors.WindowText;
        }
    }

    private void rtxtJSONDir_Leave(object sender, EventArgs e)
    {
        if (rtxtJSONDir.Text.Length == 0)
        {
            rtxtJSONDir.Text = "C:\\ExamplePath\\AssetDump";
            rtxtJSONDir.ForeColor = SystemColors.GrayText;
        }
    }

    private void rtxtJSONDir_Enter(object sender, EventArgs e)
    {
        if (rtxtJSONDir.Text == "C:\\ExamplePath\\AssetDump")
        {
            rtxtJSONDir.Text = "";
            rtxtJSONDir.ForeColor = SystemColors.WindowText;
        }
    }

    private void rtxtContentDir_Leave(object sender, EventArgs e)
    {
        if (rtxtContentDir.Text.Length == 0)
        {
            rtxtContentDir.Text = "C:\\ExamplePath\\Content";
            rtxtContentDir.ForeColor = SystemColors.GrayText;
        }
    }

    private void rtxtContentDir_Enter(object sender, EventArgs e)
    {
        if (rtxtContentDir.Text == "C:\\ExamplePath\\Content")
        {
            rtxtContentDir.Text = "";
            rtxtContentDir.ForeColor = SystemColors.WindowText;
        }
    }

    private void rtxtInfoDir_Leave(object sender, EventArgs e)
    {
        if (rtxtInfoDir.Text.Length == 0)
        {
            rtxtInfoDir.Text = "C:\\ExamplePath\\Info";
            rtxtInfoDir.ForeColor = SystemColors.GrayText;
        }
    }

    private void rtxtInfoDir_Enter(object sender, EventArgs e)
    {
        if (rtxtInfoDir.Text == "C:\\ExamplePath\\Info")
        {
            rtxtInfoDir.Text = "";
            rtxtInfoDir.ForeColor = SystemColors.WindowText;
        }
    }


    private void rtxtParseDir_Leave(object sender, EventArgs e)
    {
        if (rtxtParseDir.Text.Length == 0)
        {
            rtxtParseDir.Text = "C:\\ExamplePath\\Content\\Data";
            rtxtParseDir.ForeColor = SystemColors.GrayText;
        }
    }

    private void rtxtParseDir_Enter(object sender, EventArgs e)
    {
        if (rtxtParseDir.Text == "C:\\ExamplePath\\Content\\Data")
        {
            rtxtParseDir.Text = "";
            rtxtParseDir.ForeColor = SystemColors.WindowText;
        }
    }

    private string[] SanitiseInputs(string[] lines)
    {
        for (var i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains("/Script/")) lines[i] = lines[i].Insert(0, "/Script/");

            // This garbage allows us to copy and paste text from the text files
            // and not have to muck about deleting them manually
            lines[i] = lines[i].Replace('"'.ToString(), "");
            lines[i] = lines[i].Replace(','.ToString(), "");
        }

        return lines;
    }

    public void SetupGlobals()
    {
        var typesToCopy = new List<string>();
        typesToCopy.AddRange(SanitiseInputs(rtxtCookedAssets.Lines));
        var simpleAssets = new List<string>();
        simpleAssets.AddRange(SanitiseInputs(rtxtSimpleAssets.Lines));
        var assetsToSkip = new List<EAssetType>();
        assetsToSkip.AddRange(lbAssetsToSkipSerialization.SelectedItems.Cast<EAssetType>());
        var dummyAssets = new List<EAssetType>();
        dummyAssets.AddRange(lbDummyAssets.SelectedItems.Cast<EAssetType>());
        var assetsToDelete = new List<EAssetType>();
        assetsToDelete.AddRange(lbAssetsToDelete.SelectedItems.Cast<EAssetType>());
        var circularDependencies = new List<string>();
        circularDependencies.AddRange(SanitiseInputs(rtxtCircularDependancy.Lines));

        if (string.IsNullOrEmpty(rtxtJSONDir.Text)) 
        {
            rtxtJSONDir.Text = Path.Combine(Directory.GetParent(rtxtContentDir.Text)!.FullName, "AssetDump");
            Directory.CreateDirectory(rtxtJSONDir.Text);
        }

        if (string.IsNullOrEmpty(rtxtCookedDir.Text)) 
        {
            rtxtCookedDir.Text = Path.Combine(Directory.GetParent(rtxtContentDir.Text)!.FullName, "Cooked");
            Directory.CreateDirectory(rtxtCookedDir.Text);
        }


        jsonsettings = new JSONSettings
        {
            ContentDir = rtxtContentDir.Text,
            //ParseDir = rtxtParseDir.Text,
            JSONDir = rtxtJSONDir.Text,
            CookedDir = rtxtCookedDir.Text,
            InfoDir = rtxtInfoDir.Text,
            GlobalUEVersion = versionOptionsValues[cbUEVersion.SelectedIndex],
            RefreshAssets = chkRefreshAssets.Checked,
            DummyWithProps = chkDummyWithProps.Checked,
            DummyAssets = dummyAssets,
            SkipSerialization = assetsToSkip,
            DeleteAssets = assetsToDelete,
            CircularDependency = circularDependencies,
            SimpleAssets = simpleAssets,
            TypesToCopy = typesToCopy,
            SelectedIndex = cbUEVersion.SelectedIndex
        };

        system = new CookedAssetSerializer.System(jsonsettings);
    }

    private void LoadJSONSettings()
    {
        
        if (MessageBox.Show(@"Do you want to load in the file paths?", @"Holup!",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            rtxtContentDir.Text = jsonsettings.ContentDir;
            //rtxtParseDir.Text = jsonsettings.ParseDir;
            rtxtJSONDir.Text = jsonsettings.JSONDir;
            rtxtCookedDir.Text = jsonsettings.CookedDir;
            rtxtInfoDir.Text = jsonsettings.InfoDir;
        }

        cbUEVersion.SelectedIndex = jsonsettings.SelectedIndex;
        chkRefreshAssets.Checked = jsonsettings.RefreshAssets;
        chkDummyWithProps.Checked = jsonsettings.DummyWithProps;
        SetupAssetsListBox(jsonsettings.SkipSerialization, lbAssetsToSkipSerialization);
        SetupAssetsListBox(jsonsettings.DummyAssets, lbDummyAssets);
        SetupAssetsListBox(jsonsettings.DeleteAssets, lbAssetsToDelete);
        rtxtCircularDependancy.Lines = jsonsettings.CircularDependency.ToArray();
        rtxtSimpleAssets.Lines = jsonsettings.SimpleAssets.ToArray();
        rtxtCookedAssets.Lines = jsonsettings.TypesToCopy.ToArray();
    }

    public void SaveJSONSettings()
    {
        var output = JsonConvert.SerializeObject(jsonsettings, Formatting.Indented);

        var dialog = new SaveFileDialog();
        dialog.Filter = @"JSON files (*.json)|*.json";
        dialog.Title = @"Save current settings";
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() != DialogResult.OK) return;
        if (dialog.FileName == "") return;
        File.WriteAllText(dialog.FileName, output);
        AppSettings.Default.LastUsedCfg = dialog.FileName;
        AppSettings.Default.Save();
        OutputText("Saved settings to: " + dialog.FileName, rtxtOutput);
    }

    private void AutoLoadConfig()
    {
            var configFile = AppSettings.Default.LastUsedCfg;
            if (!File.Exists(configFile))
            {
                OutputText("Warning! Unable to find last used config!", rtxtOutput);
                return;
            }

            // TODO: Reload buggered settings when the catch is run (can't deep clone settings into temp because it can't be serialized)
            try
            {
                /*system.ClearLists();*/ // I have to do this or else the fucking lists get appended rather than set for some reason
                jsonsettings = JsonConvert.DeserializeObject<JSONSettings>(File.ReadAllText(configFile));
                LoadJSONSettings();
                OutputText("Loaded settings from: " + configFile, rtxtOutput);
            }
            catch (Exception exception)
            {
                OutputText("Warning! Last used config is invalid!", rtxtOutput);
                OutputText(exception.ToString(), rtxtOutput);
            }        
    }


    private void ToggleButtons()
    {
        if (InvokeRequired)
        {
            var d = new SafeCallDelegate(ToggleButtons);
            Invoke(d, new object[] { });
        }
        else
        {
            btnScanAssets.Enabled = !isRunning;
            btnMoveCookedAssets.Enabled = !isRunning;
            btnSerializeAssets.Enabled = !isRunning;
            btnClearLogs.Enabled = !isRunning;
            btnLoadConfig.Enabled = !isRunning;
        }
    }

    private void ProgressText(string text) {
        if (InvokeRequired)
        {
            var d = new SafeCallDelegateText(ProgressText);
            Invoke(d, new object[] { text });
        }
        else lblProgress.Text = text;
    }

    private void OutputText(string text, RichTextBox rtxt)
    {
        if (InvokeRequired)
        {
            var d = new SafeCallDelegateRichText(OutputText);
            Invoke(d, new object[] { text, rtxt });
        }
        else
        {
            if (rtxt.TextLength == 0) rtxt.Text += text;
            else rtxt.Text += Environment.NewLine + text;
        }
    }

    private void OpenFile(string path, bool bIsLog = false)
    {
        if (!File.Exists(path))
        {
            if (!bIsLog) OutputText("You need to scan the assets first!", rtxtOutput);
            return;
        }
        Process.Start(new ProcessStartInfo { FileName = @path, UseShellExecute = true });
    }

    private string OpenDirectoryDialog(string path) 
    {
        var fbd = new FolderBrowserDialog();
        if (string.IsNullOrEmpty(path) || path.StartsWith("C:\\ExamplePath"))
        {
            fbd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        }
        if (Directory.Exists(path)) fbd.SelectedPath = path + @"\";
        if (fbd.ShowDialog() == DialogResult.Cancel) return path;
        return fbd.SelectedPath;
    }

    private string OpenFileDialog(string filter, string path)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = filter;
        if (string.IsNullOrEmpty(path) || path.StartsWith("C:\\ExamplePath"))
        {
            dialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        }
        dialog.RestoreDirectory = true;
        return dialog.ShowDialog() != DialogResult.OK ? "" : dialog.FileName;
    }

    private void EmptyLogFiles()
    {
        string[] files =
        {
            "debug_log.txt",
            "output_log.txt",
            "thread_log.txt"
        };
        foreach (var file in files)
        {
            string path;
            if (rtxtInfoDir.Text.EndsWith("\\")) path = rtxtInfoDir.Text + file;
            else path = rtxtInfoDir.Text + "\\" + file;
            if (!File.Exists(path)) continue;
            File.Delete(path);
            OutputText("Cleared log file: " + path, rtxtOutput);
        }
    }

    private void ValidateParseDir(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (string.IsNullOrEmpty(rtxtParseDir.Text))
        {
            rtxtParseDir.Text = rtxtContentDir.Text;
            return;
        }

        if (IsSubPathOf(rtxtParseDir.Text, rtxtContentDir.Text)) return;
        rtxtParseDir.Text = rtxtContentDir.Text;
    }

    private static bool IsSubPathOf(string subPath, string basePath)
    {
        if (!Directory.Exists(subPath)) return false;
        var rel = Path.GetRelativePath(basePath, subPath);
        return !rel.StartsWith('.') && !Path.IsPathRooted(rel);
    }

    private void ValidateContentDir(object sender, System.ComponentModel.CancelEventArgs e)
    {
        ValidateContentDir();
    }
    private void ValidateContentDir()
    {
        if (string.IsNullOrEmpty(rtxtContentDir.Text) || !Directory.Exists(rtxtContentDir.Text))
        {
            rtxtContentDir.Text = lastValidContentDir;
        }
        lastValidContentDir = rtxtContentDir.Text;
        treeParseDir.Nodes.Clear();
        LoadDirectory(rtxtContentDir.Text);
        
        if (IsSubPathOf(rtxtParseDir.Text, rtxtContentDir.Text)) return;
        rtxtParseDir.Text = rtxtContentDir.Text;
    }

    private void ValidateDir(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var rtxtBox = (RichTextBox)sender;
        if (!Directory.Exists(rtxtBox.Text))
        {
            if (!CreateFolderMessage(rtxtBox)) rtxtBox.Text = "";
        }
    }

    private bool CreateFolderMessage(RichTextBox rtxtBox)
    {
        var caption = "";
        if (rtxtBox.Name == "rtxtJSONDir")
        {
            caption = "JSON Dir doesn't exist";
            if (string.IsNullOrEmpty(rtxtBox.Text))
            {
                rtxtBox.Text = Path.Combine(Directory.GetParent(rtxtContentDir.Text)!.FullName, "AssetDump");
            }
        }

        if (rtxtBox.Name == "rtxtOutputDir")
        {
            caption = "Cooked Dir doesn't exist";
            if (string.IsNullOrEmpty(rtxtBox.Text))
            {
                rtxtBox.Text = Path.Combine(Directory.GetParent(rtxtContentDir.Text)!.FullName, "Cooked");
            }
        }

        if (Directory.Exists(rtxtBox.Text)) return true;

        var result = MessageBox.Show($@"Create {rtxtBox.Text} directory?", caption, MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            Directory.CreateDirectory(rtxtBox.Text);
            if (!Directory.Exists(rtxtBox.Text))
            {
                MessageBox.Show($@"Can't create {rtxtBox.Text} directory", @"Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        return false;
    }

    private void btnSelectContentDir_Click(object sender, EventArgs e)
    {
        rtxtContentDir.Text = OpenDirectoryDialog(rtxtContentDir.Text);
        ValidateContentDir();
    }

    private void btnSelectJSONDir_Click(object sender, EventArgs e)
    {
        rtxtJSONDir.Text = OpenDirectoryDialog(rtxtJSONDir.Text);
    }

    private void btnSelectOutputDir_Click(object sender, EventArgs e)
    {
        rtxtCookedDir.Text = OpenDirectoryDialog(rtxtJSONDir.Text);
    }

    private void btnScanAssets_Click(object sender, EventArgs e)
    {
        SetupGlobals();
        Task.Run(() => 
        {
            try 
            {
                lock(boolLock) isRunning = true;
                ToggleButtons();
                system.ScanAssetTypes();
                lock(boolLock) isRunning = false;
                ToggleButtons();
            }
            catch (Exception exception) 
            {
                OutputText(exception.ToString(), rtxtOutput);
                lock (boolLock) isRunning = false;
                ToggleButtons();
                return;
            }
            OutputText("Scanned assets!", rtxtOutput);
        });
    }

    private void btnMoveCookedAssets_Click(object sender, EventArgs e)
    {
        SetupGlobals();

        Task.Run(() =>
        {
            try
            {
                lock(boolLock) isRunning = true;
                ToggleButtons();
                system.GetCookedAssets();
                lock (boolLock) isRunning = false;
                ToggleButtons();
            }
            catch (Exception exception)
            {
                OutputText(exception.ToString(), rtxtOutput);
                lock (boolLock) isRunning = false;
                ToggleButtons();
                return;
            }
            OutputText("Moved assets!", rtxtOutput);
        });
    }

    private void btnSerializeAssets_Click(object sender, EventArgs e)
    {
        SetupGlobals();

        Task.Run(() =>
        {
            try
            {
                lock (boolLock) isRunning = true;
                ToggleButtons();
                system.SerializeAssets();
                lock(boolLock) isRunning = false;
                ToggleButtons();
            }
            catch (Exception exception) {
                OutputText(exception.ToString(), rtxtOutput);
                lock (boolLock) isRunning = false;
                ToggleButtons();
                return;
            }
            OutputText("Serialized assets!", rtxtOutput);
        });
    }

    private void btnOpenAllTypes_Click(object sender, EventArgs e)
    {
        OpenFile(rtxtInfoDir.Text + "\\AllTypes.txt");
    }

    private void btnOpenAssetTypes_Click(object sender, EventArgs e)
    {
        OpenFile(rtxtInfoDir.Text + "\\AssetTypes.json");
    }

    private void btnOpenLogs_Click(object sender, EventArgs e)
    {
        OpenFile(rtxtInfoDir.Text + "\\output_log.txt", true);
    }

    private void btnClearLogs_Click(object sender, EventArgs e)
    {
        EmptyLogFiles();
    }

    private void btnLoadConfig_Click(object sender, EventArgs e)
    {
        var configFile = OpenFileDialog(@"JSON files (*.json)|*.json", rtxtInfoDir.Text);
        if (!File.Exists(configFile))
        {
            OutputText("Please select a valid file!", rtxtOutput);
            return;
        }

        // TODO: Reload buggered settings when the catch is run (can't deep clone settings into temp because it can't be serialized)
        try
        {
            system.ClearLists(); // I have to do this or else the fucking lists get appended rather than set for some reason
            jsonsettings = JsonConvert.DeserializeObject<JSONSettings>(File.ReadAllText(configFile));
            LoadJSONSettings();
            AppSettings.Default.LastUsedCfg = configFile;
            AppSettings.Default.Save();
            OutputText("Loaded settings from: " + configFile, rtxtOutput);
        }
        catch (Exception exception)
        {
            OutputText("Please load in a valid config file!", rtxtOutput);
            OutputText(exception.ToString(), rtxtOutput);
        }
    }

    private void btnSaveConfig_Click(object sender, EventArgs e)
    {
        SetupGlobals();
        SaveJSONSettings();
    }

    private void btnSelectParseDir_Click(object sender, EventArgs e) 
    {
        rtxtParseDir.Text = OpenDirectoryDialog(rtxtParseDir.Text);
    }

    private void btnInfoDir_Click(object sender, EventArgs e)
    {
        rtxtInfoDir.Text = OpenDirectoryDialog(rtxtInfoDir.Text);
    }

    private void chkDumNativ_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void MainForm_Load(object sender, EventArgs e)
    {

        //panel1.Visible = false;
        if (AppSettings.Default.bAutoUseLastCfg == true)
        {
            AutoLoadConfig();
            chkAutoLoad.Checked = true;
        }
        chkSettDNS.Checked = AppSettings.Default.bDNSSave;

    }
    private void treeParseDir_MouseMove(object sender, MouseEventArgs e)
    {
           // Get the node at the current mouse pointer location.  
           TreeNode theNode = this.treeParseDir.GetNodeAt(e.X, e.Y);  
  
           // Set a ToolTip only if the mouse pointer is actually paused on a node.  
           if (theNode != null && theNode.Tag != null)  
           {  
               // Change the ToolTip only if the pointer moved to a new node.  
               if (theNode.Tag.ToString() != this.tTipTree.GetToolTip(this.treeParseDir))  
                   this.tTipTree.SetToolTip(this.treeParseDir, theNode.Tag.ToString());

           }  
           else     // Pointer is not over a node so clear the ToolTip.  
           {  
               this.tTipTree.SetToolTip(this.treeParseDir, "");
           }   
    }

    private void LoadFiles(string dir, TreeNode td)
    {
        string[] Files = Directory.GetFiles(dir, "*.uasset");

        // Loop through them to see files  
        foreach (string file in Files)
        {
            TreeNode tds = td.Nodes.Add(Path.GetFileName(file));
            tds.Tag = file;
            tds.StateImageIndex = 1;
            tds.Checked = td.Checked;
        }
    }

    private void LoadSubDirectories(string dir, TreeNode td)
    {
        // Get all subdirectories  
        string[] subdirectoryEntries = Directory.GetDirectories(dir);
        // Loop through them to see if they have any other subdirectories  
        foreach (string subdirectory in subdirectoryEntries)
        {
            TreeNode tds = td.Nodes.Add(Path.GetFileName(subdirectory));
            tds.StateImageIndex = 0;
            tds.Tag = subdirectory;
            tds.Checked = td.Checked; 
        }
    }

    public void LoadDirectory(string Dir)
    {
        treeParseDir.BeginUpdate();
        TreeNode tds = treeParseDir.Nodes.Add(Path.GetFileName(Dir));
        tds.Tag = Dir;
        tds.Checked = true;
        tds.StateImageIndex = 0;
        LoadSubDirectories(Dir, tds);
        LoadFiles(Dir, tds);
        treeParseDir.SelectedNode = treeParseDir.TopNode;
        treeParseDir.EndUpdate();
    }

    private void chkSettDNS_CheckedChanged(object sender, EventArgs e)
    {
        AppSettings.Default.bDNSSave = chkSettDNS.Checked;
        AppSettings.Default.Save();
    }

    private void chkAutoLoad_CheckedChanged(object sender, EventArgs e)
    {
        AppSettings.Default.bAutoUseLastCfg = chkAutoLoad.Checked;
        AppSettings.Default.Save();
    }

    private void btnDfltGamCnfg_Click(object sender, EventArgs e)
    {

    }

    private void btnCXXDir_Click(object sender, EventArgs e)
    {

    }

    private void rtxtDfltGamCnfg_TextChanged(object sender, EventArgs e)
    {

    }

    private void rtxtCXXDir_TextChanged(object sender, EventArgs e)
    {

    }

    private void cancelSerializationToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
        treeParseDir.BeginUpdate();
        treeParseDir.SelectedNode?.ExpandAll();
        treeParseDir.EndUpdate();
    }

    private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
        treeParseDir.BeginUpdate();
        treeParseDir.SelectedNode?.Collapse(false);
        treeParseDir.EndUpdate();
    }

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        treeParseDir.BeginUpdate();
        treeParseDir.SelectedNode?.Refresh();
        treeParseDir.EndUpdate();
    }

    private void clearAllPathsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void restorePathsToDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void cancelSerializationToolStripMenuItem_Click_1(object sender, EventArgs e)
    {

    }

    private void restoreAllSettingsToDefaultthisTabToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void restoreAllSettingsToDefaultallTabsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void treeParseDir_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        if (e.Action == TreeViewAction.Unknown) return;

        foreach (var node in e.Node.Children(false))
        {
            if (node.Nodes.Count == 0)
            {
                if (!((string)node.Tag).EndsWith("uasset"))
                {
                    LoadSubDirectories((string)node.Tag, node);
                    LoadFiles((string)node.Tag, node);
                }
            }
        }         
    }

    public void GatherCheckedFiles(object sender, EventArgs e)
    {
        system.Files = treeParseDir.TopNode?.GatherCheckedFiles() ?? new();
        Debug.WriteLine(system.Files.Count);
    }

    private void treeParseDir_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            // Get the node at the current mouse pointer location.  
            TreeNode theNode = treeParseDir.GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.  
            if (theNode != null)
            {
                theNode.TreeView.SelectedNode = theNode;
            }
            else
            {
                treeParseDir.SelectedNode = treeParseDir.TopNode;
            }

        }
    }
    private void treeParseDir_AfterCheck(object sender, TreeViewEventArgs e)
    {
        if (e.Action == TreeViewAction.Unknown) return;

        foreach (TreeNode n in e.Node.Children())
            n.Checked = e.Node.Checked;

        // Comment this if you don't need it.
        foreach (TreeNode p in e.Node.Parents())
            p.Checked = p.Nodes.OfType<TreeNode>().Any(n => n.Checked);
    }

    private void cntxtTreeParse_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (treeParseDir.SelectedNode == treeParseDir.TopNode)
        {
            if (cntxtTreeParse.Items[0].Text.EndsWith("All")) return;
            foreach (ToolStripItem menuitem in cntxtTreeParse.Items)
                menuitem.Text += " All";
        }
        else
        {
            if (!cntxtTreeParse.Items[0].Text.EndsWith("All")) return;
            foreach (ToolStripItem menuitem in cntxtTreeParse.Items)
                menuitem.Text = menuitem.Text.Replace(" All","");
        }
    }

    private void cntxtMainStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {

    }
}