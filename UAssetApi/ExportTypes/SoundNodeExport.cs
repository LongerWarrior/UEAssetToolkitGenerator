using UAssetAPI.SoundCueSerializer;
using static UAssetAPI.Kismet.KismetSerializer;

namespace UAssetAPI;

public class SoundNodeExport : NormalExport
{
    public int Index;
    public FStripDataFlags SoundNodeStripDataFlags;
    public SoundCueGraphNode Node;

    public SoundNodeExport(Export super) : base(super)
    {

    }

    public SoundNodeExport()
    {

    }

    public string[] ToGraphInfo() {
        var info = new List<string>();

        info.Add($"Begin Object Class=/Script/AudioEditor.SoundCueGraphNode Name=\"{Node.Name}\"");
        info.Add($"\tBegin Object Class={Node.SoundNode.NodeClass} Name=\"{Node.SoundNode.NodeName}\"");
        info.Add("\tEnd Object");
        info.Add($"\tBegin Object Name=\"{Node.SoundNode.NodeName}\"");

        var t = ParseSoundNodeData(Data,true);
        foreach (string str in t) {
            info.Add($"\t\t{str}");
        }
        info.Add($"\t\tGraphNode=SoundCueGraphNode\'\"{Node.Name}\"\'");
        info.Add("\tEnd Object");
        info.Add($"\tSoundNode={Node.SoundNode.NodeClass.Split(".")[1]}\'\"{Node.SoundNode.NodeName}\"\'");
        (int x, int y) pos = Node.GetGraphPos();
        info.Add($"\tNodePosX={pos.x*-150}");
        info.Add($"\tNodePosY={pos.y*150}");
        info.Add($"\tNodeGuid={Node.NodeGuid.ToString("N")}");

        var outpin = Node.OutputPin.ToString() + $"Direction=\"EGPD_Output\",";
        info.Add($"\tCustomProperties Pin ({outpin})");
        if (Node.InputPin.Count > 0) {
            foreach( SoundPin pin in Node.InputPin)
            info.Add($"\tCustomProperties Pin ({pin.ToString()})");
        }
        info.Add("End Object");
        return info.ToArray();
    }

    private string[] ParseSoundNodeData(List<PropertyData> data, bool maindata = false) {

        var res = new List<string>();
        foreach (PropertyData property in data) {

            switch (property) {
                case BytePropertyData prop:
                    if (prop.ByteType == BytePropertyType.Long) {
                        res.Add($"{prop.Name.ToName()}={Asset.GetNameReference(prop.Value).Value}");
                    } else {
                        res.Add($"{prop.Name.ToName()}={prop.Value.ToString()}");
                    }
                    break;
                case FloatPropertyData prop:
                    res.Add($"{prop.Name.ToName()}={prop.Value.ToString("0.00000").Replace(",",".")}");
                    break;
                case EnumPropertyData prop:
                    res.Add($"{prop.Name.ToName()}={prop.Value.ToName()}");
                    break;
                case IntPropertyData prop:
                    res.Add($"{prop.Name.ToName()}={prop.Value}");
                    break;
                case BoolPropertyData prop:
                    res.Add ( $"{prop.Name.ToName()}={prop.Value.ToString()}");
                    break;
                case NamePropertyData prop:
                    res.Add($"{prop.Name.ToName()}={prop.Value.ToName()}");
                    break;
                case ObjectPropertyData prop:
                    var index = prop.Value.Index;
                    var value = "";
                    if (index < 0) {
                        value = $"{prop.Value.ToImport(Asset).ClassName.ToName()}\'\"{GetFullName(index, Asset)}\"\'";
                    } else if (index > 0) {
                        SoundNodeExport node = (SoundNodeExport)Asset.Exports[index - 1];
                        value = $"{prop.Value.ToExport(Asset).ClassIndex.ToImport(Asset).ObjectName.ToName()}\'\"{node.Node.Name}.{node.Node.SoundNode.NodeName}\"\'";
                    } else {
                        value = "None";
                    }
                    res.Add($"{prop.Name.ToName()}={value}");
                    break;
                case ArrayPropertyData prop:
                        var k = 0;
                    List<string> temp = new();
                    foreach (PropertyData inprop in prop.Value) {
                        
                        var templist = new List<PropertyData> { inprop };
                        temp.AddRange(ParseSoundNodeData(templist));
                        
                    }
                    if (maindata) {

                        foreach (string str in temp) {
                            var splt = str.Split("=", 2);
                            res.Add($"{prop.Name}({k++})={splt[1]}");

                        }

                    } else {
                        var arr = "";

                        if (temp.Count > 1) {
                            foreach (string str in temp) {
                                var splt = ""; 
                                if (prop.ArrayType.ToName() == "StructProperty") {
                                    splt = $"{str.Split("=", 2)[1]}";
                                } else {
                                    splt = $"({str.Split("=", 2)[1]})";
                                }
                                arr += splt + ",";
                            }
                            
                                res.Add($"{prop.Name}=({arr.Substring(0, arr.Length - 1)})");
                            
                        } else {
                            res.Add($"{prop.Name}=({temp[0].Split("=", 2)[1]})");
                        }

                    }

                    break;
                case StructPropertyData prop:

                    if (prop.StructType.ToName() == "RichCurveKey") {
                        RichCurveKeyPropertyData curve = (RichCurveKeyPropertyData)prop.Value[0];
                        var value1 = $"InterpMode={curve.InterpMode.ToString()}, TangentMode={curve.TangentMode.ToString()},TangentWeightMode={curve.TangentWeightMode.ToString()},";
                        value1 += $"Time={curve.Time.ToString("0.00000").Replace(",", ".")},Value={curve.Value.ToString("0.00000").Replace(",", ".")},ArriveTangent={curve.ArriveTangent.ToString("0.00000").Replace(",", ".")},";
                        value1 += $"ArriveTangentWeight={curve.ArriveTangentWeight.ToString("0.00000").Replace(",", ".")},LeaveTangent={curve.LeaveTangent.ToString("0.00000").Replace(",", ".")},LeaveTangentWeight={curve.LeaveTangentWeight.ToString("0.00000").Replace(",", ".")}";
                        res.Add($"{prop.Name}=({value1})");

                    } else if(prop.StructType.ToName() == "Vector") { 
                        VectorPropertyData _vector = (VectorPropertyData)prop.Value[0];
                        var vector = _vector.Value;
                        var value1 = $"X={vector.X.ToString("0.00000").Replace(",", ".")},Y={vector.Y.ToString("0.00000").Replace(",", ".")},Z={vector.Z.ToString("0.00000").Replace(",", ".")}";
                        res.Add($"{prop.Name}=({value1})");
                    } else {
                        var temp1 = ParseSoundNodeData(prop.Value);
                        var arr = "";
                        foreach (string str in temp1) {
                            arr += str + ",";
                        }
                        res.Add($"{prop.Name}=({arr.Substring(0, arr.Length - 1)})");
                    }


                    break;
                case SoftObjectPropertyData prop:
                        res.Add($"{prop.Name}={ prop.Value.AssetPathName.ToName()}");
                    break;
                default:
                    break;
            }
        }



        return res.ToArray();
    }


    public void Populate(SoundNodeExport parent, Guid pinid, int childscount=0, int childnumber=0) {

        if (Node.populated) return;

        var pins = 0;
        if (FindPropertyData(Data, "ChildNodes", out PropertyData _childs)) {
            ArrayPropertyData childs = (ArrayPropertyData)_childs;
            pins = childs.Value.Length;
        }
        if (parent != null) {
            (int x, int y) parentpos = parent.Node.GetGraphPos();


            var nodeclass = Node.SoundNode.NodeClass.Split(".")[1];
            if (nodeclass == "SoundNodeWavePlayer" || nodeclass == "SoundNodeDialoguePlayer") {
                Node.GraphPosX.Add(parentpos.x + 2);
                Node.GraphPosX.Add(parentpos.x + 3);
            } else {
                Node.GraphPosX.Add(parentpos.x + 2);
            }
            //0-4 =1
            //5-10 =2
            //11-16 =3
            var freepos = GetFreePos(Node.GraphPosX);
            if (freepos < parentpos.y) {
                freepos = parentpos.y;
            }
            Node.GraphPosY.Add(freepos);

            if (pins > 4) {
                Node.GraphPosY.Add(freepos + 1);
            }
            if (pins >= 11) {
                Node.GraphPosY.Add(freepos + 2);
            }

        } else {
            Node.GraphPosX.Add(0);
            Node.GraphPosY.Add(0);
        }
        Node.AddNodeGraphData();

        if (parent != null) {
            Node.OutputPin.LinkedTo.Add(new LinkedPin(pinid, parent.Node.Name));
        }



        if (FindPropertyData(Data, "ChildNodes", out _childs)) {
            ArrayPropertyData childs = (ArrayPropertyData)_childs;

            for (int i = 1; i <= childs.Value.Length; i++) {
                var _child = childs.Value[i - 1];
                ObjectPropertyData child = (ObjectPropertyData)_child;
                var childindex = child.Value.Index;


                var name = "Input";
                if (i > 1) {
                    name +=i;
                } 
                var inputpin = new SoundPin(Guid.NewGuid(), name);

                if (childindex != 0) {

                    SoundNodeExport newchild = (SoundNodeExport)Asset.Exports[childindex - 1];
                    inputpin.LinkedTo.Add(new LinkedPin(newchild.Node.OutputPin.PinID, newchild.Node.Name));
                    newchild.Populate(this,inputpin.PinID, childs.Value.Length,i);

                } else { 
                
                }
                Node.InputPin.Add(inputpin);
                
            }

        }
        Node.populated = true;

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);


        for (int i = 0; i < reader.Asset.Exports.Count; i++) {
            if (reader.Asset.Exports[i] == this) {
                Index = i;
            }
        }
        
        Node = new SoundCueGraphNode(this);

        reader.ReadInt32();

        if (reader.Ver >= UE4Version.VER_UE4_COOKED_ASSETS_IN_EDITOR_SUPPORT) {
            SoundNodeStripDataFlags = new FStripDataFlags (reader);
        }
        if (ClassIndex.ToImport(reader.Asset).ObjectName.ToName() == "SoundNodeWavePlayer") {
            if (reader.Asset.GetCustomVersion<FFrameworkObjectVersion>() >= FFrameworkObjectVersion.HardSoundReferences) {
                var SoundWave = new FPackageIndex(reader);
            }
        }
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);

        if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_COOKED_ASSETS_IN_EDITOR_SUPPORT) {
            SoundNodeStripDataFlags.Write(writer);
        }
    }

}
