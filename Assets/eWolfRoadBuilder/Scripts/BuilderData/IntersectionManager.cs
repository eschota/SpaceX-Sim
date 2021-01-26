using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Manage all the intersection infomation and how they are linked
    /// </summary>
    public class IntersectionManager
    {
        /// <summary>
        /// Gets the instance of the manager
        /// </summary>
        public static IntersectionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IntersectionManager();
                return _instance;
            }
        }

        #region Public Methods
        /// <summary>
        /// Add one intersection
        /// </summary>
        /// <param name="a">Road intersection</param>
        /// <returns>The index of the connection set</returns>
        public int AddLinkedIntersecions(RoadCrossSection a)
        {
            a = AddIntersection(a);

            _connections.Add(new List<Guid>() { a.ID });
            return _connections.Count - 1;
        }

        /// <summary>
        /// Add a pair of linked intersections
        /// </summary>
        /// <param name="a">road Intersection a</param>
        /// <param name="b">road Intersection b</param>
        /// <returns>The index of the connection set</returns>
        public int AddLinkedIntersecions(RoadCrossSection a, RoadCrossSection b)
        {
            a = AddIntersection(a);
            b = AddIntersection(b);

            _connections.Add(new List<Guid>() { a.ID, b.ID });
            return _connections.Count - 1;
        }

        /// <summary>
        /// Add a few linked intersections
        /// </summary>
        /// <param name="a">Road intersection a</param>
        /// <param name="b">Road intersection b</param>
        /// <param name="c">Road intersection c</param>
        public int AddLinkedIntersecions(RoadCrossSection a, RoadCrossSection b, RoadCrossSection c)
        {
            a = AddIntersection(a);
            b = AddIntersection(b);
            c = AddIntersection(c);

            _connections.Add(new List<Guid>() { a.ID, b.ID, c.ID });
            return _connections.Count - 1;
        }

        /// <summary>
        /// Add a few linked intersections
        /// </summary>
        /// <param name="a">Road intersection a</param>
        /// <param name="b">Road intersection b</param>
        /// <param name="c">Road intersection c</param>
        /// <param name="d">Road intersection d</param>
        public int AddLinkedIntersecions(RoadCrossSection a, RoadCrossSection b, RoadCrossSection c, RoadCrossSection d)
        {
            a = AddIntersection(a);
            b = AddIntersection(b);
            c = AddIntersection(c);
            d = AddIntersection(d);

            _connections.Add(new List<Guid>() { a.ID, b.ID, c.ID, d.ID });
            return _connections.Count - 1;
        }

        /// <summary>
        /// Add a few linked intersections
        /// </summary>
        /// <param name="a">Road intersection a</param>
        /// <param name="b">Road intersection b</param>
        /// <param name="c">Road intersection c</param>
        /// <param name="d">Road intersection d</param>
        /// <param name="e">Road intersection d</param>
        public int AddLinkedIntersecions(RoadCrossSection a, RoadCrossSection b, RoadCrossSection c, RoadCrossSection d, RoadCrossSection e)
        {
            a = AddIntersection(a);
            b = AddIntersection(b);
            c = AddIntersection(c);
            d = AddIntersection(d);
            e = AddIntersection(e);

            _connections.Add(new List<Guid>() { a.ID, b.ID, c.ID, d.ID, e.ID });
            return _connections.Count - 1;
        }

        /// <summary>
        /// Add a few linked intersections
        /// </summary>
        /// <param name="a">Road intersection a</param>
        /// <param name="b">Road intersection b</param>
        /// <param name="c">Road intersection c</param>
        /// <param name="d">Road intersection d</param>
        /// <param name="e">Road intersection d</param>
        /// <param name="f">Road intersection d</param>
        public int AddLinkedIntersecions(RoadCrossSection a, RoadCrossSection b, RoadCrossSection c, RoadCrossSection d, RoadCrossSection e, RoadCrossSection f)
        {
            a = AddIntersection(a);
            b = AddIntersection(b);
            c = AddIntersection(c);
            d = AddIntersection(d);
            e = AddIntersection(e);
            f = AddIntersection(f);

            _connections.Add(new List<Guid>() { a.ID, b.ID, c.ID, d.ID, e.ID, f.ID });
            return _connections.Count - 1;
        }

        /// <summary>
        /// Get the number of connections
        /// </summary>
        /// <returns>The number of connections</returns>
        public int LinksCount
        {
            get
            {
                return _connections.Count;
            }
        }

        /// <summary>
        /// Gets the connection list
        /// </summary>
        /// <param name="index">The connection to get</param>
        /// <returns>The list of connections</returns>
        public List<Guid> this[int index]
        {
            get
            {
                return _connections[index];
            }
        }

        /// <summary>
        /// Gets the intersection from the guid
        /// </summary>
        /// <param name="id">The guid of the read intersection</param>
        /// <returns>The intersection of the road</returns>
        public RoadCrossSection this[Guid id]
        {
            get
            {
                return _intersection[id];
            }
        }

        /// <summary>
        /// The the intersection
        /// </summary>
        /// <param name="id">The id of the intersection</param>
        /// <param name="rcs">The intersection of the road</param>
        public void SetIntersection(Guid id, RoadCrossSection rcs)
        {
            rcs.ID = id;
            RoadCrossSection current;
            if (!_intersection.TryGetValue(id, out current))
            {
                current = null;
            }

            if (current != rcs)
            {
                if (current != null)
                    RemoveFromIndex(id, current.Middle);
                AddToIndex(id, rcs);
            }
            
            _intersection[id] = rcs;
        }
        
        /// <summary>
        /// Gets the intersections
        /// </summary>
        public Dictionary<Guid, RoadCrossSection> Intersections
        {
            get
            {
                return _intersection;
            }
        }
        
        /// <summary>
        /// Clear out all the intersections
        /// </summary>
        public void Clear()
        {
            _intersection = new Dictionary<Guid, RoadCrossSection>();
            _connections = new List<List<Guid>>();
            _intersectionIndex = new Dictionary<int, Dictionary<int, List<Guid>>>();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add to index
        /// </summary>
        /// <param name="id">The id to add</param>
        /// <param name="rcs">The new road cross sections</param>
        private void AddToIndex(Guid id, RoadCrossSection rcs)
        {
            Vector3 mid = rcs.Middle;
            int x = (int)Math.Round(mid.x);
            int z = (int)Math.Round(mid.z);

            Dictionary<int, List<Guid>> outer;
            if (!_intersectionIndex.TryGetValue(x, out outer))
            {
                outer = new Dictionary<int, List<Guid>>();
                _intersectionIndex.Add(x, outer);
            }

            List<Guid> inner;
            if (!outer.TryGetValue(z, out inner))
            {
                inner = new List<Guid>();
                outer.Add(z, inner);
            }
            inner.Add(id);
        }

        /// <summary>
        /// Add a road intersection
        /// </summary>
        /// <param name="section">The Guid to use as the key</param>
        private RoadCrossSection AddIntersection(RoadCrossSection section)
        {
            Vector3 mid = section.Middle;
            int x = (int)Math.Round(mid.x);
            int z = (int)Math.Round(mid.z);

            List<Guid> intersections = GetClosebyNodes(x, z);

            foreach (var a in intersections)
            {
                RoadCrossSection rsc = _intersection[a];
                Vector3 v = rsc.Middle - section.Middle;
                float m = Mathf.Abs(v.magnitude);
                if (m < 0.5f)
                    return rsc;
            }

            AddToIndex(section.ID, section);
            _intersection.Add(section.ID, section);
            return section;
        }

        /// <summary>
        /// Remove from the index
        /// </summary>
        /// <param name="id">The Id to remove</param>
        /// <param name="middle">the middle of the road section</param>
        private void RemoveFromIndex(Guid id, Vector3 middle)
        {
            int x = (int)Math.Round(middle.x);
            int z = (int)Math.Round(middle.z);

            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, List<Guid>> res;
                if (_intersectionIndex.TryGetValue(i, out res))
                {
                    for (int j = z - 1; j <= z + 1; j++)
                    {
                        List<Guid> res2;
                        if (res.TryGetValue(j, out res2))
                        {
                            res2.Remove(id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the close node
        /// </summary>
        /// <param name="x">The x pos</param>
        /// <param name="z">The y pos</param>
        /// <returns>The list of guid</returns>
        private List<Guid> GetClosebyNodes(int x, int z)
        {
            var result = new List<Guid>();

            for (int i = x - 1; i <= x + 1; i++)
            {
                Dictionary<int, List<Guid>> res;
                if (_intersectionIndex.TryGetValue(i, out res))
                {
                    for (int j = z - 1; j <= z + 1; j++)
                    {
                        List<Guid> res2;
                        if (res.TryGetValue(j, out res2))
                        {
                            result.AddRange(res2);
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region Private Fields
        private static IntersectionManager _instance = null;
        private Dictionary<Guid, RoadCrossSection> _intersection = new Dictionary<Guid, RoadCrossSection>();
        private List<List<Guid>> _connections = new List<List<Guid>>();
        private Dictionary<int, Dictionary<int, List<Guid>>> _intersectionIndex;
        #endregion
    }
}
