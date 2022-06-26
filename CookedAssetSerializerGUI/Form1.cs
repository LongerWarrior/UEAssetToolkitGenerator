using System.Runtime.InteropServices;
using System.Text;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Globals;
using UAssetAPI;

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

    private void setupForm() {
        // Temporary inputs
        rtxtContentDir.Text = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\cyubeVR-WindowsNoEditor\cyubeVR\Content";
        rtxtJSONDir.Text = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON";
        rtxtOutputDir.Text = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON\Output";

        for (int i = 0; i < 27; i++) {
            cbUEVersion.Items.Add(UE4Version.VER_UE4_0 + i);
        }

        List<EAssetType> defaultAssets = new() {
            EAssetType.BlendSpaceBase,
            EAssetType.AnimSequence,
            EAssetType.SkeletalMesh,
            EAssetType.Skeleton,
            EAssetType.AnimMontage,
            EAssetType.FileMediaSource,
            EAssetType.StaticMesh,
        };
        foreach (var asset in Enum.GetValues(typeof(EAssetType))) {
            lbAssetsToSkipSerialization.Items.Add(asset);
        }
        foreach (var asset in defaultAssets) {
            lbAssetsToSkipSerialization.SetSelected(lbAssetsToSkipSerialization.FindString(asset.ToString()), true);
        }
    }

    private void setupGlobals() {
        List<string> typesToCopy = new List<string>();
        typesToCopy.AddRange(rtxtCookedAssets.Lines);
        List<string> simpleAssets = new List<string>();
        simpleAssets.AddRange(rtxtSimpleAssets.Lines);
        List<EAssetType> assetsToSkip = new List<EAssetType>();
        assetsToSkip.AddRange(lbAssetsToSkipSerialization.SelectedItems.Cast<EAssetType>());
        List<string> circularDependencies = new List<string>();
        circularDependencies.AddRange(rtxtCircularDependancy.Lines);
        CONTENT_DIR = rtxtContentDir.Text;
        OUTPUT_DIR = rtxtOutputDir.Text;
        TYPES_TO_COPY = typesToCopy;
        SIMPLE_ASSETS = simpleAssets;
        SKIP_SERIALIZATION = assetsToSkip;
        CIRCULAR_DEPENDENCY = circularDependencies;
        GLOBAL_UE_VERSION = (UE4Version)(cbUEVersion.SelectedItem ?? UE4Version.VER_UE4_27);
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

        rtxtOutput.Text += "\nScanned assets!";
    }

    private void btnMoveCookedAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        Task.Run(() => {
            disableButtons();
            try {
                MoveAssets();
            } catch (Exception exception) {
                rtxtOutput.Text += "\n" + exception;
            }
            enableButtons();

            rtxtOutput.Text += "\nMoved assets!";
        });
    }

    private void btnSerializeAssets_Click(object sender, EventArgs e) {
        setupGlobals();

        Task.Run(() => {
            disableButtons();
            try {
                SerializeAssets();
            } catch (Exception exception) {
                rtxtOutput.Text += "\n" + exception;
                throw;
            }
            enableButtons();

            rtxtOutput.Text += "\nSerialized assets!";
        });
    }
}
