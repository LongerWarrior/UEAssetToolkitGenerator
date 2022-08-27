using UAssetAPI.Kismet.Bytecode.Expressions;

namespace UAssetAPI.Kismet.Bytecode;
public static class ExpressionSerializer
{
    public static KismetExpression ReadExpression(AssetBinaryReader reader)
    {
		KismetExpression res = null;
        EExprToken token = (EExprToken)reader.ReadByte();
        switch (token)
        {
			case EExprToken.LocalVariable:
				res = new EX_LocalVariable();
				break;
			case EExprToken.InstanceVariable:
				res = new EX_InstanceVariable();
				break;
			case EExprToken.DefaultVariable:
				res = new EX_DefaultVariable();
				break;
			case EExprToken.Return:
				res = new EX_Return();
				break;
			case EExprToken.Jump:
				res = new EX_Jump();
				break;
			case EExprToken.JumpIfNot:
				res = new EX_JumpIfNot();
				break;
			case EExprToken.Assert:
				res = new EX_Assert();
				break;
			case EExprToken.Nothing:
				res = new EX_Nothing();
				break;
			case EExprToken.Let:
				res = new EX_Let();
				break;
			case EExprToken.ClassContext:
				res = new EX_ClassContext();
				break;
			case EExprToken.MetaCast:
				res = new EX_MetaCast();
				break;
			case EExprToken.LetBool:
				res = new EX_LetBool();
				break;
			case EExprToken.EndParmValue:
				res = new EX_EndParmValue();
				break;
			case EExprToken.EndFunctionParms:
				res = new EX_EndFunctionParms();
				break;
			case EExprToken.Self:
				res = new EX_Self();
				break;
			case EExprToken.Skip:
				res = new EX_Skip();
				break;
			case EExprToken.Context:
				res = new EX_Context();
				break;
			case EExprToken.Context_FailSilent:
				res = new EX_Context_FailSilent();
				break;
			case EExprToken.VirtualFunction:
				res = new EX_VirtualFunction();
				break;
			case EExprToken.FinalFunction:
				res = new EX_FinalFunction();
				break;
			case EExprToken.IntConst:
				res = new EX_IntConst();
				break;
			case EExprToken.FloatConst:
				res = new EX_FloatConst();
				break;
			case EExprToken.StringConst:
				res = new EX_StringConst();
				break;
			case EExprToken.ObjectConst:
				res = new EX_ObjectConst();
				break;
			case EExprToken.NameConst:
				res = new EX_NameConst();
				break;
			case EExprToken.RotationConst:
				res = new EX_RotationConst();
				break;
			case EExprToken.VectorConst:
				res = new EX_VectorConst();
				break;
			case EExprToken.ByteConst:
				res = new EX_ByteConst();
				break;
			case EExprToken.IntZero:
				res = new EX_IntZero();
				break;
			case EExprToken.IntOne:
				res = new EX_IntOne();
				break;
			case EExprToken.True:
				res = new EX_True();
				break;
			case EExprToken.False:
				res = new EX_False();
				break;
			case EExprToken.TextConst:
				res = new EX_TextConst();
				break;
			case EExprToken.NoObject:
				res = new EX_NoObject();
				break;
			case EExprToken.TransformConst:
				res = new EX_TransformConst();
				break;
			case EExprToken.IntConstByte:
				res = new EX_IntConstByte();
				break;
			case EExprToken.NoInterface:
				res = new EX_NoInterface();
				break;
			case EExprToken.DynamicCast:
				res = new EX_DynamicCast();
				break;
			case EExprToken.StructConst:
				res = new EX_StructConst();
				break;
			case EExprToken.EndStructConst:
				res = new EX_EndStructConst();
				break;
			case EExprToken.SetArray:
				res = new EX_SetArray();
				break;
			case EExprToken.EndArray:
				res = new EX_EndArray();
				break;
			case EExprToken.PropertyConst:
				res = new EX_PropertyConst();
				break;
			case EExprToken.UnicodeStringConst:
				res = new EX_UnicodeStringConst();
				break;
			case EExprToken.Int64Const:
				res = new EX_Int64Const();
				break;
			case EExprToken.UInt64Const:
				res = new EX_UInt64Const();
				break;
			case EExprToken.PrimitiveCast:
				res = new EX_PrimitiveCast();
				break;
			case EExprToken.SetSet:
				res = new EX_SetSet();
				break;
			case EExprToken.EndSet:
				res = new EX_EndSet();
				break;
			case EExprToken.SetMap:
				res = new EX_SetMap();
				break;
			case EExprToken.EndMap:
				res = new EX_EndMap();
				break;
			case EExprToken.SetConst:
				res = new EX_SetConst();
				break;
			case EExprToken.EndSetConst:
				res = new EX_EndSetConst();
				break;
			case EExprToken.MapConst:
				res = new EX_MapConst();
				break;
			case EExprToken.EndMapConst:
				res = new EX_EndMapConst();
				break;
			case EExprToken.StructMemberContext:
				res = new EX_StructMemberContext();
				break;
			case EExprToken.LetMulticastDelegate:
				res = new EX_LetMulticastDelegate();
				break;
			case EExprToken.LetDelegate:
				res = new EX_LetDelegate();
				break;
			case EExprToken.LocalVirtualFunction:
				res = new EX_LocalVirtualFunction();
				break;
			case EExprToken.LocalFinalFunction:
				res = new EX_LocalFinalFunction();
				break;
			case EExprToken.LocalOutVariable:
				res = new EX_LocalOutVariable();
				break;
			case EExprToken.DeprecatedOp4A:
				res = new EX_DeprecatedOp4A();
				break;
			case EExprToken.InstanceDelegate:
				res = new EX_InstanceDelegate();
				break;
			case EExprToken.PushExecutionFlow:
				res = new EX_PushExecutionFlow();
				break;
			case EExprToken.PopExecutionFlow:
				res = new EX_PopExecutionFlow();
				break;
			case EExprToken.ComputedJump:
				res = new EX_ComputedJump();
				break;
			case EExprToken.PopExecutionFlowIfNot:
				res = new EX_PopExecutionFlowIfNot();
				break;
			case EExprToken.Breakpoint:
				res = new EX_Breakpoint();
				break;
			case EExprToken.InterfaceContext:
				res = new EX_InterfaceContext();
				break;
			case EExprToken.ObjToInterfaceCast:
				res = new EX_ObjToInterfaceCast();
				break;
			case EExprToken.EndOfScript:
				res = new EX_EndOfScript();
				break;
			case EExprToken.CrossInterfaceCast:
				res = new EX_CrossInterfaceCast();
				break;
			case EExprToken.InterfaceToObjCast:
				res = new EX_InterfaceToObjCast();
				break;
			case EExprToken.WireTracepoint:
				res = new EX_WireTracepoint();
				break;
			case EExprToken.SkipOffsetConst:
				res = new EX_SkipOffsetConst();
				break;
			case EExprToken.AddMulticastDelegate:
				res = new EX_AddMulticastDelegate();
				break;
			case EExprToken.ClearMulticastDelegate:
				res = new EX_ClearMulticastDelegate();
				break;
			case EExprToken.Tracepoint:
				res = new EX_Tracepoint();
				break;
			case EExprToken.LetObj:
				res = new EX_LetObj();
				break;
			case EExprToken.LetWeakObjPtr:
				res = new EX_LetWeakObjPtr();
				break;
			case EExprToken.BindDelegate:
				res = new EX_BindDelegate();
				break;
			case EExprToken.RemoveMulticastDelegate:
				res = new EX_RemoveMulticastDelegate();
				break;
			case EExprToken.CallMulticastDelegate:
				res = new EX_CallMulticastDelegate();
				break;
			case EExprToken.LetValueOnPersistentFrame:
				res = new EX_LetValueOnPersistentFrame();
				break;
			case EExprToken.ArrayConst:
				res = new EX_ArrayConst();
				break;
			case EExprToken.EndArrayConst:
				res = new EX_EndArrayConst();
				break;
			case EExprToken.SoftObjectConst:
				res = new EX_SoftObjectConst();
				break;
			case EExprToken.CallMath:
				res = new EX_CallMath();
				break;
			case EExprToken.SwitchValue:
				res = new EX_SwitchValue();
				break;
			case EExprToken.InstrumentationEvent:
				res = new EX_InstrumentationEvent();
				break;
			case EExprToken.ArrayGetByRef:
				res = new EX_ArrayGetByRef();
				break;
			case EExprToken.ClassSparseDataVariable:
				res = new EX_ClassSparseDataVariable();
				break;
			case EExprToken.FieldPathConst:
				res = new EX_FieldPathConst();
				break;
			default:
				throw new NotImplementedException("Unimplemented token " + token);
		}

		if (res != null)
        {
			res.Read(reader);
        }
        return res;
    }

    public static int WriteExpression(KismetExpression expr, AssetBinaryWriter writer)
    {
		writer.Write((byte)expr.Token);
		return expr.Write(writer) + sizeof(byte);
    }
}

