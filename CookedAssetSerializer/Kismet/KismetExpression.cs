using Newtonsoft.Json.Linq;
using UAssetAPI.FieldTypes;
using UAssetAPI.Kismet.Bytecode;
using UAssetAPI.Kismet.Bytecode.Expressions;
using System;
using static CookedAssetSerializer.Utils;
using UAssetAPI;

namespace CookedAssetSerializer {
    public class KismetExpressionSerializer {
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

        public static FEdGraphPinType GetPropertyCategoryInfo(FProperty prop) {
            var pin = new FEdGraphPinType();
            switch (prop) {
                case FInterfaceProperty fInterface: {
                    pin.PinCategory = PC_Interface;
                    pin.PinSubCategoryObject = GetFullName(fInterface.InterfaceClass.Index);
                    break;
                }
                case FClassProperty fClassProp: {
                    pin.PinCategory = PC_Class;
                    pin.PinSubCategoryObject = GetFullName(fClassProp.MetaClass.Index);
                    break;
                }
                case FSoftClassProperty fSoftClassProp: {
                    pin.PinCategory = PC_SoftClass;
                    pin.PinSubCategoryObject = GetFullName(fSoftClassProp.MetaClass.Index);
                    break;
                }
                case FSoftObjectProperty fSoftObjectProp: {
                    pin.PinCategory = PC_SoftObject;
                    pin.PinSubCategoryObject = GetFullName(fSoftObjectProp.PropertyClass.Index);
                    break;
                }
                case FObjectProperty fObjectProp: {
                    pin.PinCategory = PC_Object;
                    pin.PinSubCategoryObject = GetFullName(fObjectProp.PropertyClass.Index);
                    if (fObjectProp.PropertyFlags.HasFlag(EPropertyFlags.CPF_AutoWeak)) pin.bIsWeakPointer = true;
                    break;
                }
                case FStructProperty fStruct: {
                    pin.PinCategory = PC_Struct;
                    pin.PinSubCategoryObject = GetFullName(fStruct.Struct.Index);
                    break;
                }
                case FByteProperty fByte: {
                    pin.PinCategory = PC_Byte;
                    pin.PinSubCategoryObject = GetFullName(fByte.Enum.Index);
                    break;
                }
                case FEnumProperty fEnum: {
                    if (fEnum.UnderlyingProp is not FByteProperty) break;
                    pin.PinCategory = PC_Byte;
                    pin.PinSubCategoryObject = GetFullName(fEnum.Enum.Index);
                    break;
                }
                case FBoolProperty: {
                    pin.PinCategory = PC_Boolean;
                    break;
                }
                case FGenericProperty fGeneric: {
                    switch (fGeneric.SerializedType.ToName()) {
                        case "FloatProperty": {
                            pin.PinCategory = PC_Float;
                            break;
                        }
                        case "Int64Property": {
                            pin.PinCategory = PC_Int64;
                            break;
                        }
                        case "IntProperty": {
                            pin.PinCategory = PC_Int;
                            break;
                        }
                        case "NameProperty": {
                            pin.PinCategory = PC_Name;
                            break;
                        }
                        case "StrProperty": {
                            pin.PinCategory = PC_String;
                            break;
                        }
                        case "TextProperty": {
                            pin.PinCategory = PC_Text;
                            break;
                        }
                    }
                    break;
                }
            }
            return pin;
        }

        public static FSimpleMemberReference FillSimpleMemberReference(int index) {
            var member = new FSimpleMemberReference();
            switch (index) {
                case > 0:
                    member.MemberName = Asset.Exports[index - 1].ObjectName.ToName();
                    member.MemberParent = GetName(Asset.Exports[index - 1].OuterIndex.Index);
                    member.MemberGuid = Asset.Exports[index - 1].PackageGuid;
                    break;
                case < 0:
                    member.MemberName = Asset.Imports[-index - 1].ObjectName.ToName();
                    member.MemberParent = Asset.Imports[-index - 1].ClassPackage.ToName();
                    member.MemberGuid = new Guid("00000000000000000000000000000000");
                    break;
            }

            return member;
        }

        public static JObject SerializeGraphPinType(FEdGraphPinType pin) {
            var jPin = new JObject {
                { "PinCategory", pin.PinCategory },
                { "PinSubCategory", pin.PinCategory }
            };
            if (string.IsNullOrEmpty(pin.PinSubCategoryObject)) {
            } else { jPin.Add("PinSubCategoryObject", pin.PinSubCategoryObject); }

            if (pin.PinSubCategoryMemberReference.MemberName != null) {
                var member = pin.PinSubCategoryMemberReference;
                if (member.MemberGuid.Equals(new Guid("00000000000000000000000000000000"))) {
                } else {
                    var jMember = new JObject {
                        { "MemberParent", member.MemberParent },
                        { "MemberName", member.MemberName },
                        { "MemberGuid", member.MemberGuid }
                    };
                    jPin.Add("PinSubCategoryMemberReference", jMember);
                }
            }

            if (pin.ContainerType == EPinContainerType.Map) {
                var valueType = pin.PinValueType;
                var jValueType = new JObject { { "TerminalCategory", valueType.TerminalCategory } };

                jValueType.Add("TerminalSubCategory", string.IsNullOrEmpty(valueType.TerminalSubCategory) ? "None" : valueType.TerminalSubCategory);
                if (!string.IsNullOrEmpty(valueType.TerminalSubCategoryObject)) jValueType.Add("TerminalSubCategoryObject", valueType.TerminalSubCategoryObject);
                jValueType.Add("TerminalIsConst", valueType.bTerminalIsConst);
                jValueType.Add("TerminalIsWeakPointer", valueType.bTerminalIsWeakPointer);
                jPin.Add("PinValueType", jValueType);
            }

            if (pin.ContainerType != EPinContainerType.None) jPin.Add("ContainerType", (int)pin.ContainerType);

            if (pin.bIsReference) jPin.Add("IsReference", true);
            if (pin.bIsConst) jPin.Add("IsConst", true);
            if (pin.bIsWeakPointer) jPin.Add("IsWeakPointer", true);
            return jPin;
        }

        public static FEdGraphPinType ConvertPropertyToPinType(FProperty property) {
            var pin = new FEdGraphPinType();
            var prop = property;

            switch (property) {
                case FMapProperty mapProperty: {
                    prop = mapProperty.KeyProp;
                    pin.ContainerType = EPinContainerType.Map;
                    pin.bIsWeakPointer = false;

                    var tempPin = GetPropertyCategoryInfo(mapProperty.ValueProp);
                    pin.PinValueType.TerminalCategory = tempPin.PinCategory;
                    pin.PinValueType.TerminalSubCategory = tempPin.PinSubCategory;
                    pin.PinValueType.TerminalSubCategoryObject = tempPin.PinSubCategoryObject;

                    pin.PinValueType.bTerminalIsConst = tempPin.bIsConst;
                    pin.PinValueType.bTerminalIsWeakPointer = tempPin.bIsWeakPointer;
                    break;
                }
                case FSetProperty setProperty:
                    prop = setProperty.ElementProp;
                    pin.ContainerType = EPinContainerType.Set;
                    break;
                case FArrayProperty arrayProperty:
                    prop = arrayProperty.Inner;
                    pin.ContainerType = EPinContainerType.Array;
                    break;
            }

            pin.bIsReference = property.PropertyFlags.HasFlag(EPropertyFlags.CPF_OutParm) &&
                               property.PropertyFlags.HasFlag(EPropertyFlags.CPF_ReferenceParm);
            pin.bIsConst = property.PropertyFlags.HasFlag(EPropertyFlags.CPF_ConstParm);


            switch (prop) {
                case FMulticastDelegateProperty delegateProperty:
                    pin.PinCategory = PC_MCDelegate;
                    pin.PinSubCategoryMemberReference = FillSimpleMemberReference(delegateProperty.SignatureFunction.Index);
                    break;
                case FDelegateProperty delegateProperty:
                    pin.PinCategory = PC_Delegate;
                    pin.PinSubCategoryMemberReference = FillSimpleMemberReference(delegateProperty.SignatureFunction.Index);
                    break;
                default: {
                    var tempPin = GetPropertyCategoryInfo(prop);
                    pin.PinCategory = tempPin.PinCategory;
                    pin.PinSubCategory = tempPin.PinSubCategory;
                    pin.PinSubCategoryObject = tempPin.PinSubCategoryObject;
                    pin.bIsWeakPointer = tempPin.bIsWeakPointer;
                    break;
                }
            }

            return pin;
        }

        public static JProperty[] SerializePropertyPointer(KismetPropertyPointer pointer, string[] names) {
            var jPropArray = new JProperty[names.Length];

            if (pointer == null || pointer.New.ResolvedOwner.Index == 0) return Array.Empty<JProperty>();
            if (FindProperty(pointer.New.ResolvedOwner.Index, pointer.New.Path[0], out var property)) {
                var propertyType = ConvertPropertyToPinType(property);
                jPropArray[0] = new JProperty(names[0], SerializeGraphPinType(propertyType));
            } else {
                jPropArray[0] = new JProperty(names[0], "##NOT SERIALIZED##");
            }

            if (names.Length > 1) jPropArray[1] = new JProperty(names[1], pointer.New.Path[0].ToName());

            return jPropArray;

        }

        public static JObject SerializeExpression(KismetExpression expression, ref int index, bool addindex = false) {
            var saveIndex = index;
            var jExp = new JObject();
            index++;
            switch (expression) {
                case EX_PrimitiveCast exp: {
                    jExp.Add("Inst", exp.Inst);
                    index++;
                    switch (exp.ConversionType) {
                        case ECastToken.InterfaceToBool: {
                            jExp.Add("CastType", "InterfaceToBool");
                            break;
                        }
                        case ECastToken.ObjectToBool: {
                            jExp.Add("CastType", "ObjectToBool");
                            break;
                        }
                        case ECastToken.ObjectToInterface: {
                            jExp.Add("CastType", "ObjectToInterface");
                            index += 8;
                            jExp.Add("InterfaceClass", "##NOT SERIALIZED##");
                            break;
                        }
                    }

                    jExp.Add("Expression", SerializeExpression(exp.Target, ref index));
                    break;
                }
                case EX_SetSet exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("LeftSideExpression", SerializeExpression(exp.SetProperty, ref index));
                    var jParams = new JArray();

                    index += 4;
                    foreach (var param in exp.Elements) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Values", jParams);
                    break;
                }
                case EX_SetConst exp: {
                    index += 8;
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add(SerializePropertyPointer(exp.InnerProperty, new[] { "InnerProperty" }));

                    index += 4;
                    var jParams = new JArray();
                    foreach (var param in exp.Elements) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Values", jParams);
                    break;
                }
                case EX_SetMap exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("LeftSideExpression", SerializeExpression(exp.MapProperty, ref index));

                    index += 4;
                    var jParams = new JArray();
                    for (var j = 1; j <= exp.Elements.Length / 2; j++) {
                        var jObject = new JObject();
                        jObject.Add("Key", SerializeExpression(exp.Elements[2 * (j - 1)], ref index));
                        jObject.Add("Value", SerializeExpression(exp.Elements[2 * (j - 1) + 1], ref index));
                        jParams.Add(jObject);
                    }

                    index++;
                    jExp.Add("Values", jParams);
                    break;
                }
                case EX_MapConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.KeyProperty, new[] { "KeyProperty" }));
                    jExp.Add(SerializePropertyPointer(exp.ValueProperty, new[] { "ValueProperty" }));

                    index += 4;
                    var jParams = new JArray();
                    for (var j = 1; j <= exp.Elements.Length / 2; j++) {
                        var jObject = new JObject();
                        jObject.Add("Key", SerializeExpression(exp.Elements[2 * (j - 1)], ref index));
                        jObject.Add("Value", SerializeExpression(exp.Elements[2 * (j - 1) + 1], ref index));
                        jParams.Add(jObject);
                    }

                    index++;
                    jExp.Add("Values", jParams);
                    break;
                }
                case EX_ObjToInterfaceCast exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("InterfaceClass", GetFullName(exp.ClassPtr.Index));
                    jExp.Add("Expression", SerializeExpression(exp.Target, ref index));
                    break;
                }
                case EX_CrossInterfaceCast exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("InterfaceClass", GetFullName(exp.ClassPtr.Index));
                    jExp.Add("Expression", SerializeExpression(exp.Target, ref index));
                    break;
                }
                case EX_InterfaceToObjCast exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("ObjectClass", GetFullName(exp.ClassPtr.Index));
                    jExp.Add("Expression", SerializeExpression(exp.Target, ref index));
                    break;
                }
                case EX_Let exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Variable", SerializeExpression(exp.Variable, ref index));
                    jExp.Add("Expression", SerializeExpression(exp.Expression, ref index));
                    break;
                }
                case EX_LetObj exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index));
                    jExp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index));
                    break;
                }
                case EX_LetWeakObjPtr exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index));
                    jExp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index));
                    break;
                }
                case EX_LetBool exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index));
                    jExp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index));
                    break;
                }
                case EX_LetValueOnPersistentFrame exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("PropertyName", exp.DestinationProperty.New.Path[0].ToName());

                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.DestinationProperty, new[] { "PropertyType" }));
                    jExp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index));
                    break;
                }
                case EX_StructMemberContext exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.Property, new[] { "PropertyType" }));
                    jExp.Add("PropertyName", exp.Property.New.Path[0].ToName());
                    jExp.Add("StructExpression", SerializeExpression(exp.StructExpression, ref index));
                    break;
                }
                case EX_LetDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index));
                    jExp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index));
                    break;
                }
                case EX_LocalVirtualFunction exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("FunctionName", exp.VirtualFunctionName.ToName());
                    var jParams = new JArray();
                    foreach (var param in exp.Parameters) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Parameters", jParams);
                    break;
                }
                case EX_LocalFinalFunction exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Function", GetName(exp.StackNode.Index));
                    index += 8;
                    var jParams = new JArray();
                    foreach (var param in exp.Parameters) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Parameters", jParams);
                    break;
                }
                case EX_LetMulticastDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Variable", SerializeExpression(exp.VariableExpression, ref index));
                    jExp.Add("Expression", SerializeExpression(exp.AssignmentExpression, ref index));
                    break;
                }
                case EX_ComputedJump exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("OffsetExpression", SerializeExpression(exp.CodeOffsetExpression, ref index));
                    break;
                }
                case EX_Jump exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 4;
                    jExp.Add("Offset", exp.CodeOffset);
                    break;
                }
                case EX_LocalVariable exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }));
                    break;
                }
                case EX_DefaultVariable exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }));
                    break;
                }
                case EX_InstanceVariable exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }));
                    break;
                }
                case EX_LocalOutVariable exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.Variable, new[] { "VariableType", "VariableName" }));
                    break;
                }
                case EX_InterfaceContext exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Expression", SerializeExpression(exp.InterfaceValue, ref index));
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
                case EX_Self: {
                    jExp.Add("Inst", expression.Inst);
                    break;
                }
                case EX_Return exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Expression", SerializeExpression(exp.ReturnExpression, ref index));
                    break;
                }
                case EX_CallMath exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Function", GetName(exp.StackNode.Index));
                    jExp.Add("ContextClass", GetParentName(exp.StackNode.Index));
                    var jParams = new JArray();
                    foreach (var param in exp.Parameters) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Parameters", jParams);
                    break;
                }
                case EX_CallMulticastDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    var jSign = new JObject();
                    var bIsSelfContext = GetClassIndex() == exp.StackNode.Index;
                    jSign.Add("IsSelfContext", bIsSelfContext);
                    jSign.Add("MemberParent", GetFullName(exp.StackNode.Index));
                    jSign.Add("MemberName", GetName(exp.StackNode.Index));
                    jExp.Add("DelegateSignatureFunction", jSign);
                    jExp.Add("Delegate", SerializeExpression(exp.Delegate, ref index));

                    var jParams = new JArray();
                    foreach (var param in exp.Parameters) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Parameters", jParams);
                    break;
                }
                case EX_FinalFunction exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Function", GetName(exp.StackNode.Index));
                    var jParams = new JArray();
                    foreach (var param in exp.Parameters) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Parameters", jParams);
                    break;
                }
                case EX_VirtualFunction exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("Function", exp.VirtualFunctionName.ToName());
                    var jParams = new JArray();

                    foreach (var param in exp.Parameters) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Parameters", jParams);
                    break;
                }
                //case EX_ClassContext:
                //case EX_Context_FailSilent: {
                case EX_Context exp: {
                    exp = exp switch {
                        EX_Context_FailSilent silent => silent,
                        EX_ClassContext context => context,
                        _ => exp
                    };

                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Context", SerializeExpression(exp.ObjectExpression, ref index));
                    index += 4;
                    jExp.Add("SkipOffsetForNull", exp.Offset);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.RValuePointer,
                        new[] { "RValuePropertyType", "RValuePropertyName" }));
                    jExp.Add("Expression", SerializeExpression(exp.ContextExpression, ref index));
                    break;
                }
                case EX_IntConst exp: {
                    index += 4;
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_SkipOffsetConst exp: {
                    index += 4;
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_FloatConst exp: {
                    index += 4;
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_StringConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += exp.Value.Length + 1;
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_UnicodeStringConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 2 * (exp.Value.Length + 1);
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_TextConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index++;
                    switch (exp.Value.TextLiteralType) {
                        case EBlueprintTextLiteralType.Empty: {
                            jExp.Add("TextLiteralType", "Empty");
                            break;
                        }
                        case EBlueprintTextLiteralType.LocalizedText: {
                            jExp.Add("TextLiteralType", "LocalizedText");
                            jExp.Add("SourceString", ReadString(exp.Value.LocalizedSource, ref index));
                            jExp.Add("LocalizationKey", ReadString(exp.Value.LocalizedKey, ref index));
                            jExp.Add("LocalizationNamespace", ReadString(exp.Value.LocalizedNamespace, ref index));
                            break;
                        }
                        case EBlueprintTextLiteralType.InvariantText: {
                            jExp.Add("TextLiteralType", "InvariantText");
                            jExp.Add("SourceString", ReadString(exp.Value.InvariantLiteralString, ref index));

                            break;
                        }
                        case EBlueprintTextLiteralType.LiteralString: {
                            jExp.Add("TextLiteralType", "LiteralString");
                            jExp.Add("SourceString", ReadString(exp.Value.LiteralString, ref index));
                            break;
                        }
                        case EBlueprintTextLiteralType.StringTableEntry: {
                            jExp.Add("TextLiteralType", "StringTableEntry");
                            index += 8;
                            jExp.Add("TableId", ReadString(exp.Value.StringTableId, ref index));
                            jExp.Add("TableKey", ReadString(exp.Value.StringTableKey, ref index));
                            break;
                        }
                    }

                    break;
                }
                case EX_ObjectConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Object", GetFullName(exp.Value.Index));
                    break;
                }
                case EX_SoftObjectConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Value", SerializeExpression(exp.Value, ref index));
                    break;
                }
                case EX_NameConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("Value", exp.Value.ToName());
                    break;
                }
                case EX_RotationConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("Pitch", exp.Pitch);
                    jExp.Add("Yaw", exp.Yaw);
                    jExp.Add("Roll", exp.Roll);
                    break;
                }
                case EX_VectorConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("X", exp.Value.X);
                    jExp.Add("Y", exp.Value.Y);
                    jExp.Add("Z", exp.Value.Z);
                    break;
                }
                case EX_TransformConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 40;
                    var jRot = new JObject();
                    var jTrans = new JObject();
                    var jScale = new JObject();

                    jRot.Add("X", exp.Value.Rotation.X);
                    jRot.Add("Y", exp.Value.Rotation.Y);
                    jRot.Add("Z", exp.Value.Rotation.Z);
                    jRot.Add("W", exp.Value.Rotation.W);

                    jTrans.Add("X", exp.Value.Translation.X);
                    jTrans.Add("Y", exp.Value.Translation.Y);
                    jTrans.Add("Z", exp.Value.Translation.Z);

                    jScale.Add("X", exp.Value.Scale3D.X);
                    jScale.Add("Y", exp.Value.Scale3D.Y);
                    jScale.Add("Z", exp.Value.Scale3D.Z);

                    jExp.Add("Rotation", jRot);
                    jExp.Add("Translation", jTrans);
                    jExp.Add("Scale", jScale);
                    break;
                }
                case EX_StructConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Struct", GetFullName(exp.Struct.Index));

                    index += 4;
                    var jStruct = new JObject();
                    var tempIndex = 0;
                    foreach (var param in exp.Value) {
                        var jStructPart = new JArray { SerializeExpression(param, ref index) };
                        jStruct.Add("Missing property name" + tempIndex, jStructPart);
                        tempIndex++;
                    }

                    index++;
                    jExp.Add("Properties", jStruct);
                    break;
                }
                case EX_SetArray exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("LeftSideExpression", SerializeExpression(exp.AssigningProperty, ref index));
                    var jParams = new JArray();
                    foreach (var param in exp.Elements) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Values", jParams);
                    break;
                }
                case EX_ArrayConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add(SerializePropertyPointer(exp.InnerProperty, new[] { "VariableType" }));

                    index += 4;
                    var jParams = new JArray();
                    foreach (var param in exp.Elements) jParams.Add(SerializeExpression(param, ref index));
                    index++;
                    jExp.Add("Values", jParams);
                    break;
                }
                case EX_ByteConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    index++;
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_IntConstByte exp: {
                    jExp.Add("Inst", exp.Inst);
                    index++;
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_Int64Const exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_UInt64Const exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Value", exp.Value);
                    break;
                }
                case EX_FieldPathConst exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Expression", SerializeExpression(exp.Value, ref index));
                    break;
                }
                case EX_MetaCast exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Class", GetFullName(exp.ClassPtr.Index));
                    jExp.Add("Expression", SerializeExpression(exp.TargetExpression, ref index));
                    break;
                }
                case EX_DynamicCast exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 8;
                    jExp.Add("Class", GetFullName(exp.ClassPtr.Index));
                    jExp.Add("Expression", SerializeExpression(exp.TargetExpression, ref index));
                    break;
                }
                case EX_JumpIfNot exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 4;
                    jExp.Add("Offset", exp.CodeOffset);
                    jExp.Add("Condition", SerializeExpression(exp.BooleanExpression, ref index));
                    break;
                }
                case EX_Assert exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 3;
                    jExp.Add("LineNumber", exp.LineNumber);
                    jExp.Add("Debug", exp.DebugMode);
                    jExp.Add("Expression", SerializeExpression(exp.AssertExpression, ref index));
                    break;
                }
                case EX_InstanceDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("FunctionName", exp.FunctionName.ToName());
                    break;
                }
                case EX_AddMulticastDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("MulticastDelegate", SerializeExpression(exp.Delegate, ref index));
                    jExp.Add("Delegate", SerializeExpression(exp.DelegateToAdd, ref index));
                    break;
                }
                case EX_RemoveMulticastDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("MulticastDelegate", SerializeExpression(exp.Delegate, ref index));
                    jExp.Add("Delegate", SerializeExpression(exp.DelegateToAdd, ref index));
                    break;
                }
                case EX_ClearMulticastDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("MulticastDelegate", SerializeExpression(exp.DelegateToClear, ref index));
                    break;
                }
                case EX_BindDelegate exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 12;
                    jExp.Add("FunctionName", exp.FunctionName.ToName());
                    jExp.Add("Delegate", SerializeExpression(exp.Delegate, ref index));
                    jExp.Add("Object", SerializeExpression(exp.ObjectTerm, ref index));
                    break;
                }
                case EX_PushExecutionFlow exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 4;
                    jExp.Add("Offset", exp.PushingAddress);
                    break;
                }
                case EX_PopExecutionFlow exp: {
                    jExp.Add("Inst", exp.Inst);
                    break;
                }
                case EX_PopExecutionFlowIfNot exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("Condition", SerializeExpression(exp.BooleanExpression, ref index));
                    break;
                }
                case EX_Breakpoint exp: {
                    jExp.Add("Inst", exp.Inst);
                    break;
                }
                case EX_WireTracepoint exp: {
                    jExp.Add("Inst", exp.Inst);
                    break;
                }
                case EX_InstrumentationEvent exp: {
                    jExp.Add("Inst", exp.Inst);
                    index++;
                    switch (exp.EventType) {
                        case EScriptInstrumentationType.Class:
                            jExp.Add("EventType", "Class");
                            break;
                        case EScriptInstrumentationType.ClassScope:
                            jExp.Add("EventType", "ClassScope");
                            break;
                        case EScriptInstrumentationType.Instance:
                            jExp.Add("EventType", "Instance");
                            break;
                        case EScriptInstrumentationType.Event:
                            jExp.Add("EventType", "Event");
                            break;
                        case EScriptInstrumentationType.InlineEvent: {
                            index += 12;
                            jExp.Add("EventType", "InlineEvent");
                            jExp.Add("EventName", exp.EventName.ToName());
                            break;
                        }
                        case EScriptInstrumentationType.ResumeEvent:
                            jExp.Add("EventType", "ResumeEvent");
                            break;
                        case EScriptInstrumentationType.PureNodeEntry:
                            jExp.Add("EventType", "PureNodeEntry");
                            break;
                        case EScriptInstrumentationType.NodeDebugSite:
                            jExp.Add("EventType", "NodeDebugSite");
                            break;
                        case EScriptInstrumentationType.NodeEntry:
                            jExp.Add("EventType", "NodeEntry");
                            break;
                        case EScriptInstrumentationType.NodeExit:
                            jExp.Add("EventType", "NodeExit");
                            break;
                        case EScriptInstrumentationType.PushState:
                            jExp.Add("EventType", "PushState");
                            break;
                        case EScriptInstrumentationType.RestoreState:
                            jExp.Add("EventType", "RestoreState");
                            break;
                        case EScriptInstrumentationType.ResetState:
                            jExp.Add("EventType", "ResetState");
                            break;
                        case EScriptInstrumentationType.SuspendState:
                            jExp.Add("EventType", "SuspendState");
                            break;
                        case EScriptInstrumentationType.PopState:
                            jExp.Add("EventType", "PopState");
                            break;
                        case EScriptInstrumentationType.TunnelEndOfThread:
                            jExp.Add("EventType", "TunnelEndOfThread");
                            break;
                        case EScriptInstrumentationType.Stop:
                            jExp.Add("EventType", "Stop");
                            break;
                    }
                    break;
                }
                case EX_Tracepoint exp: {
                    jExp.Add("Inst", exp.Inst);
                    break;
                }
                case EX_SwitchValue exp: {
                    jExp.Add("Inst", exp.Inst);
                    index += 6;

                    jExp.Add("Expression", SerializeExpression(exp.IndexTerm, ref index));
                    jExp.Add("OffsetToSwitchEnd", exp.EndGotoOffset);
                    var jCases = new JArray();

                    for (var j = 0; j < exp.Cases.Length; j++) {
                        var jCase = new JObject { { "CaseValue", SerializeExpression(exp.Cases[j].CaseIndexValueTerm, ref index) } };
                        index += 4;
                        jCase.Add("OffsetToNextCase", exp.Cases[j].NextOffset);
                        jCase.Add("CaseResult", SerializeExpression(exp.Cases[j].CaseTerm, ref index));
                        jCases.Add(jCase);
                    }

                    jExp.Add("Cases", jCases);
                    jExp.Add("DefaultResult", SerializeExpression(exp.DefaultTerm, ref index));

                    break;
                }
                case EX_ArrayGetByRef exp: {
                    jExp.Add("Inst", exp.Inst);
                    jExp.Add("ArrayExpression", SerializeExpression(exp.ArrayVariable, ref index));
                    jExp.Add("IndexExpression", SerializeExpression(exp.ArrayIndex, ref index));
                    break;
                }
                default: {
                    // This should never occur.
                    //checkf(0, TEXT("Unknown bytecode 0x%02X"), (uint8)Opcode);
                    break;
                }
            }

            if (addindex) jExp.Add("StatementIndex", saveIndex);
            return jExp;
        }

        public static string ReadString(KismetExpression expr, ref int index) {
            var result = "";
            index++;
            switch (expr) {
                case EX_StringConst exp: {
                    result = exp.Value;
                    index += result.Length + 1;
                    break;
                }
                case EX_UnicodeStringConst exp: {
                    result = exp.Value;
                    index += 2 * (result.Length + 1);
                    break;
                }
            }

            return result;
        }
    }
}
