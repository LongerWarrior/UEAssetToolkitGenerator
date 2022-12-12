namespace UAssetAPI;

public struct FAnimCurveType {
    public bool bMaterial;
    public bool bMorphtarget;

    public FAnimCurveType(bool bMaterial, bool bMorphtarget) {
        this.bMaterial = bMaterial;
        this.bMorphtarget = bMorphtarget;
    }
}

public struct FCurveMetaData {
    public FAnimCurveType Type;
    public FName[] LinkedBones;
    public byte MaxLOD;

    public void Read(AssetBinaryReader reader) {
        Type = new FAnimCurveType(reader.ReadIntBoolean(), reader.ReadIntBoolean());
        int length = reader.ReadInt32();
        List<FName> bones = new List<FName>(length);
        if (length > 0) {
            for (var i = 0; i < length; i++) {
                bones[i] = reader.ReadFName();
            }
        }
        MaxLOD = reader.ReadByte();
    }

}

public struct FBoneNode {
    public EBoneTranslationRetargetingMode TranslationRetargetingMode;

    public FBoneNode(EBoneTranslationRetargetingMode translationRetargetingMode) {
        TranslationRetargetingMode = translationRetargetingMode;
    }
}

public struct FMeshBoneInfo {
    public FName Name;
    public int ParentIndex;

    public FMeshBoneInfo(FName name, int parentIndex) {
        Name = name;
        ParentIndex = parentIndex;
    }
}

public struct FTrackToSkeletonMap
{
    public int BoneTreeIndex;
}