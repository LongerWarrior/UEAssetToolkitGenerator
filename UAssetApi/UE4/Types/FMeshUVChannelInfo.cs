namespace UAssetAPI;

public struct FMeshUVChannelInfo {
    public bool bInitialized;
    public bool bOverrideDensities;
    public float[] LocalUVDensities;
    const int TEXSTREAM_MAX_NUM_UVCHANNELS = 4;

    public FMeshUVChannelInfo(AssetBinaryReader reader) {
        bInitialized = reader.ReadInt32()!=0;
        bOverrideDensities = reader.ReadInt32()!=0;
        LocalUVDensities = new float[TEXSTREAM_MAX_NUM_UVCHANNELS];
        for (int i = 0; i < TEXSTREAM_MAX_NUM_UVCHANNELS; i++) {
            LocalUVDensities[i] = reader.ReadSingle();
        }
    }
}
