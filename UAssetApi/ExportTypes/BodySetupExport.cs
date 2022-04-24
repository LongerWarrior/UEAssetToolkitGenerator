using System;
using System.Collections.Generic;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace UAssetAPI
{
    public class FFormatContainer {
        public SortedDictionary<FName, FByteBulkData> Formats;

        public FFormatContainer(AssetBinaryReader reader) {
            int numFormats = reader.ReadInt32();
            Formats = new SortedDictionary<FName, FByteBulkData>();
            for (var i = 0; i < numFormats; i++) {
                Formats[reader.ReadFName()] = new FByteBulkData(reader);
            }
        }

        public void Write(AssetBinaryWriter writer) {
            writer.Write(Formats.Count);
            foreach(KeyValuePair < FName, FByteBulkData> entry in Formats) {
                writer.Write(entry.Key);
                entry.Value.Write(writer);
            }
        }
    }

    public class BodySetupExport : NormalExport
    {
        public Guid BodySetupGuid;
        public FFormatContainer CookedFormatData;
        public bool bCooked;
        public bool bTemp;

        public BodySetupExport(Export super) : base(super)
        {

        }


        public BodySetupExport()
        {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting)
        {
            base.Read(reader, nextStarting);

            int num = reader.ReadInt32();

            BodySetupGuid = new Guid (reader.ReadBytes(16));

            bCooked = reader.ReadInt32()!=0;
            if (!bCooked) return;
            if (reader.Asset.EngineVersion >= UE4Version.VER_UE4_STORE_HASCOOKEDDATA_FOR_BODYSETUP) {
                bTemp = reader.ReadInt32() != 0; 
            }

            CookedFormatData = new FFormatContainer(reader);
        }
        

        public override void Write(AssetBinaryWriter writer)
        {
            base.Write(writer);

            writer.Write(0);
            writer.Write(((Guid)BodySetupGuid).ToByteArray());
            writer.Write(bCooked?1:0);
            if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_STORE_HASCOOKEDDATA_FOR_BODYSETUP) {
                writer.Write(bTemp ? 1 : 0);
            }

        }
    }
}
