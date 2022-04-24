using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes {
	#region Enums
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


	#endregion


	public struct FMovieSceneFloatValue {
		public float Value; // 0x00(0x04)
		public FMovieSceneTangentData Tangent; // 0x04(0x14)
		public ERichCurveInterpMode InterpMode; // 0x18(0x01)
		public ERichCurveTangentMode TangentMode; // 0x19(0x01)
		public short PaddingByte; // 0x1a(0x01)
								  //char pad_1B[0x1]; // 0x1b(0x01)
		public JObject ToJson() {
			JObject value = new JObject();

			value.Add("Value", Value);
			value.Add("Tangent", Tangent.ToJson());
			value.Add("InterpMode", InterpMode.ToString());
			value.Add("TangentMode", TangentMode.ToString());
			value.Add("PaddingByte", PaddingByte);

			return value;
		}
		public void Read(AssetBinaryReader reader) {
			Value = reader.ReadSingle();
			Tangent.ArriveTangent = reader.ReadSingle();
			Tangent.LeaveTangent = reader.ReadSingle();
			Tangent.ArriveTangentWeight = reader.ReadSingle();
			Tangent.LeaveTangentWeight = reader.ReadSingle();
			Tangent.TangentWeightMode = (ERichCurveTangentWeightMode)reader.ReadByte();
			Tangent.padding = reader.ReadBytes(3);
			InterpMode = (ERichCurveInterpMode)reader.ReadSByte();
			TangentMode = (ERichCurveTangentMode)reader.ReadSByte();
			PaddingByte = reader.ReadInt16();
		}
		public void Write(AssetBinaryWriter writer) {
			writer.Write(Value);
			Tangent.Write(writer);
			writer.Write((sbyte)InterpMode);
			writer.Write((sbyte)TangentMode);
			writer.Write(PaddingByte);
		}
	};

	public struct FMovieSceneTangentData {
		public float ArriveTangent; // 0x00(0x04)
		public float LeaveTangent; // 0x04(0x04)
		public float ArriveTangentWeight; // 0x08(0x04)
		public float LeaveTangentWeight; // 0x0c(0x04)
		public ERichCurveTangentWeightMode TangentWeightMode; // 0x10(0x01)
		public byte[] padding;
		//char pad_11[0x3]; // 0x11(0x03)
		public JObject ToJson() {
			JObject value = new JObject();

			value.Add("ArriveTangent", ArriveTangent);
			value.Add("LeaveTangent", LeaveTangent);
			value.Add("ArriveTangentWeight", ArriveTangentWeight);
			value.Add("LeaveTangentWeight", LeaveTangentWeight);
			value.Add("TangentWeightMode", TangentWeightMode.ToString());

			return value;
		}

		public void Write(AssetBinaryWriter writer) {
			writer.Write(ArriveTangent);
			writer.Write(LeaveTangent);
			writer.Write(ArriveTangentWeight);
			writer.Write(LeaveTangentWeight);
			writer.Write((byte)TangentWeightMode);
			writer.Write(padding);
		}
	};

	// ScriptStruct MovieScene.MovieSceneFloatChannel
	// Size: 0xa0 (Inherited: 0x08)
	public class FMovieSceneFloatChannel {
		public ERichCurveExtrapolation PreInfinityExtrap; // 0x08(0x01)
		public ERichCurveExtrapolation PostInfinityExtrap; // 0x09(0x01)
		public FFrameNumber[] Times; // 0x10(0x10)
		public FMovieSceneFloatValue[] Values; // 0x20(0x10)
		public float DefaultValue; // 0x30(0x04)
		public bool bHasDefaultValue; // 0x34(0x01)
		//FMovieSceneKeyHandleMap KeyHandles; // 0x38(0x60)
		public FFrameRate TickResolution; // 0x98(0x08)

		public FMovieSceneFloatChannel() {
			PreInfinityExtrap = ERichCurveExtrapolation.RCCE_Constant;
			PostInfinityExtrap = ERichCurveExtrapolation.RCCE_Constant;
			Times = new FFrameNumber[0];
			Values = new FMovieSceneFloatValue[0];
			DefaultValue = 0.0f;
			bHasDefaultValue = false;
			TickResolution = new FFrameRate(60000, 1);
        }
		public JObject ToJson() {
			JObject res = new JObject();

			res.Add(new JProperty("PreInfinityExtrap", PreInfinityExtrap.ToString()));
			res.Add(new JProperty("PostInfinityExtrap", PostInfinityExtrap.ToString()));

			JArray times = new JArray();
			foreach (FFrameNumber time in Times) {
				times.Add(time.ToJson());
			}
			res.Add(new JProperty("Times", times));

			JArray values = new JArray();
			foreach (FMovieSceneFloatValue value in Values) {
				values.Add(value.ToJson());
			}
			res.Add(new JProperty("Values", values));

			res.Add(new JProperty("DefaultValue", DefaultValue));
			res.Add(new JProperty("bHasDefaultValue", bHasDefaultValue));
			res.Add(new JProperty("TickResolution", TickResolution.ToJson()));

			return res;
		}

	};

	public class FMovieSceneTrackIdentifier {
		public uint Value;

		public FMovieSceneTrackIdentifier(uint value) {
			Value = value;
		}

		public JObject ToJson() {
			JObject value = new JObject();
			value.Add("Value", Value);
			return value;
		}

        public void Write(AssetBinaryWriter writer) {
			writer.Write(Value);
        }
    }

	public class FMovieSceneSequenceID {
		public uint Value;

		public FMovieSceneSequenceID(uint value) {
			Value = value;
		}
		public JObject ToJson() {
			JObject value = new JObject();
			value.Add("Value", Value);
			return value;
		}
	};

	public class FMovieSceneEvaluationKey {
		public FMovieSceneSequenceID SequenceID;
		public FMovieSceneTrackIdentifier TrackIdentifier;
		public uint SectionIndex;

		public FMovieSceneEvaluationKey(uint _SequenceID, uint _TrackIdentifier, uint _SectionIndex) {
			SequenceID = new FMovieSceneSequenceID(_SequenceID);
			TrackIdentifier = new FMovieSceneTrackIdentifier(_TrackIdentifier);
			SectionIndex = _SectionIndex;

		}
		public JObject ToJson() {
			JObject value = new JObject();
			value.Add("SequenceID", SequenceID.ToJson());
			value.Add("TrackIdentifier", TrackIdentifier.ToJson());
			value.Add("SectionIndex", SectionIndex);
			return value;
		}
	};


	public struct FMovieSceneEvaluationFieldEntityTree {
		public TMovieSceneEvaluationTree<FEntityAndMetaDataIndex> SerializedData;

		public FMovieSceneEvaluationFieldEntityTree(TMovieSceneEvaluationTree<FEntityAndMetaDataIndex> serializedData) {
			SerializedData = serializedData;
		}

		public FMovieSceneEvaluationFieldEntityTree Read(AssetBinaryReader reader) {
			SerializedData = new TMovieSceneEvaluationTree<FEntityAndMetaDataIndex>();

			SerializedData.RootNode = new FMovieSceneEvaluationTreeNode();
			SerializedData.RootNode.Read(reader);

			SerializedData.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
			int entriesamount = reader.ReadInt32();
			SerializedData.ChildNodes.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				SerializedData.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			int itemsamount = reader.ReadInt32();

			SerializedData.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				SerializedData.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
				SerializedData.ChildNodes.Items[i].Read(reader);
			}

			SerializedData.Data = new TEvaluationTreeEntryContainer<FEntityAndMetaDataIndex>();

			entriesamount = reader.ReadInt32();
			SerializedData.Data.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				SerializedData.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			itemsamount = reader.ReadInt32();

			SerializedData.Data.Items = new FEntityAndMetaDataIndex[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				SerializedData.Data.Items[i] = new FEntityAndMetaDataIndex(reader.ReadInt32(), reader.ReadInt32());
			}
			return new FMovieSceneEvaluationFieldEntityTree(SerializedData);
		}

		public void Write(AssetBinaryWriter writer) {

			SerializedData.RootNode.Write(writer);
			int entriesamount = SerializedData.ChildNodes.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				SerializedData.ChildNodes.Entries[i].Write(writer);
			}

			int itemsamount = SerializedData.ChildNodes.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				SerializedData.ChildNodes.Items[i].Write(writer);
			}

			entriesamount = SerializedData.Data.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				SerializedData.Data.Entries[i].Write(writer);
			}
			itemsamount = SerializedData.Data.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				SerializedData.Data.Items[i].Write(writer);
			}

		}

		public JObject ToJson() {
			JObject res = new JObject();

			JObject serdata = new JObject();
			serdata.Add("RootNode", SerializedData.RootNode.ToJson());

			JArray entries = new JArray();
			JArray items = new JArray();
			foreach (FEntry entry in SerializedData.ChildNodes.Entries) {
				entries.Add(entry.ToJson());
			}
			foreach (FMovieSceneEvaluationTreeNode item in SerializedData.ChildNodes.Items) {
				items.Add(item.ToJson());
			}
			JObject childnodes = new JObject();

			childnodes.Add("Entries", entries);
			childnodes.Add("Items", items);
			serdata.Add("ChildNodes", childnodes);

			entries = new JArray();
			items = new JArray();
			foreach (FEntry entry in SerializedData.Data.Entries) {
				entries.Add(entry.ToJson());
			}
			foreach (FEntityAndMetaDataIndex item in SerializedData.Data.Items) {
				items.Add(item.ToJson());
			}
			JObject data = new JObject();
			data.Add("Entries", entries);
			data.Add("Items", items);
			serdata.Add("Data", data);
			res.Add("SerializedData", serdata);

			return res;
		}
	}

	public struct FMovieSceneSubSequenceTreeEntry {
		public FMovieSceneSequenceID SequenceID;
		public ESectionEvaluationFlags Flags;

        public FMovieSceneSubSequenceTreeEntry(FMovieSceneSequenceID sequenceID, byte flags) {
            SequenceID = sequenceID;
            Flags = (ESectionEvaluationFlags)flags;
        }

		public void Write(AssetBinaryWriter writer) {
			writer.Write(SequenceID.Value);
			writer.Write((byte)Flags);
        }

		public JObject ToJson() {
			JObject res = new JObject();
			res.Add("SequenceID", SequenceID.ToJson());
			res.Add("Items", Flags.ToString());
			return res;
		}
    }

	public struct FMovieSceneSubSequenceTree {
		public TMovieSceneEvaluationTree<FMovieSceneSubSequenceTreeEntry> Data;

        public FMovieSceneSubSequenceTree(TMovieSceneEvaluationTree<FMovieSceneSubSequenceTreeEntry> data) {
            Data = data;
        }

        public FMovieSceneSubSequenceTree Read(AssetBinaryReader reader) {

			Data = new TMovieSceneEvaluationTree<FMovieSceneSubSequenceTreeEntry>();

			Data.RootNode = new FMovieSceneEvaluationTreeNode();
			Data.RootNode.Read(reader);

			Data.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
			int entriesamount = reader.ReadInt32();
			Data.ChildNodes.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				Data.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			int itemsamount = reader.ReadInt32();

			Data.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				Data.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
				Data.ChildNodes.Items[i].Read(reader);
			}

			Data.Data = new TEvaluationTreeEntryContainer<FMovieSceneSubSequenceTreeEntry>();

			entriesamount = reader.ReadInt32();
			Data.Data.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				Data.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			itemsamount = reader.ReadInt32();

			Data.Data.Items = new FMovieSceneSubSequenceTreeEntry[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				Data.Data.Items[i] = new FMovieSceneSubSequenceTreeEntry(new FMovieSceneSequenceID(reader.ReadUInt32()), reader.ReadByte());
			}
			return new FMovieSceneSubSequenceTree(Data);
		}

		public void Write(AssetBinaryWriter writer) {

			Data.RootNode.Write(writer);
			int entriesamount = Data.ChildNodes.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				Data.ChildNodes.Entries[i].Write(writer);
			}

			int itemsamount = Data.ChildNodes.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				Data.ChildNodes.Items[i].Write(writer);
			}

			entriesamount = Data.Data.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				Data.Data.Entries[i].Write(writer);
			}
			itemsamount = Data.Data.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				Data.Data.Items[i].Write(writer);
			}

		}
		public JObject ToJson() {
			JObject res = new JObject();

			JObject serdata = new JObject();
			serdata.Add("RootNode", Data.RootNode.ToJson());

			JArray entries = new JArray();
			JArray items = new JArray();
			foreach (FEntry entry in Data.ChildNodes.Entries) {
				entries.Add(entry.ToJson());
			}
			foreach (FMovieSceneEvaluationTreeNode item in Data.ChildNodes.Items) {
				items.Add(item.ToJson());
			}
			JObject childnodes = new JObject();

			childnodes.Add("Entries", entries);
			childnodes.Add("Items", items);
			serdata.Add("ChildNodes", childnodes);

			entries = new JArray();
			items = new JArray();
			foreach (FEntry entry in Data.Data.Entries) {
				entries.Add(entry.ToJson());
			}
			foreach (FMovieSceneSubSequenceTreeEntry item in Data.Data.Items) {
				items.Add(item.ToJson());
			}
			JObject data = new JObject();
			data.Add("Entries", entries);
			data.Add("Items", items);
			serdata.Add("Data", data);
			res.Add("Data", serdata);

			return res;
		}
	}



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

	public struct FMovieSceneEvaluationTreeNode {
		/** The time-range that this node represents */
		public FFrameNumberRange Range;
		public FMovieSceneEvaluationTreeNodeHandle Parent;
		/** Identifier for the child node entries associated with this node (FMovieSceneEvaluationTree::ChildNodes) */
		public FEvaluationTreeEntryHandle ChildrenID;
		/** Identifier for externally stored data entries associated with this node */
		public FEvaluationTreeEntryHandle DataID;

		public void Read(AssetBinaryReader reader) {
			Range = new FFrameNumberRange();
			Range.Read(reader);
			Parent = new FMovieSceneEvaluationTreeNodeHandle(reader.ReadInt32(), reader.ReadInt32());
			ChildrenID = new FEvaluationTreeEntryHandle(reader.ReadInt32());
			DataID = new FEvaluationTreeEntryHandle(reader.ReadInt32());
		}

		public void Write(AssetBinaryWriter writer) {
			Range.Write(writer);
			writer.Write(Parent.ChildrenHandle.EntryIndex);
			writer.Write(Parent.Index);
			writer.Write(ChildrenID.EntryIndex);
			writer.Write(DataID.EntryIndex);
		}
		public JObject ToJson() {
			JObject res = new JObject();

			res.Add("Range", Range.ToJson());
			res.Add("Parent", Parent.ToJson());
			res.Add("ChildrenID", ChildrenID.ToJson());
			res.Add("DataID", DataID.ToJson());

			return res;
		}

	}

	public struct FMovieSceneEvaluationTreeNodeHandle {
		/** Entry handle for the parent's children in FMovieSceneEvaluationTree::ChildNodes */
		public FEvaluationTreeEntryHandle ChildrenHandle;
		/** The index of this child within its parent's children */
		public int Index;

		public FMovieSceneEvaluationTreeNodeHandle(int _ChildrenHandle, int _Index) {
			ChildrenHandle.EntryIndex = _ChildrenHandle;
			Index = _Index;
		}
		public JObject ToJson() {
			JObject res = new JObject();
			res.Add("ChildrenHandle", ChildrenHandle.ToJson());
			res.Add("Index", Index);
			return res;
		}

	}

	public struct FEvaluationTreeEntryHandle {
		/** Specifies an index into TEvaluationTreeEntryContainer<T>::Entries */
		public int EntryIndex;
		public FEvaluationTreeEntryHandle (int _EntryIndex) {
			EntryIndex = _EntryIndex;

		}

		public JObject ToJson() {
			JObject res = new JObject();
			res.Add("EntryIndex", EntryIndex);
			return res;
		}
	}

	public struct FEntry {
		/** The index into Items of the first item */
		public int StartIndex;
		/** The number of currently valid items */
		public int Size;
		/** The total capacity of allowed items before reallocating */
		public int Capacity;

        public FEntry(int startIndex, int size, int capacity) {
            StartIndex = startIndex;
            Size = size;
            Capacity = capacity;
        }

		public void Write(AssetBinaryWriter writer) {
			writer.Write(StartIndex);
			writer.Write(Size);
			writer.Write(Capacity);
        }

		public JObject ToJson() {
			JObject res = new JObject();
			res.Add("StartIndex", StartIndex);
			res.Add("Size", Size);
			res.Add("Capacity", Capacity);
			return res;
		}
    }

	public struct TEvaluationTreeEntryContainer<T>{
		/** List of allocated entries for each allocated entry. Should only ever grow, never shrink. Shrinking would cause previously established handles to become invalid. */
		public FEntry[] Entries;
		/** Linear array of allocated entry contents. Once allocated, indices are never invalidated until Compact is called. Entries needing more capacity are re-allocated on the end of the array. */
		public T[] Items;

	}


	public struct FSectionEvaluationDataTree {
		public TMovieSceneEvaluationTree<List<PropertyData>> Tree;

        public FSectionEvaluationDataTree(TMovieSceneEvaluationTree<List<PropertyData>> tree) {
            Tree = tree;
        }

		public FSectionEvaluationDataTree Read(AssetBinaryReader reader) {

			Tree = new TMovieSceneEvaluationTree<List<PropertyData>>();

			Tree.RootNode = new FMovieSceneEvaluationTreeNode();
			Tree.RootNode.Read(reader);

			Tree.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
			int entriesamount = reader.ReadInt32();
			Tree.ChildNodes.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				Tree.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			int itemsamount = reader.ReadInt32();

			Tree.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				Tree.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
				Tree.ChildNodes.Items[i].Read(reader);
			}

			Tree.Data = new TEvaluationTreeEntryContainer<List<PropertyData>>();

			entriesamount = reader.ReadInt32();
			Tree.Data.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				Tree.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			itemsamount = reader.ReadInt32();

			List<PropertyData>[] items = new List<PropertyData>[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				List<PropertyData> resultingList = new List<PropertyData>();
				PropertyData data = null;
				while ((data = MainSerializer.Read(reader, true)) != null) {
					resultingList.Add(data);
				}
				items[i] = resultingList;
			}
			Tree.Data.Items = items;
			//Tree.Data.Items = new StructPropertyData[itemsamount];
			//for (int i = 0; i < itemsamount; i++) {
			//	Tree.Data.Items[i] = new StructPropertyData(new FName("Impls"), new FName("SectionEvaluationData"));
			//	Tree.Data.Items[i].Read(reader, false, 1);
			//}
			return new FSectionEvaluationDataTree(Tree);
		}

		public void Write(AssetBinaryWriter writer) {

			Tree.RootNode.Write(writer);
			int entriesamount = Tree.ChildNodes.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				Tree.ChildNodes.Entries[i].Write(writer);
			}

			int itemsamount = Tree.ChildNodes.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				Tree.ChildNodes.Items[i].Write(writer);
			}

			entriesamount = Tree.Data.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				Tree.Data.Entries[i].Write(writer);
			}
			itemsamount = Tree.Data.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {

				if (Tree.Data.Items[i] != null) {
					foreach (var t in Tree.Data.Items[i]) {
						MainSerializer.Write(t, writer, true);
					}
				}
				writer.Write(new FName("None"));
			}

		}
		public JObject ToJson() {
			JObject res = new JObject();

			//JObject serdata = new JObject();
			//serdata.Add("RootNode", Tree.RootNode.ToJson());

			//JArray entries = new JArray();
			//JArray items = new JArray();
			//foreach (FEntry entry in Tree.ChildNodes.Entries) {
			//	entries.Add(entry.ToJson());
			//}
			//foreach (FMovieSceneEvaluationTreeNode item in Tree.ChildNodes.Items) {
			//	items.Add(item.ToJson());
			//}
			//JObject childnodes = new JObject();

			//childnodes.Add("Entries", entries);
			//childnodes.Add("Items", items);
			//serdata.Add("ChildNodes", childnodes);

			//entries = new JArray();
			//items = new JArray();
			//foreach (FEntry entry in Tree.Data.Entries) {
			//	entries.Add(entry.ToJson());
			//}


			////foreach (List<PropertyData> item in Tree.Data.Items) {
			////	items.Add(item.ToJson());
			//	//items.Add(Program.S item);
			////}

			//JObject data = new JObject();
			//data.Add("Entries", entries);
			//data.Add("Items", items);
			//serdata.Add("Data", data);
			//res.Add("Tree", serdata);

			return res;
		}


	}

	interface IUStruct {
		public virtual void Read(AssetBinaryReader reader) { }
    }


	public struct FEntityAndMetaDataIndex {
		public int EntityIndex;
		public int MetaDataIndex;

        public FEntityAndMetaDataIndex(int entityIndex, int metaDataIndex) {
            EntityIndex = entityIndex;
            MetaDataIndex = metaDataIndex;
        }

		public void Write(AssetBinaryWriter writer) {
			writer.Write(EntityIndex);
			writer.Write(MetaDataIndex);
        }
		public JObject ToJson() {
			JObject res = new JObject();
			res.Add("EntityIndex", EntityIndex);
			res.Add("MetaDataIndex", MetaDataIndex);
			return res;
		}
	}

    public struct FSectionEvaluationData {
        public int ImplIndex; // 0x00(0x04)
        public FFrameNumber ForcedTime; // 0x04(0x04)
		public ESectionEvaluationFlags Flags; // 0x08(0x01)

        public FSectionEvaluationData(int implIndex, FFrameNumber forcedTime, byte flags) {
            ImplIndex = implIndex;
            ForcedTime = forcedTime;
            Flags = (ESectionEvaluationFlags)flags;
        }

		public void Write(AssetBinaryWriter writer) {
			writer.Write(ImplIndex);
			writer.Write(ForcedTime.Value);
			writer.Write((byte)Flags);
		}
		public JObject ToJson() {
			JObject res = new JObject();
			res.Add("ImplIndex", ImplIndex);
			res.Add("ForcedTime", ForcedTime.ToJson());
			res.Add("Flags", Flags.ToString());
			return res;
		}

	}

    public class FMovieSceneSegment {

		public FFrameNumberRange Range;
		public FMovieSceneSegmentIdentifier ID;
		public bool bAllowEmpty;
		public List<PropertyData>[] Impls;
		//public FSectionEvaluationData[] Impls;
		
		public JToken ToJson() {
			JObject value = new JObject();
			value.Add("Range", Range.ToJson());
			value.Add("ID", ID.IdentifierIndex);
			value.Add("bAllowEmpty", bAllowEmpty);

			JArray jimpls = new JArray();
            /*for (int i = 0; i < Impls.Length; i++) {
				jimpls.Add(new JObject().Add());
            }*/
			value.Add("Impls", jimpls);
			return value;
		}
        
    }

	public struct FMovieSceneSegmentIdentifier {
		public int IdentifierIndex; // 0x00(0x04)

		public FMovieSceneSegmentIdentifier(int identifierIndex) {
			IdentifierIndex = identifierIndex;
		}

		public void Write(AssetBinaryWriter writer) {
			writer.Write(IdentifierIndex);
        }
		public JToken ToJson() {
			JObject value = new JObject();
			value.Add("IdentifierIndex", IdentifierIndex);
			return value;
		}
	}

	public struct FMovieSceneTrackFieldData {
		public TMovieSceneEvaluationTree<FMovieSceneTrackIdentifier> Field;

        public FMovieSceneTrackFieldData(TMovieSceneEvaluationTree<FMovieSceneTrackIdentifier> field) {
            Field = field;
        }

		public FMovieSceneTrackFieldData Read(AssetBinaryReader reader) {
			Field = new TMovieSceneEvaluationTree<FMovieSceneTrackIdentifier>();

			Field.RootNode = new FMovieSceneEvaluationTreeNode();
			Field.RootNode.Read(reader);

			Field.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
			int entriesamount = reader.ReadInt32();
			Field.ChildNodes.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				Field.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			int itemsamount = reader.ReadInt32();

			Field.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				Field.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
				Field.ChildNodes.Items[i].Read(reader);
			}

			Field.Data = new TEvaluationTreeEntryContainer<FMovieSceneTrackIdentifier>();

			entriesamount = reader.ReadInt32();
			Field.Data.Entries = new FEntry[entriesamount];
			for (int i = 0; i < entriesamount; i++) {
				Field.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			}
			itemsamount = reader.ReadInt32();

			Field.Data.Items = new FMovieSceneTrackIdentifier[itemsamount];
			for (int i = 0; i < itemsamount; i++) {
				Field.Data.Items[i] = new FMovieSceneTrackIdentifier(reader.ReadUInt32());
			}
			return new FMovieSceneTrackFieldData(Field);
		}

		public void Write(AssetBinaryWriter writer) {

			Field.RootNode.Write(writer);
			int entriesamount = Field.ChildNodes.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				Field.ChildNodes.Entries[i].Write(writer);
			}

			int itemsamount = Field.ChildNodes.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				Field.ChildNodes.Items[i].Write(writer);
			}

			entriesamount = Field.Data.Entries.Length;
			writer.Write(entriesamount);
			for (int i = 0; i < entriesamount; i++) {
				Field.Data.Entries[i].Write(writer);
			}
			itemsamount = Field.Data.Items.Length;
			writer.Write(itemsamount);
			for (int i = 0; i < itemsamount; i++) {
				Field.Data.Items[i].Write(writer);
			}

		}

		public JObject ToJson() {
			JObject res = new JObject();

			JObject serdata = new JObject();
			serdata.Add("RootNode", Field.RootNode.ToJson());

			JArray entries = new JArray();
			JArray items = new JArray();
			foreach (FEntry entry in Field.ChildNodes.Entries) {
				entries.Add(entry.ToJson());
			}
			foreach (FMovieSceneEvaluationTreeNode item in Field.ChildNodes.Items) {
				items.Add(item.ToJson());
			}
			JObject childnodes = new JObject();

			childnodes.Add("Entries", entries);
			childnodes.Add("Items", items);
			serdata.Add("ChildNodes", childnodes);

			entries = new JArray();
			items = new JArray();
			foreach (FEntry entry in Field.Data.Entries) {
				entries.Add(entry.ToJson());
			}
			foreach (FMovieSceneTrackIdentifier item in Field.Data.Items) {
				items.Add(item.ToJson());
			}
			JObject data = new JObject();
			data.Add("Entries", entries);
			data.Add("Items", items);
			serdata.Add("Data", data);
			res.Add("Field", serdata);

			return res;
		}


	}

}

