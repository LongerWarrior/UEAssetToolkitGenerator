using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using CookedAssetSerializer;
using Newtonsoft.Json;
using UAssetAPI;
using UAssetAPI.AssetRegistry;

namespace CookedAssetSerializerGUI;

public partial class Form1 : Form
{
    public Form1() {
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

    private Settings settings;

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

    private void OnClosed(object sender, FormClosedEventArgs e)
    {
        if (MessageBox.Show(@"Do you want to save?", @"Holup!",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            SetupGlobals();
            SaveSettings();
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
        rtxtContentDir.Text = Environment.CurrentDirectory;
        lastValidContentDir = Environment.CurrentDirectory;
        rtxtParseDir.Text = Environment.CurrentDirectory;
        rtxtJSONDir.Text = Environment.CurrentDirectory;
        rtxtOutputDir.Text = Environment.CurrentDirectory;
        rtxtInfoDir.Text = Environment.CurrentDirectory;

        cbUEVersion.Items.AddRange(versionOptionsKeys);
        cbUEVersion.SelectedIndex = 28; // This is a dumb thing to do, but oh well

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

    private void SetupGlobals()
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

        if (string.IsNullOrEmpty(rtxtOutputDir.Text)) 
        {
            rtxtOutputDir.Text = Path.Combine(Directory.GetParent(rtxtContentDir.Text)!.FullName, "Cooked");
            Directory.CreateDirectory(rtxtOutputDir.Text);
        }


        settings = new Settings
        {
            ContentDir = rtxtContentDir.Text,
            ParseDir = rtxtParseDir.Text,
            JSONDir = rtxtJSONDir.Text,
            CookedDir = rtxtOutputDir.Text,
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

        system = new CookedAssetSerializer.System(settings);
    }

    private void LoadSettings()
    {
        if (MessageBox.Show(@"Do you want to load in the file paths?", @"Holup!",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            rtxtContentDir.Text = settings.ContentDir;
            rtxtParseDir.Text = settings.ParseDir;
            rtxtJSONDir.Text = settings.JSONDir;
            rtxtOutputDir.Text = settings.CookedDir;
            rtxtInfoDir.Text = settings.InfoDir;
        }

        cbUEVersion.SelectedIndex = settings.SelectedIndex;
        chkRefreshAssets.Checked = settings.RefreshAssets;
        chkDummyWithProps.Checked = settings.DummyWithProps;
        SetupAssetsListBox(settings.SkipSerialization, lbAssetsToSkipSerialization);
        SetupAssetsListBox(settings.DummyAssets, lbDummyAssets);
        SetupAssetsListBox(settings.DeleteAssets, lbAssetsToDelete);
        rtxtCircularDependancy.Lines = settings.CircularDependency.ToArray();
        rtxtSimpleAssets.Lines = settings.SimpleAssets.ToArray();
        rtxtCookedAssets.Lines = settings.TypesToCopy.ToArray();
    }

    private void SaveSettings()
    {
        var output = JsonConvert.SerializeObject(settings, Formatting.Indented);

        var dialog = new SaveFileDialog();
        dialog.Filter = @"JSON files (*.json)|*.json";
        dialog.Title = @"Save current settings";
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() != DialogResult.OK) return;
        if (dialog.FileName == "") return;
        File.WriteAllText(dialog.FileName, output);
        OutputText("Saved settings to: " + dialog.FileName, rtxtOutput);
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

        // I don't know why, I don't know how, but doing just Process.Start(path) doesn't fucking work,
        // even though that's the preferred option since it opens whatever editor is associated with the file extension
        Process.Start("notepad.exe", path);
    }

    private string OpenDirectoryDialog(string path) 
    {
        var fbd = new FolderBrowserDialog();
        if (Directory.Exists(path)) fbd.SelectedPath = path + @"\";
        if (fbd.ShowDialog() == DialogResult.Cancel) return path;
        return fbd.SelectedPath;
    }

    private string OpenFileDialog(string filter)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = filter;
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
        if (string.IsNullOrEmpty(rtxtContentDir.Text) || !Directory.Exists(rtxtContentDir.Text))
        {
            rtxtContentDir.Text = lastValidContentDir;
        }
        lastValidContentDir = rtxtContentDir.Text;
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
    }

    private void btnSelectJSONDir_Click(object sender, EventArgs e)
    {
        rtxtJSONDir.Text = OpenDirectoryDialog(rtxtJSONDir.Text);
    }

    private void btnSelectOutputDir_Click(object sender, EventArgs e)
    {
        rtxtOutputDir.Text = OpenDirectoryDialog(rtxtJSONDir.Text);
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
        var configFile = OpenFileDialog(@"JSON files (*.json)|*.json");
        if (!File.Exists(configFile))
        {
            OutputText("Please select a valid file!", rtxtOutput);
            return;
        }

        // TODO: Reload buggered settings when the catch is run (can't deep clone settings into temp because it can't be serialized)
        try
        {
            system.ClearLists(); // I have to do this or else the fucking lists get appended rather than set for some reason
            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(configFile));
            LoadSettings();
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
        SaveSettings();
    }

    private void btnSelectParseDir_Click(object sender, EventArgs e) {
        rtxtParseDir.Text = OpenDirectoryDialog(rtxtParseDir.Text);
    }

    private void btnInfoDir_Click(object sender, EventArgs e)
    {
        rtxtInfoDir.Text = OpenDirectoryDialog(rtxtInfoDir.Text);
    }

    private void btnScanAR_Click(object sender, EventArgs e) {


        SetupGlobals();
        Task.Run(() => {

            try {
                lock (boolLock) isRunning = true;
                EnableButtons();

                var AR = new FAssetRegistryState(@"D:\FSDTest\FSD\AssetRegistry.bin", settings.GlobalUEVersion);

                ARData.AssetList = new Dictionary<string, AssetData>(AR.PreallocatedAssetDataBuffers.Length);
                foreach (var data in AR.PreallocatedAssetDataBuffers) {            
                    if (data.PackageName.ToName().StartsWith("/Game")) {
                        ARData.AssetList[data.PackageName.ToName().ToLower()] = new AssetData(data.AssetClass, data.AssetName, data.TagsAndValues);
                    }
                }
                AR = null;
                GC.Collect();

                lock (boolLock) isRunning = false;
                EnableButtons();
            }
            catch (Exception exception) {
                OutputText(exception.ToString());
                lock (boolLock) isRunning = false;
                EnableButtons();
                return;
            }
            OutputText("Scanned AR!");
        });
    }
}