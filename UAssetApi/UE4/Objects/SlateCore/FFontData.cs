namespace UAssetAPI.StructTypes; 

public class FFontData  {
    public FPackageIndex LocalFontFaceAsset; // UObject
    public FString FontFilename;
    public EFontHinting Hinting;
    public EFontLoadingPolicy LoadingPolicy;
    public int SubFaceIndex;
    public bool bIsCooked;

    public FFontData(AssetBinaryReader reader) {
        if (reader.Asset.GetCustomVersion<FEditorObjectVersion>() < FEditorObjectVersion.AddedFontFaceAssets) return;

        bIsCooked = reader.ReadIntBoolean();
        if (bIsCooked) {
            LocalFontFaceAsset = reader.XFER_OBJECT_POINTER();

            if (LocalFontFaceAsset.Index == 0) {
                FontFilename = reader.ReadFString();
                Hinting =(EFontHinting)reader.ReadByte();
                LoadingPolicy = (EFontLoadingPolicy)reader.ReadByte();
            }

            SubFaceIndex = reader.ReadInt32();
        }
    }

    public void Write(AssetBinaryWriter writer) {
        if (writer.Asset.GetCustomVersion<FEditorObjectVersion>() < FEditorObjectVersion.AddedFontFaceAssets) return;

        writer.Write(bIsCooked?1:0);
        if (bIsCooked) {
            writer.XFER_OBJECT_POINTER(LocalFontFaceAsset);

            if (LocalFontFaceAsset.Index == 0) {
                writer.Write(FontFilename);
                writer.Write((byte)Hinting);
                writer.Write((byte)LoadingPolicy);
                
            }

            writer.Write(SubFaceIndex);
        }
    }

}
