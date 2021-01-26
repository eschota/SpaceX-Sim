using System;
using UnityEngine;
using eWolfRoadBuilderHelpers;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The cross section of road
    /// </summary>
    public class RoadCrossSection : ICloneable
    {
        /// <summary>
        /// Gets and Sets the angle of the road inster section
        /// </summary>
        public float Angle
        {
            get
            {
                return _angle;
            }

            set
            {
                _angle = value;
            }
        }

        /// <summary>
        /// Gets the middle point of the road
        /// </summary>
        public Vector3 Middle
        {
            get
            {
                return _middle;
            }
        }

        /// <summary>
        /// Gets the left side of the road
        /// </summary>
        public Vector3 Left
        {
            get
            {
                return _left;
            }
        }

        /// <summary>
        /// Gets the right side of the road
        /// </summary>
        public Vector3 Right
        {
            get
            {
                return _right;
            }
        }

        /// <summary>
        /// gets the top of the left curb
        /// </summary>
        public Vector3 CurbLeftLip
        {
            get
            {
                return _curbUpLeft;
            }
        }

        /// <summary>
        /// Gets the far side of the left curb
        /// </summary>
        public Vector3 CurbLeftEnd
        {
            get
            {
                return _curbEndLeft;
            }
        }

        /// <summary>
        /// Gets the far edge of the left curb drop point
        /// </summary>
        public Vector3 CurbLeftEndDrop
        {
            get
            {
                return _curbEndLeftDrop;
            }
        }

        /// <summary>
        /// Gets the top of the right curb
        /// </summary>
        public Vector3 CurbRightLip
        {
            get
            {
                return _curbUpRight;
            }
        }

        /// <summary>
        /// Gets the fat side of the right curb
        /// </summary>
        public Vector3 CurbRightEnd
        {
            get
            {
                return _curbEndRight;
            }
        }

        /// <summary>
        /// Gets the far edge of the right curb drop point
        /// </summary>
        public Vector3 CurbRightEndDrop
        {
            get
            {
                return _curbEndRightDrop;
            }
        }

        /// <summary>
        /// Gets and Sets Should we add curbs
        /// </summary>
        public bool WithCurbs
        {
            get
            {
                return _withCurbs;
            }

            set
            {
                _withCurbs = value;
            }
        }

        /// <summary>
        /// Get and Sets the unique id for the road
        /// </summary>
        public Guid ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// The default constructor
        /// </summary>
        public RoadCrossSection()
        {
        }

        /// <summary>
        /// Create a mirrored verion of the intersection
        /// </summary>
        /// <param name="oldSection">The intersection to mirror</param>
        /// <returns>The mirrored version of the intersection</returns>
        internal static RoadCrossSection CreateMirror(RoadCrossSection oldSection)
        {
            return new RoadCrossSection(oldSection);
        }

        /// <summary>
        /// Copy mirror constructor
        /// </summary>
        /// <param name="oldSection">The road cross section to copy</param>
        private RoadCrossSection(RoadCrossSection oldSection)
        {
            _angle = oldSection.Angle + (Mathf.PI * 2);

            _left = oldSection.Right;
            _right = oldSection.Left;

            _curbUpLeft = oldSection._curbUpRight;
            _curbUpRight = oldSection._curbUpLeft;

            _curbEndLeft = oldSection._curbEndRight;
            _curbEndRight = oldSection._curbEndLeft;

            _curbEndLeftDrop = oldSection._curbEndRightDrop;
            _curbEndRightDrop = oldSection._curbEndLeftDrop;

            _middle = oldSection._middle;
            _withCurbs = oldSection._withCurbs;

            _curbLipHeight = oldSection._curbLipHeight;
            _curbEdgeDrop = oldSection._curbEdgeDrop;
        }

        /// <summary>
        /// Standard construsctors
        /// </summary>
        /// <param name="startPosition">The middle point on the road</param>
        /// <param name="angle">The angle of the road</param>
        /// <param name="crossSections">The cross section for the road</param>
        /// <param name="mf">The materials frequency</param>
        public RoadCrossSection(Vector3 startPosition, float angle, ICrossSection crossSections, IMaterialFrequency mf)
        {
            _angle = angle;

            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            _middle = startPosition;

            _left = startPosition;
            _left.x -= (float)(x * crossSections.RoadWidthValue / 2);
            _left.z += (float)(z * crossSections.RoadWidthValue / 2);

            _curbUpLeft = Left;
            if (crossSections.WithCurbValue)
            {
                _curbUpLeft.y += crossSections.CurbLipHeightValue;
                _curbUpLeft.x += (float)(x * -crossSections.CurbLipSlopeValue);
                _curbUpLeft.z -= (float)(z * -crossSections.CurbLipSlopeValue);
            }

            _curbEndLeft = _curbUpLeft;
            if (crossSections.WithCurbValue)
            {
                _curbEndLeft.x -= (float)(x * crossSections.CurbWidthValue);
                _curbEndLeft.z += (float)(z * crossSections.CurbWidthValue);
            }

            _right = startPosition;
            _right.x += (float)(x * crossSections.RoadWidthValue / 2);
            _right.z -= (float)(z * crossSections.RoadWidthValue / 2);

            _curbUpRight = Right;
            if (crossSections.WithCurbValue)
            {
                _curbUpRight.y += crossSections.CurbLipHeightValue;
                _curbUpRight.x += (float)(x * crossSections.CurbLipSlopeValue);
                _curbUpRight.z -= (float)(z * crossSections.CurbLipSlopeValue);
            }

            _curbEndRight = _curbUpRight;
            if (crossSections.WithCurbValue)
            {
                _curbEndRight.x += (float)(x * crossSections.CurbWidthValue);
                _curbEndRight.z -= (float)(z * crossSections.CurbWidthValue);
            }

            _curbEndLeftDrop = _curbEndLeft;
            _curbEndRightDrop = _curbEndRight;
            if (crossSections.WithCurbValue)
            {
                _curbEndLeftDrop.y -= crossSections.CurbEdgeDropValue;
                _curbEndRightDrop.y -= crossSections.CurbEdgeDropValue;
            }

            if (crossSections.WithCurbValue)
            {
                _curbLipHeight = crossSections.CurbLipHeightValue;
                _curbEdgeDrop = crossSections.CurbEdgeDropValue;
            }
            else
            {
                _curbLipHeight = 0;
                _curbEdgeDrop = 0;
            }
        }

        /// <summary>
        /// Create the cross section at the point the roads roads intersect
        /// </summary>
        /// <param name="angle">The angel of the intersecting road</param>
        /// <param name="point">The point on the road to use</param>
        /// <returns>Creates a new intersecion of the road at the angle given</returns>
        internal RoadCrossSection GetIntersection(float angle, Vector3 point)
        {
            RoadCrossSection intersection = (RoadCrossSection)this.Clone();

            Vector3 pos;
            GetPointFrom(intersection.Angle, angle, intersection._curbEndRight, point, out pos);
            intersection._curbEndRight = pos;

            GetPointFrom(intersection.Angle, angle, intersection._curbUpLeft, point, out pos);
            intersection._curbUpLeft = pos;

            GetPointFrom(intersection.Angle, angle, intersection.CurbLeftEnd, point, out pos);
            intersection._curbEndLeft = pos;

            GetPointFrom(intersection.Angle, angle, intersection._curbUpRight, point, out pos);
            intersection._curbUpRight = pos;

            GetPointFrom(intersection.Angle, angle, intersection.Left, point, out pos);
            intersection._left = pos;

            GetPointFrom(intersection.Angle, angle, intersection.Right, point, out pos);
            intersection._right = pos;

            GetPointFrom(intersection.Angle, angle, intersection._curbEndLeftDrop, point, out pos);
            intersection._curbEndLeftDrop = pos;

            GetPointFrom(intersection.Angle, angle, intersection._curbEndRightDrop, point, out pos);
            intersection._curbEndRightDrop = pos;

            intersection._angle = angle + (float)(Mathf.PI / 2);

            intersection._curbLipHeight = _curbLipHeight;
            intersection._curbEdgeDrop = _curbEdgeDrop;

            intersection._middle = (intersection._left + intersection._right) / 2;

            return intersection;
        }

        /// <summary>
        /// Get the intersection of two points
        /// </summary>
        /// <param name="angleA">The first angle</param>
        /// <param name="angleB">The second angle</param>
        /// <param name="pointA">The first point</param>
        /// <param name="pointB">the second point</param>
        /// <param name="intersectionPoint">The intersection of the two angles</param>
        private static void GetPointFrom(float angleA, float angleB, Vector3 pointA, Vector3 pointB, out Vector3 intersectionPoint)
        {
            Vector3 offSetPointA = MathsHelper.OffSetVector(pointA, angleA - (float)(Math.PI / 2), 50);
            Vector3 offsetPointB = MathsHelper.OffSetVector(pointB, angleB - (float)(Math.PI / 2), 50);
            Vector3 outB = new Vector3();
            Vector3 diffA = offSetPointA - pointA;
            Vector3 diffB = offsetPointB - pointB;
            MathsHelper.ClosestPointsOnTwoLines(out intersectionPoint, out outB, pointA, diffA.normalized, pointB, diffB.normalized);
        }

        /// <summary>
        /// Drop this road section to the groupd
        /// </summary>
        public void DropToGroundSection()
        {
            float gap = 2;
            _left = GetDownGap(_left, gap, 0.05f);
            _right = GetDownGap(_right, gap, 0.05f);

            _curbEndLeft = GetDownGap(_curbEndLeft, gap, _curbLipHeight);
            _curbEndRight = GetDownGap(_curbEndRight, gap, _curbLipHeight);
            _curbUpLeft = GetDownGap(_curbUpLeft, gap, _curbLipHeight);
            _curbUpRight = GetDownGap(_curbUpRight, gap, _curbLipHeight);

            _curbEndLeftDrop = ApplyCurbOffSetWithOutCollisionChecks(_curbEndLeft, _curbEdgeDrop);
            _curbEndRightDrop = ApplyCurbOffSetWithOutCollisionChecks(_curbEndRight, _curbEdgeDrop);
        }

        /// <summary>
        /// Get the alount we can move down until we hit something
        /// </summary>
        /// <param name="pos">The starting position</param>
        /// <param name="gap">The amount we wish to move</param>
        /// <param name="offSet">The offset to add back</param>
        /// <returns>The final point</returns>
		public Vector3 GetDownGap(Vector3 pos, float gap, float offSet)
        {
            Vector3 posEnd = pos;
            pos.y += 0.5f;
            posEnd.y -= gap;
            Vector3 diff = posEnd - pos;

            float mag = diff.magnitude;
            diff.Normalize();

            RaycastHit hitInfo;
            if (Physics.Raycast(pos, diff, out hitInfo, mag, 1 << LayerMask.NameToLayer("Ground")))
            {
                Vector3 pohi = hitInfo.point;
                pohi.y += offSet;
                return pohi;
            }

            return posEnd;
        }

        /// <summary>
        /// Apply the offset without collision checks
        /// </summary>
        /// <param name="pos">The starting position</param>
        /// <param name="offSet">The off set to apply</param>
        /// <returns>The offset position</returns>
        public Vector3 ApplyCurbOffSetWithOutCollisionChecks(Vector3 pos, float offSet)
        {
            pos.y -= offSet;
            return pos;
        }

        /// <summary>
        /// Is the road twisted compared with the other
        /// </summary>
        /// <param name="rsc">The over road to compear to</param>
        /// <returns>True if the road is twisted</returns>
        internal bool IsRoadTwisted(RoadCrossSection rsc)
        {
            Radian r1 = new Radian(_angle);
            Radian r2 = new Radian(rsc.Angle);

            if (r1.Value == r2.Value)
                return false;

            Radian r3 = new Radian(r2.Value - r1.Value);

            r3.Value += Mathf.PI / 2;
            if (r3.Value < Mathf.PI)
                return false;

            Gizmos.color = Color.white;
            return true;
        }

        /// <summary>
        /// Create a clone of this intersection
        /// </summary>
        /// <returns>A new object as a clone of this one</returns>
        public object Clone()
        {
            RoadCrossSection rcs = new RoadCrossSection();
            rcs._id = _id;
            rcs._angle = _angle;
            rcs._left = _left;
            rcs._right = _right;
            rcs._middle = _middle;

            rcs._curbUpLeft = _curbUpLeft;
            rcs._curbEndLeft = _curbEndLeft;
            rcs._right = _right;
            rcs._curbUpRight = _curbUpRight;
            rcs._curbEndRight = _curbEndRight;

            rcs._curbEndLeftDrop = _curbEndLeftDrop;
            rcs._curbEndRightDrop = _curbEndRightDrop;
            rcs._curbLipHeight = _curbLipHeight;
            return rcs;
        }

        #region Fields
        private Guid _id = Guid.NewGuid();
        private float _angle = 0;
        private bool _withCurbs = true;
        private Vector3 _middle;
        private Vector3 _left;
        private Vector3 _curbUpLeft;
        private Vector3 _curbEndLeft;
        private Vector3 _right;
        private Vector3 _curbUpRight;
        private Vector3 _curbEndRight;
        private Vector3 _curbEndLeftDrop;
        private Vector3 _curbEndRightDrop;
        private float _curbLipHeight;
        private float _curbEdgeDrop;
        #endregion
    }
}