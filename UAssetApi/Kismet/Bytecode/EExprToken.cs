namespace UAssetAPI.Kismet.Bytecode;

/// <summary>
/// Evaluatable expression item types.
/// </summary>
public enum EExprToken
{
	/// <summary>A local variable.</summary>
	LocalVariable = 0x00,
	/// <summary>An object variable.</summary>
	InstanceVariable = 0x01,
	/// <summary>Default variable for a class context.</summary>
	DefaultVariable = 0x02,
	/// <summary>Return from function.</summary>
	Return = 0x04,
	/// <summary>Goto a local address in code.</summary>
	Jump = 0x06,
	/// <summary>Goto if not expression.</summary>
	JumpIfNot = 0x07,
	/// <summary>Assertion.</summary>
	Assert = 0x09,
	/// <summary>No operation.</summary>
	Nothing = 0x0B,
	/// <summary>Assign an arbitrary size value to a variable.</summary>
	Let = 0x0F,
	/// <summary>Class default object context.</summary>
	ClassContext = 0x12,
	/// <summary>Metaclass cast.</summary>
	MetaCast = 0x13,
	/// <summary>Let boolean variable.</summary>
	LetBool = 0x14,
	/// <summary>end of default value for optional function parameter</summary>
	EndParmValue = 0x15,
	/// <summary>End of function call parameters.</summary>
	EndFunctionParms = 0x16,
	/// <summary>Self object.</summary>
	Self = 0x17,
	/// <summary>Skippable expression.</summary>
	Skip = 0x18,
	/// <summary>Call a function through an object context.</summary>
	Context = 0x19,
	/// <summary>Call a function through an object context (can fail silently if the context is NULL; only generated for functions that don't have output or return values).</summary>
	Context_FailSilent = 0x1A,
	/// <summary>A function call with parameters.</summary>
	VirtualFunction = 0x1B,
	/// <summary>A prebound function call with parameters.</summary>
	FinalFunction = 0x1C,
	/// <summary>Int constant.</summary>
	IntConst = 0x1D,
	/// <summary>Floating point constant.</summary>
	FloatConst = 0x1E,
	/// <summary>String constant.</summary>
	StringConst = 0x1F,
	/// <summary>An object constant.</summary>
	ObjectConst = 0x20,
	/// <summary>A name constant.</summary>
	NameConst = 0x21,
	/// <summary>A rotation constant.</summary>
	RotationConst = 0x22,
	/// <summary>A vector constant.</summary>
	VectorConst = 0x23,
	/// <summary>A byte constant.</summary>
	ByteConst = 0x24,
	/// <summary>Zero.</summary>
	IntZero = 0x25,
	/// <summary>One.</summary>
	IntOne = 0x26,
	/// <summary>Bool True.</summary>
	True = 0x27,
	/// <summary>Bool False.</summary>
	False = 0x28,
	/// <summary>FText constant</summary>
	TextConst = 0x29,
	/// <summary>NoObject.</summary>
	NoObject = 0x2A,
	/// <summary>A transform constant</summary>
	TransformConst = 0x2B,
	/// <summary>Int constant that requires 1 byte.</summary>
	IntConstByte = 0x2C,
	/// <summary>A null interface (similar to NoObject, but for interfaces)</summary>
	NoInterface = 0x2D,
	/// <summary>Safe dynamic class casting.</summary>
	DynamicCast = 0x2E,
	/// <summary>An arbitrary UStruct constant</summary>
	StructConst = 0x2F,
	/// <summary>End of UStruct constant</summary>
	EndStructConst = 0x30,
	/// <summary>Set the value of arbitrary array</summary>
	SetArray = 0x31,
	EndArray = 0x32,
	/// <summary>FProperty constant.</summary>
	PropertyConst = 0x33,
	/// <summary>Unicode string constant.</summary>
	UnicodeStringConst = 0x34,
	/// <summary>64-bit integer constant.</summary>
	Int64Const = 0x35,
	/// <summary>64-bit unsigned integer constant.</summary>
	UInt64Const = 0x36,
	/// <summary>A casting operator for primitives which reads the type as the subsequent byte</summary>
	PrimitiveCast = 0x38,
	SetSet = 0x39,
	EndSet = 0x3A,
	SetMap = 0x3B,
	EndMap = 0x3C,
	SetConst = 0x3D,
	EndSetConst = 0x3E,
	MapConst = 0x3F,
	EndMapConst = 0x40,
	/// <summary>Context expression to address a property within a struct</summary>
	StructMemberContext = 0x42,
	/// <summary>Assignment to a multi-cast delegate</summary>
	LetMulticastDelegate = 0x43,
	/// <summary>Assignment to a delegate</summary>
	LetDelegate = 0x44,
	/// <summary>Special instructions to quickly call a virtual function that we know is going to run only locally</summary>
	LocalVirtualFunction = 0x45,
	/// <summary>Special instructions to quickly call a final function that we know is going to run only locally</summary>
	LocalFinalFunction = 0x46,
	/// <summary>local out (pass by reference) function parameter</summary>
	LocalOutVariable = 0x48,
	DeprecatedOp4A = 0x4A,
	/// <summary>const reference to a delegate or normal function object</summary>
	InstanceDelegate = 0x4B,
	/// <summary>push an address on to the execution flow stack for future execution when a PopExecutionFlow is executed. Execution continues on normally and doesn't change to the pushed address.</summary>
	PushExecutionFlow = 0x4C,
	/// <summary>continue execution at the last address previously pushed onto the execution flow stack.</summary>
	PopExecutionFlow = 0x4D,
	/// <summary>Goto a local address in code, specified by an integer value.</summary>
	ComputedJump = 0x4E,
	/// <summary>continue execution at the last address previously pushed onto the execution flow stack, if the condition is not true.</summary>
	PopExecutionFlowIfNot = 0x4F,
	/// <summary>Breakpoint. Only observed in the editor, otherwise it behaves like Nothing.</summary>
	Breakpoint = 0x50,
	/// <summary>Call a function through a native interface variable</summary>
	InterfaceContext = 0x51,
	/// <summary>Converting an object reference to native interface variable</summary>
	ObjToInterfaceCast = 0x52,
	/// <summary>Last byte in script code</summary>
	EndOfScript = 0x53,
	/// <summary>Converting an interface variable reference to native interface variable</summary>
	CrossInterfaceCast = 0x54,
	/// <summary>Converting an interface variable reference to an object</summary>
	InterfaceToObjCast = 0x55,
	/// <summary>Trace point.  Only observed in the editor, otherwise it behaves like Nothing.</summary>
	WireTracepoint = 0x5A,
	/// <summary>A CodeSizeSkipOffset constant</summary>
	SkipOffsetConst = 0x5B,
	/// <summary>Adds a delegate to a multicast delegate's targets</summary>
	AddMulticastDelegate = 0x5C,
	/// <summary>Clears all delegates in a multicast target</summary>
	ClearMulticastDelegate = 0x5D,
	/// <summary>Trace point.  Only observed in the editor, otherwise it behaves like Nothing.</summary>
	Tracepoint = 0x5E,
	/// <summary>assign to any object ref pointer</summary>
	LetObj = 0x5F,
	/// <summary>assign to a weak object pointer</summary>
	LetWeakObjPtr = 0x60,
	/// <summary>bind object and name to delegate</summary>
	BindDelegate = 0x61,
	/// <summary>Remove a delegate from a multicast delegate's targets</summary>
	RemoveMulticastDelegate = 0x62,
	/// <summary>Call multicast delegate</summary>
	CallMulticastDelegate = 0x63,
	LetValueOnPersistentFrame = 0x64,
	ArrayConst = 0x65,
	EndArrayConst = 0x66,
	SoftObjectConst = 0x67,
	/// <summary>static pure function from on local call space</summary>
	CallMath = 0x68,
	SwitchValue = 0x69,
	/// <summary>Instrumentation event</summary>
	InstrumentationEvent = 0x6A,
	ArrayGetByRef = 0x6B,
	/// <summary>Sparse data variable</summary>
	ClassSparseDataVariable = 0x6C,
	FieldPathConst = 0x6D,
	Max = 0x100,
};
public enum ECastToken {
	ObjectToInterface = 0x46,
	ObjectToBool = 0x47,
	InterfaceToBool = 0x49,
	Max = 0xFF,
};