using UAssetApi.ExportTypes;

namespace UAssetAPI;

public class AnimSequenceExport : AnimSequenceBaseExport
{
    /*public int NumFrames;
    public FTrackToSkeletonMap[] TrackToSkeletonMapTable; // used for raw data
    public FRawAnimSequenceTrack[] RawAnimationData;
    public ResolvedObject? BoneCompressionSettings; // UAnimBoneCompressionSettings
    public ResolvedObject? CurveCompressionSettings; // UAnimCurveCompressionSettings
    
    public FTrackToSkeletonMap[] CompressedTrackToSkeletonMapTable; // used for compressed data, missing before 4.12
    public FSmartName[] CompressedCurveNames;
    //public byte[] CompressedByteStream; The actual data will be in CompressedDataStructure, no need to store as field
    public byte[]? CompressedCurveByteStream;
    public FRawCurveTracks CompressedCurveData; // disappeared in 4.23
    public ICompressedAnimData CompressedDataStructure;
    public UAnimBoneCompressionCodec? BoneCompressionCodec;
    public UAnimCurveCompressionCodec? CurveCompressionCodec;
    public int CompressedRawDataSize;

    public EAdditiveAnimationType AdditiveAnimType;
    public EAdditiveBasePoseType RefPoseType;
    public ResolvedObject? RefPoseSeq;
    public int RefFrameIndex;
    public FName RetargetSource;
    public FTransform[]? RetargetSourceAssetReferencePose;

    public bool bUseRawDataOnly;
    */
    public AnimSequenceExport(Export super) : base(super) { }
    
    public AnimSequenceExport() { }
    
    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);
    }
}