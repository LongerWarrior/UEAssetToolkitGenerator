using static UAssetAPI.Kismet.KismetSerializer;

namespace UAssetAPI.SoundCueSerializer;
    
public struct LinkedPin {
    public Guid PinID;
    public string NodeName;

    public LinkedPin(Guid pinID, string nodeName) {
        PinID = pinID;
        NodeName = nodeName;
    }
}

public struct SoundPin {
    public Guid PinID;
    public string PinName;
    public List<LinkedPin> LinkedTo;

    public SoundPin(Guid pinID, string pinName) {
        PinID = pinID;
        PinName = pinName;
        LinkedTo = new();
    }

    public override string ToString() {

        var res = $"PinId={PinID.ToString("N")},PinName=\"{PinName}\",";
        var linked = "";
        foreach (LinkedPin entry in LinkedTo) {
            linked += $"{entry.NodeName} {entry.PinID.ToString("N")},";
        }

        if (linked != "") {
            res += $"LinkedTo=({linked}),";
        }

        return res;
    }
} 
    
public struct SoundNode {
    public string NodeClass;
    public string NodeName;

    public SoundNode(string nodeClass, string nodeName) {
        NodeClass = nodeClass;
        NodeName = nodeName;
    }
}

public class SoundCueGraphNode {

    public string Name;
    public bool populated = false;
    public SoundNode SoundNode;
    //public Position NodePos;
    public Guid NodeGuid;
    public List<int> GraphPosX = new List<int> ();
    public List<int> GraphPosY = new List<int> ();
    public SoundPin OutputPin;
    public List<SoundPin> InputPin = new();

    public SoundCueGraphNode(SoundNodeExport export) {
        Name = "SoundCueGraphNode_" + export.Index;
        SoundNode = new SoundNode(GetFullName(export.ClassIndex.Index,export.Asset),export.ObjectName.ToName());
        NodeGuid = Guid.NewGuid();
        OutputPin = new SoundPin(Guid.NewGuid(), "Output");


    }

    public (int,int) GetGraphPos() {
            return (GraphPosX.Max(), GraphPosY.Min());
    }

    public void AddNodeGraphData() {
        foreach (int x in GraphPosX) {
            foreach (int y in GraphPosY) {
                AddSoundGraphData((x,y));
            }
        }
    }

}
