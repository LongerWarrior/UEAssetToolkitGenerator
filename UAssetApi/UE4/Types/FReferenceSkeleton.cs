namespace UAssetAPI;

public struct FReferenceSkeleton {
    public FMeshBoneInfo[] FinalRefBoneInfo; // RawRefBoneInfo
    public FTransform[] FinalRefBonePose; // RawRefBonePose
    public Dictionary<FName, int> FinalNameToIndexMap;

    public void Read(AssetBinaryReader reader) {
        int infolength = reader.ReadInt32();
        if (infolength > 0) {
            List<FMeshBoneInfo> infolist = new List<FMeshBoneInfo>();
            for (int i = 0; i < infolength; i++) {
                infolist.Add(new FMeshBoneInfo(reader.ReadFName(), reader.ReadInt32()));
            }
            FinalRefBoneInfo = infolist.ToArray();
        } else {
            FinalRefBoneInfo = null;
        }

        int poselength = reader.ReadInt32();
        if (poselength > 0) {
            List<FTransform> poselist = new List<FTransform>();
            for (int i = 0; i < poselength; i++) {
                poselist.Add(new FTransform(reader.ReadQuat(), reader.ReadVector(), reader.ReadVector()));
            }
            FinalRefBonePose = poselist.ToArray();
        } else {
            FinalRefBonePose = null;
        }

        int num = reader.ReadInt32();
        FinalNameToIndexMap = new Dictionary<FName, int>(num);
        for (var i = 0; i < num; ++i) {
            FinalNameToIndexMap[reader.ReadFName()] = reader.ReadInt32();
        }

        if (FinalRefBonePose != null) {
            AdjustBoneScales(FinalRefBonePose);
        }
    }

    public void AdjustBoneScales(FTransform[] transforms) {
        if (FinalRefBoneInfo.Length != transforms.Length)
            return;

        for (int boneIndex = 0; boneIndex < transforms.Length; boneIndex++) {
            var scale = GetBoneScale(transforms, boneIndex);
            transforms[boneIndex].Translation.Scale(scale);
        }
    }

    public FVector GetBoneScale(FTransform[] transforms, int boneIndex) {
        var scale = new FVector(1.0f);

        // Get the parent bone, ignore scale of the current one
        boneIndex = FinalRefBoneInfo[boneIndex].ParentIndex;
        while (boneIndex >= 0) {
            var boneScale = transforms[boneIndex].Scale3D;
            // Accumulate the scale
            scale.Scale(boneScale);
            // Get the bone's parent
            boneIndex = FinalRefBoneInfo[boneIndex].ParentIndex;
        }

        return scale;
    }

    public JToken ToJson() {
        JArray res = new JArray();
        foreach (FMeshBoneInfo bone in FinalRefBoneInfo) {
            JObject jbone = new JObject();

            jbone.Add("Name",bone.Name.ToName());
            jbone.Add("ParentIndex",bone.ParentIndex);
            FinalNameToIndexMap.TryGetValue(bone.Name, out int index);
            jbone.Add("Index",index);
            jbone.Add("Pose",FinalRefBonePose[index].ToString());
            res.Add(jbone);
        }

        return new JObject(new JProperty("Bones",res));
    }
}

