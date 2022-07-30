using System.Diagnostics;
using System.Runtime.InteropServices;
using CookedAssetSerializer;
using Newtonsoft.Json;
using UAssetAPI;

namespace CookedAssetSerializerGUI;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        SetupForm();
        SetupGlobals();

        Task.Run(EventLoop);
    }

    #region Vars

    private CookedAssetSerializer.System system;

    private Settings settings;

    private volatile bool isRunning;

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
            if (isRunning)
            {
                rtxtOutput.Text += system.GetAssetCount();
                lblProgress.Text = system.GetAssetCount() / system.GetAssetTotal() * 100 + @"%";
            }

            await Task.Delay(500);
        }
    }

    private void SetupAssetsToSkipSerialization(List<EAssetType> assets)
    {
        lbAssetsToSkipSerialization.DataSource = Enum.GetValues(typeof(EAssetType));
        var hasBP = false;
        foreach (var asset in assets)
        {
            lbAssetsToSkipSerialization.SetSelected(lbAssetsToSkipSerialization.FindString(asset.ToString()), true);
            if (asset == EAssetType.Blueprint) hasBP = true;
        }

        // For some stupid reason, the first item in the lb is always enabled, which in this case, is the Blueprint,
        // which is the absolute worst time for this """feature""" to happen
        lbAssetsToSkipSerialization.SetSelected(0, hasBP);
    }

    private void SetupForm()
    {
        rtxtContentDir.Text = Environment.CurrentDirectory;
        rtxtJSONDir.Text = Environment.CurrentDirectory;
        rtxtOutputDir.Text = Environment.CurrentDirectory;

        cbUEVersion.Items.AddRange(versionOptionsKeys);
        cbUEVersion.SelectedIndex = 28; // This is a dumb thing to do, but oh well

        List<EAssetType> defaultAssets = new()
        {
            EAssetType.BlendSpaceBase,
            EAssetType.AnimSequence,
            EAssetType.SkeletalMesh,
            EAssetType.Skeleton,
            EAssetType.AnimMontage,
            EAssetType.FileMediaSource,
            EAssetType.StaticMesh
        };
        SetupAssetsToSkipSerialization(defaultAssets);
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
        var circularDependencies = new List<string>();
        circularDependencies.AddRange(SanitiseInputs(rtxtCircularDependancy.Lines));

        settings = new Settings
        {
            ContentDir = rtxtContentDir.Text,
            JSONDir = rtxtJSONDir.Text,
            OutputDir = rtxtOutputDir.Text,
            GlobalUEVersion = versionOptionsValues[cbUEVersion.SelectedIndex],
            RefreshAssets = chkRefreshAssets.Checked,
            SkipSerialization = assetsToSkip,
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
            rtxtJSONDir.Text = settings.JSONDir;
            rtxtOutputDir.Text = settings.OutputDir;
        }

        cbUEVersion.SelectedIndex = settings.SelectedIndex;
        chkRefreshAssets.Checked = settings.RefreshAssets;
        SetupAssetsToSkipSerialization(settings.SkipSerialization);
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
        OutputText("Saved settings to: " + dialog.FileName);
    }

    private void DisableButtons()
    {
        btnScanAssets.Enabled = false;
        btnMoveCookedAssets.Enabled = false;
        btnSerializeAssets.Enabled = false;
        btnClearLogs.Enabled = false;
        btnLoadConfig.Enabled = false;
    }

    private void EnableButtons()
    {
        btnScanAssets.Enabled = true;
        btnMoveCookedAssets.Enabled = true;
        btnSerializeAssets.Enabled = true;
        btnClearLogs.Enabled = true;
        btnLoadConfig.Enabled = true;
    }

    private void OutputText(string text)
    {
        if (rtxtOutput.TextLength == 0) rtxtOutput.Text += text;
        else rtxtOutput.Text += Environment.NewLine + text;
    }

    private void OpenFile(string path, bool bIsLog = false)
    {
        if (!File.Exists(path))
        {
            if (!bIsLog) OutputText("You need to scan the assets first!");
            return;
        }

        // I don't know why, I don't know how, but doing just Process.Start(path) doesn't fucking work,
        // even though that's the preferred option since it opens whatever editor is associated with the file extension
        Process.Start("notepad.exe", path);
    }

    private string OpenDirectoryDialog()
    {
        var fbd = new FolderBrowserDialog();
        fbd.ShowDialog();
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
            "output_log.txt"
        };
        foreach (var file in files)
        {
            string path;
            if (rtxtJSONDir.Text.EndsWith("\\")) path = rtxtJSONDir.Text + file;
            else path = rtxtJSONDir.Text + "\\" + file;
            if (!File.Exists(path)) continue;
            File.Delete(path);
            OutputText("Cleared log file: " + path);
        }
    }

    private void btnSelectContentDir_Click(object sender, EventArgs e)
    {
        rtxtContentDir.Text = OpenDirectoryDialog();
    }

    private void btnSelectJSONDir_Click(object sender, EventArgs e)
    {
        rtxtJSONDir.Text = OpenDirectoryDialog();
    }

    private void btnSelectOutputDir_Click(object sender, EventArgs e)
    {
        rtxtOutputDir.Text = OpenDirectoryDialog();
    }

    private void btnScanAssets_Click(object sender, EventArgs e)
    {
        SetupGlobals();

        Task.Run(() =>
        {
            DisableButtons();
            try
            {
                isRunning = true;
                system.ScanAssetTypes();
                isRunning = false;
            }
            catch (Exception exception)
            {
                rtxtOutput.Text += Environment.NewLine + exception;
                return;
            }

            EnableButtons();

            OutputText("Scanned assets!");
        });
    }

    private void btnMoveCookedAssets_Click(object sender, EventArgs e)
    {
        SetupGlobals();

        Task.Run(() =>
        {
            DisableButtons();
            try
            {
                system.GetCookedAssets();
            }
            catch (Exception exception)
            {
                rtxtOutput.Text += Environment.NewLine + exception;
                return;
            }

            EnableButtons();

            OutputText("Moved assets!");
        });
    }

    private void btnSerializeAssets_Click(object sender, EventArgs e)
    {
        SetupGlobals();

        //Task.Run(() =>
        //{
            DisableButtons();
            system.SerializeAssets();
            // try
            // {
            //     system.SerializeAssets();
            // }
            // catch (Exception exception)
            // {
            //     rtxtOutput.Text += Environment.NewLine + exception;
            //     return;
            // }

            EnableButtons();

            OutputText("Serialized assets!");
        //});
    }

    private void btnOpenAllTypes_Click(object sender, EventArgs e)
    {
        OpenFile(rtxtJSONDir.Text + "\\AllTypes.txt");
    }

    private void btnOpenAssetTypes_Click(object sender, EventArgs e)
    {
        OpenFile(rtxtJSONDir.Text + "\\AssetTypes.json");
    }

    private void btnOpenLogs_Click(object sender, EventArgs e)
    {
        OpenFile(rtxtJSONDir.Text + "\\output_log.txt", true);
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
            OutputText("Please select a valid file!");
            return;
        }

        // TODO: Reload buggered settings when the catch is run (can't deep clone settings into temp because it can't be serialized)
        try
        {
            system
                .ClearLists(); // I have to do this or else the fucking lists get appended rather than set for some reason
            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(configFile));
            LoadSettings();
            OutputText("Loaded settings from: " + configFile);
        }
        catch (Exception exception)
        {
            OutputText("Please load in a valid config file!");
            OutputText(exception.ToString());
        }
    }

    private void btnSaveConfig_Click(object sender, EventArgs e)
    {
        SetupGlobals();
        SaveSettings();
    }
}