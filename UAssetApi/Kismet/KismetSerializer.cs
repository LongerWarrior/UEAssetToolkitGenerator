namespace UAssetAPI.Kismet;

public static class KismetSerializer {

    public static UAsset asset;
    public static Dictionary<int, List<int>> SoundGraphData = new();

    public static int GetFreePos(List<int> columns) {
        var res = 0;
        foreach (int column in columns) {
            if (SoundGraphData.ContainsKey(column)) {
                var firstspot = SoundGraphData[column].Max()+1;
                if (firstspot > res) {
                    res = firstspot;
                }
            }
        }

        return res;
    }

    public static void AddSoundGraphData((int x, int y) pos) {
        if (SoundGraphData.ContainsKey(pos.x)) {
            SoundGraphData[pos.x].Add(pos.y);
        } else {
            SoundGraphData[pos.x] = new List<int> { pos.y };
        }
    }

    public struct FSimpleMemberReference {
        public string MemberParent;
        public string MemberName;
        public Guid MemberGuid;

    }
    public struct FEdGraphTerminalType {
        public string TerminalCategory;
        public string TerminalSubCategory;
        public string TerminalSubCategoryObject;
        public bool bTerminalIsConst;
        public bool bTerminalIsWeakPointer;
        public bool bTerminalIsUObjectWrapper;

    }
    public struct FEdGraphPinType {
        public string PinCategory;
        public string PinSubCategory;
        public string PinSubCategoryObject;
        public FSimpleMemberReference PinSubCategoryMemberReference;
        public FEdGraphTerminalType PinValueType;
        public EPinContainerType ContainerType;

        public bool bIsReference;
        public bool bIsConst;
        public bool bIsWeakPointer;
        public bool bIsUObjectWrapper;
    }

    public enum EPinContainerType : byte {
        None,
        Array,
        Set,
        Map
    };

    //public static bool FindPropertyData(FPackageIndex export, string propname, out PropertyData prop) {

    //    prop = null;
    //    if (export.IsExport() && export.ToExport(asset) is NormalExport exp) {
    //        foreach (PropertyData property in exp.Data) {
    //            if (property.Name.ToName() == propname) {
    //                prop = property;
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}

    //public static bool FindPropertyData(Export export, string propname, out PropertyData prop) {

    //    prop = null;
    //    if (export is NormalExport exp) {
    //        foreach (PropertyData property in exp.Data) {
    //            if (property.Name.ToName() == propname) {
    //                prop = property;
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}
    public static bool FindPropertyData(List<PropertyData> list, string propname, out PropertyData prop) {

        prop = null;
        foreach (PropertyData property in list) {
            if (property.Name.ToName() == propname) {
                prop = property;
                return true;
            }
        }

        return false;
    }



    //public static string GetName(int index) {
    //    if (index > 0) {
    //        return asset.Exports[index - 1].ObjectName.ToString();
    //    } else if (index < 0) {
    //        return asset.Imports[-index - 1].ObjectName.ToString();
    //    } else {
    //        return "";
    //    }
    //}

    //public static int GetClassIndex() {
    //    for (int i = 1; i <= asset.Exports.Count; i++) {
    //        if (asset.Exports[i - 1] is ClassExport) {
    //            return i;
    //        }
    //    }
    //    return 0;
    //}

    //public static string GetFullName(int index) {

    //    if (index > 0) {
    //        if (asset.Exports[index - 1].OuterIndex.Index != 0) {
    //            string parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index);
    //            return parent + "." + asset.Exports[index - 1].ObjectName.ToString();
    //        } else {
    //            return asset.Exports[index - 1].ObjectName.ToString();
    //        }

    //    } else if (index < 0) {

    //        if (asset.Imports[-index - 1].OuterIndex.Index != 0) {
    //            string parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index);
    //            return parent + "." + asset.Imports[-index - 1].ObjectName.ToString();
    //        } else {
    //            return asset.Imports[-index - 1].ObjectName.ToString();
    //        }

    //    } else {
    //        return "";
    //    }
    //}

    public static string GetFullName(int index, UAsset asset) {

        if (index > 0) {
            if (asset.Exports[index - 1].OuterIndex.Index != 0) {
                string parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index,asset);
                return parent + "." + asset.Exports[index - 1].ObjectName.ToString();
            } else {
                return asset.Exports[index - 1].ObjectName.ToString();
            }

        } else if (index < 0) {

            if (asset.Imports[-index - 1].OuterIndex.Index != 0) {
                string parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index,asset);
                return parent + "." + asset.Imports[-index - 1].ObjectName.ToString();
            } else {
                return asset.Imports[-index - 1].ObjectName.ToString();
            }

        } else {
            return "";
        }
    }

    //public static string GetParentName(int index) {
    //    if (index > 0) {
    //        if (asset.Exports[index - 1].OuterIndex.Index != 0) {
    //            string parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index);
    //            return parent;
    //        } else {
    //            return "";
    //        }

    //    } else if (index < 0) {

    //        if (asset.Imports[-index - 1].OuterIndex.Index != 0) {
    //            string parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index);
    //            return parent;
    //        } else {
    //            return "";
    //        }

    //    } else {
    //        return "";
    //    }
    //}

    //public static bool FindProperty(int index, FName propname, out FProperty property) {
    //    if (index < 0) {

    //        property = new FObjectProperty();
    //        return false;

    //    }
    //    Export export = asset.Exports[index - 1];
    //    if (export is StructExport) {
    //        foreach (FProperty prop in (export as StructExport).LoadedProperties) {
    //            if (prop.Name == propname) {
    //                property = prop;
    //                return true;
    //            }
    //        }
    //    }
    //    property = new FObjectProperty();
    //    return false;
    //}
}