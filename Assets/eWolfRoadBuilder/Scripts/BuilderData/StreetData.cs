using System;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The steet data class
    /// </summary>
    public class StreetData
    {
        #region Constructor
        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="id">The id of the first road</param>
        /// <param name="crossSection">The cross section details</param>
        /// <param name="mf">The materials frequency</param>
        public StreetData(RoadCrossSection id, ICrossSection crossSection, IMaterialFrequency mf)
        {
            _startRoadId = id;
            _startCrossSection = crossSection;
            _materialFrequency = mf;
        }
        #endregion

        /// <summary>
        /// Get set if we have used this street for a node
        /// </summary>
        public bool Used
        {
            get; internal set;
        }

        #region Public Methods
        /// <summary>
        /// replace an existing node
        /// </summary>
        /// <param name="roadSection">The road section</param>
        /// <param name="crossSection">The cross section details</param>
        public void ReplaceRoadWith(RoadCrossSection roadSection, ICrossSection crossSection)
        {
            if (_startRoadId.ID == roadSection.ID)
            {
                _startRoadId = roadSection;
                _startCrossSection = crossSection;
            }
            else if (_endRoadId.ID == roadSection.ID)
            {
                _endRoadId = roadSection;
                _endCrossSection = crossSection;
            }
            else
            {
                Debug.LogError("StreetData.ReplaceRoadWith: Can't find node to replace");
            }
        }

        /// <summary>
        /// Add a second road
        /// </summary>
        /// <param name="roadSection">The road section</param>
        /// <param name="crossSection">The cross section details</param>
        public void AddSecondRoad(RoadCrossSection roadSection, ICrossSection crossSection)
        {
            _endRoadId = roadSection;
            _endCrossSection = crossSection;
        }

        /// <summary>
        /// Create the mesh for the street
        /// </summary>
        /// <param name="roadObject">The base object</param>
        public void CreateMesh(IRoadBuildData roadBuilderObject)
        {
            if (_drawn)
                return;

            _drawn = true;
            _meshSection.CreateMesh(roadBuilderObject);
        }

        /// <summary>
        /// Create the steet layout
        /// </summary>
        public void CreateStreetLayout(int subDivide)
        {
            if (_startRoadId == null)
                return;

            if (_endRoadId == null)
                return;

            RoadCrossSection rA = _startRoadId;
            ICrossSection crossSectionStart = _startCrossSection;
            if (crossSectionStart == null)
                crossSectionStart = RoadConstructorHelper.CrossSectionDetails;

            RoadCrossSection rB = _endRoadId;
            ICrossSection crossSectionEnd = _endCrossSection;
            if (crossSectionEnd == null)
                crossSectionEnd = RoadConstructorHelper.CrossSectionDetails;

            IMaterialFrequency materialFrequency = _materialFrequency;
            if (materialFrequency == null)
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;

            Vector3 len = rA.Middle - rB.Middle;
            float mag = len.magnitude;
            int sections = (int)(mag / crossSectionStart.RoadWidthValue);
            RoadCrossSection[] array = new RoadCrossSection[sections + 1];
            string[] materialNames = new string[sections + 1];

            float an = rB.Angle;
            Vector3 start = rB.Middle;

            if (sections < 2)
            {
                Vector3 another = rB.Middle;
                another = rA.Middle;
                RoadCrossSection rn = new RoadCrossSection(another, an, crossSectionStart, materialFrequency);
                _meshSection.AddBasicRoad(IntersectionManager.Instance.AddLinkedIntersecions(rB, rn), RoadConstructorHelper.GetMainMaterial(materialFrequency), 0); // TODO SubDivide
                return;
            }

            Vector3 gap = len / sections;
            float mag2 = gap.magnitude;
            for (int i = 0; i < sections + 1; i++)
            {
                ICrossSection crossSection = CrossSection.Lerp(crossSectionEnd, crossSectionStart, (mag2 * i) / mag);
                RoadCrossSection rn = new RoadCrossSection(start, an, crossSection, materialFrequency);
                array[i] = rn;
                start += gap;
            }

            RoadConstructorHelper.SetMaterialsArray(materialNames, materialFrequency);
            for (int i = 0; i < sections; i++)
            {
                _meshSection.AddBasicRoad(IntersectionManager.Instance.AddLinkedIntersecions(array[i], array[i + 1]), materialNames[i], subDivide);
            }
        }

        /// <summary>
        /// Get the first part of the street
        /// </summary>
        public RoadCrossSection GetFirst
        {
            get
            {
                return _startRoadId;
            }
        }

        /// <summary>
        /// Get the second part of the road
        /// </summary>
        public RoadCrossSection GetSecond
        {
            get
            {
                return _endRoadId;
            }
        }
        #endregion

        #region Private Fields
        private RoadCrossSection _startRoadId = null;
        private ICrossSection _startCrossSection = null;
        private RoadCrossSection _endRoadId = null;
        private ICrossSection _endCrossSection = null;
        private MeshSectionDetails _meshSection = new MeshSectionDetails();
        private IMaterialFrequency _materialFrequency;
        private bool _drawn = false;
        #endregion
    }
}