using UAssetAPI.FieldTypes;
using UAssetAPI.Kismet.Bytecode;
using UAssetAPI.Kismet.Bytecode.Expressions;

namespace CookedAssetSerializer;

public class KismetExpressionSerializer
{
    public struct FSimpleMemberReference
    {
        public string MemberParent;
        public string MemberName;
        public Guid MemberGuid;
    }

    public struct FEdGraphTerminalType
    {
        public string TerminalCategory;
        public string TerminalSubCategory;
        public string TerminalSubCategoryObject;
        public bool bTerminalIsConst;
        public bool bTerminalIsWeakPointer;
        public bool bTerminalIsUObjectWrapper;
    }

    public struct FEdGraphPinType
    {
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

    public enum EPinContainerType : byte
    {
        None,
        Array,
        Set,
        Map
    }

    private const string PC_Boolean = "bool";
    private const string PC_Byte = "byte";
    private const string PC_Class = "Class";
    private const string PC_Int = "int";
    private const string PC_Int64 = "Int64";
    private const string PC_Float = "Float";
    private const string PC_Name = "Name";
    private const string PC_Delegate = "Delegate";
    private const string PC_MCDelegate = "mcdelegate";
    private const string PC_Object = "Object";
    private const string PC_Interface = "Interface";
    private const string PC_String = "String";
    private const string PC_Text = "Text";
    private const string PC_Struct = "Struct";
    private const string PC_Enum = "Enum";
    private const string PC_SoftObject = "Softobject";
    private const string PC_SoftClass = "Softclass";
    private const string PC_None = "None";

    public static FEdGraphPinType GetPropertyCategoryInfo(FProperty prop, UAsset asset)
    {
        var pin = new FEdGraphPinType();
        switch (prop)
        {
            case FInterfaceProperty finterface:
            {
                pin.PinCategory = PC_Interface;
                pin.PinSubCategoryObject = GetFullName(finterface.InterfaceClass.Index, asset);
                break;
            }
            case FClassProperty fclassprop:
            {
                pin.PinCategory = PC_Class;
                pin.PinSubCategoryObject = GetFullName(fclassprop.MetaClass.Index, asset);
                break;
            }
            case FSoftClassProperty fsoftclassprop:
            {
                pin.PinCategory = PC_SoftClass;
                pin.PinSubCategoryObject = GetFullName(fsoftclassprop.MetaClass.Index, asset);
                break;
            }
            case FSoftObjectProperty fsoftobjprop:
            {
                pin.PinCategory = PC_SoftObject;
                pin.PinSubCategoryObject = GetFullName(fsoftobjprop.PropertyClass.Index, asset);
                break;
            }
            case FObjectProperty fobjprop:
            {
                pin.PinCategory = PC_Object;
                pin.PinSubCategoryObject = GetFullName(fobjprop.PropertyClass.Index, asset);
                if (fobjprop.PropertyFlags.HasFlag(EPropertyFlags.CPF_AutoWeak)) pin.bIsWeakPointer = true;
                break;
            }
            case FStructProperty fstruct:
            {
                pin.PinCategory = PC_Struct;
                pin.PinSubCategoryObject = GetFullName(fstruct.Struct.Index, asset);
                break;
            }
            case FByteProperty fbyte:
            {
                pin.PinCategory = PC_Byte;
                pin.PinSubCategoryObject = GetFullName(fbyte.Enum.Index, asset);
                break;
            }
            case FEnumProperty fenum:
            {
                if (!(fenum.UnderlyingProp is FByteProperty)) break;
                pin.PinCategory = PC_Byte;
                pin.PinSubCategoryObject = GetFullName(fenum.Enum.Index, asset);
                break;
            }
            case FBoolProperty fbool:
            {
                pin.PinCategory = PC_Boolean;
                break;
            }
            case FGenericProperty fgeneric:
            {
                switch (fgeneric.SerializedType.ToName())
                {
                    case "FloatProperty":
                    {
                        pin.PinCategory = PC_Float;
                        break;
                    }
                    case "Int64Property":
                    {
                        pin.PinCategory = PC_Int64;
                        break;
                    }
                    case "IntProperty":
                    {
                        pin.PinCategory = PC_Int;
                        break;
                    }
                    case "NameProperty":
                    {
                        pin.PinCategory = PC_Name;
                        break;
                    }
                    case "StrProperty":
                    {
                        pin.PinCategory = PC_String;
                        break;
                    }
                    case "TextProperty":
                    {
                        pin.PinCategory = PC_Text;
                        break;
                    }
                }
                break;
            }
        }

        return pin;
    }

    public static FSimpleMemberReference FillSimpleMemberReference(int index, UAsset asset)
    {
        var member = new FSimpleMemberReference();
        if (index > 0)
        {
            member.MemberName = asset.Exports[index - 1].ObjectName.ToName();
            member.MemberParent = GetName(asset.Exports[index - 1].OuterIndex.Index, asset);
            member.MemberGuid = asset.Exports[index - 1].PackageGuid;
        }
        else if (index < 0)
        {
            member.MemberName = asset.Imports[-index - 1].ObjectName.ToName();
            member.MemberParent = asset.Imports[-index - 1].ClassPackage.ToName();
            member.MemberGuid = new Guid("00000000000000000000000000000000");
        }

        return member;
    }

    public static JObject SerializeGraphPinType(FEdGraphPinType pin)
    {
        var jpin = new JObject();
        jpin.Add("PinCategory", pin.PinCategory);
        jpin.Add("PinSubCategory", pin.PinCategory);
        if (pin.PinSubCategoryObject == "" || pin.PinSubCategoryObject == null)
        {
        }
        else
        {
            jpin.Add("PinSubCategoryObject", pin.PinSubCategoryObject);
        }

        if (pin.PinSubCategoryMemberReference.MemberName != null)
        {
            var member = pin.PinSubCategoryMemberReference;
            if (member.MemberGuid.Equals(new Guid("00000000000000000000000000000000")))
            {
            }
            else
            {
                var jmember = new JObject();
                if (member.MemberParent != "" || member.MemberParent != null)
                    jmember.Add("MemberParent", member.MemberParent);
                jmember.Add("MemberName", member.MemberName);
                jmember.Add("MemberGuid", member.MemberGuid);
                jpin.Add("PinSubCategoryMemberReference", jmember);
            }
        }

        if (pin.ContainerType == EPinContainerType.Map)
        {
            var valuetype = pin.PinValueType;
            var jvaluetype = new JObject();

            jvaluetype.Add("TerminalCategory", valuetype.TerminalCategory);
            if (valuetype.TerminalSubCategory == null || valuetype.TerminalSubCategory == "")
                jvaluetype.Add("TerminalSubCategory", "None");
            else
                jvaluetype.Add("TerminalSubCategory", valuetype.TerminalSubCategory);
            if (valuetype.TerminalSubCategoryObject != "" && valuetype.TerminalSubCategoryObject != null)
                jvaluetype.Add("TerminalSubCategoryObject", valuetype.TerminalSubCategoryObject);
            jvaluetype.Add("TerminalIsConst", valuetype.bTerminalIsConst);
            jvaluetype.Add("TerminalIsWeakPointer", valuetype.bTerminalIsWeakPointer);
            jpin.Add("PinValueType", jvaluetype);
        }

        if (pin.ContainerType != EPinContainerType.None) jpin.Add("ContainerType", (int)pin.ContainerType);

        if (pin.bIsReference) jpin.Add("IsReference", pin.bIsReference);
        if (pin.bIsConst) jpin.Add("IsConst", pin.bIsConst);
        if (pin.bIsWeakPointer) jpin.Add("IsWeakPointer", pin.bIsWeakPointer);
        return jpin;
    }

    public static FEdGraphPinType ConvertPropertyToPinType(FProperty property, UAsset asset)
    {
        var pin = new FEdGraphPinType();
        var prop = property;

        if (property is FMapProperty)
        {
            prop = (property as FMapProperty).KeyProp;
            pin.ContainerType = EPinContainerType.Map;
            pin.bIsWeakPointer = false;
            var temppin = GetPropertyCategoryInfo((property as FMapProperty).ValueProp, asset);
            pin.PinValueType.TerminalCategory = temppin.PinCategory;
            pin.PinValueType.TerminalSubCategory = temppin.PinSubCategory;
            pin.PinValueType.TerminalSubCategoryObject = temppin.PinSubCategoryObject;

            pin.PinValueType.bTerminalIsConst = temppin.bIsConst;
            pin.PinValueType.bTerminalIsWeakPointer = temppin.bIsWeakPointer;
        }
        else if (property is FSetProperty)
        {
            prop = (property as FSetProperty).ElementProp;
            pin.ContainerType = EPinContainerType.Set;
        }
        else if (property is FArrayProperty)
        {
            prop = (property as FArrayProperty).Inner;
            pin.ContainerType = EPinContainerType.Array;
        }

        pin.bIsReference = property.PropertyFlags.HasFlag(EPropertyFlags.CPF_OutParm) &&
                           property.PropertyFlags.HasFlag(EPropertyFlags.CPF_ReferenceParm);
        pin.bIsConst = property.PropertyFlags.HasFlag(EPropertyFlags.CPF_ConstParm);


        if (prop is FMulticastDelegateProperty)
        {
            pin.PinCategory = PC_MCDelegate;
            pin.PinSubCategoryMemberReference =
                FillSimpleMemberReference((prop as FMulticastDelegateProperty).SignatureFunction.Index, asset);
        }
        else if (prop is FDelegateProperty)
        {
            pin.PinCategory = PC_Delegate;
            pin.PinSubCategoryMemberReference =
                FillSimpleMemberReference((prop as FDelegateProperty).SignatureFunction.Index, asset);
        }
        else
        {
            var temppin = GetPropertyCategoryInfo(prop, asset);
            pin.PinCategory = temppin.PinCategory;
            pin.PinSubCategory = temppin.PinSubCategory;
            pin.PinSubCategoryObject = temppin.PinSubCategoryObject;
            pin.bIsWeakPointer = temppin.bIsWeakPointer;
        }

        return pin;
    }

    public static JProperty[] SerializePropertyPointer(KismetPropertyPointer pointer, string[] names, UAsset asset,
        List<string> importVariables)
    {
        var jproparray = new JProperty[names.Length];

        FProperty property;
        if (pointer != null && pointer.New.ResolvedOwner.Index != 0)
        {
            if (FindProperty(pointer.New.ResolvedOwner.Index, pointer.New.Path[0], out property, asset, ref importVariables))
            {
                var PropertyType = ConvertPropertyToPinType(property, asset);
                jproparray[0] = new JProperty(names[0], SerializeGraphPinType(PropertyType));
            }
            else
            {
                jproparray[0] = new JProperty(names[0], "##NOT SERIALIZED##");
            }

            if (names.Length > 1) jproparray[1] = new JProperty(names[1], pointer.New.Path[0].ToName());

            return jproparray;
        }

        return new JProperty[0];
    }

    public static JObject SerializeExpression(KismetExpression expression, ref int index, UAsset asset,
        List<string> importVariables, bool addindex = false)
    {
        var savedindex = index;
        var jexp = new JObject();
        index++;
        switch (expression)
        {
            case EX_PrimitiveCast exp:
            {
                jexp.Add("Inst", exp.Inst);
                index++;
                switch (exp.ConversionType)
                {
                    case ECastToken.InterfaceToBool:
                    {
                        jexp.Add("CastType", "InterfaceToBool");
                        break;
                    }
                    case ECastToken.ObjectToBool:
                    {
                        jexp.Add("CastType", "ObjectToBool");
                        break;
                    }
                    case ECastToken.ObjectToInterface:
                    {
                        jexp.Add("CastType", "ObjectToInterface");
                        index += 8;
                        jexp.Add("InterfaceClass", "##NOT SERIALIZED##");
                        break;
                    }
                }

                jexp.Add("Expression", SerializeExpression(exp.Target, ref index, asset, importVariables));
                break;
            }
            case EX_SetSet exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("LeftSideExpression", SerializeExpression(exp.SetProperty, ref index, asset, importVariables));
                var jparams = new JArray();

                index += 4;
                foreach (var param in exp.Elements) jparams.Add(SerializeExpression(param, ref index, 
                    asset, importVariables));
                index++;
                jexp.Add("Values", jparams);
                break;
            }
            case EX_SetConst exp:
            {
                index += 8;
                jexp.Add("Inst", exp.Inst);
                jexp.Add(SerializePropertyPointer(exp.InnerProperty, new[] { "InnerProperty" }, asset, importVariables));

                index += 4;
                var jparams = new JArray();
                foreach (var param in exp.Elements) jparams.Add(SerializeExpression(param, ref index, 
                    asset, importVariables));
                index++;
                jexp.Add("Values", jparams);
                break;
            }
            case EX_SetMap exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("LeftSideExpression", SerializeExpression(exp.MapProperty, ref index, asset, importVariables));

                index += 4;
                var jparams = new JArray();
                for (var j = 1; j <= exp.Elements.Length / 2; j++)
                {
                    var jobject = new JObject();
                    jobject.Add("Key", SerializeExpression(exp.Elements[2 * (j - 1)], ref index, asset, importVariables));
                    jobject.Add("Value", SerializeExpression(exp.Elements[2 * (j - 1) + 1], ref index, asset, importVariables));
                    jparams.Add(jobject);
                }

                index++;
                jexp.Add("Values", jparams);
                break;
            }
            case EX_MapConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.KeyProperty, new[] { "KeyProperty" }, asset, importVariables));
                jexp.Add(SerializePropertyPointer(exp.ValueProperty, new[] { "ValueProperty" }, asset, importVariables));

                index += 4;
                var jparams = new JArray();
                for (var j = 1; j <= exp.Elements.Length / 2; j++)
                {
                    var jobject = new JObject();
                    jobject.Add("Key", SerializeExpression(exp.Elements[2 * (j - 1)], ref index, asset, importVariables));
                    jobject.Add("Value", SerializeExpression(exp.Elements[2 * (j - 1) + 1], ref index, asset, importVariables));
                    jparams.Add(jobject);
                }

                index++;
                jexp.Add("Values", jparams);
                break;
            }
            case EX_ObjToInterfaceCast exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("InterfaceClass", GetFullName(exp.ClassPtr.Index, asset));
                jexp.Add("Expression", SerializeExpression(exp.Target, ref index, asset, importVariables));
                break;
            }
            case EX_CrossInterfaceCast exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("InterfaceClass", GetFullName(exp.ClassPtr.Index, asset));
                jexp.Add("Expression", SerializeExpression(exp.Target, ref index, asset, importVariables));
                break;
            }
            case EX_InterfaceToObjCast exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("ObjectClass", GetFullName(exp.ClassPtr.Index, asset));
                jexp.Add("Expression", SerializeExpression(exp.Target, ref index, asset, importVariables));
                break;
            }
            case EX_Let exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Variable", SerializeExpression(exp.Variable, ref index, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.Expression, ref index, asset, importVariables));
                break;
            }
            case EX_LetObj exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index, asset, importVariables));
                break;
            }
            case EX_LetWeakObjPtr exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index, asset, importVariables));
                break;
            }
            case EX_LetBool exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index, asset, importVariables));
                break;
            }
            case EX_LetValueOnPersistentFrame exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("PropertyName", exp.DestinationProperty.New.Path[0].ToName());

                index += 8;
                jexp.Add(SerializePropertyPointer(exp.DestinationProperty, new[] { "PropertyType" }, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index, asset, importVariables));
                break;
            }
            case EX_StructMemberContext exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.Property, new[] { "PropertyType" }, asset, importVariables));
                jexp.Add("PropertyName", exp.Property.New.Path[0].ToName());
                jexp.Add("StructExpression", SerializeExpression(exp.StructExpression, ref index, asset, importVariables));
                break;
            }
            case EX_LetDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index, asset, importVariables));
                break;
            }
            case EX_LocalVirtualFunction exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("FunctionName", exp.VirtualFunctionName.ToName());
                var jparams = new JArray();
                foreach (var param in exp.Parameters) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Parameters", jparams);
                break;
            }
            case EX_LocalFinalFunction exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Function", GetName(exp.StackNode.Index, asset));
                index += 8;
                var jparams = new JArray();
                foreach (var param in exp.Parameters) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Parameters", jparams);
                break;
            }
            case EX_LetMulticastDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index, asset, importVariables));
                break;
            }
            case EX_ComputedJump exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("OffsetExpression", SerializeExpression(exp.CodeOffsetExpression, ref index, asset, importVariables));
                break;
            }
            case EX_Jump exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 4;
                jexp.Add("Offset", exp.CodeOffset);
                break;
            }
            case EX_LocalVariable exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }, asset, importVariables));
                break;
            }
            case EX_DefaultVariable exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }, asset, importVariables));
                break;
            }
            case EX_InstanceVariable exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }, asset, importVariables));
                break;
            }
            case EX_LocalOutVariable exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }, asset, importVariables));
                break;
            }
            case EX_InterfaceContext exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Expression", SerializeExpression(exp.InterfaceValue, ref index, asset, importVariables));
                break;
            }
            case EX_DeprecatedOp4A:
            case EX_Nothing:
            case EX_EndOfScript:
            case EX_IntZero:
            case EX_IntOne:
            case EX_True:
            case EX_False:
            case EX_NoObject:
            case EX_NoInterface:
            case EX_Self:
            {
                jexp.Add("Inst", expression.Inst);
                break;
            }
            case EX_Return exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Expression", SerializeExpression(exp.ReturnExpression, ref index, asset, importVariables));
                break;
            }
            case EX_CallMath exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Function", GetName(exp.StackNode.Index, asset));
                jexp.Add("ContextClass", GetParentName(exp.StackNode.Index, asset));
                var jparams = new JArray();
                foreach (var param in exp.Parameters) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Parameters", jparams);
                break;
            }
            case EX_CallMulticastDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                var jsign = new JObject();
                var bIsSelfContext = GetClassIndex(asset) == exp.StackNode.Index;
                jsign.Add("IsSelfContext", bIsSelfContext);
                jsign.Add("MemberParent", GetFullName(exp.StackNode.Index, asset));
                jsign.Add("MemberName", GetName(exp.StackNode.Index, asset));
                jexp.Add("DelegateSignatureFunction", jsign);
                jexp.Add("Delegate", SerializeExpression(exp.Delegate, ref index, asset, importVariables));

                var jparams = new JArray();
                foreach (var param in exp.Parameters) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Parameters", jparams);
                break;
            }
            case EX_FinalFunction exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Function", GetName(exp.StackNode.Index, asset));
                var jparams = new JArray();
                foreach (var param in exp.Parameters) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Parameters", jparams);
                break;
            }
            case EX_VirtualFunction exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("Function", exp.VirtualFunctionName.ToName());
                var jparams = new JArray();

                foreach (var param in exp.Parameters) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Parameters", jparams);
                break;
            }
            //case EX_ClassContext:
            //case EX_Context_FailSilent: {
            case EX_Context exp:
            {
                if (exp is EX_Context_FailSilent)
                    exp = exp as EX_Context_FailSilent;
                else if (exp is EX_ClassContext) exp = exp as EX_ClassContext;

                jexp.Add("Inst", exp.Inst);
                jexp.Add("Context", SerializeExpression(exp.ObjectExpression, ref index, asset, importVariables));
                index += 4;
                jexp.Add("SkipOffsetForNull", exp.Offset);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.RValuePointer,
                    new[] { "RValuePropertyType", "RValuePropertyName" }, asset, importVariables));
                jexp.Add("Expression", SerializeExpression(exp.ContextExpression, ref index, asset, importVariables));
                break;
            }
            case EX_IntConst exp:
            {
                index += 4;
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_SkipOffsetConst exp:
            {
                index += 4;
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_FloatConst exp:
            {
                index += 4;
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_StringConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += exp.Value.Length + 1;
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_UnicodeStringConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 2 * (exp.Value.Length + 1);
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_TextConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index++;
                switch (exp.Value.TextLiteralType)
                {
                    case EBlueprintTextLiteralType.Empty:
                    {
                        jexp.Add("TextLiteralType", "Empty");
                        break;
                    }
                    case EBlueprintTextLiteralType.LocalizedText:
                    {
                        jexp.Add("TextLiteralType", "LocalizedText");
                        jexp.Add("SourceString", ReadString(exp.Value.LocalizedSource, ref index));
                        jexp.Add("LocalizationKey", ReadString(exp.Value.LocalizedKey, ref index));
                        jexp.Add("LocalizationNamespace", ReadString(exp.Value.LocalizedNamespace, ref index));
                        break;
                    }
                    case EBlueprintTextLiteralType.InvariantText:
                    {
                        jexp.Add("TextLiteralType", "InvariantText");
                        jexp.Add("SourceString", ReadString(exp.Value.InvariantLiteralString, ref index));

                        break;
                    }
                    case EBlueprintTextLiteralType.LiteralString:
                    {
                        jexp.Add("TextLiteralType", "LiteralString");
                        jexp.Add("SourceString", ReadString(exp.Value.LiteralString, ref index));
                        break;
                    }
                    case EBlueprintTextLiteralType.StringTableEntry:
                    {
                        jexp.Add("TextLiteralType", "StringTableEntry");
                        index += 8;
                        jexp.Add("TableId", ReadString(exp.Value.StringTableId, ref index));
                        jexp.Add("TableKey", ReadString(exp.Value.StringTableKey, ref index));
                        break;
                    }
                }

                break;
            }
            case EX_ObjectConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Object", GetFullName(exp.Value.Index, asset));
                break;
            }
            case EX_SoftObjectConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Value", SerializeExpression(exp.Value, ref index, asset, importVariables));
                break;
            }
            case EX_NameConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("Value", exp.Value.ToName());
                break;
            }
            case EX_RotationConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("Pitch", exp.Pitch);
                jexp.Add("Yaw", exp.Yaw);
                jexp.Add("Roll", exp.Roll);
                break;
            }
            case EX_VectorConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("X", exp.Value.X);
                jexp.Add("Y", exp.Value.Y);
                jexp.Add("Z", exp.Value.Z);
                break;
            }
            case EX_TransformConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 40;
                var jrot = new JObject();
                var jtrans = new JObject();
                var jscale = new JObject();

                jrot.Add("X", exp.Value.Rotation.X);
                jrot.Add("Y", exp.Value.Rotation.Y);
                jrot.Add("Z", exp.Value.Rotation.Z);
                jrot.Add("W", exp.Value.Rotation.W);

                jtrans.Add("X", exp.Value.Translation.X);
                jtrans.Add("Y", exp.Value.Translation.Y);
                jtrans.Add("Z", exp.Value.Translation.Z);

                jscale.Add("X", exp.Value.Scale3D.X);
                jscale.Add("Y", exp.Value.Scale3D.Y);
                jscale.Add("Z", exp.Value.Scale3D.Z);

                jexp.Add("Rotation", jrot);
                jexp.Add("Translation", jtrans);
                jexp.Add("Scale", jscale);
                break;
            }
            case EX_StructConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Struct", GetFullName(exp.Struct.Index, asset));

                index += 4;
                var jstruct = new JObject();
                var tempindex = 0;
                foreach (var param in exp.Value)
                {
                    var jstructpart = new JArray();
                    jstructpart.Add(SerializeExpression(param, ref index, asset, importVariables));
                    jstruct.Add("Missing property name" + tempindex, jstructpart);
                    tempindex++;
                }

                index++;
                jexp.Add("Properties", jstruct);
                break;
            }
            case EX_SetArray exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("LeftSideExpression", SerializeExpression(exp.AssigningProperty, ref index, asset, importVariables));
                var jparams = new JArray();
                foreach (var param in exp.Elements) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Values", jparams);
                break;
            }
            case EX_ArrayConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add(SerializePropertyPointer(exp.InnerProperty, new[] { "VariableType" }, asset, importVariables));

                index += 4;
                var jparams = new JArray();
                foreach (var param in exp.Elements) jparams.Add(SerializeExpression(param, ref index, asset, importVariables));
                index++;
                jexp.Add("Values", jparams);
                break;
            }
            case EX_ByteConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                index++;
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_IntConstByte exp:
            {
                jexp.Add("Inst", exp.Inst);
                index++;
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_Int64Const exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_UInt64Const exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Value", exp.Value);
                break;
            }
            case EX_FieldPathConst exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Expression", SerializeExpression(exp.Value, ref index, asset, importVariables));
                break;
            }
            case EX_MetaCast exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Class", GetFullName(exp.ClassPtr.Index, asset));
                jexp.Add("Expression", SerializeExpression(exp.TargetExpression, ref index, asset, importVariables));
                break;
            }
            case EX_DynamicCast exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 8;
                jexp.Add("Class", GetFullName(exp.ClassPtr.Index, asset));
                jexp.Add("Expression", SerializeExpression(exp.TargetExpression, ref index, asset, importVariables));
                break;
            }
            case EX_JumpIfNot exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 4;
                jexp.Add("Offset", exp.CodeOffset);
                jexp.Add("Condition", SerializeExpression(exp.BooleanExpression, ref index, asset, importVariables));
                break;
            }
            case EX_Assert exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 3;
                jexp.Add("LineNumber", exp.LineNumber);
                jexp.Add("Debug", exp.DebugMode);
                jexp.Add("Expression", SerializeExpression(exp.AssertExpression, ref index, asset, importVariables));
                break;
            }
            case EX_InstanceDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("FunctionName", exp.FunctionName.ToName());
                break;
            }
            case EX_AddMulticastDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("MulticastDelegate", SerializeExpression(exp.Delegate, ref index, asset, importVariables));
                jexp.Add("Delegate", SerializeExpression(exp.DelegateToAdd, ref index, asset, importVariables));
                break;
            }
            case EX_RemoveMulticastDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("MulticastDelegate", SerializeExpression(exp.Delegate, ref index, asset, importVariables));
                jexp.Add("Delegate", SerializeExpression(exp.DelegateToAdd, ref index, asset, importVariables));
                break;
            }
            case EX_ClearMulticastDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("MulticastDelegate", SerializeExpression(exp.DelegateToClear, ref index, asset, importVariables));
                break;
            }
            case EX_BindDelegate exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 12;
                jexp.Add("FunctionName", exp.FunctionName.ToName());
                jexp.Add("Delegate", SerializeExpression(exp.Delegate, ref index, asset, importVariables));
                jexp.Add("Object", SerializeExpression(exp.ObjectTerm, ref index, asset, importVariables));
                break;
            }
            case EX_PushExecutionFlow exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 4;
                jexp.Add("Offset", exp.PushingAddress);
                break;
            }
            case EX_PopExecutionFlow exp:
            {
                jexp.Add("Inst", exp.Inst);
                break;
            }
            case EX_PopExecutionFlowIfNot exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("Condition", SerializeExpression(exp.BooleanExpression, ref index, asset, importVariables));
                break;
            }
            case EX_Breakpoint exp:
            {
                jexp.Add("Inst", exp.Inst);
                break;
            }
            case EX_WireTracepoint exp:
            {
                jexp.Add("Inst", exp.Inst);
                break;
            }
            case EX_InstrumentationEvent exp:
            {
                jexp.Add("Inst", exp.Inst);
                index++;
                switch (exp.EventType)
                {
                    case EScriptInstrumentationType.Class:
                        jexp.Add("EventType", "Class");
                        break;
                    case EScriptInstrumentationType.ClassScope:
                        jexp.Add("EventType", "ClassScope");
                        break;
                    case EScriptInstrumentationType.Instance:
                        jexp.Add("EventType", "Instance");
                        break;
                    case EScriptInstrumentationType.Event:
                        jexp.Add("EventType", "Event");
                        break;
                    case EScriptInstrumentationType.InlineEvent:
                    {
                        index += 12;
                        jexp.Add("EventType", "InlineEvent");
                        jexp.Add("EventName", exp.EventName.ToName());
                        break;
                    }
                    case EScriptInstrumentationType.ResumeEvent:
                        jexp.Add("EventType", "ResumeEvent");
                        break;
                    case EScriptInstrumentationType.PureNodeEntry:
                        jexp.Add("EventType", "PureNodeEntry");
                        break;
                    case EScriptInstrumentationType.NodeDebugSite:
                        jexp.Add("EventType", "NodeDebugSite");
                        break;
                    case EScriptInstrumentationType.NodeEntry:
                        jexp.Add("EventType", "NodeEntry");
                        break;
                    case EScriptInstrumentationType.NodeExit:
                        jexp.Add("EventType", "NodeExit");
                        break;
                    case EScriptInstrumentationType.PushState:
                        jexp.Add("EventType", "PushState");
                        break;
                    case EScriptInstrumentationType.RestoreState:
                        jexp.Add("EventType", "RestoreState");
                        break;
                    case EScriptInstrumentationType.ResetState:
                        jexp.Add("EventType", "ResetState");
                        break;
                    case EScriptInstrumentationType.SuspendState:
                        jexp.Add("EventType", "SuspendState");
                        break;
                    case EScriptInstrumentationType.PopState:
                        jexp.Add("EventType", "PopState");
                        break;
                    case EScriptInstrumentationType.TunnelEndOfThread:
                        jexp.Add("EventType", "TunnelEndOfThread");
                        break;
                    case EScriptInstrumentationType.Stop:
                        jexp.Add("EventType", "Stop");
                        break;
                }

                break;
            }
            case EX_Tracepoint exp:
            {
                jexp.Add("Inst", exp.Inst);
                break;
            }
            case EX_SwitchValue exp:
            {
                jexp.Add("Inst", exp.Inst);
                index += 6;

                jexp.Add("Expression", SerializeExpression(exp.IndexTerm, ref index, asset, importVariables));
                jexp.Add("OffsetToSwitchEnd", exp.EndGotoOffset);
                var jcases = new JArray();

                for (var j = 0; j < exp.Cases.Length; j++)
                {
                    var jcase = new JObject();
                    jcase.Add("CaseValue", SerializeExpression(exp.Cases[j].CaseIndexValueTerm, ref index, asset, importVariables));
                    index += 4;
                    jcase.Add("OffsetToNextCase", exp.Cases[j].NextOffset);
                    jcase.Add("CaseResult", SerializeExpression(exp.Cases[j].CaseTerm, ref index, asset, importVariables));
                    jcases.Add(jcase);
                }

                jexp.Add("Cases", jcases);
                jexp.Add("DefaultResult", SerializeExpression(exp.DefaultTerm, ref index, asset, importVariables));

                break;
            }
            case EX_ArrayGetByRef exp:
            {
                jexp.Add("Inst", exp.Inst);
                jexp.Add("ArrayExpression", SerializeExpression(exp.ArrayVariable, ref index, asset, importVariables));
                jexp.Add("IndexExpression", SerializeExpression(exp.ArrayIndex, ref index, asset, importVariables));
                break;
            }
        }

        if (addindex) jexp.Add("StatementIndex", savedindex);
        return jexp;
    }

    public static string ReadString(KismetExpression expr, ref int index)
    {
        var result = "";
        index++;
        switch (expr)
        {
            case EX_StringConst exp:
            {
                result = exp.Value;
                index += result.Length + 1;
                break;
            }
            case EX_UnicodeStringConst exp:
            {
                result = exp.Value;
                index += 2 * (result.Length + 1);
                break;
            }
        }

        return result;
    }
}