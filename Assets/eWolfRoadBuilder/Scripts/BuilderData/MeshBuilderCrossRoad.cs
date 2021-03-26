using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Mesh builder for the cross road
    /// </summary>
    public class MeshBuilderCrossRoad : MeshBuilderBase
    {
        /// <summary>
        /// Standarad Constructor
        /// </summary>
        /// <param name="roadObject">The base road object</param>
        /// <param name="list">The list of guids for the cross roads</param>
        /// <param name="roadNetworkNode"></param>
        /// <param name="materialName"></param>
        public MeshBuilderCrossRoad(IRoadBuildData roadObject, List<Guid> list, RoadNetworkNode roadNetworkNode, string materialName)
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
            RoadCrossRoadOrder order;
            MeshBuilderSection mbs;
            RoadCrossSection leftIntersectionInner, leftIntersectionOutter, rightIntersectionInner, rightIntersectionOutter, middleIntersectionInner, oppositeIntersectionInner;
            UpdateRoadNodesEndPoints(out order, out mbs, out leftIntersectionInner, out leftIntersectionOutter, out rightIntersectionInner, out rightIntersectionOutter, out middleIntersectionInner, out oppositeIntersectionInner);
        }

        /// <summary>
        /// Build the mesh for the Cross road
        /// </summary>
        public void Build()
        {
            CrossRoadsArray ja = BuildArray(true, null);

            _cornerDrawToLeft = new CornerDrawHolder(null, null);
            _cornerDrawToRight = new CornerDrawHolder(null, null);
            _cornerDrawToFarLeft = new CornerDrawHolder(null, null);
            _cornerDrawToFarRight = new CornerDrawHolder(null, null);

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

            _cornerDrawToLeft.DrawRoad();
            _cornerDrawToRight.DrawRoad();
            _cornerDrawToFarLeft.DrawRoad();
            _cornerDrawToFarRight.DrawRoad();

            CreateMeshFromArray(ja);
            _cornerDrawToLeft.DrawKerb();
            _cornerDrawToRight.DrawKerb();
            _cornerDrawToFarLeft.DrawKerb();
            _cornerDrawToFarRight.DrawKerb();

            _cornerDrawToLeft.DrawPavement();
            _cornerDrawToRight.DrawPavement();
            _cornerDrawToFarLeft.DrawPavement();
            _cornerDrawToFarRight.DrawPavement();
        }

        /// <summary>
        /// Apply the terrain height
        /// </summary>
        /// <param name="tm">The terrain modifier</param>
        public void ApplyTerrain(TerrainModifier tm)
        {
            CrossRoadsArray ja = BuildArray(false, tm);

            RectVector3 r;
            r = new RectVector3(ja.LeftSide[6], ja.LeftSide[7], ja.RightSide[6], ja.RightSide[7]);
            tm.ApplyToTerrain(r, true);

            if (UVDATA.JunctionAcrossUV)
            {
                // Main road section Left
                Vector3 posAMid = (ja.RightFarSide[5] + ja.LeftSide[5]) / 2;
                Vector3 posBMid = (ja.RightFarSide[6] + ja.LeftSide[6]) / 2;

                r = new RectVector3(posBMid, posAMid, ja.LeftSide[6], ja.LeftSide[5]);
                tm.ApplyToTerrain(r, true);

                r = new RectVector3(ja.RightFarSide[6], ja.RightFarSide[5], posBMid, posAMid);
                tm.ApplyToTerrain(r, true);
            }
            else
            {
                r = new RectVector3(ja.RightFarSide[6], ja.RightFarSide[5], ja.LeftSide[6], ja.LeftSide[5]);
                tm.ApplyToTerrain(r, true);
            }

            if (UVDATA.JunctionAcrossUV)
            {
                // Road section Right
                Vector3 posAMid = (ja.RightSide[5] + ja.LeftFarSide[5]) / 2;
                Vector3 posBMid = (ja.RightSide[6] + ja.LeftFarSide[6]) / 2;

                r = new RectVector3(ja.RightSide[6], ja.RightSide[5], posBMid, posAMid);
                tm.ApplyToTerrain(r, true);

                r = new RectVector3(posBMid, posAMid, ja.LeftFarSide[6], ja.LeftFarSide[5]);
                tm.ApplyToTerrain(r, true);
            }
            else
            {
                r = new RectVector3(ja.RightSide[6], ja.RightSide[5], ja.LeftFarSide[6], ja.LeftFarSide[5]);
                tm.ApplyToTerrain(r, true);
            }

            // Road section Middle
            r = new RectVector3(ja.LeftFarSide[6], ja.LeftFarSide[7], ja.RightFarSide[6], ja.RightFarSide[7]);
            tm.ApplyToTerrain(r, true);

            Vector3 posC = (ja.LeftFarSide[6] + ja.RightSide[6]) / 2;
            Vector3 posD = (ja.RightFarSide[6] + ja.LeftSide[6]) / 2;

            r = new RectVector3(posC, posD, ja.RightSide[6], ja.LeftSide[6]);
            tm.ApplyToTerrain(r, true);

            r = new RectVector3(ja.LeftFarSide[6], ja.RightFarSide[6], posC, posD);
            tm.ApplyToTerrain(r, true);

            // pavements
            r = new RectVector3(ja.LeftSide[1], ja.LeftSide[4], ja.LeftSide[2], ja.LeftSide[3]);
            tm.ApplyToTerrain(r, true);

            r = new RectVector3(ja.RightSide[1], ja.RightSide[4], ja.RightSide[2], ja.RightSide[3]);
            tm.ApplyToTerrain(r, true);

            r = new RectVector3(ja.LeftFarSide[1], ja.LeftFarSide[4], ja.LeftFarSide[2], ja.LeftFarSide[3]);
            tm.ApplyToTerrain(r, true);

            r = new RectVector3(ja.RightFarSide[1], ja.RightFarSide[4], ja.RightFarSide[2], ja.RightFarSide[3]);
            tm.ApplyToTerrain(r, true);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Build the array
        /// </summary>
        /// <param name="buildMesh">Weather to build the mesh or not</param>
        /// <param name="tm">The terrain modifier</param>
        /// <returns>The array details of the cross road</returns>
        private CrossRoadsArray BuildArray(bool buildMesh, TerrainModifier tm)
        {
            RoadCrossRoadOrder order;
            MeshBuilderSection mbs;
            RoadCrossSection leftIntersectionInner, leftIntersectionOutter, rightIntersectionInner, rightIntersectionOutter, middleIntersectionInner, oppositeIntersectionInner;
            UpdateRoadNodesEndPoints(out order, out mbs, out leftIntersectionInner, out leftIntersectionOutter, out rightIntersectionInner, out rightIntersectionOutter, out middleIntersectionInner, out oppositeIntersectionInner);

            if (buildMesh)
            {
                mbs.DirectBuild(leftIntersectionInner, order.LeftRoad);
                mbs.DirectBuild(rightIntersectionInner, order.RightRoad);

                mbs.DirectBuild(middleIntersectionInner, order.MiddleRoad);
                mbs.DirectBuild(oppositeIntersectionInner, order.OppositeRoad);
            }
            else
            {
                tm.ApplyToTerrain(leftIntersectionInner, order.LeftRoad);
                tm.ApplyToTerrain(rightIntersectionInner, order.RightRoad);

                tm.ApplyToTerrain(middleIntersectionInner, order.MiddleRoad);
                tm.ApplyToTerrain(oppositeIntersectionInner, order.OppositeRoad);
            }

            CrossRoadsArray ja = new CrossRoadsArray();
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

            ja.LeftFarSide[7] = oppositeIntersectionInner.Left;
            ja.LeftFarSide[4] = oppositeIntersectionInner.CurbLeftLip;
            ja.LeftFarSide[1] = oppositeIntersectionInner.CurbLeftEnd;
            ja.LeftFarSide[5] = rightIntersectionInner.Right;
            ja.LeftFarSide[2] = rightIntersectionInner.CurbRightLip;
            ja.LeftFarSide[6] = rightIntersectionOutter.Right;
            ja.LeftFarSide[3] = AddSlope(rightIntersectionOutter.CurbRightLip, oppositeIntersectionInner.Angle);

            ja.RightFarSide[7] = oppositeIntersectionInner.Right;
            ja.RightFarSide[4] = oppositeIntersectionInner.CurbRightLip;
            ja.RightFarSide[1] = oppositeIntersectionInner.CurbRightEnd;
            ja.RightFarSide[5] = leftIntersectionInner.Left;
            ja.RightFarSide[2] = leftIntersectionInner.CurbLeftLip;
            ja.RightFarSide[6] = leftIntersectionOutter.Left;
            ja.RightFarSide[3] = AddSlope(leftIntersectionOutter.CurbLeftLip, oppositeIntersectionInner.Angle - (float)(Mathf.PI));
            return ja;
        }

        /// <summary>
        /// Update all of the end points of the cross roads
        /// </summary>
        /// <param name="order">The orderd list of roads</param>
        /// <param name="mbs">The created mesg builder</param>
        /// <param name="leftIntersectionInner">The inner left cross section</param>
        /// <param name="leftIntersectionOutter">The outter left cross section</param>
        /// <param name="rightIntersectionInner">The inner right cross section</param>
        /// <param name="rightIntersectionOutter">The outter right cross section</param>
        /// <param name="middleIntersectionInner">The middle road cross sections</param>
        /// <param name="oppositeIntersectionInner">The opposite road cross sections</param>
        private void UpdateRoadNodesEndPoints(out RoadCrossRoadOrder order, out MeshBuilderSection mbs, out RoadCrossSection leftIntersectionInner, out RoadCrossSection leftIntersectionOutter, out RoadCrossSection rightIntersectionInner, out RoadCrossSection rightIntersectionOutter, out RoadCrossSection middleIntersectionInner, out RoadCrossSection oppositeIntersectionInner)
        {
            RoadNetworkNode roadA = _roadNetworkNode.Details.Roads[0];
            RoadNetworkNode roadB = _roadNetworkNode.Details.Roads[1];
            RoadNetworkNode roadC = _roadNetworkNode.Details.Roads[2];
            RoadNetworkNode roadD = _roadNetworkNode.Details.Roads[3];

            order = new RoadCrossRoadOrder(
                                    _list[0], roadA,
                                    _list[1], roadB,
                                    _list[2], roadC,
                                    _list[3], roadD);
            mbs = new MeshBuilderSection(_roadObject, null, _materialName, 0);

            // left road
            Vector3 posA;
            Vector3 posA2;
            Vector3 posB;
            FindOverlapPoint(order.MiddleRoad.Angle, order.LeftRoad.Angle, order.MiddleRoad.CurbLeftEnd, order.LeftRoad.CurbRightEnd, out posA);
            FindOverlapPoint(order.MiddleRoad.Angle, order.LeftRoad.Angle, order.MiddleRoad.Left, order.LeftRoad.CurbRightEnd, out posA2);
            FindOverlapPoint(order.OppositeRoad.Angle, order.LeftRoad.Angle, order.OppositeRoad.CurbRightEnd, order.LeftRoad.CurbLeftEnd, out posB);

            Vector3 curbToCurb = posA - posB;
            float newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) + (Mathf.PI / 2);
            leftIntersectionInner = order.LeftRoad.GetIntersection(newRoadAngle, posA);
            leftIntersectionOutter = order.LeftRoad.GetIntersection(newRoadAngle, posA2);
            FindOverlapPoint(order.MiddleRoad.Angle, order.RightRoad.Angle, order.MiddleRoad.CurbRightEnd, order.RightRoad.CurbLeftEnd, out posA);
            FindOverlapPoint(order.MiddleRoad.Angle, order.RightRoad.Angle, order.MiddleRoad.Right, order.RightRoad.CurbLeftEnd, out posA2);
            FindOverlapPoint(order.OppositeRoad.Angle, order.RightRoad.Angle, order.OppositeRoad.CurbLeftEnd, order.RightRoad.CurbRightEnd, out posB);
            curbToCurb = posB - posA;

            newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) + (Mathf.PI / 2);
            rightIntersectionInner = order.RightRoad.GetIntersection(newRoadAngle, posA);
            rightIntersectionOutter = order.RightRoad.GetIntersection(newRoadAngle, posA2);
            curbToCurb = rightIntersectionInner.CurbLeftEnd - leftIntersectionInner.CurbRightEnd;
            newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) + (Mathf.PI / 2);
            middleIntersectionInner = order.MiddleRoad.GetIntersection(newRoadAngle, rightIntersectionInner.CurbLeftEnd);
            curbToCurb = rightIntersectionInner.CurbRightEnd - leftIntersectionInner.CurbLeftEnd;
            newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) - (Mathf.PI / 2);

            oppositeIntersectionInner = order.OppositeRoad.GetIntersection(newRoadAngle, rightIntersectionInner.CurbRightEnd);

            // Store the ids so we know where to update them later
            Guid leftRoadNodeId = order.LeftRoad.ID;
            Guid rightRoadNodeId = order.RightRoad.ID;
            Guid middleRoadNodeId = order.MiddleRoad.ID;
            Guid oppositeRoadNodeId = order.OppositeRoad.ID;
            
            order.ReSetLeft(FindSmallestRoadForJunction(order.LeftRoadNode, leftIntersectionInner, order.LeftRoad.Middle));
            order.ReSetRight(FindSmallestRoadForJunction(order.RightRoadNode, rightIntersectionInner, order.RightRoad.Middle));
            order.ReSetMiddle(FindSmallestRoadForJunction(order.MiddleRoadNode, middleIntersectionInner, order.MiddleRoad.Middle));
            order.ReSetOpposite(FindSmallestRoadForJunction(order.OppositeRoadNode, oppositeIntersectionInner, order.OppositeRoad.Middle));

            IntersectionManager.Instance.SetIntersection(leftRoadNodeId, order.LeftRoad);
            IntersectionManager.Instance.SetIntersection(middleRoadNodeId, order.MiddleRoad);
            IntersectionManager.Instance.SetIntersection(rightRoadNodeId, order.RightRoad);
            IntersectionManager.Instance.SetIntersection(oppositeRoadNodeId, order.OppositeRoad);
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
        /// Create the mesh for the cross road
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        private void CreateMeshFromArray(CrossRoadsArray ja)
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
                Vector3 posAMid = (ja.RightFarSide[5] + ja.LeftSide[5]) / 2;
                Vector3 posBMid = (ja.RightFarSide[6] + ja.LeftSide[6]) / 2;

                DrawQuad(posBMid, posAMid, ja.LeftSide[6], ja.LeftSide[5],
                            new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionTopEnd),
                            new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionTopStart),
                            new Vector2(UVDATA.JunctionTopRightInner, UVDATA.JunctionTopEnd),
                            new Vector2(UVDATA.JunctionTopRightInner, UVDATA.JunctionTopStart));
                // otherside
                DrawQuad(ja.RightFarSide[6], ja.RightFarSide[5], posBMid, posAMid,
                            new Vector2(UVDATA.JunctionTopRightInner, UVDATA.JunctionBottomEnd),
                            new Vector2(UVDATA.JunctionTopRightInner, UVDATA.JunctionBottomStart),
                            new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionBottomEnd),
                            new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionBottomStart));
            }
            else
            {
                DrawQuad(ja.RightFarSide[6], ja.RightFarSide[5], ja.LeftSide[6], ja.LeftSide[5],
                          new Vector2(UVDATA.JunctionTopEnd, UVDATA.JunctionTopLeftInner),
                          new Vector2(UVDATA.JunctionTopStart, UVDATA.JunctionTopLeftInner),
                          new Vector2(UVDATA.JunctionTopEnd, UVDATA.JunctionTopRightInner),
                          new Vector2(UVDATA.JunctionTopStart, UVDATA.JunctionTopRightInner));
            }

            if (UVDATA.JunctionAcrossUV)
            {
                // Road section Right
                Vector3 posAMid = (ja.RightSide[5] + ja.LeftFarSide[5]) / 2;
                Vector3 posBMid = (ja.RightSide[6] + ja.LeftFarSide[6]) / 2;

                DrawQuad(ja.RightSide[6], ja.RightSide[5], posBMid, posAMid,
                            new Vector2(UVDATA.JunctionBottomLeftInner, UVDATA.JunctionBottomEnd),
                            new Vector2(UVDATA.JunctionBottomLeftInner, UVDATA.JunctionBottomStart),
                            new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionBottomEnd),
                            new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionBottomStart));

                DrawQuad(posBMid, posAMid, ja.LeftFarSide[6], ja.LeftFarSide[5],
                           new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionTopEnd),
                           new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.JunctionTopStart),
                           new Vector2(UVDATA.JunctionBottomLeftInner, UVDATA.JunctionTopEnd),
                           new Vector2(UVDATA.JunctionBottomLeftInner, UVDATA.JunctionTopStart));
            }
            else
            {
                DrawQuad(ja.RightSide[6], ja.RightSide[5], ja.LeftFarSide[6], ja.LeftFarSide[5],
                            new Vector2(UVDATA.JunctionBottomEnd, UVDATA.JunctionBottomRightInner),
                            new Vector2(UVDATA.JunctionBottomStart, UVDATA.JunctionBottomRightInner),
                            new Vector2(UVDATA.JunctionBottomEnd, UVDATA.JunctionBottomLeftInner),
                            new Vector2(UVDATA.JunctionBottomStart, UVDATA.JunctionBottomLeftInner));
            }

            // Road section Middle
            DrawQuad(ja.LeftFarSide[6], ja.LeftFarSide[7], ja.RightFarSide[6], ja.RightFarSide[7],
                        new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftLipInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                        new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightLipInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner));

            Vector3 posC = (ja.LeftFarSide[6] + ja.RightSide[6]) / 2;
            Vector3 posD = (ja.RightFarSide[6] + ja.LeftSide[6]) / 2;

            DrawQuad(posC, posD, ja.RightSide[6], ja.LeftSide[6],
               new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.CurbRightLipInner),
               new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.CurbLeftLipInner),
               new Vector2(UVDATA.JunctionIntersectionStart, UVDATA.CurbRightLipInner),
               new Vector2(UVDATA.JunctionIntersectionStart, UVDATA.CurbLeftLipInner));

            DrawQuad(ja.LeftFarSide[6], ja.RightFarSide[6], posC, posD,
               new Vector2(UVDATA.JunctionIntersectionStart, UVDATA.CurbLeftLipInner),
               new Vector2(UVDATA.JunctionIntersectionStart, UVDATA.CurbRightLipInner),
               new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.CurbLeftLipInner),
               new Vector2(UVDATA.JunctionIntersectionMiddle, UVDATA.CurbRightLipInner));
        }

        /// <summary>
        /// Create the Curb and pavement mesh if needed
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">the leading stright before the curve starts</param>
        private void CreateCurbAndPavementCurve(CrossRoadsArray ja, float leadingStright)
        {
            int sections = _roadNetworkNode.Details.Sections;

            _cornerDrawToLeft = CreateLeftCorner(ja, leadingStright, sections);
            _cornerDrawToRight = CreateRightCorner(ja, leadingStright, sections);

            _cornerDrawToFarLeft = CreateLeftFarCorner(ja, leadingStright, sections);
            _cornerDrawToFarRight = CreateRightFarCorner(ja, leadingStright, sections);
        }

        /// <summary>
        /// Create the right hand corner
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        private CornerDrawHolder CreateRightCorner(CrossRoadsArray ja, float leadingStright, int sections)
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
        /// Create the right hand corner
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        private CornerDrawHolder CreateRightFarCorner(CrossRoadsArray ja, float leadingStright, int sections)
        {
            SmotherCornerContext scc = new SmotherCornerContext();
            scc.Main.Vector = ja.RightFarSide[1];
            scc.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter);

            scc.Leading.Vector = ja.RightFarSide[2];
            scc.Leading.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightOutter);

            scc.Ending.Vector = ja.RightFarSide[4];
            scc.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner);

            scc.Far.Vector = ja.RightFarSide[3];
            scc.Far.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner);

            List<VectorUvs> pavementTriList = CreateFanfrom(scc, leadingStright, sections);

            // Draw the road
            SmotherCornerContext sccRoad = new SmotherCornerContext();
            Vector3 kerb = new Vector3(0, RoadConstructorHelper.CrossSectionDetails.CurbLipHeightValue, 0);
            sccRoad.Main.Vector = ja.RightFarSide[1] - kerb;
            sccRoad.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter);

            sccRoad.Leading.Vector = ja.RightFarSide[5];
            sccRoad.Leading.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightOutter);

            sccRoad.Ending.Vector = ja.RightFarSide[7];
            sccRoad.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner);

            sccRoad.Far.Vector = ja.RightFarSide[6];
            sccRoad.Far.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightLipInner);

            List<VectorUvs> roadTriList = CreateFanfrom(sccRoad, leadingStright, sections);
            // swap from the inner curb to the outer curb for the road
            roadTriList[0] = sccRoad.Far;

            CornerDrawHolder cdh = new CornerDrawHolder(pavementTriList, roadTriList);
            cdh.DrawPavementImpl = DrawFanTriListBackWards;
            cdh.DrawRoadImpl = DrawFanTriList;

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                cdh.SetKerb(pavementTriList, roadTriList, 1);
                cdh.DrawKerbImpl = DrawStripBackward;
            }
            return cdh;
        }

        /// <summary>
        /// Creates the drawer for the left corner
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        /// <returns>The drawer object to draw the left corner</returns>
        private CornerDrawHolder CreateLeftCorner(CrossRoadsArray ja, float leadingStright, int sections)
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

        /// <summary>
        /// Creates the drawer for the far left corner
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        /// <returns>The drawer object to draw the far left corner</returns>
        private CornerDrawHolder CreateLeftFarCorner(CrossRoadsArray ja, float leadingStright, int sections)
        {
            SmotherCornerContext scc = new SmotherCornerContext();
            scc.Main.Vector = ja.LeftFarSide[1];
            scc.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter);

            scc.Leading.Vector = ja.LeftFarSide[2];
            scc.Leading.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftOutter);

            scc.Ending.Vector = ja.LeftFarSide[4];
            scc.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner);

            scc.Far.Vector = ja.LeftFarSide[3];
            scc.Far.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner);

            List<VectorUvs> pavementTriList = CreateFanfrom(scc, leadingStright, sections);

            // Draw the road
            SmotherCornerContext sccRoad = new SmotherCornerContext();
            Vector3 kerb = new Vector3(0, RoadConstructorHelper.CrossSectionDetails.CurbLipHeightValue, 0);
            sccRoad.Main.Vector = ja.LeftFarSide[1] - kerb;
            sccRoad.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter);

            sccRoad.Leading.Vector = ja.LeftFarSide[5];
            sccRoad.Leading.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftOutter);

            sccRoad.Ending.Vector = ja.LeftFarSide[7];
            sccRoad.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner);

            sccRoad.Far.Vector = ja.LeftFarSide[6];
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

        /// <summary>
        /// Create the Curb and pavement mesh if needed
        /// </summary>
        /// <param name="ja">The array that holder all the points</param>
        private void CreateCurbAndPavement(CrossRoadsArray ja)
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
                // left hand top corner
                DrawQuad(ja.LeftFarSide[5], ja.LeftFarSide[2], ja.LeftFarSide[6], ja.LeftFarSide[3],
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));

                DrawQuad(ja.LeftFarSide[4], ja.LeftFarSide[7], ja.LeftFarSide[3], ja.LeftFarSide[6],
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftLipInner));
            }

            DrawTri(ja.LeftFarSide[1], ja.LeftFarSide[4], ja.LeftFarSide[3],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));

            DrawTri(ja.LeftFarSide[1], ja.LeftFarSide[3], ja.LeftFarSide[2],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner));

            if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
            {
                // Right hand top corner
                DrawQuad(ja.RightFarSide[6], ja.RightFarSide[3], ja.RightFarSide[5], ja.RightFarSide[2],
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner));

                DrawQuad(ja.RightFarSide[3], ja.RightFarSide[6], ja.RightFarSide[4], ja.RightFarSide[7],
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner));
            }

            DrawTri(ja.RightFarSide[1], ja.RightFarSide[3], ja.RightFarSide[4],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner));

            DrawTri(ja.RightFarSide[1], ja.RightFarSide[2], ja.RightFarSide[3],
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightOutter),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner));
        }

        /// <summary>
        /// Find the overlap point of two angles
        /// </summary>
        /// <param name="angleA">Angle a</param>
        /// <param name="angleB">angle B</param>
        /// <param name="posA">Starting point A</param>
        /// <param name="posB">Starting point B</param>
        /// <param name="retPos">The overlap point of the two angles</param>
        /// <returns>The magnitude of the point</returns>
        private static float FindOverlapPoint(float angleA, float angleB, Vector3 posA, Vector3 posB, out Vector3 retPos)
        {
            Vector3 offSetPosA = MathsHelper.OffSetVector(posA, angleA - (float)(Math.PI / 2), 50);
            Vector3 offSetPosB = MathsHelper.OffSetVector(posB, angleB - (float)(Math.PI / 2), 50);
            Vector3 outB = new Vector3();
            Vector3 diffA = offSetPosA - posA;
            Vector3 diffB = offSetPosB - posB;
            MathsHelper.ClosestPointsOnTwoLines(out retPos, out outB, posA, diffA.normalized, posB, diffB.normalized);
            Vector3 gap = retPos - posA;
            return gap.magnitude;
        }
        #endregion

        #region Private Fields
        private CornerDrawHolder _cornerDrawToLeft;
        private CornerDrawHolder _cornerDrawToRight;
        private CornerDrawHolder _cornerDrawToFarLeft;
        private CornerDrawHolder _cornerDrawToFarRight;
        #endregion

        #region Helper Class
        /// <summary>
        /// Holder class to holder all the point for the junction mesh
        /// </summary>
        private class CrossRoadsArray
        {
            public Vector3[] LeftSide = new Vector3[8];
            public Vector3[] RightSide = new Vector3[8];

            public Vector3[] LeftFarSide = new Vector3[8];
            public Vector3[] RightFarSide = new Vector3[8];
        }
        #endregion
    }
}