using UAssetAPI;
using UAssetAPI.UE4;
using static UAssetAPI.StructTypes.ERichCurveExtrapolation;
using static UAssetAPI.StructTypes.RichCurveInterpMode;
using static UAssetAPI.StructTypes.RichCurveTangentWeightMode;
using static UAssetApi.UE4.Objects.Engine.UnrealMath;

namespace UAssetApi.UE4.Types;

public class FRichCurve : FRealCurve
{
    public RichCurveKeyPropertyData[] Keys;

    /*public FRichCurve()
    {
        Keys = Array.Empty<RichCurveKeyPropertyData>();
        Keys = this["Keys"] is ArrayPropertyData keys ? keys.Value : Array.Empty<RichCurveKeyPropertyData>();
    }*/

    public override void RemapTimeValue(ref float inTime, ref float cycleValueOffset)
    {
        var numKeys = Keys.Length;
        if (numKeys < 2) return;

        if (inTime <= Keys[0].Time)
        {
            if (PreInfinityExtrap != RCCE_Linear &&
                PreInfinityExtrap != RCCE_Constant)
            {
                var minTime = Keys[0].Time;
                var maxTime = Keys[numKeys - 1].Time;

                var cycleCount = 0;
                CycleTime(minTime, maxTime, ref inTime, ref cycleCount);

                if (PreInfinityExtrap == RCCE_CycleWithOffset)
                {
                    var dv = Keys[0].Value - Keys[numKeys - 1].Value;
                    cycleValueOffset = dv * cycleCount;
                }
                else if (PreInfinityExtrap == RCCE_Oscillate)
                {
                    if (cycleCount % 2 == 1)
                    {
                        inTime = minTime + (maxTime - inTime);
                    }
                }
            }
        }
        else if (inTime >= Keys[numKeys - 1].Time)
        {
            if (PostInfinityExtrap != RCCE_Linear &&
                PostInfinityExtrap != RCCE_Constant)
            {
                var minTime = Keys[0].Time;
                var maxTime = Keys[numKeys - 1].Time;

                var cycleCount = 0;
                CycleTime(minTime, maxTime, ref inTime, ref cycleCount);

                if (PostInfinityExtrap == RCCE_CycleWithOffset)
                {
                    var dv = Keys[numKeys - 1].Value - Keys[0].Value;
                    cycleValueOffset = dv * cycleCount;
                }
                else if (PostInfinityExtrap == RCCE_Oscillate)
                {
                    if (cycleCount % 2 == 1)
                    {
                        inTime = minTime + (maxTime - inTime);
                    }
                }
            }
        }
    }

    public override float Eval(float inTime, float inDefaultValue = 0)
    {
        // Remap time if extrapolation is present and compute offset value to use if cycling
        var cycleValueOffset = 0f;
        var inTimeRef = inTime;
        var cycleValueOffsetRef = cycleValueOffset;
        RemapTimeValue(ref inTimeRef, ref cycleValueOffsetRef);
        inTime = inTimeRef;
        cycleValueOffset = cycleValueOffsetRef;

        var numKeys = Keys.Length;

        // If the default value hasn't been initialized, use the incoming default value
        var interpVal = DefaultValue == float.MaxValue ? inDefaultValue : DefaultValue;

        if (numKeys == 0)
        {
            //
        }
        else if (numKeys < 2 || inTime <= Keys[0].Time)
        {
            if (PreInfinityExtrap == RCCE_Linear && numKeys > 1)
            {
                var dt = Keys[1].Time - Keys[0].Time;

                if (Math.Abs(dt) <= SmallNumber)
                {
                    interpVal = Keys[0].Value;
                }
                else
                {
                    var dv = Keys[1].Value - Keys[0].Value;
                    var slope = dv / dt;

                    interpVal = slope * (inTime - Keys[0].Time) + Keys[0].Value;
                }
            }
            else
            {
                // Otherwise if constant or in a cycle or oscillate, always use the first key value
                interpVal = Keys[0].Value;
            }
        }
        else if (inTime < Keys[numKeys - 1].Time)
        {
            // perform a lower bound to get the second of the interpolation nodes
            var first = 1;
            var last = numKeys - 1;
            var count = last - first;

            while (count > 0)
            {
                var step = count / 2;
                var middle = first + step;

                if (inTime >= Keys[middle].Time)
                {
                    first = middle + 1;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }

            interpVal = EvalForTwoKeys(Keys[first - 1], Keys[first], inTime);
        }
        else
        {
            if (PostInfinityExtrap == RCCE_Linear)
            {
                var dt = Keys[numKeys - 2].Time - Keys[numKeys - 1].Time;

                if (Math.Abs(dt) <= SmallNumber)
                {
                    interpVal = Keys[numKeys - 1].Value;
                }
                else
                {
                    var dv = Keys[numKeys - 2].Value - Keys[numKeys - 1].Value;
                    var slope = dv / dt;

                    interpVal = slope * (inTime - Keys[numKeys - 1].Time) + Keys[numKeys - 1].Value;
                }
            }
            else
            {
                // Otherwise if constant or in a cycle or oscillate, always use the last key value
                interpVal = Keys[numKeys - 1].Value;
            }
        }

        return interpVal + cycleValueOffset;
    }

    private float EvalForTwoKeys(RichCurveKeyPropertyData key1, RichCurveKeyPropertyData key2, float inTime)
    {
        var diff = key2.Time - key1.Time;

        if (diff > 0f && key1.InterpMode != RCIM_Constant)
        {
            var alpha = (inTime - key1.Time) / diff;
            var p0 = key1.Value;
            var p3 = key2.Value;

            if (key1.InterpMode == RCIM_Linear)
            {
                return MathUtils.Lerp(p0, p3, alpha);
            }

            if (IsItNotWeighted(key1, key2))
            {
                const float oneThird = 1f / 3f;
                var p1 = p0 + key1.LeaveTangent * diff * oneThird;
                var p2 = p3 - key2.ArriveTangent * diff * oneThird;

                return BezierInterp(p0, p1, p2, p3, alpha);
            }

            // it's weighted
            return WeightedEvalForTwoKeys(key1, key2, inTime);
        }

        return key1.Value;
    }

    private float WeightedEvalForTwoKeys(RichCurveKeyPropertyData key1, RichCurveKeyPropertyData key2, float inTime)
    {
        var diff = key2.Time - key1.Time;
        var alpha = (inTime - key1.Time) / diff;
        var p0 = key1.Value;
        var p3 = key2.Value;
        var oneThird = 1f / 3f;
        var time1 = key1.Time;
        var time2 = key2.Time;
        var x = time2 - time1;
        var angle = Math.Atan(key1.LeaveTangent);
        var cosAngle = Math.Cos(angle);
        var sinAngle = Math.Sin(angle);

        double leaveWeight = key1.LeaveTangentWeight;
        if (key1.TangentWeightMode is RCTWM_WeightedNone or RCTWM_WeightedArrive)
        {
            var leaveTangentNormalized = key1.LeaveTangent;
            var y = leaveTangentNormalized * x;

            leaveWeight = Math.Sqrt(x * x + y * y) * oneThird;
        }

        var key1TanX = cosAngle * leaveWeight + time1;
        var key1TanY = sinAngle * leaveWeight + key1.Value;
        angle = Math.Atan(key2.ArriveTangent);
        cosAngle = Math.Cos(angle);
        sinAngle = Math.Cos(angle);

        double arriveWeight = key2.ArriveTangentWeight;
        if (key2.TangentWeightMode is RCTWM_WeightedNone or RCTWM_WeightedLeave)
        {
            var arriveTangentNormalized = key2.ArriveTangent;
            var y = arriveTangentNormalized * x;
            arriveWeight = Math.Sqrt(x * x + y * y) * oneThird;
        }

        var key2TanX = -cosAngle * arriveWeight + time2;
        var key2TanY = -sinAngle * arriveWeight + key2.Value;

        // Normalize the time range
        var rangeX = time2 - time1;

        var dx1 = key1TanX - time1;
        var dx2 = key2TanX - time1;

        // Normalize values
        var normalizedX1 = dx1 / rangeX;
        var normalizedX2 = dx2 / rangeX;

        var results = new double[3];

        BezierToPower(0.0, normalizedX1, normalizedX2, 1.0, out double[] coeff);
        coeff[0] -= alpha;

        var numResults = CubicCurve2D.SolveCubic(ref coeff, ref results);
        float newInterp;

        if (numResults == 1)
        {
            newInterp = (float)results[0];
        }
        else
        {
            newInterp = float.MinValue;
            foreach (var result in results)
            {
                if (result is >= 0.0f and <= 1.0f)
                {
                    if (newInterp < 0.0f || result > newInterp)
                    {
                        newInterp = (float)result;
                    }
                }
            }

            if (newInterp == float.MinValue)
            {
                newInterp = 0f;
            }
        }

        var outVal = BezierInterp(p0, (float)key1TanY, (float)key2TanY, p3, newInterp);
        return outVal;
    }

    private void BezierToPower(double a1, double b1, double c1, double d1, out double[] output)
    {
        var o = new double[4];

        var a = b1 - a1;
        var b = c1 - b1;
        var c = d1 - c1;
        var d = b - a;
        o[3] = c - b - d;
        o[2] = 3.0 * d;
        o[1] = 3.0 * a;
        o[0] = a1;

        output = o;
    }

    private float BezierInterp(float p0, float p1, float p2, float p3, float alpha)
    {
        var p01 = MathUtils.Lerp(p0, p1, alpha);
        var p12 = MathUtils.Lerp(p1, p2, alpha);
        var p23 = MathUtils.Lerp(p2, p3, alpha);
        var p012 = MathUtils.Lerp(p01, p12, alpha);
        var p123 = MathUtils.Lerp(p12, p23, alpha);
        var p0123 = MathUtils.Lerp(p012, p123, alpha);

        return p0123;
    }

    private static bool IsItNotWeighted(RichCurveKeyPropertyData key1, RichCurveKeyPropertyData key2)
    {
        return key1.TangentWeightMode is RCTWM_WeightedNone or RCTWM_WeightedArrive &&
               key2.TangentWeightMode is RCTWM_WeightedNone or RCTWM_WeightedLeave;
    }
    
    internal interface IKeyTimeAdapter
    {
        public int KeyDataOffset { get; }
        public float GetTime(int keyIndex);
    }

    internal interface IKeyDataAdapter
    {
        public int GetKeyDataHandle(int keyIndexToQuery);
        public float GetKeyValue(int handle);
        public float GetKeyArriveTangent(int handle);
        public float GetKeyLeaveTangent(int handle);
        public ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex);
        public ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex);
        public float GetKeyArriveTangentWeight(int handle);
        public float GetKeyLeaveTangentWeight(int handle);
    }

    internal readonly unsafe struct Quantized16BitKeyTimeAdapter : IKeyTimeAdapter
    {
        public const float QuantizationScale = 1.0f / 65535.0f;
        public const int KeySize = sizeof(ushort);
        public const int RangeDataSize = 2 * sizeof(float);

        public readonly ushort* KeyTimes;
        public readonly float MinTime;
        public readonly float DeltaTime;
        public int KeyDataOffset { get; }

        public Quantized16BitKeyTimeAdapter(byte* basePtr, int keyTimesOffset, int numKeys)
        {
            var rangeDataOffset = (keyTimesOffset + (numKeys * sizeof(ushort))).Align(sizeof(float));
            KeyDataOffset = rangeDataOffset + RangeDataSize;
            var rangeData = (float*) (basePtr + rangeDataOffset);

            KeyTimes = (ushort*) (basePtr + keyTimesOffset);
            MinTime = rangeData[0];
            DeltaTime = rangeData[1];
        }

        public float GetTime(int keyIndex)
        {
            var keyNormalizedTime = KeyTimes[keyIndex] * QuantizationScale;
            return (keyNormalizedTime * DeltaTime) + MinTime;
        }
    }

    internal readonly unsafe struct Float32BitKeyTimeAdapter : IKeyTimeAdapter
    {
        public const int KeySize = sizeof(float);
        public const int RangeDataSize = 0;

        public readonly float* KeyTimes;
        public int KeyDataOffset { get; }

        public Float32BitKeyTimeAdapter(byte* basePtr, int keyTimesOffset, int numKeys)
        {
            KeyTimes = (float*) (basePtr + keyTimesOffset);
            KeyDataOffset = (keyTimesOffset + (numKeys * sizeof(float))).Align(sizeof(float));
        }

        public float GetTime(int keyIndex) => KeyTimes[keyIndex];
    }

    internal readonly unsafe struct UniformKeyDataAdapter : IKeyDataAdapter
    {
        private readonly ERichCurveCompressionFormat _format;
        private readonly float* _keyData;

        public UniformKeyDataAdapter(ERichCurveCompressionFormat format, byte* basePtr, IKeyTimeAdapter keyTimeAdapter)
        {
            _format = format;
            _keyData = (float*) (basePtr + keyTimeAdapter.KeyDataOffset);
        }

        public int GetKeyDataHandle(int keyIndexToQuery) => _format == (ERichCurveCompressionFormat)RichCurveInterpMode.RCIM_Cubic ? (keyIndexToQuery * 3) : keyIndexToQuery;
        public float GetKeyValue(int handle) => _keyData[handle];
        public float GetKeyArriveTangent(int handle) => _keyData[handle + 1];
        public float GetKeyLeaveTangent(int handle) => _keyData[handle + 2];
        public ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex) => _format;
        public ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex) => (ERichCurveTangentWeightMode)RCTWM_WeightedNone;
        public float GetKeyArriveTangentWeight(int handle) => 0.0f;
        public float GetKeyLeaveTangentWeight(int handle) => 0.0f;
    }

    internal readonly unsafe struct MixedKeyDataAdapter : IKeyDataAdapter
    {
        private readonly byte* _interpModes;
        private readonly float* _keyData;

        public MixedKeyDataAdapter(byte* basePtr, int interpModesOffset, IKeyTimeAdapter keyTimeAdapter)
        {
            _interpModes = basePtr + interpModesOffset;
            _keyData = (float*) (basePtr + keyTimeAdapter.KeyDataOffset);
        }

        public int GetKeyDataHandle(int keyIndexToQuery) => keyIndexToQuery * 3;
        public float GetKeyValue(int handle) => _keyData[handle];
        public float GetKeyArriveTangent(int handle) => _keyData[handle + 1];
        public float GetKeyLeaveTangent(int handle) => _keyData[handle + 2];
        public ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex) => (ERichCurveCompressionFormat) _interpModes[keyIndex];
        public ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex) => (ERichCurveTangentWeightMode)RCTWM_WeightedNone;
        public float GetKeyArriveTangentWeight(int handle) => 0.0f;
        public float GetKeyLeaveTangentWeight(int handle) => 0.0f;
    }

    internal readonly unsafe struct WeightedKeyDataAdapter : IKeyDataAdapter
    {
        private readonly byte* _interpModes;
        private readonly float* _keyData;

        public WeightedKeyDataAdapter(byte* basePtr, int interpModesOffset, IKeyTimeAdapter keyTimeAdapter)
        {
            _interpModes = basePtr + interpModesOffset;
            _keyData = (float*) (basePtr + keyTimeAdapter.KeyDataOffset);
        }

        public int GetKeyDataHandle(int keyIndexToQuery) => keyIndexToQuery * 5;
        public float GetKeyValue(int handle) => _keyData[handle];
        public float GetKeyArriveTangent(int handle) => _keyData[handle + 1];
        public float GetKeyLeaveTangent(int handle) => _keyData[handle + 2];
        public float GetKeyArriveTangentWeight(int handle) => _keyData[handle + 3];
        public float GetKeyLeaveTangentWeight(int handle) => _keyData[handle + 4];
        public ERichCurveCompressionFormat GetKeyInterpMode(int keyIndex) => (ERichCurveCompressionFormat) _interpModes[keyIndex];
        public ERichCurveTangentWeightMode GetKeyTangentWeightMode(int keyIndex) => (ERichCurveTangentWeightMode) _interpModes[keyIndex + 1];
    }
}