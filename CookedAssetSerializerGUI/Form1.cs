using System.Diagnostics;
using System.Runtime.InteropServices;
using CookedAssetSerializer;
using Newtonsoft.Json;
using UAssetAPI;
using Timer = System.Windows.Forms.Timer;

namespace CookedAssetSerializerGUI;

public partial class Form1 : Form {
    public Form1() {
        InitializeComponent();
        setupForm();
        setupGlobals();
        isSetupFinished = true;

        //Task.Run(eventLoop);
    }

    #region Vars
    private Globals settings;

    private readonly bool[] isSaved = {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
    };

    private readonly bool isSetupFinished;

    private volatile bool isRunning;

    private readonly object[] versionOptionsKeys = {
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

    private readonly UE4Version[] versionOptionsValues = {
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
        UE4Version.VER_UE4_27,
    };
    #endregion

    #region CustomFormSetup
    private const int HT_CAPTION = 0x2;
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern bool ReleaseCapture();
    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    protected override void OnMouseDown(MouseEventArgs e) {
        if (e.Button != MouseButtons.Left) return;
        Rectangle rct = DisplayRectangle;
        if (!rct.Contains(e.Location)) return;
        ReleaseCapture();
        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }
    #endregion

    private void OnClosed(object sender, FormClosedEventArgs e) {
        if (isSaved.All(status => status)) return;
        if (MessageBox.Show(@"Your changes have not been saved! Do you want to save?", @"Holup!",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        setupGlobals();
        saveSettings();
    }

    private async Task eventLoop() {
        while (true) {
            if (isRunning) {
                // TODO: This can only work if Utils is refactored to be an instanced object rather than static (low priority)
                lblProgress.Text = settings.GetAssetCount() / settings.GetAssetTotal() * 100 + @"%";
            }
            await Task.Delay(500);
        }
    }

    private void setupAssetsToSkipSerialization(List<EAssetType> assets) {
        lbAssetsToSkipSerialization.DataSource = Enum.GetValues(typeof(EAssetType));
        bool hasBP = false;
        foreach (var asset in assets) {
            lbAssetsToSkipSerialization.SetSelected(lbAssetsToSkipSerialization.FindString(asset.ToString()), true);
            // Temporary awful hack until I get loading for lbAssetsToSkipSerialization data working
            if (asset == EAssetType.Blueprint) hasBP = true;
        }
        lbAssetsToSkipSerialization.SetSelected(0, hasBP);
    }

    private void setupForm() {
        rtxtContentDir.Text = Environment.CurrentDirectory;
        rtxtJSONDir.Text = Environment.CurrentDirectory;
        rtxtOutputDir.Text = Environment.CurrentDirectory;

        cbUEVersion.Items.AddRange(versionOptionsKeys);
        cbUEVersion.SelectedIndex = 28; // This is a dumb thing to do, but oh well

        List<EAssetType> defaultAssets = new() {
            EAssetType.BlendSpaceBase,
            EAssetType.AnimSequence,
            EAssetType.SkeletalMesh,
            EAssetType.Skeleton,
            EAssetType.AnimMontage,
            EAssetType.FileMediaSource,
            EAssetType.StaticMesh,
        };
        setupAssetsToSkipSerialization(defaultAssets);
        // For some stupid reason, the first item in the lb is always enabled, which in this case, is the Blueprint,
        // which is the absolute worst time for this """feature""" to happen
        lbAssetsToSkipSerialization.SetSelected(0, false);

        isSaved[0] = true;
    }

    private string[] sanitiseInputs(string[] lines) {
        for (int i = 0; i < lines.Length; i++) {
            if (!lines[i].Contains("/Script/")) {
                lines[i] = lines[i].Insert(0, "/Script/");
            }

            // This garbage allows us to copy and paste text from the text files
            // and not have to muck about deleting them manually
            lines[i] = lines[i].Replace('"'.ToString(), "");
            lines[i] = lines[i].Replace(','.ToString(), "");
        }

        return lines;
    }

    private void setupGlobals() {
        List<string> typesToCopy = new List<string>();
        typesToCopy.AddRange(sanitiseInputs(rtxtCookedAssets.Lines));
        List<string> simpleAssets = new List<string>();
        simpleAssets.AddRange(sanitiseInputs(rtxtSimpleAssets.Lines));
        List<EAssetType> assetsToSkip = new List<EAssetType>();
        assetsToSkip.AddRange(lbAssetsToSkipSerialization.SelectedItems.Cast<EAssetType>());
        List<string> circularDependencies = new List<string>();
        circularDependencies.AddRange(sanitiseInputs(rtxtCircularDependancy.Lines));

        settings = new Globals(rtxtContentDir.Text, rtxtJSONDir.Text, rtxtOutputDir.Text,
            versionOptionsValues[cbUEVersion.SelectedIndex], cbUEVersion.SelectedIndex,
            chkRefreshAssets.Checked, assetsToSkip, circularDependencies, simpleAssets, typesToCopy);
    }

    private void loadSettings() {
        if (MessageBox.Show(@"Do you want to load in the file paths?", @"Holup!",
                MessageBoxButtons.YesNo) == DialogResult.Yes) {
            rtxtContentDir.Text = settings.GetContentDir();
            rtxtJSONDir.Text = settings.GetJSONDir();
            rtxtOutputDir.Text = settings.GetOutputDir();
        }
        cbUEVersion.SelectedIndex = settings.GetSelectedIndex();
        chkRefreshAssets.Checked = settings.GetRefreshAssets();
        setupAssetsToSkipSerialization(settings.GetSkipSerialization());
        rtxtCircularDependancy.Lines = settings.GetCircularDependency().ToArray();
        rtxtSimpleAssets.Lines = settings.GetSimpleAssets().ToArray();
        rtxtCookedAssets.Lines = settings.GetTypesToCopy().ToArray();
        isSaved[0] = true;
    }

    private void saveSettings() {
        string output = JsonConvert.SerializeObject(settings, Formatting.Indented);

        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = @"JSON files (*.json)|*.json";
        dialog.Title = @"Save current settings";
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() != DialogResult.OK) return;
        if (dialog.FileName == "") return;
        File.WriteAllText(dialog.FileName, output);
        outputText("Saved settings to: " + dialog.FileName);
        isSaved[0] = true;
    }

    private void disableButtons() {
        btnScanAssets.Enabled = false;
        btnMoveCookedAssets.Enabled = false;
        btnSerializeAssets.Enabled = false;
    }

    private void enableButtons() {
        btnScanAssets.Enabled = true;
        btnMoveCookedAssets.Enabled = true;
        btnSerializeAssets.Enabled = true;
    }

    private void outputText(string text) {
        if (rtxtOutput.TextLength == 0) rtxtOutput.Text += text;
        else rtxtOutput.Text += Environment.NewLine + text;
    }

    private void openFile(string path, bool bIsLog = false) {
        if (!File.Exists(path)) {
            if (!bIsLog) outputText("You need to scan the assets first!");
            return;
        }

        // I don't know why, I don't know how, but doing just Process.Start(path) doesn't fucking work,
        // even though that's the preferred option since it opens whatever editor is associated with the file extension
        Process.Start("notepad.exe", path);
    }

    private string openDirectoryDialog() {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        fbd.ShowDialog();
        return fbd.SelectedPath;
    }

    private string openFileDialog(string filter) {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = filter;
        dialog.RestoreDirectory = true;
        return dialog.ShowDialog() != DialogResult.OK ? "" : dialog.FileName;
    }

    private void emptyLogFiles() {
        string[] files = {
            "debug_log.txt",
            "output_log.txt"
        };
        foreach (string file in files) {
            string path;
            if (rtxtJSONDir.Text.EndsWith("\\")) path = rtxtJSONDir.Text + file;
            else path = rtxtJSONDir.Text + "\\" + file;
            if (!File.Exists(path)) continue;
            File.Delete(path);
            outputText("Cleared log file: " + path);
        }
    }

    private void btnSelectContentDir_Click(object sender, EventArgs e) {
        rtxtContentDir.Text = openDirectoryDialog();
    }

    private void btnSelectJSONDir_Click(object sender, EventArgs e) {
        rtxtJSONDir.Text = openDirectoryDialog();
    }

    private void btnSelectOutputDir_Click(object sender, EventArgs e) {
        rtxtOutputDir.Text = openDirectoryDialog();
    }

    private void btnScanAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        Task.Run(() => {
            disableButtons();
            try {
                isRunning = true;
                settings.ScanAssetTypes();
                isRunning = false;
            } catch (Exception exception) {
                rtxtOutput.Text += Environment.NewLine + exception;
                return;
            }
            enableButtons();

            outputText("Scanned assets!");
        });
    }

    private void btnMoveCookedAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        Task.Run(() => {
            disableButtons();
            try {
                settings.GetCookedAssets();
            } catch (Exception exception) {
                rtxtOutput.Text += Environment.NewLine + exception;
                return;
            }
            enableButtons();

            outputText("Moved assets!");
        });
    }

    private void btnSerializeAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        Task.Run(() => {
            disableButtons();
            try {
                settings.SerializeAssets();
            } catch (Exception exception) {
                rtxtOutput.Text += Environment.NewLine + exception;
                return;
            }
            enableButtons();

            outputText("Serialized assets!");
        });
    }

    private void btnOpenAllTypes_Click(object sender, EventArgs e) {
        openFile(rtxtJSONDir.Text + "\\AllTypes.txt");
    }

    private void btnOpenAssetTypes_Click(object sender, EventArgs e) {
        openFile(rtxtJSONDir.Text + "\\AssetTypes.json");
    }

    private void btnOpenLogs_Click(object sender, EventArgs e) {
        openFile(rtxtJSONDir.Text + "\\output_log.txt", true);
    }

    private void btnClearLogs_Click(object sender, EventArgs e) {
        emptyLogFiles();
    }

    private void btnLoadConfig_Click(object sender, EventArgs e) {
        string configFile = openFileDialog(@"JSON files (*.json)|*.json");
        if (!File.Exists(configFile)) {
            outputText("Please select a valid file!");
            return;
        }

        // TODO: Reload buggered settings when the catch is run (can't deep clone settings into temp because it can't be serialized)
        try {
            settings.ClearLists(); // I have to do this or else the fucking lists get appended rather than set for some reason
            settings = JsonConvert.DeserializeObject<Globals>(File.ReadAllText(configFile));
            loadSettings();
            outputText("Loaded settings from: " + configFile);
        } catch (Exception exception) {
            outputText("Please load in a valid config file!");
            outputText(exception.ToString());
        }
    }

    private void btnSaveConfig_Click(object sender, EventArgs e) {
        setupGlobals();
        saveSettings();
    }

    private void rtxtContentDir_TextChanged(object sender, EventArgs e) {
        isSaved[1] = rtxtContentDir.Text != settings.GetContentDir();
    }

    private void rtxtJSONDir_TextChanged(object sender, EventArgs e) {
        isSaved[2] = rtxtJSONDir.Text != settings.GetJSONDir();
    }

    private void rtxtOutputDir_TextChanged(object sender, EventArgs e) {
        isSaved[3] = rtxtOutputDir.Text != settings.GetOutputDir();
    }

    private void lbAssetsToSkipSerialization_SelectedIndexChanged(object sender, EventArgs e) {
        // TODO: Fix
        //isSaved[4] = lbAssetsToSkipSerialization.SelectedItems.Cast<EAssetType>() != settings.GetSkipSerialization().ToArray();
    }

    private void rtxtSimpleAssets_TextChanged(object sender, EventArgs e) {
        isSaved[5] = rtxtSimpleAssets.Lines == settings.GetSimpleAssets().ToArray();
    }

    private void rtxtCookedAssets_TextChanged(object sender, EventArgs e) {
        isSaved[6] = rtxtCookedAssets.Lines != settings.GetTypesToCopy().ToArray();
    }

    private void rtxtCircularDependancy_TextChanged(object sender, EventArgs e) {
        isSaved[7] = rtxtCircularDependancy.Lines != settings.GetCircularDependency().ToArray();
    }

    private void chkRefreshAssets_CheckedChanged(object sender, EventArgs e)
    {
        isSaved[8] = chkRefreshAssets.Checked != settings.GetRefreshAssets();
    }

    private void cbUEVersion_SelectedIndexChanged(object sender, EventArgs e) {
        if (!isSetupFinished) return;
        isSaved[9] = cbUEVersion.SelectedIndex != settings.GetSelectedIndex();
    }
}
