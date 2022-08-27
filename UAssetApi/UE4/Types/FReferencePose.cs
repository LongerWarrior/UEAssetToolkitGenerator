namespace UAssetAPI {
    public struct FReferencePose {
        public FName PoseName;
        public FTransform[] ReferencePose;

        public FReferencePose(FName poseName, FTransform[] referencePose) {
            PoseName = poseName;
            ReferencePose = referencePose;
        }

        public void Read(AssetBinaryReader reader) {
            PoseName = reader.ReadFName();

            int poselength = reader.ReadInt32();
            if (poselength > 0) {
                List<FTransform> poselist = new List<FTransform>();
                poselist.Add(new FTransform(reader.ReadQuat(), reader.ReadVector(), reader.ReadVector()));
                ReferencePose = poselist.ToArray();
            } else {
                ReferencePose = null;
            }
        }

        public JToken ToJson() {
            JObject res = new JObject();
            res.Add("PoseName", PoseName.ToName());

            if (ReferencePose != null) {
                JArray poses = new JArray();
                foreach (FTransform trans in ReferencePose) {
                    poses.Add(trans.ToString());
                }
                res.Add("ReferencePose", poses);
            } else {
                res.Add("ReferencePose", new JArray());
            }
            return res;
        }
    }
}
