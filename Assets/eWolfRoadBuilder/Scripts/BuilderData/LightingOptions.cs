using System;

namespace eWolfRoadBuilder
{
    [Serializable]
    public class LightingOptions
    {
        public bool BakedLighting;

        public float HardAngle = 88;
        public float PackMargin = 4;
        public float AngleError = 8;
        public float AreaError = 15;
    }
}
