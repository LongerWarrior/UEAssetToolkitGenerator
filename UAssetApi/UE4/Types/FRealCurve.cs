using Newtonsoft.Json.Converters;
using UAssetApi.UE4.Objects.Engine;

namespace UAssetApi.UE4.Types;
    
    /** A rich, editable float curve */
    public abstract class FRealCurve : IUStruct
    {
        public float DefaultValue;
        [JsonConverter(typeof(StringEnumConverter))]
        public ERichCurveExtrapolation PreInfinityExtrap;
        [JsonConverter(typeof(StringEnumConverter))]
        public ERichCurveExtrapolation PostInfinityExtrap;

        /*public FRealCurve()
        {
            DefaultValue = this["DefaultValue"] is FloatPropertyData curve ? curve.Value : 3.402823466e+38f; // MAX_flt;
            PreInfinityExtrap = this["PreInfinityExtrap"] is EnumPropertyData pre ? 
                (ERichCurveExtrapolation)pre.RawValue : ERichCurveExtrapolation.RCCE_Constant;
            PostInfinityExtrap = this["PostInfinityExtrap"] is EnumPropertyData post ? 
                (ERichCurveExtrapolation)post.RawValue : ERichCurveExtrapolation.RCCE_Constant;
        }*/

        public abstract void RemapTimeValue(ref float inTime, ref float cycleValueOffset);
        public abstract float Eval(float inTime, float inDefaultTime = 0);

        protected static void CycleTime(float minTime, float maxTime, ref float inTime, ref int cycleCount)
        {
            var initTime = inTime;
            var duration = maxTime - minTime;

            if (inTime > maxTime)
            {
                cycleCount = (int) ((maxTime - inTime) / duration);
                inTime += duration * cycleCount;
            }
            else if (inTime < minTime)
            {
                cycleCount = (int) ((inTime - minTime) / duration);
                inTime -= duration * cycleCount;
            }

            if (inTime == maxTime && initTime < minTime)
                inTime = minTime;
            if (inTime == minTime && initTime > maxTime)
                inTime = maxTime;

            cycleCount = Math.Abs(cycleCount);
        }
    }