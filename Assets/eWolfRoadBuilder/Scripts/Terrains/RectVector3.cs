using UnityEngine;

namespace eWolfRoadBuilder.Terrains
{
    public class RectVector3
    {
        public RectVector3(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br)
        {
            TopLeft = tl;
            TopRight = tr;
            BottomLeft = bl;
            BottomRight = br;
        }

        public Vector3 TopLeft;
        public Vector3 TopRight;
        public Vector3 BottomLeft;
        public Vector3 BottomRight;
    }
}