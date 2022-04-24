using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAssetAPI
{
    /// <summary>
    /// A vector in 3-D space composed of components (X, Y, Z) with floating point precision.
    /// </summary>
    public class FVector
    {
        /// <summary>Vector's X-component.</summary>
        [JsonProperty]
        [JsonConverter(typeof(FSignedZeroJsonConverter))]
        public float X;

        /// <summary>Vector's Y-component.</summary>
        [JsonProperty]
        [JsonConverter(typeof(FSignedZeroJsonConverter))]
        public float Y;

        /// <summary>Vector's Z-component.</summary>
        [JsonProperty]
        [JsonConverter(typeof(FSignedZeroJsonConverter))]
        public float Z;

        public FVector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public FVector(float scale) {
            X = scale;
            Y = scale;
            Z = scale;
        }
        public FVector()
        {

        }

        public FVector Scale(float scale) {
            return new FVector(X * scale, Y * scale, Z * scale);
        }

        public void Scale(FVector scale) {
            X *= scale.X;
            Y *= scale.Y;
            Z *= scale.Z;
        }
        public JObject ToJson() {
            JObject res = new JObject();
            res.Add(new JProperty("X", X));
            res.Add(new JProperty("Y", Y));
            res.Add(new JProperty("Z", Z));
            return res;
        }
    }
}
