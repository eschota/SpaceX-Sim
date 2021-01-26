using System;
using System.Collections.Generic;
#if DEBUG
using UnityEditor;
#endif
using UnityEngine;

// TODO: Could add the terrains as list, just in case the roads are over multiple terrains.
namespace eWolfRoadBuilder.Terrains
{
    /// <summary>
    /// The terrain modifier
    /// </summary>
    public class TerrainModifier : MonoBehaviour
    {
        #region Public Fields
        /// <summary>
        /// The amont on plateau on the top
        /// </summary>
        public float Plateau = 0;

        /// <summary>
        /// How far the slope reachers out
        /// </summary>
        public float Slope = 8;

        /// <summary>
        /// Weather to flatten the terrain to start with (mostly used for testing)
        /// </summary>
        public bool FlattenTerrain = false;

        /// <summary>
        /// The terrain modifier details
        /// </summary>
        public TerrainModifierDetails Details;

        /// <summary>
        /// The terrain object to modify
        /// </summary>
        public GameObject Terrain;
        #endregion

        #region Public Methods
        /// <summary>
        /// Do we have a terrain stored
        /// </summary>
        public bool HasStoredTerrain
        {
            get
            {
                if (_terrainStore == null)
                    return false;

                return _terrainStore.Stored;
            }
        }

        /// <summary>
        /// Restore the previous terrain
        /// </summary>
        public void RestoreTerrain()
        {
            _terrainData.SetHeights(0, 0, _terrainStore.Data);
        }

        /// <summary>
        /// Apply the road height to the terrain
        /// </summary>
        /// <param name="baseObject">The road network layout</param>
        public void ApplyTerrainHeight(GameObject baseObject)
        {
#if DEBUG
            TerrainCollider tc = Terrain.GetComponent<TerrainCollider>();
            if (tc == null)
            {
                Debug.LogAssertion("Can't find the terrain: check the terrain is set inthe TerrainModifier setting in the inspector.");
                return;
            }

            _terrainData = tc.terrainData;

            _width = _terrainData.heightmapResolution;
            _depth = _width;

            _terrainStore = new TerrainStore();
            _terrainStore.Stored = true;
            _terrainStore.Width = _width;
            _terrainStore.Depth = _depth;
            _terrainStore.Data = _terrainData.GetHeights(0, 0, _width, _depth);
            _terrainHeightDetails = new TerrainHeightDetails[_width, _depth];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _depth; j++)
                {
                    _terrainHeightDetails[i, j] = new TerrainHeightDetails();
                    float height = FlattenTerrain ? 0 : _terrainStore.Data[i, j];

                    _terrainHeightDetails[i, j].OriginalHeight = height;
                    _terrainHeightDetails[i, j].Height = height;
                }
            }

            Transform[] allChildren = baseObject.GetComponentsInChildren<Transform>(true);
            _totalSteps = allChildren.Length * 2;
            _currentStep = -1;
            IncreaseProgressBar();

            ApplyHeightAsOrigianlHeight();

            ProcessNodes(allChildren);
            IncreaseProgressBar();
            ProcessNodes(allChildren);
            IncreaseProgressBar();
            float[,] terrainArray = new float[_width, _depth];
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _depth; j++)
                {
                    terrainArray[i, j] = _terrainHeightDetails[i, j].Height;
                }
            }

            _terrainData.SetHeights(0, 0, terrainArray);

            EditorUtility.ClearProgressBar();
#endif
        }

        /// <summary>
        /// Apply the rectangle to the terrain
        /// </summary>
        /// <param name="rect">The rectangle to apply</param>
        public void ApplyToTerrain(RectVector3 rect, bool isPlateau)
        {
            for (float i = 0; i < Details.Divider; i++)
            {
                float iDivider = i / Details.Divider;
                Vector3 top = Vector3.Lerp(rect.TopLeft, rect.TopRight, iDivider);
                Vector3 bottom = Vector3.Lerp(rect.BottomLeft, rect.BottomRight, iDivider);
                for (float j = 0; j < Details.Divider; j++)
                {
                    Vector3 mid = Vector3.Lerp(top, bottom, j / Details.Divider);
                    Vector3 start = GetPos(mid);
                    float heightV = _terrainData.size.y;
                    SetHeight((int)(start.z), (int)(start.x), (start.y / heightV) - Details.OffSet, isPlateau);
                }
            }
        }

        /// <summary>
        /// Apply the terrain height smothly
        /// </summary>
        /// <param name="rect">The rectangle to apply</param>
        public void ApplyToTerrainSoft(RectVector3 rect)
        {
            ApplyHeightAsOrigianlHeight();

            for (float i = 0; i < Details.Divider; i++)
            {
                Vector3 top = Vector3.Lerp(rect.TopLeft, rect.TopRight, i / Details.Divider);

                // get the height
                Vector3 topTerrain = top;
                topTerrain.y = GetTerrainHeight(top);

                Vector3 bottom = Vector3.Lerp(rect.BottomLeft, rect.BottomRight, i / Details.Divider);
                for (float j = 0; j < Details.Divider; j++)
                {
                    float percentage = j / Details.Divider;
                    Vector3 mid = Vector3.Lerp(topTerrain, bottom, percentage);
                    Vector3 start = GetPos(mid);

                    float heightV = _terrainData.size.y;
                    SetHeight((int)start.z, (int)start.x, (start.y / heightV) - Details.OffSet, false);
                }
            }
        }

        /// <summary>
        /// Apply the road cross sections to the terrain
        /// </summary>
        /// <param name="rscA">The first cross section</param>
        /// <param name="rscB">The second cross section</param>
        public void ApplyToTerrain(RoadCrossSection rscA, RoadCrossSection rscB)
        {
            ApplyHeightAsOrigianlHeight();

            float angle = rscA.Angle;
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            Vector3 plateauOffSetA = new Vector3(0, 0, 0);
            plateauOffSetA.x -= (float)(x * Plateau);
            plateauOffSetA.z += (float)(z * Plateau);

            Vector3 slopOffSetA = new Vector3(0, 0, 0);
            slopOffSetA.x -= (float)(x * Slope);
            slopOffSetA.z += (float)(z * Slope);

            angle = rscB.Angle;
            x = Mathf.Sin(angle);
            z = Mathf.Cos(angle);

            Vector3 plateauOffSetB = new Vector3(0, 0, 0);
            plateauOffSetB.x -= (float)(x * Plateau);
            plateauOffSetB.z += (float)(z * Plateau);

            Vector3 slopOffSetB = new Vector3(0, 0, 0);
            slopOffSetB.x -= (float)(x * Slope);
            slopOffSetB.z += (float)(z * Slope);

            RectVector3 r;
            if (!rscB.IsRoadTwisted(rscA))
            {
                r = new RectVector3(rscA.Left, rscB.Left, rscA.Right, rscB.Right);
                ApplyToTerrain(r, true);
                r = new RectVector3(rscA.CurbRightEnd - plateauOffSetA, rscB.CurbRightEnd - plateauOffSetB, rscA.CurbRightLip, rscB.CurbRightLip);
                ApplyToTerrain(r, true);
                r = new RectVector3(rscA.CurbLeftEnd + plateauOffSetA, rscB.CurbLeftEnd + plateauOffSetB, rscA.CurbLeftLip, rscB.CurbLeftLip);
                ApplyToTerrain(r, true);

                if (Slope != 0)
                {
                    // slope
                    Vector3 slopedA = rscA.CurbRightEnd - (plateauOffSetA + slopOffSetA);
                    Vector3 slopedB = rscB.CurbRightEnd - (plateauOffSetB + slopOffSetB);
                    slopedA.y = 0;
                    slopedB.y = 0;
                    r = new RectVector3(slopedA, slopedB, rscA.CurbRightEnd - plateauOffSetA, rscB.CurbRightEnd - plateauOffSetB);
                    ApplyToTerrainSoft(r);

                    // slope
                    slopedA = rscA.CurbLeftEnd + (plateauOffSetA + slopOffSetA);
                    slopedB = rscB.CurbLeftEnd + (plateauOffSetB + slopOffSetB);
                    slopedA.y = 0;
                    slopedB.y = 0;
                    r = new RectVector3(slopedA, slopedB, rscA.CurbLeftEnd + plateauOffSetA, rscB.CurbLeftEnd + plateauOffSetB);
                    ApplyToTerrainSoft(r);
                }
            }
            else
            {
                r = new RectVector3(rscA.Right, rscB.Left, rscA.Left, rscB.Right);
                ApplyToTerrain(r, true);
                r = new RectVector3(rscA.CurbRightEnd - plateauOffSetA, rscB.CurbLeftEnd + plateauOffSetB, rscA.CurbRightLip, rscB.CurbLeftLip);
                ApplyToTerrain(r, true);
                r = new RectVector3(rscA.CurbLeftEnd + plateauOffSetA, rscB.CurbRightEnd - plateauOffSetB, rscA.CurbLeftLip, rscB.CurbRightLip);
                ApplyToTerrain(r, true);

                if (Slope != 0)
                {
                    // slope
                    Vector3 slopedA = rscA.CurbRightEnd - (plateauOffSetA + slopOffSetA);
                    Vector3 slopedB = rscB.CurbLeftEnd + (plateauOffSetB + slopOffSetB);
                    slopedA.y = 0;
                    slopedB.y = 0;
                    r = new RectVector3(slopedA, slopedB, rscA.CurbRightEnd - plateauOffSetA, rscB.CurbLeftEnd + plateauOffSetB);
                    ApplyToTerrainSoft(r);

                    // slope
                    slopedA = rscA.CurbLeftEnd + (plateauOffSetA + slopOffSetA);
                    slopedB = rscB.CurbRightEnd - (plateauOffSetB + slopOffSetB);
                    slopedA.y = 0;
                    slopedB.y = 0;
                    r = new RectVector3(slopedA, slopedB, rscA.CurbLeftEnd + plateauOffSetA, rscB.CurbRightEnd - plateauOffSetB);
                    ApplyToTerrainSoft(r);
                }
            }
        }

        /// <summary>
        /// Get the final position 
        /// </summary>
        /// <param name="pos">The position to transform</param>
        /// <returns>The position on the terrain</returns>
        public Vector3 GetPos(Vector3 pos)
        {
            pos -= Terrain.transform.position;

            Vector3 scale = _terrainData.size;
            pos.x = pos.x / scale.x;
            pos.z = pos.z / scale.z;

            pos.x *= _terrainData.heightmapResolution;
            pos.z *= _terrainData.heightmapResolution;

            return pos;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Update the original height from the current height
        /// </summary>
        private void ApplyHeightAsOrigianlHeight()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _depth; j++)
                {
                    _terrainHeightDetails[i, j].OriginalHeight = _terrainHeightDetails[i, j].Height;
                }
            }
        }

        /// <summary>
        /// Sets the height for the terrain
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="newHeight">The new height for the terrain</param>
        /// <param name="isPlateau">weather is a plateau or not</param>
        private void SetHeight(int x, int y, float newHeight, bool isPlateau)
        {
            if (y < 0 || y > _depth)
                return;

            if (x < 0 || x > _width)
                return;

            if (_terrainHeightDetails[x, y].IsPlateau)
            {
                if (isPlateau)
                {
                    _terrainHeightDetails[x, y].Height = Math.Max(_terrainHeightDetails[x, y].Height, newHeight);
                }

                return;
            }

            _terrainHeightDetails[x, y].Height = newHeight;
            _terrainHeightDetails[x, y].IsPlateau = isPlateau;
        }

        /// <summary>
        /// Gets the current hight of the terrain
        /// </summary>
        /// <param name="pos">The position in the terrain</param>
        /// <returns>The height of the terrain at the postion</returns>
        private float GetTerrainHeight(Vector3 pos)
        {
            Vector3 start = GetPos(pos);
            int x = (int)start.z;
            int y = (int)start.x;

            if (y < 0 || y > _depth)
                return 0;

            if (x < 0 || x > _width)
                return 0;

            return _terrainHeightDetails[x, y].OriginalHeight * _terrainData.size.y;
        }
        #endregion

        /// <summary>
        /// Process the node of the road
        /// </summary>
        /// <param name="allChildren">All the road children</param>
        private void ProcessNodes(Transform[] allChildren)
        {
            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                if (rub != null)
                {
                    if (rub.Details.Union == RoadNetworkNode.UNION_TYPE.NONE)
                        continue;
                    rub.RoadUnion.ModifiyTerrain(this);
                }

                IncreaseProgressBar();
            }
        }

        /// <summary>
        /// Increase the progress bar one step
        /// </summary>
        private void IncreaseProgressBar()
        {
#if DEBUG
            _currentStep++;
            float progress = _currentStep;
            progress /= _totalSteps;
            EditorUtility.DisplayProgressBar("ElectricWolf.co.uk : Road Builder", "Terrain: Modifying terrain to match road", progress);
#endif
        }

        /// <summary>
        /// Apply the stright road terrain
        /// </summary>
        /// <param name="rsca">The starting cross seciton</param>
        /// <param name="list">The list of objects</param>
        /// <returns>The final cross section</returns>
        private RoadCrossSection ApplyRoadStrightTerrain(RoadCrossSection rsca, List<Guid> list)
        {
            foreach (Guid item in list)
            {
                RoadCrossSection rsc = IntersectionManager.Instance[item];
                if (rsca != null)
                {
                    ApplyToTerrain(rsc, rsca);
                }

                rsca = rsc;
            }

            return rsca;
        }

        /// <summary>
        /// The terrain store
        /// </summary>
        private class TerrainStore
        {
            /// <summary>
            /// Do we have a valid terrain stored
            /// </summary>
            public bool Stored = false;

            /// <summary>
            /// The width of the stored terrain
            /// </summary>
            public int Width;

            /// <summary>
            /// The depth of the stored terrain
            /// </summary>
            public int Depth;

            /// <summary>
            /// The stored terrain data (heights)
            /// </summary>
            public float[,] Data;
        }

        #region Private Fields
        private int _width;
        private int _depth;
        private TerrainStore _terrainStore;
        private TerrainHeightDetails[,] _terrainHeightDetails;
        private TerrainData _terrainData;
        private int _totalSteps = 0;
        private int _currentStep = 0;
        #endregion
    }
}