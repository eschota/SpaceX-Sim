using System;
using System.Collections.Generic;
using UnityEngine;
using eWolfRoadBuilderHelpers;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Mesh builder for a simple section
    /// </summary>
    public class MeshBuilderSection
    {
        #region Constructor
        /// <summary>
        /// Standard Constructors
        /// </summary>
        /// <param name="roadObject">The base road object</param>
        /// <param name="list">The list of roadsection Guids</param>
        /// <param name="materialNames">The name of the material to use</param>
        /// <param name="subDivide">The subdivide amount</param>
        public MeshBuilderSection(IRoadBuildData roadObject, List<Guid> list, string materialNames, int subDivide)
        {
            _roadObject = roadObject;
            _list = list;
            _materialName = materialNames;
            // TODO: Added sub divide option: _subDivide = subDivide;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create the section of the road
        /// </summary>
        public void Build()
        {
            RoadCrossSection rA = IntersectionManager.Instance[_list[0]];
            RoadCrossSection rB = IntersectionManager.Instance[_list[1]];
            Build(rA, rB, 1);
        }

        /// <summary>
        /// Create the section of the road
        /// </summary>
        /// <param name="rA">The start road cross section</param>
        /// <param name="rB">The end road cross section</param>
        public void DirectBuild(RoadCrossSection rA, RoadCrossSection rB)
        {
            Build(rA, rB, 1);
        }

        /// <summary>
        /// Create the section of the road
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="texturePercent">The percentage of the texture to use</param>
        public void Build(RoadCrossSection rA, RoadCrossSection rB, float texturePercent)
        {
            bool flip = false;

            List<int> triangles = _roadObject.GetTriangles(_materialName);
            if (rA.IsRoadTwisted(rB))
            {
                rB = RoadCrossSection.CreateMirror(rB);
            }

            flip = AddMainRoadSection(rA, rB, triangles, texturePercent);

            if (!RoadConstructorHelper.CrossSectionDetails.WithCurbValue)
                return;

            AddCurbTopLeft(rA, rB, flip, triangles, texturePercent);
            AddCurbTopRight(rA, rB, flip, triangles, texturePercent);

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                AddCurbUpLeft(rA, rB, flip, triangles);
                AddCurbUpRight(rA, rB, flip, triangles);
            }

            // Add the Curb Down polys.
            AddCurbLeftDrop(rA, rB, flip, triangles);
            AddCurbRightDrop(rA, rB, flip, triangles);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add a curb top section to the right hand side of the road 
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangle to add to</param>
        /// <param name="texturePercent">The percage of the texture to use</param>
        private void AddCurbTopRight(RoadCrossSection rA, RoadCrossSection rB, bool flip, List<int> triangles, float texturePercent)
        {
            _roadObject.MeshVertices.Add(rB.CurbRightLip);
            _roadObject.MeshVertices.Add(rA.CurbRightLip);

            _roadObject.MeshVertices.Add(rB.CurbRightEnd);
            _roadObject.MeshVertices.Add(rA.CurbRightEnd);

            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength * texturePercent, UVDATA.CurbLeftInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength * texturePercent, UVDATA.CurbLeftOutter));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter));

            AddTriangles(flip, triangles);
        }

        /// <summary>
        /// Add a curb lip section to the right hand side of the road 
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangle to add to</param>
        private void AddCurbUpRight(RoadCrossSection rA, RoadCrossSection rB, bool flip, List<int> triangles)
        {
            _roadObject.MeshVertices.Add(rB.Right);
            _roadObject.MeshVertices.Add(rA.Right);

            _roadObject.MeshVertices.Add(rB.CurbRightLip);
            _roadObject.MeshVertices.Add(rA.CurbRightLip);

            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));

            AddTriangles(flip, triangles);
        }

        /// <summary>
        /// Add a curb drop section to the right hand side of the road 
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangle to add to</param>
        private void AddCurbRightDrop(RoadCrossSection rA, RoadCrossSection rB, bool flip, List<int> triangles)
        {
            _roadObject.MeshVertices.Add(rB.CurbRightEnd);
            _roadObject.MeshVertices.Add(rA.CurbRightEnd);

            _roadObject.MeshVertices.Add(rB.CurbRightEndDrop);
            _roadObject.MeshVertices.Add(rA.CurbRightEndDrop);

            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));

            AddTriangles(flip, triangles);
        }

        /// <summary>
        /// Add a curb top section to the left hand side of the road 
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangle to add to</param>
        /// <param name="texturePercent">The percage of the texture to use</param>
        private void AddCurbTopLeft(RoadCrossSection rA, RoadCrossSection rB, bool flip, List<int> triangles, float texturePercent)
        {
            _roadObject.MeshVertices.Add(rA.CurbLeftLip);
            _roadObject.MeshVertices.Add(rB.CurbLeftLip);
            _roadObject.MeshVertices.Add(rA.CurbLeftEnd);
            _roadObject.MeshVertices.Add(rB.CurbLeftEnd);

            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength * texturePercent, UVDATA.CurbRightInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightOutter));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength * texturePercent, UVDATA.CurbRightOutter));

            AddTriangles(flip, triangles);
        }

        /// <summary>
        /// Adjust the texture size based of the distance of the two road cross sections
        /// </summary>
        /// <param name="roadCrossSection1"></param>
        /// <param name="roadCrossSection2"></param>
        public void DirectBuildDynamicTextureLength(RoadCrossSection roadCrossSection1, RoadCrossSection roadCrossSection2)
        {
            RoadCrossSection rcsA = (RoadCrossSection)roadCrossSection1.Clone();
            RoadCrossSection rcsB = (RoadCrossSection)roadCrossSection2.Clone();

            Vector3 diff = rcsA.Middle - rcsB.Middle;
            if (diff.magnitude > 5f)
            {
                Build(roadCrossSection1, roadCrossSection2, 1);
                return;
            }

            float a = (100 / 5) * diff.magnitude;
            Build(roadCrossSection1, roadCrossSection2, a / 100);
        }

        /// <summary>
        /// Add a curb lip section to the left hand side of the road 
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangle to add to</param>
        private void AddCurbUpLeft(RoadCrossSection rA, RoadCrossSection rB, bool flip, List<int> triangles)
        {
            _roadObject.MeshVertices.Add(rA.Left);
            _roadObject.MeshVertices.Add(rB.Left);

            _roadObject.MeshVertices.Add(rA.CurbLeftLip);
            _roadObject.MeshVertices.Add(rB.CurbLeftLip);

            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbRightLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbRightInner));

            AddTriangles(flip, triangles);
        }

        /// <summary>
        /// Add a curb drop off section to the left hand side of the road 
        /// </summary>
        /// <param name="rA">The start of the road section</param>
        /// <param name="rB">The end of the road section</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangle to add to</param>
        private void AddCurbLeftDrop(RoadCrossSection rA, RoadCrossSection rB, bool flip, List<int> triangles)
        {
            _roadObject.MeshVertices.Add(rA.CurbLeftEnd);
            _roadObject.MeshVertices.Add(rB.CurbLeftEnd);

            _roadObject.MeshVertices.Add(rA.CurbLeftEndDrop);
            _roadObject.MeshVertices.Add(rB.CurbLeftEndDrop);

            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbRightLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength, UVDATA.CurbRightInner));

            AddTriangles(flip, triangles);
        }

        /// <summary>
        /// Create the main section of the road. EG the road
        /// </summary>
        /// <param name="rA">Start of road</param>
        /// <param name="rB">End of the road</param>
        /// <param name="texturePercent">The percentage of texture to use</param>
        /// <returns>The flip flag</returns>
        private bool AddMainRoadSection(RoadCrossSection rA, RoadCrossSection rB, List<int> triangles, float texturePercent)
        {
            bool flip = false;

            // TODO: ADD subdivde here: Debug.Log("_subDivide = "+ _subDivide);
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbRightLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength * texturePercent, UVDATA.CurbRightLipInner));
            _roadObject.MeshUVs.Add(new Vector2(UVDATA.StraightLength * texturePercent, UVDATA.CurbLeftLipInner));

            _roadObject.MeshVertices.Add(rA.Left);
            _roadObject.MeshVertices.Add(rA.Right);

            _roadObject.MeshVertices.Add(rB.Left);
            _roadObject.MeshVertices.Add(rB.Right);

            int v = _roadObject.MeshVertices.Count - 4;

            // now we can work out if we need to flip the normals
            Vector3 nA = MathsHelper.NormalTri(
                _roadObject.MeshVertices[v],
                _roadObject.MeshVertices[v + 1],
                _roadObject.MeshVertices[v + 2]);

            nA.Normalize();

            Vector3 nB = MathsHelper.NormalTri(
                _roadObject.MeshVertices[v +3 ],
                _roadObject.MeshVertices[v + 2],
                _roadObject.MeshVertices[v + 1]);

            nB.Normalize();

            if (nA.y < 0 && nB.y < 0)
            {
                flip = true;
            }

            AddTriangles(flip, triangles);

            return flip;
        }

        /// <summary>
        /// Add the triangle to the last 4 tris, 
        /// </summary>
        /// <param name="_roadObject">The object to update</param>
        /// <param name="flip">Do we need to flip the triangles</param>
        /// <param name="triangles">The list of triangles to add to</param>
        private void AddTriangles(bool flip, List<int> triangles)
        {
            int v = _roadObject.MeshVertices.Count - 4;
            if (!flip)
            {
                triangles.AddRange(new int[] { v, v + 1, v + 2 });
                triangles.AddRange(new int[] { v + 3, v + 2, v + 1 });
            }
            else
            {
                triangles.AddRange(new int[] { v, v + 2, v + 1 });
                triangles.AddRange(new int[] { v + 3, v + 1, v + 2 });
            }
        }
        #endregion

        #region Private Fields
        private IRoadBuildData _roadObject;
        private List<Guid> _list;
        private string _materialName;
        // TODO: Added sub divide option: private int _subDivide;
        #endregion
    }
}
