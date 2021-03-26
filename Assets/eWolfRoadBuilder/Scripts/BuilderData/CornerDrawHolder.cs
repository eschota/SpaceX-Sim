using System;
using System.Collections.Generic;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The class to hold how a corner is drawn
    /// </summary>
    public class CornerDrawHolder
    {
        #region Public Methods
        /// <summary>
        /// The standared constructor
        /// </summary>
        /// <param name="pavements">The list of verts for the pavement</param>
        /// <param name="road">The list of verts for the road</param>
        public CornerDrawHolder(List<VectorUvs> pavements, List<VectorUvs> road)
        {
            _pavements = pavements;
            _road = road;
            DrawPavementImpl = DoNothing;
            DrawKerbImpl = DoNothing;
            DrawRoadImpl = DoNothing;
        }

        /// <summary>
        /// Sets the kerb
        /// </summary>
        /// <param name="kerbTop">The list of verts at the top of the kerb</param>
        /// <param name="kerbBottom">The list verts at the bottom of the kerb</param>
        /// <param name="startingIndex">The starting index</param>
        public void SetKerb(List<VectorUvs> kerbTop, List<VectorUvs> kerbBottom, int startingIndex)
        {
            _kerbTop = kerbTop;
            _kerbBottom = kerbBottom;
            _startingIndex = startingIndex;
        }

        /// <summary>
        /// Draw the pavement
        /// </summary>
        public void DrawPavement()
        {
            _drawPavement(_pavements);
        }

        /// <summary>
        /// Draw the road
        /// </summary>
        public void DrawRoad()
        {
            _drawRoad(_road);
        }

        /// <summary>
        /// Draw the kerb
        /// </summary>
        public void DrawKerb()
        {
            _drawKerb(_kerbTop, _kerbBottom, _startingIndex);
        }

        /// <summary>
        /// Sets the draw the pavement impl
        /// </summary>
        public Action<List<VectorUvs>> DrawPavementImpl
        {
            set
            {
                _drawPavement = value;
            }
        }

        /// <summary>
        /// Sets the draw road impl
        /// </summary>
        public Action<List<VectorUvs>> DrawRoadImpl
        {
            set
            {
                _drawRoad = value;
            }
        }

        /// <summary>
        /// Sets the Draw kerb impl
        /// </summary>
        public Action<List<VectorUvs>, List<VectorUvs>, int> DrawKerbImpl
        {
            set
            {
                _drawKerb = value;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Do nothing list
        /// </summary>
        /// <param name="nothingList"></param>
        private void DoNothing(List<VectorUvs> nothingList)
        {
        }

        /// <summary>
        /// The do nothing lists
        /// </summary>
        /// <param name="nothingList">The first nothing list</param>
        /// <param name="nothingListb">The second nothind list</param>
        /// <param name="v">nothing value</param>
        private void DoNothing(List<VectorUvs> nothingList, List<VectorUvs> nothingListb, int v)
        {
        }
        #endregion

        #region Private Fields
        private Action<List<VectorUvs>> _drawPavement;
        private List<VectorUvs> _pavements;
        private Action<List<VectorUvs>, List<VectorUvs>, int> _drawKerb;
        private List<VectorUvs> _kerbTop;
        private List<VectorUvs> _kerbBottom;
        private int _startingIndex;
        private Action<List<VectorUvs>> _drawRoad;
        private List<VectorUvs> _road;
        #endregion
    }
}
