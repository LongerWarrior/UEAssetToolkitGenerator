using System.Diagnostics;

namespace UAssetAPI.PropertyTypes
{

    /// <summary>
    /// Describes a byte or an enumeration value.
    /// </summary>
    public class FieldPathPropertyData : PropertyData<FName[]>
    {
        [JsonProperty]
        public FPackageIndex ResolvedOwner;

        public FieldPathPropertyData(FName name) : base(name)
        {

        }

        public FieldPathPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("FieldPathProperty");
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            ReadCustom(reader, includeHeader, leng1, leng2, true);
        }

        private void ReadCustom(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2, bool canRepeat)
        {
            if (includeHeader)
            {
                PropertyGuid = reader.ReadPropertyGuid();
            }


            List<FName> Paths = new List<FName>();
            int pathNum = reader.ReadInt32();


            for (int i = 0; i < pathNum; i++) {
                Paths.Add(reader.ReadFName());
            }
            Value = Paths.ToArray();

            ResolvedOwner = reader.XFERPTR();


#if DEBUG
            if (leng1 != 8 && leng1 != 16) {
                Debug.WriteLine("-----------------");
                Debug.WriteLine("FieldPathPropertyData");
                Debug.WriteLine("Len: " + leng1 + "; "+"Num: " +pathNum+"; " + "Owner: " + ResolvedOwner.Index + "; ");
                if (reader != null) Debug.WriteLine("Pos: " + reader.BaseStream.Position);
                Debug.WriteLine("-----------------");
            }

#endif


        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.WritePropertyGuid(PropertyGuid);
            }

            writer.Write(Value.Length);

            for (int i = 0; i < Value.Length; i++) {
                writer.XFERNAME(Value[i]);
            }

            writer.XFERPTR(ResolvedOwner);

            return sizeof(int)*(2+Value.Length*2);
        }

    }
}