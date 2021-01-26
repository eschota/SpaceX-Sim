using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Mesh builder for a Junction
    /// </summary>
    public class MeshBuilderJunction : MeshBuilderBase
    {
        /// <summary>
        /// Standarad Constructor
        /// </summary>
        /// <param name="roadObject">The base road object</param>
        /// <param name="list">The list of guids for the junction</param>
        /// <param name="roadNetworkNode"></param>
        /// <param name="materialName"></param>
        public MeshBuilderJunction(IRoadBuildData roadObject, List<Guid> list, RoadNetworkNode roadNetworkNode, string materialName)
        {
            _roadObject = roadObject;
            _list = list;
            _roadNetworkNode = roadNetworkNode;
            _materialName = materialName;
        }

        #region Public Methods
        /// <summary>
        /// Update the end points
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        public void UpdateEndPoints()
        {
            RoadJunctionOrder order;
            MeshBuilderSection mbs;
            RoadCrossSection leftIntersectionInner, leftIntersectionOutter, rightIntersectionInner, rightIntersectionOutter, middleIntersectionInner;
            UpdateRoadNodesEndPoint(out order, out mbs, out leftIntersectionInner, out leftIntersectionOutter, out rightIntersectionInner, out rightIntersectionOutter, out middleIntersectionInner);
        }

        /// <summary>
        /// Build the mesh for the Junction
        /// </summary>
        public void Build()
        {
            JunctionArray ja = BuildArray(true, null);

            CreateMeshFromArray(ja);

            if (RoadConstructorHelper.CrossSectionDetails.WithCurbValue)
            {
                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavement)
                    CreateCurbAndPavement(ja);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementExtended)
                    CreateCurbAndPavement(ja);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementInnerCurveA)
                    CreateCurbAndPavementCurve(ja, 0.1f);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementInnerCurveB)
                    CreateCurbAndPavementCurve(ja, 0.0f);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementInnerCurveC)
                    CreateCurbAndPavementCurve(ja, 0.5f);
            }
        }

        /// <summary>
        /// Modify the terrain for the Junction
        /// </summary>
        public void ApplyTerrain(TerrainModifier terrainModifier)
        {
            JunctionArray ja = BuildArray(false, terrainModifier);

            RectVector3 rect;
            rect = new RectVector3(ja.LeftSide[6], ja.LeftSide[7], ja.RightSide[6], ja.RightSide[7]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.LeftFarSide[5], ja.LeftFarSide[4], ja.LeftSide[6], ja.LeftSide[5]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.RightSide[6], ja.RightSide[5], ja.RightFarSide[5], ja.RightFarSide[4]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.RightSide[6], ja.RightFarSide[5], ja.LeftSide[6], ja.LeftFarSide[5]);
            terrainModifier.ApplyToTerrain(rect, true);

            // pavements
            rect = new RectVector3(ja.LeftSide[1], ja.LeftSide[4], ja.LeftSide[2], ja.LeftSide[3]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.RightSide[1], ja.RightSide[4], ja.RightSide[2], ja.RightSide[3]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.LeftFarSide[1], ja.LeftFarSide[2], ja.LeftFarSide[6], ja.LeftFarSide[3]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.RightFarSide[3], ja.RightFarSide[2], ja.RightFarSide[6], ja.RightFarSide[1]);
            terrainModifier.ApplyToTerrain(rect, true);
            rect = new RectVector3(ja.LeftFarSide[6], ja.LeftFarSide[3], ja.RightFarSide[6], ja.RightFarSide[3]);
            terrainModifier.ApplyToTerrain(rect, true);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Build the array for the junction
        /// </summary>
        /// <param name="buildMesh">Wheter to create the mesh or update terrain</param>
        /// <param name="terrainModifier">The terrain to update</param>
        /// <returns>The junction array for the junction</returns>
        private JunctionArray BuildArray(bool buildMesh, TerrainModifier terrainModifier)
        {
            RoadJunctionOrder order;
            MeshBuilderSection mbs;
            RoadCrossSection leftIntersectionInner, leftIntersectionOutter, rightIntersectionInner, rightIntersectionOutter, middleIntersectionInner;
            UpdateRoadNodesEndPoint(out order, out mbs, out leftIntersectionInner, out leftIntersectionOutter, out rightIntersectionInner, out rightIntersectionOutter, out middleIntersectionInner);

            if (buildMesh)
            {
                mbs.DirectBuild(leftIntersectionInner, order.LeftRoad);
                mbs.DirectBuild(rightIntersectionInner, order.RightRoad);
                mbs.DirectBuild(middleIntersectionInner, order.MiddleRoad);
            }
            else
            {
                terrainModifier.ApplyToTerrain(leftIntersectionInner, order.LeftRoad);
                terrainModifier.ApplyToTerrain(rightIntersectionInner, order.RightRoad);
                terrainModifier.ApplyToTerrain(middleIntersectionInner, order.MiddleRoad);

                terrainModifier.ApplyToTerrain(leftIntersectionInner, rightIntersectionInner);
            }

            JunctionArray ja = new JunctionArray();
            ja.LeftSide[7] = middleIntersectionInner.Left;
            ja.LeftSide[4] = middleIntersectionInner.CurbLeftLip;
            ja.LeftSide[1] = middleIntersectionInner.CurbLeftEnd;

            ja.LeftSide[5] = leftIntersectionInner.Right;
            ja.LeftSide[2] = leftIntersectionInner.CurbRightLip;
            ja.LeftSide[6] = leftIntersectionOutter.Right;
            ja.LeftSide[3] = AddSlope(leftIntersectionOutter.CurbRightLip, middleIntersectionInner.Angle);

            ja.RightSide[7] = middleIntersectionInner.Right;
            ja.RightSide[4] = middleIntersectionInner.CurbRightLip;
            ja.RightSide[1] = middleIntersectionInner.CurbRightEnd;

            ja.RightSide[5] = rightIntersectionInner.Left;
            ja.RightSide[2] = rightIntersectionInner.CurbLeftLip;
            ja.RightSide[6] = rightIntersectionOutter.Left;
            ja.RightSide[3] = AddSlope(rightIntersectionOutter.CurbLeftLip, middleIntersectionInner.Angle - (float)(Mathf.PI));

            ja.LeftFarSide[1] = leftIntersectionInner.CurbLeftEnd;
            ja.LeftFarSide[2] = leftIntersectionInner.CurbLeftLip;
            ja.LeftFarSide[4] = leftIntersectionInner.Left;

            ja.LeftFarSide[6] = leftIntersectionOutter.CurbLeftEnd;
            ja.LeftFarSide[3] = leftIntersectionOutter.CurbLeftLip;
            ja.LeftFarSide[5] = leftIntersectionOutter.Left;

            ja.RightFarSide[1] = rightIntersectionInner.CurbRightEnd;
            ja.RightFarSide[2] = rightIntersectionInner.CurbRightLip;
            ja.RightFarSide[4] = rightIntersectionInner.Right;

            ja.RightFarSide[6] = rightIntersectionOutter.CurbRightEnd;
            ja.RightFarSide[3] = rightIntersectionOutter.CurbRightLip;
            ja.RightFarSide[5] = rightIntersectionOutter.Right;
            return ja;
        }

        /// <summary>
        /// Update all of the end points of the jucntions
        /// </summary>
        /// <param name="order">The orderd list of roads</param>
        /// <param name="mbs">The created mesg builder</param>
        /// <param name="leftIntersectionInner">The inner left cross section</param>
        /// <param name="leftIntersectionOutter">The outter left cross section</param>
        /// <param name="rightIntersectionInner">The inner right cross section</param>
        /// <param name="rightIntersectionOutter">The outter right cross section</param>
        /// <param name="middleIntersectionInner">The middle road cross sections</param>
        private void UpdateRoadNodesEndPoint(out RoadJunctionOrder order, out MeshBuilderSection mbs, out RoadCrossSection leftIntersectionInner,
            out RoadCrossSection leftIntersectionOutter, out RoadCrossSection rightIntersectionInner, out RoadCrossSection rightIntersectionOutter,
            out RoadCrossSection middleIntersectionInner)
        {
            RoadNetworkNode roadA = _roadNetworkNode.Details.Roads[0];
            RoadNetworkNode roadB = _roadNetworkNode.Details.Roads[1];
            RoadNetworkNode roadC = _roadNetworkNode.Details.Roads[2];

            order = new RoadJunctionOrder(
                                    _list[0], roadA,
                                    _list[1], roadB,
                                    _list[2], roadC);
            mbs = new MeshBuilderSection(_roadObject, null, _materialName, 0);

            // left road
            leftIntersectionInner = order.LeftRoad.GetIntersection(order.MiddleRoad.Angle, order.MiddleRoad.CurbLeftEnd);
            leftIntersectionOutter = order.LeftRoad.GetIntersection(order.MiddleRoad.Angle, order.MiddleRoad.Left);
            leftIntersectionInner.Angle += (float)(Math.PI);

            rightIntersectionInner = order.RightRoad.GetIntersection(order.MiddleRoad.Angle, order.MiddleRoad.CurbRightEnd);
            rightIntersectionOutter = order.RightRoad.GetIntersection(order.MiddleRoad.Angle, order.MiddleRoad.Right);
            Vector3 curbToCurb = rightIntersectionInner.CurbLeftEnd - leftIntersectionInner.CurbRightEnd;
            float newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) + (Mathf.PI / 2);
            middleIntersectionInner = order.MiddleRoad.GetIntersection(newRoadAngle, leftIntersectionInner.CurbRightEnd);

            // Store the ids so we know where to update them later
            Guid leftRoadNodeId = order.LeftRoad.ID;
            Guid rightRoadNodeId = order.RightRoad.ID;
            Guid middleRoadNodeId = order.MiddleRoad.ID;

            // reduce the run off roads
            order.ReSetLeft(FindSmallestRoadForJunction(order.LeftRoadNode, leftIntersectionInner, order.LeftRoad.Middle));
            order.ReSetRight(FindSmallestRoadForJunction(order.RightRoadNode, rightIntersectionInner, order.RightRoad.Middle));
            order.ReSetMiddle(FindSmallestRoadForJunction(order.MiddleRoadNode, middleIntersectionInner, order.MiddleRoad.Middle));

            IntersectionManager.Instance.SetIntersection(leftRoadNodeId, order.LeftRoad);
            IntersectionManager.Instance.SetIntersection(middleRoadNodeId, order.MiddleRoad);
            IntersectionManager.Instance.SetIntersection(rightRoadNodeId, order.RightRoad);
        }

        /// <summary>
        /// Find the smallest road needed for this junction
        /// </summary>
        /// <param name="currentroadNode">The road node</param>
        /// <param name="crossSections">The cross section of the road</param>
        /// <param name="middlePoint">The middle point in the cross section</param>
        /// <returns>The cross seciton of the road that is nearest to the junction</returns>
        private RoadCrossSection FindSmallestRoadForJunction(RoadNetworkNode currentroadNode, RoadCrossSection crossSections, Vector3 middlePoint)
        {
            Vector3 a = crossSections.Middle;
            Vector3 diffline = a - middlePoint;
            float angleA = MathsHelper.GetAngleFrom(diffline.x, diffline.z);
            angleA = RoadUnionHelper.AngleClamp(crossSections.Angle - angleA) - Mathf.PI / 2;
            Vector3 diff = a - crossSections.CurbLeftEndDrop;
            float mag2 = diff.magnitude * Mathf.Cos(angleA);
            if (mag2 < 0)
                mag2 = -mag2;
            Vector3 roadPointA = currentroadNode.GetOffSetDownRoad(crossSections.Middle, mag2);
            float angleB = RoadUnionHelper.AngleClamp(GetAngleOfRoad(currentroadNode) + Mathf.PI / 2);
            RoadCrossSection rA = new RoadCrossSection(roadPointA, angleB - (float)(Math.PI / 2), RoadConstructorHelper.CrossSection(currentroadNode), RoadConstructorHelper.Materials(currentroadNode));
            return rA;
        }

        /// <summary>
        /// Get the angle of the road
        /// </summary>
        /// <param name="roadNetworkNode">The main node</param>
        /// <returns>The angle of the road</returns>
        private float GetAngleOfRoad(RoadNetworkNode rnn)
        {
            GameObject StartObj = _roadNetworkNode.gameObject;
            GameObject EndObj = rnn.gameObject;

            Vector3 startPosition = StartObj.transform.position;
            Vector3 endPosition = EndObj.transform.position;
            Vector3 diff = startPosition - endPosition;

            return MathsHelper.GetAngleFrom(diff.x, diff.z);
        }

        /// <summary>
        /// Create the Curb and pavement mesh if needed
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        private void CreateCurbAndPavement(JunctionArray ja)
        {
            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // left hand bottom corner
                DrawQuad(ja.LeftSide[5], ja.LeftSide[2], ja.LeftSide[6], ja.LeftSide[3],
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));

                DrawQuad(ja.LeftSide[4], ja.LeftSide[7], ja.LeftSide[3], ja.LeftSide[6],
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftLipInner));
            }

            DrawTri(ja.LeftSide[1], ja.LeftSide[4], ja.LeftSide[3],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));

            DrawTri(ja.LeftSide[1], ja.LeftSide[3], ja.LeftSide[2],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Right hand bottom corner
                DrawQuad(ja.RightSide[6], ja.RightSide[3], ja.RightSide[5], ja.RightSide[2],
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner));

                DrawQuad(ja.RightSide[3], ja.RightSide[6], ja.RightSide[4], ja.RightSide[7],
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner));
            }

            DrawTri(ja.RightSide[1], ja.RightSide[3], ja.RightSide[4],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner));

            DrawTri(ja.RightSide[1], ja.RightSide[2], ja.RightSide[3],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Middle main road
                DrawQuad(ja.LeftFarSide[5], ja.LeftFarSide[3], ja.LeftFarSide[4], ja.LeftFarSide[2],
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner));
            }

            if (RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue != 0)
            {
                // drop edge
                DrawQuad(ja.LeftFarSide[6], ApplyEdge(ja.LeftFarSide[6]), ja.LeftFarSide[1], ApplyEdge(ja.LeftFarSide[1]),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }

            DrawTri(ja.LeftFarSide[1], ja.LeftFarSide[2], ja.LeftFarSide[3],
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbRightInner),
                        new Vector2(UVDATA.StraightLength / 2, UVDATA.CurbRightInner));

            DrawTri(ja.LeftFarSide[3], ja.LeftFarSide[6], ja.LeftFarSide[1],
                        new Vector2(UVDATA.StraightLength / 2, UVDATA.CurbRightInner),
                        new Vector2(UVDATA.StraightLength / 2, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbRightOutter));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                DrawQuad(ja.RightFarSide[4], ja.RightFarSide[2], ja.RightFarSide[5], ja.RightFarSide[3],
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));
            }

            if (RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue != 0)
            {
                // drop edge
                DrawQuad(ja.RightFarSide[1], ApplyEdge(ja.RightFarSide[1]), ja.RightFarSide[6], ApplyEdge(ja.RightFarSide[6]),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }

            DrawTri(ja.RightFarSide[3], ja.RightFarSide[2], ja.RightFarSide[1],
                        new Vector2(UVDATA.StraightLength / 2, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter));

            DrawTri(ja.RightFarSide[1], ja.RightFarSide[6], ja.RightFarSide[3],
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightLength / 2, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightLength / 2, UVDATA.CurbLeftInner));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Curb
                DrawQuad(ja.RightFarSide[5], ja.RightFarSide[3], ja.LeftFarSide[5], ja.LeftFarSide[3],
                            new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }

            DrawTri(ja.LeftFarSide[6], ja.LeftFarSide[3], ja.RightFarSide[3],
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner));

            DrawTri(ja.RightFarSide[6], ja.LeftFarSide[6], ja.RightFarSide[3],
                        new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner));

            if (RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue != 0)
            {
                // drop edge
                DrawQuad(ja.RightFarSide[6], ApplyEdge(ja.RightFarSide[6]), ja.LeftFarSide[6], ApplyEdge(ja.LeftFarSide[6]),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }
        }

        /// <summary>
        /// Create the mesh for the junctions
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        private void CreateMeshFromArray(JunctionArray ja)
        {
            // Road section Middle
            DrawQuad(ja.LeftSide[6], ja.LeftSide[7], ja.RightSide[6], ja.RightSide[7],
                        new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftLipInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                        new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightLipInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner));

            if (UVDATA.JunctionAcrossUV)
            {
                // Main road section Left
                DrawQuad(ja.LeftFarSide[5], ja.LeftFarSide[4], ja.LeftSide[6], ja.LeftSide[5],
                            new Vector2(UVDATA.JunctionTopLeftInner, UVDATA.JunctionTopEnd),
                            new Vector2(UVDATA.JunctionTopLeftInner, UVDATA.JunctionTopStart),
                            new Vector2(UVDATA.JunctionTopRightInner, UVDATA.JunctionTopEnd),
                            new Vector2(UVDATA.JunctionTopRightInner, UVDATA.JunctionTopStart));
            }
            else
            {
                DrawQuad(ja.LeftFarSide[5], ja.LeftFarSide[4], ja.LeftSide[6], ja.LeftSide[5],
                          new Vector2(UVDATA.JunctionTopEnd, UVDATA.JunctionTopLeftInner),
                          new Vector2(UVDATA.JunctionTopStart, UVDATA.JunctionTopLeftInner),
                          new Vector2(UVDATA.JunctionTopEnd, UVDATA.JunctionTopRightInner),
                          new Vector2(UVDATA.JunctionTopStart, UVDATA.JunctionTopRightInner));
            }

            // Road section Right
            if (UVDATA.JunctionAcrossUV)
            {
                DrawQuad(ja.RightSide[6], ja.RightSide[5], ja.RightFarSide[5], ja.RightFarSide[4],
                            new Vector2(UVDATA.JunctionBottomLeftInner, UVDATA.JunctionBottomEnd),
                            new Vector2(UVDATA.JunctionBottomLeftInner, UVDATA.JunctionBottomStart),
                            new Vector2(UVDATA.JunctionBottomRightInner, UVDATA.JunctionBottomEnd),
                            new Vector2(UVDATA.JunctionBottomRightInner, UVDATA.JunctionBottomStart));
            }
            else
            {
                DrawQuad(ja.RightSide[6], ja.RightSide[5], ja.RightFarSide[5], ja.RightFarSide[4],
                            new Vector2(UVDATA.JunctionBottomEnd, UVDATA.JunctionBottomLeftInner),
                            new Vector2(UVDATA.JunctionBottomStart, UVDATA.JunctionBottomLeftInner),
                            new Vector2(UVDATA.JunctionBottomEnd, UVDATA.JunctionBottomRightInner),
                            new Vector2(UVDATA.JunctionBottomStart, UVDATA.JunctionBottomRightInner));
            }

            // Middle middle road
            DrawQuad(ja.RightSide[6], ja.RightFarSide[5], ja.LeftSide[6], ja.LeftFarSide[5],
                        new Vector2(UVDATA.JunctionIntersectionStart, UVDATA.CurbRightLipInner),
                        new Vector2(UVDATA.JunctionIntersectionEnd, UVDATA.CurbRightLipInner),
                        new Vector2(UVDATA.JunctionIntersectionStart, UVDATA.CurbLeftLipInner),
                        new Vector2(UVDATA.JunctionIntersectionEnd, UVDATA.CurbLeftLipInner));
        }

        /// <summary>
        /// Create the Curb and pavement mesh if needed
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">the leading stright before the curve starts</param>
        private void CreateCurbAndPavementCurve(JunctionArray ja, float leadingStright)
        {
            int sections = _roadNetworkNode.Details.Sections;

            // Draw the pavement Top Corner
            _cornerDrawToLeft = CreateLeftCorner(ja, leadingStright, sections);
            _cornerDrawToRight = CreateRightCorner(ja, leadingStright, sections);

            _cornerDrawToLeft.DrawRoad();
            _cornerDrawToRight.DrawRoad();

            _cornerDrawToLeft.DrawKerb();
            _cornerDrawToRight.DrawKerb();

            _cornerDrawToLeft.DrawPavement();
            _cornerDrawToRight.DrawPavement();

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Curb left 
                DrawQuad(ja.LeftFarSide[5], ja.LeftFarSide[3], ja.LeftFarSide[4], ja.LeftFarSide[2],
                            new Vector2(0.16f, UVDATA.CurbRightLipInner),
                            new Vector2(0.16f, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbRightInner));
            }

            if (RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue != 0)
            {
                // drop edge
                DrawQuad(ja.LeftFarSide[6], ApplyEdge(ja.LeftFarSide[6]), ja.LeftFarSide[1], ApplyEdge(ja.LeftFarSide[1]),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }

            // Top Right Corner - looks ok
            DrawTri(ja.LeftFarSide[1], ja.LeftFarSide[2], ja.LeftFarSide[3],
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbRightInner),
                        new Vector2(0.16f, UVDATA.CurbRightInner));

            DrawTri(ja.LeftFarSide[3], ja.LeftFarSide[6], ja.LeftFarSide[1],
                        new Vector2(0.16f, UVDATA.CurbRightInner),
                        new Vector2(0.16f, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbRightOutter));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                DrawQuad(ja.RightFarSide[4], ja.RightFarSide[2], ja.RightFarSide[5], ja.RightFarSide[3],
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner),
                            new Vector2(0.16f, UVDATA.CurbLeftLipInner),
                            new Vector2(0.16f, UVDATA.CurbLeftInner));
            }

            if (RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue != 0)
            {
                // drop edge
                DrawQuad(ja.RightFarSide[1], ApplyEdge(ja.RightFarSide[1]), ja.RightFarSide[6], ApplyEdge(ja.RightFarSide[6]),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }

            // bottom Right Corner - looks ok
            DrawTri(ja.RightFarSide[3], ja.RightFarSide[2], ja.RightFarSide[1],
                        new Vector2(0.16f, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter));

            DrawTri(ja.RightFarSide[1], ja.RightFarSide[6], ja.RightFarSide[3],
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter),
                        new Vector2(0.16f, UVDATA.CurbLeftOutter),
                        new Vector2(0.16f, UVDATA.CurbLeftInner));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Curb
                DrawQuad(ja.RightFarSide[5], ja.RightFarSide[3], ja.LeftFarSide[5], ja.LeftFarSide[3],
                            new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }

            // far middle pavments
            DrawTri(ja.LeftFarSide[6], ja.LeftFarSide[3], ja.RightFarSide[3],
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner));

            DrawTri(ja.RightFarSide[6], ja.LeftFarSide[6], ja.RightFarSide[3],
                        new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner));

            if (RoadConstructorHelper.CrossSectionDetails.CurbEdgeDropValue != 0)
            {
                // drop edge
                DrawQuad(ja.RightFarSide[6], ApplyEdge(ja.RightFarSide[6]), ja.LeftFarSide[6], ApplyEdge(ja.LeftFarSide[6]),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightLength, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.StraightStart, UVDATA.CurbLeftInner));
            }
        }

        /// <summary>
        /// Create the right hand corner
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        private CornerDrawHolder CreateRightCorner(JunctionArray ja, float leadingStright, int sections)
        {
            SmotherCornerContext scc = new SmotherCornerContext();
            scc.Main.Vector = ja.RightSide[1];
            scc.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter);

            scc.Leading.Vector = ja.RightSide[2];
            scc.Leading.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightOutter);

            scc.Ending.Vector = ja.RightSide[4];
            scc.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner);

            scc.Far.Vector = ja.RightSide[3];
            scc.Far.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner);

            List<VectorUvs> pavementTriList = CreateFanfrom(scc, leadingStright, sections);

            // Draw the road
            SmotherCornerContext sccRoad = new SmotherCornerContext();
            Vector3 kerb = new Vector3(0, RoadConstructorHelper.CrossSectionDetails.CurbLipHeightValue, 0);
            sccRoad.Main.Vector = ja.RightSide[1] - kerb;
            sccRoad.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter);

            sccRoad.Leading.Vector = ja.RightSide[5];
            sccRoad.Leading.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightOutter);

            sccRoad.Ending.Vector = ja.RightSide[7];
            sccRoad.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner);

            sccRoad.Far.Vector = ja.RightSide[6];
            sccRoad.Far.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightLipInner);

            List<VectorUvs> roadTriList = CreateFanfrom(sccRoad, leadingStright, sections);
            // swap from the inner curb to the outer curb for the road
            roadTriList[0] = sccRoad.Far;

            CornerDrawHolder cdh = new CornerDrawHolder(pavementTriList, roadTriList);
            cdh.DrawPavementImpl = DrawFanTriListBackWards;
            cdh.DrawRoadImpl = DrawFanTriList;

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Draw the kurb if any
                cdh.SetKerb(pavementTriList, roadTriList, 1);
                cdh.DrawKerbImpl = DrawStripBackward;
            }

            return cdh;
        }

        /// <summary>
        /// Create the left hand corner
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        private CornerDrawHolder CreateLeftCorner(JunctionArray ja, float leadingStright, int sections)
        {
            SmotherCornerContext scc = new SmotherCornerContext();
            scc.Main.Vector = ja.LeftSide[1];
            scc.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter);

            scc.Leading.Vector = ja.LeftSide[2];
            scc.Leading.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftOutter);

            scc.Ending.Vector = ja.LeftSide[4];
            scc.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner);

            scc.Far.Vector = ja.LeftSide[3];
            scc.Far.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner);

            List<VectorUvs> pavementTriList = CreateFanfrom(scc, leadingStright, sections);

            // Draw the road
            SmotherCornerContext sccRoad = new SmotherCornerContext();
            Vector3 kerb = new Vector3(0, RoadConstructorHelper.CrossSectionDetails.CurbLipHeightValue, 0);
            sccRoad.Main.Vector = ja.LeftSide[1] - kerb;
            sccRoad.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter);

            sccRoad.Leading.Vector = ja.LeftSide[5];
            sccRoad.Leading.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftOutter);

            sccRoad.Ending.Vector = ja.LeftSide[7];
            sccRoad.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner);

            sccRoad.Far.Vector = ja.LeftSide[6];
            sccRoad.Far.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftLipInner);

            List<VectorUvs> roadTriList = CreateFanfrom(sccRoad, leadingStright, sections);
            // swap from the inner curb to the outer curb for the road
            roadTriList[0] = sccRoad.Far;

            CornerDrawHolder cdh = new CornerDrawHolder(pavementTriList, roadTriList);
            cdh.DrawPavementImpl = DrawFanTriList;
            cdh.DrawRoadImpl = DrawFanTriListBackWards;

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                cdh.SetKerb(pavementTriList, roadTriList, 1);
                cdh.DrawKerbImpl = DrawStrip;
            }

            return cdh;
        }
        #endregion

        #region Helper Class
        /// <summary>
        /// Holder class to holder all the point for the cross road mesh
        /// </summary>
        private class JunctionArray
        {
            public Vector3[] LeftSide = new Vector3[8];
            public Vector3[] RightSide = new Vector3[8];

            public Vector3[] LeftFarSide = new Vector3[7];
            public Vector3[] RightFarSide = new Vector3[7];
        }
        #endregion

        private CornerDrawHolder _cornerDrawToLeft;
        private CornerDrawHolder _cornerDrawToRight;
    }
}