using System.Collections.Generic;
using System.IO;

namespace UAssetAPI {
    public struct FURL {

        FString Protocol;// Protocol, i.e. "unreal" or "http".
        FString Host;// Optional hostname, i.e. "204.157.115.40" or "unreal.epicgames.com", blank if local.
        int Port;// Optional host port.
        int Valid;
        FString Map;// Map name, i.e. "SkyCity", default is "Entry".
        //FString RedirectURL;// Optional place to download Map if client does not possess it
        List<FString> Op;// Options.
        FString Portal;// Portal to enter through, default is "".


        public FURL(AssetBinaryReader reader) {
            //Ar << U.Protocol << U.Host << U.Map << U.Portal << U.Op << U.Port << U.Valid;
            Protocol = reader.ReadFString();
            Host = reader.ReadFString();
            Map = reader.ReadFString();
            Portal = reader.ReadFString();
            var len = reader.ReadInt32();
            Op = new List<FString>();
            for (int i = 0; i < len; i++) {
                Op.Add(reader.ReadFString());
            }
            Port = reader.ReadInt32();
            Valid = reader.ReadInt32();
        }


    }

    public class LevelExport : NormalExport {
        public List<FPackageIndex> Actors;
        public FURL URL;
        public FPackageIndex Model;
        public List<FPackageIndex> ModelComponents;

        public LevelExport(Export super) : base(super) {

        }

        public LevelExport(UAsset asset, byte[] extras) : base(asset, extras) {

        }

        public LevelExport() {

        }
        public override void Read(AssetBinaryReader reader, int nextStarting) {
            base.Read(reader, nextStarting);

            reader.ReadInt32();
            int numIndexEntries = reader.ReadInt32();

            Actors = new List<FPackageIndex>();
            for (int i = 0; i < numIndexEntries; i++) {
                Actors.Add(reader.XFERPTR());
            }
            URL = new FURL(reader);

            Model = reader.XFERPTR();

            numIndexEntries = reader.ReadInt32();
            ModelComponents = new List<FPackageIndex>();
            for (int i = 0; i < numIndexEntries; i++) {
                ModelComponents.Add(reader.XFERPTR());
            }

            //reader.ReadByte();
        }

        public override void Write(AssetBinaryWriter writer) {
            base.Write(writer);

            writer.Write((int)0);
            //writer.Write(IndexData.Count);
            //for (int i = 0; i < IndexData.Count; i++)
            //{
            //    writer.Write(IndexData[i]);
            //}

            //writer.Write(LevelType.Namespace);
            //writer.Write((int)0);
            //writer.Write(LevelType.Value);

            //writer.Write((long)0);
            //writer.Write(FlagsProbably);

            //for (int i = 0; i < MiscCategoryData.Count; i++)
            //{
            //    writer.Write(MiscCategoryData[i]);
            //}

            //writer.Write((byte)0);
        }
    }
}
