using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Vector and UV for a point on the model
    /// </summary>
    public class VectorUvs
    {
        /// <summary>
        /// The position of the points
        /// </summary>
        public Vector3 Vector;

        /// <summary>
        /// The UVs
        /// </summary>
        public Vector2 UV;

        /// <summary>
        /// Default constructor
        /// </summary>
        public VectorUvs()
        {
        }

        /// <summary>
        /// The Standard constructor
        /// </summary>
        /// <param name="vector">The position of the poins</param>
        /// <param name="uV">The uv on the point</param>
        public VectorUvs(Vector3 vector, Vector2 uV)
        {
            Vector = vector;
            UV = uV;
        }

        /// <summary>
        /// Gets the lerp of the position and UVS
        /// </summary>
        /// <param name="leading">The leading strright before the curve starts</param>
        /// <param name="far">The far point</param>
        /// <param name="startingPercent">the amont to lepr</param>
        /// <returns>The new VectorUvs of the lerped position</returns>
        public static VectorUvs Lerp(VectorUvs leading, VectorUvs far, float startingPercent)
        {
            Vector3 leadingVector = leading.Vector;
            Vector3 farVector = far.Vector;

            Vector2 leadingUV = leading.UV;
            Vector2 farUV = far.UV;

            VectorUvs uv = new VectorUvs();
            uv.Vector = Vector3.Lerp(leadingVector, farVector, startingPercent);
            uv.UV = Vector3.Lerp(leadingUV, farUV, startingPercent);
            return uv;
        }
    }
}
