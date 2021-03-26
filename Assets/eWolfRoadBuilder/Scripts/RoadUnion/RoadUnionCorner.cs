using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The road Corner union
    /// </summary>
    public class RoadUnionCorner : IRoadUnion
    {
        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="node">The road network node</param>
        public RoadUnionCorner(RoadNetworkNode rnn)
        {
            _roadNetworkNode = rnn;
        }

        #region Public Methods
        /// <summary>
        /// Gets the connection set index
        /// </summary>
        public int ConnectionSetIndex
        {
            get
            {
                return _connectionSet;
            }
        }

        /// <summary>
        /// Created the intersections of the roads
        /// </summary>
        /// <param name="roadBuilderObject">The road builder object</param>
        public void CreateLayout(RoadBuilder roadBuilderObject)
        {
            if (_roadNetworkNode.Details.Roads[0] == null)
                return;
            if (_roadNetworkNode.Details.Roads[1] == null)
                return;

            CreateCorner(roadBuilderObject, _roadNetworkNode.Details.Sections);
        }

        /// <summary>
        /// Update the layout for all the streets
        /// </summary>
        /// <param name="roadBuilderObject"></param>
        public void CreateStreetLayout(RoadBuilder roadBuilderObject)
        {
            RoadNetworkNodeHelper.CreateAllStreets(_roadNetworkNode);
        }

        /// <summary>
        /// Create the mesh for this Union and streets
        /// </summary>
        /// <param name="roadBuilderObject">The object to update the mesh for</param>
        public void CreateMesh(IRoadBuildData roadBuilderObject)
        {
            _meshSection.CreateMesh(roadBuilderObject);
            RoadNetworkNodeHelper.MeshStreets(roadBuilderObject, _streetNames);
        }

        /// <summary>
        /// Add the name of the street to the list
        /// </summary>
        /// <param name="streetFullName">The street name to add</param>
        public void AddStreetList(string streetFullName)
        {
            _streetNames.Add(streetFullName);
        }

        /// <summary>
		/// Apply to terrain
		/// </summary>
		/// <param name="TerrainModifier">The Terrain Modifier helper</param>
        public void ModifiyTerrain(TerrainModifier tm)
        {
            CreateCornerTerrain(_roadNetworkNode.Details.Sections, tm);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the length of the road
        /// </summary>
        /// <param name="index">Index of the road</param>
        /// <returns>The length of the road</returns>
        private float GetLengthOfRoad(int index)
        {
            GameObject StartObj = _roadNetworkNode.gameObject;
            GameObject EndObj = _roadNetworkNode.Details.Roads[index].gameObject;

            Vector3 diff = StartObj.transform.position - EndObj.transform.position;
            return diff.magnitude;
        }

        /// <summary>
        /// Create the road object
        /// </summary>
        /// <param name="roadObject">The base object to add the road to</param>
        /// <param name="sections">The number of sections to use for this road</param>
        private void CreateCorner(RoadBuilder roadObject, int sections)
        {
            // find the point where the two roads meet
            Vector3 pos = _roadNetworkNode.gameObject.transform.position;

            // create a corne at this postion
            Vector3 newpos = pos;

            float roadAngleA = RoadUnionHelper.AngleClamp(RoadUnionHelper.GetAngleOfRoadClampped(_roadNetworkNode, 0) - (Mathf.PI / 2));
            float roadAngleB = RoadUnionHelper.AngleClamp(RoadUnionHelper.GetAngleOfRoadClampped(_roadNetworkNode, 1) + (Mathf.PI / 2));
            float roadAngleDifference = roadAngleB - roadAngleA;
            float couveSize = 1.85f;
            float x, z;

            float road_A_length = GetLengthOfRoad(0);
            float road_B_length = GetLengthOfRoad(1);

            float minLength = Mathf.Min(road_A_length, road_B_length);

            float offSetDownRoad = RoadConstructorHelper.CrossSectionDetails.RoadWidthValue * couveSize;
            if (offSetDownRoad > minLength / 2)
                offSetDownRoad = minLength / 2;

            RoadNetworkNode oppositeEnd = _roadNetworkNode.Details.Roads[1].GetComponent<RoadNetworkNode>();
            Vector3 roadPointA = oppositeEnd.GetOffSetDownRoad(pos, (offSetDownRoad));
            Vector3 outA;
            bool offSetPos = _roadNetworkNode.GetInnerCorner(0, 1, (offSetDownRoad), out outA);
            Vector3 CornerPoint = outA;
            newpos = CornerPoint;

            // get the gap form point to point
            Vector3 gap = CornerPoint - roadPointA;
            couveSize = offSetPos ? gap.magnitude : 0;
            couveSize = offSetPos ? gap.magnitude : 0;
            bool backward = false;

            Radian currentAngle = new Radian(roadAngleA - (float)(Math.PI / 2));
            roadAngleDifference = MathsHelper.ClampAngle(roadAngleDifference);
            if (roadAngleDifference > Mathf.PI)
            {
                roadAngleDifference = (Mathf.PI * 2) - roadAngleDifference;
                currentAngle = new Radian(roadAngleB + (float)(Math.PI / 2));
                backward = true;
            }

            float diff = roadAngleDifference;
            x = Mathf.Sin(currentAngle.Value) * (couveSize);
            z = Mathf.Cos(currentAngle.Value) * (couveSize);

            newpos.x -= x;
            newpos.z += z;

            ICrossSection crossSectionMiddle = RoadConstructorHelper.CrossSection(_roadNetworkNode);
            IMaterialFrequency materialFrequency = _roadNetworkNode.gameObject.GetComponent<IMaterialFrequency>();
            if (materialFrequency == null)
            {
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;
            }

            RoadCrossSection rA = new RoadCrossSection(newpos, currentAngle.Value, crossSectionMiddle, materialFrequency);
            RoadCrossSection Start = rA;

            float angleStep = Mathf.Abs(diff / sections);
            for (int i = 0; i < sections; i++)
            {
                newpos = CornerPoint;
                currentAngle.Value += angleStep;

                x = Mathf.Sin(currentAngle.Value) * (couveSize);
                z = Mathf.Cos(currentAngle.Value) * (couveSize);
                newpos.x -= x;
                newpos.z += z;

                RoadCrossSection rB = new RoadCrossSection(newpos, currentAngle.Value, crossSectionMiddle, materialFrequency);
                // TODO Sub divide corners - need to update before the add basic road!
                _meshSection.AddBasicRoad(IntersectionManager.Instance.AddLinkedIntersecions(rA, rB), RoadConstructorHelper.GetMainMaterial(materialFrequency), 0);
                rA = rB;
            }

            RoadCrossSection End = rA;
            if (backward)
            {
                StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name, crossSectionMiddle, materialFrequency, End);
                StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[1].name, crossSectionMiddle, materialFrequency, Start);
            }
            else
            {
                StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[1].name, crossSectionMiddle, materialFrequency, End);
                StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name, crossSectionMiddle, materialFrequency, Start);
            }
        }

        /// <summary>
        /// Modify the terrain for the corner
        /// </summary>
        /// <param name="sections">The number of sections</param>
        /// <param name="tm">The terrain modifier</param>
        private void CreateCornerTerrain(int sections, TerrainModifier tm)
        {
            // this is not the best way - but it's a start
            // maybe have a function that returns a list of cross sections

            // find the point where the two roads meet
            Vector3 pos = _roadNetworkNode.gameObject.transform.position;

            // create a corne at this postion
            Vector3 newpos = pos;

            float roadAngleA = RoadUnionHelper.AngleClamp(RoadUnionHelper.GetAngleOfRoadClampped(_roadNetworkNode, 0) - Mathf.PI / 2);
            float roadAngleB = RoadUnionHelper.AngleClamp(RoadUnionHelper.GetAngleOfRoadClampped(_roadNetworkNode, 1) + Mathf.PI / 2);
            float roadAngleDifference = roadAngleB - roadAngleA;
            float couveSize = 1.85f;
            float x, z;

            float road_A_length = GetLengthOfRoad(0);
            float road_B_length = GetLengthOfRoad(1);

            float minLength = Mathf.Min(road_A_length, road_B_length);

            float offSetDownRoad = RoadConstructorHelper.CrossSectionDetails.RoadWidthValue * couveSize;
            if (offSetDownRoad > minLength / 2)
                offSetDownRoad = minLength / 2;

            RoadNetworkNode oppositeEnd = _roadNetworkNode.Details.Roads[1].GetComponent<RoadNetworkNode>();
            Vector3 roadPointA = oppositeEnd.GetOffSetDownRoad(pos, (offSetDownRoad));
            Vector3 outA;
            bool offSetPos = _roadNetworkNode.GetInnerCorner(0, 1, (offSetDownRoad), out outA);
            Vector3 CornerPoint = outA;
            newpos = CornerPoint;

            // get the gap form point to point
            Vector3 gap = CornerPoint - roadPointA;
            couveSize = offSetPos ? gap.magnitude : 0;
            couveSize = offSetPos ? gap.magnitude : 0;

            Radian currentAngle = new Radian(roadAngleA - (float)(Math.PI / 2));
            roadAngleDifference = MathsHelper.ClampAngle(roadAngleDifference);
            if (roadAngleDifference > Mathf.PI)
            {
                roadAngleDifference = (Mathf.PI * 2) - roadAngleDifference;
                currentAngle = new Radian(roadAngleB + (float)(Math.PI / 2));
            }

            float diff = roadAngleDifference;
            x = Mathf.Sin(currentAngle.Value) * (couveSize);
            z = Mathf.Cos(currentAngle.Value) * (couveSize);

            newpos.x -= x;
            newpos.z += z;

            ICrossSection crossSectionMiddle = RoadConstructorHelper.CrossSection(_roadNetworkNode);
            IMaterialFrequency materialFrequency = _roadNetworkNode.gameObject.GetComponent<IMaterialFrequency>();
            if (materialFrequency == null)
            {
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;
            }

            RoadCrossSection rA = new RoadCrossSection(newpos, currentAngle.Value, crossSectionMiddle, materialFrequency);

            float angleStep = Mathf.Abs(diff / sections);
            for (int i = 0; i < sections; i++)
            {
                newpos = CornerPoint;
                currentAngle.Value += angleStep;

                x = Mathf.Sin(currentAngle.Value) * (couveSize);
                z = Mathf.Cos(currentAngle.Value) * (couveSize);
                newpos.x -= x;
                newpos.z += z;

                RoadCrossSection rB = new RoadCrossSection(newpos, currentAngle.Value, crossSectionMiddle, materialFrequency);
                tm.ApplyToTerrain(rA, rB);
                rA = rB;
            }

            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 0);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 1);
        }
        #endregion

        #region Private Fields
        /// <summary>
        /// The road network parent
        /// </summary>
        private RoadNetworkNode _roadNetworkNode;

        /// <summary>
        /// The index of the connect set
        /// </summary>
        private int _connectionSet = -1;

        /// <summary>
        /// The list of steet names for this node
        /// </summary>
        private List<string> _streetNames = new List<string>();

        /// <summary>
        /// The mesh section
        /// </summary>
		private MeshSectionDetails _meshSection = new MeshSectionDetails();
        #endregion
    }
}