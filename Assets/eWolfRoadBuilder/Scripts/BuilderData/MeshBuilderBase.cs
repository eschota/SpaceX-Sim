using System;
using System.Collections.Generic;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The base class to builder
    /// </summary>
    public abstract class MeshBuilderBase
    {
        #region Protected Methods
        /// <summary>
        /// Create a poly(quad)
        /// </summary>
        /// <param name="a">Point A</param>
        /// <param name="b">Point B</param>
        /// <param name="c">Point C</param>
        /// <param name="d">Point D</param>
        /// <param name="uva">UV Point A</param>
        /// <param name="uvb">UV Point B</param>
        /// <param name="uvc">UV Point C</param>
        /// <param name="uvd">UV Point D</param>
        protected void DrawQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector2 uva, Vector2 uvb, Vector2 uvc, Vector2 uvd)
        {
            _roadObject.MeshUVs.Add(uva);
            _roadObject.MeshUVs.Add(uvb);
            _roadObject.MeshUVs.Add(uvc);
            _roadObject.MeshUVs.Add(uvd);

            _roadObject.MeshVertices.Add(a);
            _roadObject.MeshVertices.Add(b);

            _roadObject.MeshVertices.Add(c);
            _roadObject.MeshVertices.Add(d);

            int v = _roadObject.MeshVertices.Count - 4;
            _roadObject.GetTriangles(_materialName).AddRange(new int[] { v + 2, v + 1, v });
            _roadObject.GetTriangles(_materialName).AddRange(new int[] { v + 1, v + 2, v + 3 });
            /*          Future  code : Vert caching
                        int indexA = AddVectorUVSets(a, uva);
                        int indexB = AddVectorUVSets(b, uvb);
                        int indexC = AddVectorUVSets(c, uvc);
                        int indexD = AddVectorUVSets(d, uvd);

                        _roadObject.GetTriangles(_materialName).AddRange(new int[] { indexA, indexB, indexC });
                        _roadObject.GetTriangles(_materialName).AddRange(new int[] { indexD, indexC, indexB });*/
        }

        /// <summary>
        /// Create a poly(tri)
        /// </summary>
        /// <param name="a">Point A</param>
        /// <param name="b">Point B</param>
        /// <param name="c">Point C</param>
        /// <param name="uva">UV Point A</param>
        /// <param name="uvb">UV Point B</param>
        /// <param name="uvc">UV Point C</param>
        protected void DrawTri(Vector3 a, Vector3 b, Vector3 c, Vector2 uva, Vector2 uvb, Vector2 uvc)
        {
            _roadObject.MeshUVs.Add(uva);
            _roadObject.MeshUVs.Add(uvb);
            _roadObject.MeshUVs.Add(uvc);

            _roadObject.MeshVertices.Add(a);
            _roadObject.MeshVertices.Add(b);
            _roadObject.MeshVertices.Add(c);

            int v = _roadObject.MeshVertices.Count - 3;
            _roadObject.GetTriangles(_materialName).AddRange(new int[] { v + 2, v + 1, v });
            /* Future code : Vert caching
            int indexA = AddVectorUVSets(a, uva);
            int indexB = AddVectorUVSets(b, uvb);
            int indexC = AddVectorUVSets(c, uvc);

            _roadObject.GetTriangles(_materialName).AddRange(new int[] { indexA, indexB, indexC });*/
        }

        /// <summary>
        /// Add the slope amount to the vector
        /// </summary>
        /// <param name="pos">The vector to update</param>
        /// <param name="angle">The angle to apply the slope off set</param>
        /// <returns>The update angle with the applied off set</returns>
        protected Vector3 AddSlope(Vector3 pos, float angle)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            pos.x -= (float)(x * RoadConstructorHelper.CrossSectionDetails.CurbLipSlopeValue);
            pos.z += (float)(z * RoadConstructorHelper.CrossSectionDetails.CurbLipSlopeValue);

            return pos;
        }

        /// <summary>
        /// Help class
        /// </summary>
        protected class SmotherCornerContext
        {
            /// <summary>
            /// The main starting point
            /// </summary>
            public VectorUvs Main = new VectorUvs();

            /// <summary>
            /// The frist point
            /// </summary>
            public VectorUvs Leading = new VectorUvs();

            /// <summary>
            /// The opposite point
            /// </summary>
            public VectorUvs Far = new VectorUvs();

            /// <summary>
            /// The ending point
            /// </summary>
            public VectorUvs Ending = new VectorUvs();
        }

        /// <summary>
        /// Draws a strip from two lists
        /// </summary>
        /// <param name="topList">The first list</param>
        /// <param name="bottomList">The second list</param>
        /// <param name="startingIndex">The starting index</param>
        protected void DrawStrip(List<VectorUvs> topList, List<VectorUvs> bottomList, int startingIndex)
        {
            int t1 = startingIndex;
            int t2 = startingIndex + 1;
            for (int i = startingIndex; i < (topList.Count - 1); i++)
            {
                DrawQuad(bottomList[t2].Vector, bottomList[t1].Vector, topList[t2].Vector, topList[t1].Vector,
                        bottomList[t2].UV,
                        bottomList[t1].UV,
                        topList[t2].UV,
                        topList[t1].UV);
                t1++;
                t2++;
            }
        }

        /// <summary>
        /// Draws a strip from the two lists
        /// </summary>
        /// <param name="topList">The first list</param>
        /// <param name="bottomList">The second list</param>
        /// <param name="startingIndex">The starting index</param>
        protected void DrawStripBackward(List<VectorUvs> topList, List<VectorUvs> bottomList, int startingIndex)
        {
            int t1 = (topList.Count - 1);
            int t2 = (topList.Count - 2);
            for (int i = (topList.Count - 1); i > startingIndex; i--)
            {
                DrawQuad(bottomList[t2].Vector, bottomList[t1].Vector, topList[t2].Vector, topList[t1].Vector,
                        bottomList[t2].UV,
                        bottomList[t1].UV,
                        topList[t2].UV,
                        topList[t1].UV);
                t1--;
                t2--;
            }
        }

        /// <summary>
        /// Draw a fan of tris.
        /// </summary>
        /// <param name="fanList">The list of verts</param>
        protected void DrawFanTriList(List<VectorUvs> fanList)
        {
            VectorUvs a = fanList[0];
            VectorUvs b = fanList[1];
            VectorUvs c = fanList[2];

            DrawTri(a.Vector, c.Vector, b.Vector,
                         a.UV,
                         c.UV,
                         b.UV);

            for (int i = 3; i < fanList.Count; i++)
            {
                b = c;
                c = fanList[i];

                DrawTri(a.Vector, c.Vector, b.Vector,
                             a.UV,
                             c.UV,
                             b.UV);
            }
        }

        /// <summary>
        /// Draw a afan of tris, starting at the far end
        /// </summary>
        /// <param name="fanList">The list of verts</param>
        protected void DrawFanTriListBackWards(List<VectorUvs> fanList)
        {
            VectorUvs a = fanList[0];
            int count = fanList.Count - 1;
            VectorUvs b = fanList[count--];
            VectorUvs c = fanList[count--];

            DrawTri(a.Vector, c.Vector, b.Vector,
                         a.UV,
                         c.UV,
                         b.UV);

            for (int i = 3; i < fanList.Count; i++)
            {
                b = c;
                c = fanList[count--];

                DrawTri(a.Vector, c.Vector, b.Vector,
                             a.UV,
                             c.UV,
                             b.UV);
            }
        }

        /// <summary>
        /// Creates a fan list from a smotherCornerContext
        /// </summary>
        /// <param name="scc">The smother corner context</param>
        /// <param name="startingPercent">The starting point along the first lines</param>
        /// <param name="sections">The number of sections to use</param>
        /// <returns>The list of tris, as a fan list</returns>
        protected List<VectorUvs> CreateFanfrom(SmotherCornerContext scc, float startingPercent, int sections)
        {
            List<VectorUvs> l = new List<VectorUvs>();

            // TODO: When starting percent is 0 - are we adding the first poly twice?
            l.Add(scc.Main);
            l.Add(scc.Leading);
            VectorUvs leftStartUV = VectorUvs.Lerp(scc.Leading, scc.Far, startingPercent);
            VectorUvs rightStartUV = VectorUvs.Lerp(scc.Ending, scc.Far, startingPercent);

            l.Add(leftStartUV);

            float sectionsGap = 1.0f / sections;

            for (int i = 0; i < sections + 1; i++)
            {
                l.Add(GetBezierCurvePoint(leftStartUV, rightStartUV, scc.Far, (i * sectionsGap)));
            }

            l.Add(scc.Ending);
            return l;
        }

        /// <summary>
        /// Apply the curb edge off set
        /// </summary>
        /// <param name="pos">The position to update</param>
        /// <returns>The droped position</returns>
        protected Vector3 ApplyEdge(Vector3 pos)
        {
            pos.y -= RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue;
            return pos;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add the vertor and UV sets if they are unique
        /// </summary>
        /// <param name="points">The vector point to add</param>
        /// <param name="uvs">The UV to add</param>
        /// <returns>The index of existing pair, or the index of the newly added pair.</returns>
        private int AddVectorUVSets(Vector3 points, Vector2 uvs)
        {
            for (int i = 0; i < _roadObject.MeshVertices.Count; i++)
            {
                Vector3 vec = _roadObject.MeshVertices[i];
                Vector2 v2 = _roadObject.MeshUVs[i];

                if (vec == points && v2 == uvs)
                {
                    return i;
                }
            }

            _roadObject.MeshVertices.Add(points);
            _roadObject.MeshUVs.Add(uvs);
            return _roadObject.MeshVertices.Count - 1;
        }

        /// <summary>
        /// Get the VectorUv along the curve
        /// </summary>
        /// <param name="leftStart">The left most point</param>
        /// <param name="rightStart">The right most point</param>
        /// <param name="far">The far corner</param>
        /// <param name="percentage">The percentage along the line to get</param>
        /// <returns>The vectorUV point along the line</returns>
        private VectorUvs GetBezierCurvePoint(VectorUvs leftStart, VectorUvs rightStart, VectorUvs far, float percentage)
        {
            VectorUvs partA = VectorUvs.Lerp(leftStart, far, percentage);
            VectorUvs partB = VectorUvs.Lerp(far, rightStart, percentage);
            VectorUvs target = VectorUvs.Lerp(partA, partB, percentage);
            return target;
        }
        #endregion

        #region Private Fields
        protected IRoadBuildData _roadObject;
        protected List<Guid> _list;
        protected RoadNetworkNode _roadNetworkNode;
        protected string _materialName;
        #endregion
    }
}
