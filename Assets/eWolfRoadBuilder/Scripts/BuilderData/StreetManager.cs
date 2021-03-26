using System.Collections.Generic;
using System;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// managers all of the streets - make sure we don't create the mesh more than once
    /// </summary>
    public class StreetManager
    {
        /// <summary>
        /// Gets the instance of the manager
        /// </summary>
        public static StreetManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StreetManager();
                return _instance;
            }
        }

        /// <summary>
        /// Clear out all the intersections
        /// </summary>
        public void Clear()
        {
            _streets.Clear();
        }

        /// <summary>
        /// Add a street to the list
        /// </summary>
        /// <param name="streetFullName">The name of the street</param>
        /// <param name="streetData">The street data</param>
        /// <returns>True if we have added the street data</returns>
        public bool Add(string streetFullName, StreetData streetData)
        {
            StreetData oldsd = null;
            if (_streets.TryGetValue(streetFullName, out oldsd))
            {
                return false;
            }

            _streets.Add(streetFullName, streetData);
            return true;
        }

        /// <summary>
        /// Get the StreetData from the name
        /// </summary>
        /// <param name="name">The name of the street</param>
        /// <returns>The street data for the street</returns>
        public StreetData this[string name]
        {
            get
            {
                return _streets[name];
            }
        }

        /// <summary>
        /// Add the street end
        /// </summary>
        /// <param name="nameStart">The name of the street</param>
        /// <param name="nameEnd">The second path of the name</param>
        /// <param name="crossSectionDetails">The cross section details</param>
        /// <param name="materialFreq">The material frequency details</param>
        /// <param name="roadCrossSection">The road cross section</param>
		public void AddStreetEnd(string nameStart, string nameEnd, ICrossSection crossSectionDetails, IMaterialFrequency materialFreq, RoadCrossSection roadCrossSection)
        {

            string[] stringArray = new string[2] { nameStart, nameEnd };
            Array.Sort(stringArray);

            string streetFullName = string.Join("-", stringArray);

            StreetData street = null;
            if (!_streets.TryGetValue(streetFullName, out street))
            {
                street = new StreetData(roadCrossSection, crossSectionDetails, materialFreq);
                _streets.Add(streetFullName, street);
            }
            else
            {
                street.AddSecondRoad(roadCrossSection, crossSectionDetails);
            }
        }

        /// <summary>
        /// Replace the road with the same id
        /// </summary>
        /// <param name="nameStart">The first name</param>
        /// <param name="nameEnd">The second name</param>
        /// <param name="crossSectionDetails">The cross section details to use</param>
        /// <param name="roadCrossSection">The replacement cross section</param>
        public void ReplaceRoadWithId(string nameStart, string nameEnd, ICrossSection crossSectionDetails, RoadCrossSection roadCrossSection)
        {
            string[] stringArray = new string[2] { nameStart, nameEnd };
            Array.Sort(stringArray);

            string streetFullName = string.Join("-", stringArray);

            StreetData street = null;
            if (_streets.TryGetValue(streetFullName, out street))
            {
                street.ReplaceRoadWith(roadCrossSection, crossSectionDetails);
            }
            else
            {
                Debug.LogError("SteetData.ReplaceRoadWithId Can't find street to update node on");
            }
        }

        #region Private Fields
        private static StreetManager _instance = null;
        private Dictionary<string, StreetData> _streets = new Dictionary<string, StreetData>();
        #endregion
    }
}
