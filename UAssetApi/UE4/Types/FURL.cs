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
}
