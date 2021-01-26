using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Mesh builder for the five roads junctions
    /// </summary>
    public class MeshBuilderFiveRoad : MeshBuilderBase
    {
        /// <summary>
        /// Standarad Constructor
        /// </summary>
        /// <param name="roadObject">The base road object</param>
        /// <param name="list">The list of guids for the cross roads</param>
        /// <param name="roadNetworkNode"></param>
        /// <param name="materialName"></param>
        public MeshBuilderFiveRoad(IRoadBuildData roadObject, List<Guid> list, RoadNetworkNode roadNetworkNode, string materialName)
        {
            _roadObject = roadObject;
            _list = list;
            _roadNetworkNode = roadNetworkNode;
            _materialName = materialName;
            _totalRoads = list.Count;
        }

        #region Public Methods
        /// <summary>
        /// Update the end points
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        public void UpdateEndPoints()
        {
            RoadFiveRoadOrder order;
            MeshBuilderSection mbs;
            OutterInner outterInner;
            UpdateRoadNodesEndPoints(out order, out mbs, out outterInner);
        }

        /// <summary>
        /// Build the mesh for the Cross road
        /// </summary>
        public void Build()
        {
            OutterInner outterInner = BuildArray(true, null);
            _corner = new CornerDrawHolder[_totalRoads];
            for (int i = 0; i < _totalRoads; i++)
            {
                _corner[i] = new CornerDrawHolder(null, null);
            }

            if (RoadConstructorHelper.CrossSectionDetails.WithCurbValue)
            {
                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavement)
                    CreateCurbAndPavement(outterInner);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementExtended)
                    CreateCurbAndPavement(outterInner);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementInnerCurveA)
                    CreateCurbAndPavementCurve(outterInner, 0.1f);
                
                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementInnerCurveB)
                    CreateCurbAndPavementCurve(outterInner, 0.0f);

                if (RoadConstructorHelper.RoadUVSet == UV_SET.RoadPavementInnerCurveC)
                    CreateCurbAndPavementCurve(outterInner, 0.5f);
            }

            for (int i = 0; i < _totalRoads; i++)
            {
                _corner[i].DrawRoad();
            }

            DrawMiddleRoadSection(outterInner);

            for (int i = 0; i < _totalRoads; i++)
            {
                _corner[i].DrawKerb();
            }

            for (int i = 0; i < _totalRoads; i++)
            {
                _corner[i].DrawPavement();
            }

            for (int i = 0; i < _totalRoads; i++)
            {
                DrawMiddleFive(outterInner, i);
            }
        }

        /// <summary>
        /// Draw the middle road section
        /// </summary>
        /// <param name="oi">The array of outter and inner cross sections</param>
        private void DrawMiddleRoadSection(OutterInner oi)
        {
            for (int i = 0; i < _totalRoads; i++)
            {
                DrawQuad(oi.Inner[i].Left, oi.Outter[i].Left, oi.Inner[i].Right, oi.Outter[i].Right,
                            new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                            new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbRightLipInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner));
            }
        }

        /// <summary>
        /// Apply the terrain height
        /// </summary>
        /// <param name="tm">The terrain modifier</param>
        public void ApplyTerrain(TerrainModifier tm)
        {
            OutterInner outterInner = BuildArray(false, tm);

            for (int i = 0; i < _totalRoads; i++)
            {
                int rightRoad = i + 1;
                if (rightRoad > _totalRoads - 1)
                    rightRoad -= _totalRoads;

                int leftRoad = i - 1;
                if (leftRoad < 0)
                    leftRoad += _totalRoads;

                RectVector3 r;
                r = new RectVector3(outterInner.Inner[i].Left, outterInner.Outter[i].Left, outterInner.Inner[i].Right, outterInner.Outter[i].Right);
                tm.ApplyToTerrain(r, true);

                r = new RectVector3(outterInner.Outter[i].CurbRightEnd, outterInner.Outter[i].Right, outterInner.Outter[rightRoad].CurbLeftEnd, outterInner.Inner[i].Right);
                tm.ApplyToTerrain(r, true);

                r = new RectVector3(outterInner.Outter[i].CurbLeftEnd, outterInner.Outter[i].Left, outterInner.Outter[leftRoad].CurbRightEnd, outterInner.Inner[i].Left);
                tm.ApplyToTerrain(r, true);

                r = new RectVector3(_roadNetworkNode.transform.position, _roadNetworkNode.transform.position, outterInner.Inner[i].Left, outterInner.Inner[i].Right);
                tm.ApplyToTerrain(r, true);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Build the array
        /// </summary>
        /// <param name="buildMesh">Weather to build the mesh or not</param>
        /// <param name="tm">The terrain modifier</param>
        /// <returns>The array of outter and inner cross sections</returns>
        private OutterInner BuildArray(bool buildMesh, TerrainModifier tm)
        {
            RoadFiveRoadOrder order;
            MeshBuilderSection mbs;
            OutterInner outterInner;
            UpdateRoadNodesEndPoints(out order, out mbs, out outterInner);

            if (buildMesh)
            {
                for (int i = 0; i < _totalRoads; i++)
                {
                    mbs.DirectBuildDynamicTextureLength(outterInner.Outter[i], order.Road(i));
                }
            }
            else
            {
                for (int i = 0; i < _totalRoads; i++)
                {
                    tm.ApplyToTerrain(outterInner.Outter[i], order.Road(i));
                }
            }
            return outterInner;
        }

        /// <summary>
        /// Update all of the end points of the cross roads
        /// </summary>
        /// <param name="order">The orderd list of roads</param>
        /// <param name="mbs">The created mesg builder</param>
        /// <param name="outterInner">Holds the list of outter and inner cross sections</param>
        private void UpdateRoadNodesEndPoints(out RoadFiveRoadOrder order, out MeshBuilderSection mbs, out OutterInner outterInner)
        {
            outterInner = new OutterInner();
            order = new RoadFiveRoadOrder(_list, _roadNetworkNode.Details.Roads);
            
            mbs = new MeshBuilderSection(_roadObject, null, _materialName, 0);

            for (int i = 0; i < _totalRoads; i++)
            {
                outterInner.Outter[i] = GetOutter(order, i);
                outterInner.Inner[i] = GetInner(order, i);
            }

            // Store the ids so we know where to update them later
            for (int i = 0; i < _totalRoads; i++)
            {
                Guid id = order.Road(i).ID;
                order.ReSet(i, FindSmallestRoadForJunction(order.RoadNode(i), outterInner.Outter[i], order.Road(i).Middle));
                IntersectionManager.Instance.SetIntersection(id, order.Road(i));
            }          
        }

        /// <summary>
        /// Get the sortest outter cross for the roads
        /// </summary>
        /// <param name="order">The road order list</param>
        /// <param name="mainRoad">The road to get the middle of</param>
        /// <returns>The cross section of the road</returns>
        private RoadCrossSection GetOutter(RoadFiveRoadOrder order, int mainRoad)
        {
            int rightRoad = order.GetNextRightIndex(mainRoad);
            int leftRoad = order.GetNextLeftIndex(mainRoad);

            Vector3 posLeft;
            Vector3 posRight;
            FindOverlapPoint(order.Road(mainRoad).Angle, order.Road(rightRoad).Angle, order.Road(mainRoad).CurbRightEnd, order.Road(rightRoad).CurbLeftEnd, out posLeft);
            FindOverlapPoint(order.Road(leftRoad).Angle, order.Road(mainRoad).Angle, order.Road(leftRoad).CurbRightEnd, order.Road(mainRoad).CurbLeftEnd, out posRight);

            Vector3 curbToCurb = posLeft - posRight;
            float newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) + (Mathf.PI / 2);
            return order.Road(mainRoad).GetIntersection(newRoadAngle, posLeft);
        }

        /// <summary>
        /// Get the sortest inner cross for the roads
        /// </summary>
        /// <param name="order">The road order list</param>
        /// <param name="mainRoad">The road to get the middle of</param>
        /// <returns>The cross section of the road</returns>
        private RoadCrossSection GetInner(RoadFiveRoadOrder order, int mainRoad)
        {
            int rightRoad = order.GetNextRightIndex(mainRoad);
            int leftRoad = order.GetNextLeftIndex(mainRoad);

            Vector3 posLeft;
            Vector3 posRight;
            FindOverlapPoint(order.Road(mainRoad).Angle, order.Road(rightRoad).Angle, order.Road(mainRoad).Right, order.Road(rightRoad).Left, out posLeft);
            FindOverlapPoint(order.Road(leftRoad).Angle, order.Road(mainRoad).Angle, order.Road(leftRoad).Right, order.Road(mainRoad).Left, out posRight);

            Vector3 curbToCurb = posLeft - posRight;
            float newRoadAngle = -MathsHelper.GetAngleFrom(curbToCurb.z, curbToCurb.x) + (Mathf.PI / 2);
            return order.Road(mainRoad).GetIntersection(newRoadAngle, posLeft);
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
        /// <param name="io">The array that holder all the points</param>
        /// <param name="leadingStright">the leading stright before the curve starts</param>
        private void CreateCurbAndPavementCurve(OutterInner io, float leadingStright)
        {
            int sections = _roadNetworkNode.Details.Sections;

            for (int i = 0; i < _totalRoads; i++)
            {
                int rightRoad = i + 1;
                if (rightRoad > _totalRoads-1)
                    rightRoad -= _totalRoads;

                _corner[i] = CreateInnerCorner(io, leadingStright, sections, i, rightRoad);
            }
        }

        /// <summary>
        /// Creates the drawer for the left corner
        /// </summary>
        /// <param name="oi">The array of outter and inner cross sections</param>
        /// <param name="leadingStright">The leading section on the corner</param>
        /// <param name="sections">the number of sections with-in a curve</param>
        /// <param name="mainRoad">The index of the main road</param>
        /// <param name="rightRoad">The index of the road to the right</param>
        /// <returns>The drawer object to draw the left corner</returns>
        private CornerDrawHolder CreateInnerCorner(OutterInner oi, float leadingStright, int sections, int mainRoad, int rightRoad)
        {
            SmotherCornerContext scc = new SmotherCornerContext();
            scc.Main.Vector = oi.Outter[mainRoad].CurbRightEnd;
            scc.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter);

            scc.Leading.Vector = oi.Outter[mainRoad].CurbRightLip;
            scc.Leading.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftOutter);

            scc.Ending.Vector = oi.Outter[rightRoad].CurbLeftLip;
            scc.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner);

            scc.Far.Vector = oi.Inner[mainRoad].CurbRightLip;
            scc.Far.UV = new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner);

            List<VectorUvs> pavementTriList = CreateFanfrom(scc, leadingStright, sections);

            // Draw the road
            SmotherCornerContext sccRoad = new SmotherCornerContext();
            Vector3 kerb = new Vector3(0, RoadConstructorHelper.CrossSectionDetails.CurbLipHeightValue, 0);
            sccRoad.Main.Vector = oi.Outter[mainRoad].CurbRightEnd - kerb;
            sccRoad.Main.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter);

            sccRoad.Leading.Vector = oi.Outter[mainRoad].Right;
            sccRoad.Leading.UV = new Vector2(UVDATA.JunctionLengthKerb, UVDATA.CurbLeftOutter);

            sccRoad.Ending.Vector = oi.Outter[rightRoad].Left;
            sccRoad.Ending.UV = new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner);

            sccRoad.Far.Vector = oi.Inner[mainRoad].Right;
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
        /// <param name="oi">The array of outter and inner cross sections</param>
        private void CreateCurbAndPavement(OutterInner oi)
        {
            for (int i = 0; i < _totalRoads; i++)
            {
                if (RoadConstructorHelper.CrossSectionDetails.HasCurbDataValue)
                {
                    // left hand bottom corner
                    DrawQuad(oi.Outter[i].Right, oi.Outter[i].CurbRightLip, oi.Inner[i].Right, oi.Inner[i].CurbRightLip,
                                new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightLipInner),
                                new Vector2(UVDATA.JunctionStart, UVDATA.CurbRightInner),
                                new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightLipInner),
                                new Vector2(UVDATA.JunctionLength, UVDATA.CurbRightInner));

                    DrawQuad(oi.Outter[i].Left, oi.Inner[i].Left, oi.Outter[i].CurbLeftLip, oi.Inner[i].CurbLeftLip,
                               new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                               new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftLipInner),
                               new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));
                }

                DrawTri(oi.Outter[i].CurbRightEnd, oi.Inner[i].CurbRightLip, oi.Outter[i].CurbRightLip,
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter),
                            new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner),
                            new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner)
                            );

                DrawTri(oi.Outter[i].CurbLeftEnd, oi.Outter[i].CurbLeftLip, oi.Inner[i].CurbLeftLip,
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftOutter),
                        new Vector2(UVDATA.JunctionStart, UVDATA.CurbLeftInner),
                        new Vector2(UVDATA.JunctionLength, UVDATA.CurbLeftInner));
            }
        }

        /// <summary>
        /// Draw the middle of the junction
        /// </summary>
        /// <param name="oi">The array of outter and inner cross sections</param>
        /// <param name="index">The index of the road to draw</param>
        private void DrawMiddleFive(OutterInner oi, int index)
        {
            float inner = UVDATA.JunctionBottomLeftInner + 0.1f;
            float outter = (UVDATA.JunctionBottomLeftInner + UVDATA.JunctionBottomRightInner)/2 - 0.1f;

            DrawTri(_roadNetworkNode.transform.position, oi.Inner[index].Left, oi.Inner[index].Middle,
                   new Vector2(inner, 0),
                   new Vector2(inner, 0.5f),
                   new Vector2(outter, 0));

            DrawTri(_roadNetworkNode.transform.position, oi.Inner[index].Middle, oi.Inner[index].Right,
                   new Vector2(inner, 0),
                   new Vector2(outter, 0),
                   new Vector2(inner, 0.5f));
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
        private CornerDrawHolder[] _corner;
        private int _totalRoads = 5;
        #endregion
    }
}