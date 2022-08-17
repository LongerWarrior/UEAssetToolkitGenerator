namespace UAssetAPI.StructTypes;

// Enum MovieScene.EMovieSceneKeyInterpolation
public enum EMovieSceneKeyInterpolation : byte {
	Auto = 0,
	User = 1,
	Break = 2,
	Linear = 3,
	Constant = 4,
	EMovieSceneKeyInterpolation_MAX = 5
};

// Enum MovieScene.EMovieSceneBlendType
public enum EMovieSceneBlendType : byte {
	Invalid = 0,
	Absolute = 1,
	Additive = 2,
	Relative = 4,
	EMovieSceneBlendType_MAX = 5
};

// Enum MovieScene.EMovieSceneBuiltInEasing
public enum EMovieSceneBuiltInEasing : byte {
	Linear = 0,
	SinIn = 1,
	SinOut = 2,
	SinInOut = 3,
	QuadIn = 4,
	QuadOut = 5,
	QuadInOut = 6,
	CubicIn = 7,
	CubicOut = 8,
	CubicInOut = 9,
	QuartIn = 10,
	QuartOut = 11,
	QuartInOut = 12,
	QuintIn = 13,
	QuintOut = 14,
	QuintInOut = 15,
	ExpoIn = 16,
	ExpoOut = 17,
	ExpoInOut = 18,
	CircIn = 19,
	CircOut = 20,
	CircInOut = 21,
	EMovieSceneBuiltInEasing_MAX = 22
};

// Enum MovieScene.EEvaluationMethod
public enum EEvaluationMethod : byte {
	Static = 0,
	Swept = 1,
	EEvaluationMethod_MAX = 2
};

// Enum MovieScene.EUpdateClockSource
public enum EUpdateClockSource : byte {
	Tick = 0,
	Platform = 1,
	Audio = 2,
	RelativeTimecode = 3,
	Timecode = 4,
	Custom = 5,
	EUpdateClockSource_MAX = 6
};

// Enum MovieScene.EMovieSceneEvaluationType
public enum EMovieSceneEvaluationType : byte {
	FrameLocked = 0,
	WithSubFrames = 1,
	EMovieSceneEvaluationType_MAX = 2
};

// Enum MovieScene.EMovieScenePlayerStatus
public enum EMovieScenePlayerStatus : byte {
	Stopped = 0,
	Playing = 1,
	Recording = 2,
	Scrubbing = 3,
	Jumping = 4,
	Stepping = 5,
	Paused = 6,
	MAX = 7
};

// Enum MovieScene.EMovieSceneObjectBindingSpace
public enum EMovieSceneObjectBindingSpace : byte {
	Local = 0,
	Root = 1,
	EMovieSceneObjectBindingSpace_MAX = 2
};

// Enum MovieScene.EMovieSceneCompletionMode
public enum EMovieSceneCompletionMode : byte {
	KeepState = 0,
	RestoreState = 1,
	ProjectDefault = 2,
	EMovieSceneCompletionMode_MAX = 3
};

// Enum MovieScene.ESectionEvaluationFlags
public enum ESectionEvaluationFlags : byte {
	None = 0,
	PreRoll = 1,
	PostRoll = 2,
	ESectionEvaluationFlags_MAX = 3
};

// Enum MovieScene.EUpdatePositionMethod
public enum EUpdatePositionMethod : byte {
	Play = 0,
	Jump = 1,
	Scrub = 2,
	EUpdatePositionMethod_MAX = 3
};

// Enum MovieScene.ESpawnOwnership
public enum ESpawnOwnership : byte {
	InnerSequence = 0,
	MasterSequence = 1,
	External = 2,
	ESpawnOwnership_MAX = 3
};

public class FMovieSceneEvaluationTree {
	/** This tree's root node */
	public FMovieSceneEvaluationTreeNode RootNode;
	/** Segmented array of all child nodes within this tree (in no particular order) */
	public TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode> ChildNodes;

}

public class TMovieSceneEvaluationTree<T> : FMovieSceneEvaluationTree {
	/** Tree data container that corresponds to FMovieSceneEvaluationTreeNode::DataID */
	public TEvaluationTreeEntryContainer<T> Data;
}

public struct TEvaluationTreeEntryContainer<T>{
	/** List of allocated entries for each allocated entry. Should only ever grow, never shrink. Shrinking would cause previously established handles to become invalid. */
	public FEntry[] Entries;
	/** Linear array of allocated entry contents. Once allocated, indices are never invalidated until Compact is called. Entries needing more capacity are re-allocated on the end of the array. */
	public T[] Items;
}


