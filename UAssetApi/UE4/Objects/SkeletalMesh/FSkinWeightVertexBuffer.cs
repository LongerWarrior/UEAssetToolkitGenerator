namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkinWeightVertexBuffer
{
    private const int _NUM_INFLUENCES_UE4 = 4;

    public readonly FSkinWeightInfo[] Weights;

    public FSkinWeightVertexBuffer(AssetBinaryReader reader, bool numSkelCondition)
    {
        var bNewWeightFormat = reader.Asset.GetCustomVersion<FAnimObjectVersion>() >= FAnimObjectVersion.UnlimitedBoneInfluences;

        #region FSkinWeightDataVertexBuffer
        var dataStripFlags =  new FStripDataFlags(reader);

        #region FSkinWeightDataVertexBuffer::SerializeMetaData
        bool bVariableBonesPerVertex;
        bool bExtraBoneInfluences;
        uint maxBoneInfluences;
        bool bUse16BitBoneIndex;
        uint numVertices;
        uint numBones;

        if (!reader["SkeletalMesh.UseNewCookedFormat"])
        {
            bExtraBoneInfluences = reader.ReadIntBoolean();
            numVertices = reader.ReadUInt32();
            maxBoneInfluences = bExtraBoneInfluences ? 8u : 4u;
        }
        else if (!bNewWeightFormat)
        {
            bExtraBoneInfluences = reader.ReadIntBoolean();
            if (reader.Asset.GetCustomVersion<FSkeletalMeshCustomVersion>() >= FSkeletalMeshCustomVersion.SplitModelAndRenderData)
            {
                reader.BaseStream.Position += 4; // var stride = Ar.Read<uint>();
            }
            numVertices = reader.ReadUInt32();
            maxBoneInfluences = bExtraBoneInfluences ? 8u : 4u;
            numBones = maxBoneInfluences * numVertices;
            bVariableBonesPerVertex = false;
        }
        else
        {
            bVariableBonesPerVertex = reader.ReadIntBoolean();
            maxBoneInfluences = reader.ReadUInt32();
            numBones = reader.ReadUInt32();
            numVertices = reader.ReadUInt32();
            bExtraBoneInfluences = maxBoneInfluences > _NUM_INFLUENCES_UE4;
            // bUse16BitBoneIndex doesn't exist before version IncreaseBoneIndexLimitPerChunk
            if (reader.Asset.GetCustomVersion<FAnimObjectVersion>() >= FAnimObjectVersion.IncreaseBoneIndexLimitPerChunk)
            {
                bUse16BitBoneIndex = reader.ReadIntBoolean();
            }
        }
        #endregion

        byte[] newData = Array.Empty<byte>();
        if (!dataStripFlags.IsDataStrippedForServer())
        {
            if (!bNewWeightFormat)
            {
                Weights = reader.ReadBulkArray(() => new FSkinWeightInfo(reader, bExtraBoneInfluences));
            }
            else
            {
                //newData = reader.ReadBulkArray(() => reader.ReadByte());
                var size = reader.ReadInt32();
                var len = reader.ReadInt32();
                if (len > 0) {
                    Weights = new FSkinWeightInfo[numVertices];
                    for (var i = 0; i < Weights.Length; i++) {
                        Weights[i] = new FSkinWeightInfo(reader, bExtraBoneInfluences);
                    }
                }
            }
        }
        else
        {
            bExtraBoneInfluences = numSkelCondition;
        }
        #endregion

        if (bNewWeightFormat)
        {
            #region FSkinWeightLookupVertexBuffer
            var lookupStripFlags = new FStripDataFlags(reader);

            #region FSkinWeightLookupVertexBuffer::SerializeMetaData
            //if (bNewWeightFormat)
            //{
            var numLookupVertices = reader.ReadInt32();
            //}
            #endregion

            if (!lookupStripFlags.IsDataStrippedForServer())
            {
                reader.ReadBulkArray(() => reader.ReadUInt32()); // LookupData
            }
            #endregion

            // Convert influence data
            //if (newData.Length > 0)
            //{
            //    using var tempAr = new FByteArchive("WeightsReader", newData, Ar.Versions);
            //    Weights = new FSkinWeightInfo[numVertices];
            //    for (var i = 0; i < Weights.Length; i++)
            //    {
            //        Weights[i] = new FSkinWeightInfo(tempAr, bExtraBoneInfluences);
            //    }
            //}
        }
    }

    public static int MetadataSize(AssetBinaryReader reader)
    {
        var numBytes = 0;
        var bNewWeightFormat = reader.Asset.GetCustomVersion<FAnimObjectVersion>() >= FAnimObjectVersion.UnlimitedBoneInfluences;

        if (!reader["SkeletalMesh.UseNewCookedFormat"])
        {
            numBytes = 2 * 4;
        }
        else if (!bNewWeightFormat)
        {
            numBytes = 3 * 4;
        }
        else
        {
            numBytes = 4 * 4;
            if (reader.Asset.GetCustomVersion<FAnimObjectVersion>() >= FAnimObjectVersion.IncreaseBoneIndexLimitPerChunk)
                numBytes += 4;
        }

        if (bNewWeightFormat)
        {
            numBytes += 4;
        }

        return numBytes;
    }
}