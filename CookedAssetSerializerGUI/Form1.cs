using System.Diagnostics;
using System.Runtime.InteropServices;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Globals;

namespace CookedAssetSerializerGUI;

public partial class Form1 : Form {
    public Form1() {
        InitializeComponent();
        setupForm();
    }

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

    #region Vars
    private object[] versionOptionsKeys = {
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

    private UE4Version[] versionOptionsValues = {
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

    private void setupForm() {
        // Temporary inputs
        rtxtContentDir.Text = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\cyubeVR-WindowsNoEditor\cyubeVR\Content";
        rtxtJSONDir.Text = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON";
        rtxtOutputDir.Text = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON\Output";

        cbUEVersion.Items.AddRange(versionOptionsKeys);
        cbUEVersion.SelectedIndex = 28;

        List<EAssetType> defaultAssets = new() {
            EAssetType.BlendSpaceBase,
            EAssetType.AnimSequence,
            EAssetType.SkeletalMesh,
            EAssetType.Skeleton,
            EAssetType.AnimMontage,
            EAssetType.FileMediaSource,
            EAssetType.StaticMesh,
        };
        lbAssetsToSkipSerialization.DataSource = Enum.GetValues(typeof(EAssetType));
        foreach (var asset in defaultAssets) {
            lbAssetsToSkipSerialization.SetSelected(lbAssetsToSkipSerialization.FindString(asset.ToString()), true);
        }
    }

    private void emptyLogFiles() {
        string[] files = {
            "debug_log.txt",
            "output_log.txt"
        };
        foreach (string file in files) {
            string path = rtxtJSONDir.Text + "\\" + file;
            if (File.Exists(path)) {
                File.Delete(path);
                outputText("Cleared log file: " + path);
            }
        }
    }

    private string[] sanitiseInputs(string[] lines) {
        for (int i = 0; i < lines.Length; i++) {
            if (!lines[i].Contains("/Script/")) {
                lines[i] = lines[i].Insert(0, "/Script/");
            }

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

        CONTENT_DIR = rtxtContentDir.Text;
        OUTPUT_DIR = rtxtOutputDir.Text;
        TYPES_TO_COPY = typesToCopy;
        SIMPLE_ASSETS = simpleAssets;
        SKIP_SERIALIZATION = assetsToSkip;
        CIRCULAR_DEPENDENCY = circularDependencies;
        GLOBAL_UE_VERSION = versionOptionsValues[cbUEVersion.SelectedIndex];
        REFRESH_ASSETS = rbRefreshAssets.Checked; // This may not be required
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
        Process.Start("notepad.exe", path);
    }

    private void btnSelectContentDir_Click(object sender, EventArgs e) {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        fbd.ShowDialog();
        rtxtContentDir.Text = fbd.SelectedPath;
    }

    private void btnSelectJSONDir_Click(object sender, EventArgs e) {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        fbd.ShowDialog();
        rtxtJSONDir.Text = fbd.SelectedPath;
    }

    private void btnSelectOutputDir_Click(object sender, EventArgs e) {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        fbd.ShowDialog();
        rtxtOutputDir.Text = fbd.SelectedPath;
    }

    private void btnScanAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        disableButtons();
        ScanAssetTypes();
        enableButtons();

        outputText("Scanned assets!");
    }

    private void btnMoveCookedAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        Task.Run(() => {
            disableButtons();
            try {
                MoveAssets();
            } catch (Exception exception) {
                // outputText("\n" + exception.ToString()); // TODO: Why the fuck does this not work lol?
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
                SerializeAssets();
            } catch (Exception exception) {
                rtxtOutput.Text += Environment.NewLine + exception;
                return;
            }
            enableButtons();

            outputText("Serialized assets!");
        });
    }

    private void btnOpenAllTypes_Click(object sender, EventArgs e) {
        openFile(JSON_DIR + "\\AllTypes.txt");
    }

    private void btnOpenAssetTypes_Click(object sender, EventArgs e) {
        openFile(JSON_DIR + "\\AssetTypes.json");
    }

    private void btnOpenLogs_Click(object sender, EventArgs e) {
        openFile(JSON_DIR + "\\output_log.txt", true);
    }

    private void btnClearLogs_Click(object sender, EventArgs e) {
        emptyLogFiles();
    }
}
