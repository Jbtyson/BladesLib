// Triangle.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BladesLib.Util.Shapes
{
    public class Triangle
    {
        private Point _a, _b, _c; // a is the right angle point
        private float _angleA, _angleB, _angleC; // a is the right angle
        private float _distAB, _distBC, _distCA; // bc is the hypotenuse

        #region Accessors
        // Points
        public Point A
        {
            get { return _a; }
            set { _a = value; }
        }
        public Point B
        {
            get { return _b; }
            set { _b = value; }
        }
        public Point C
        {
            get { return _c; }
            set { _c = value; }
        }
        // Angles
        public float AngleA
        {
            get { return _angleA; }
            set { _angleA = value; }
        }
        public float AngleB
        {
            get { return _angleB; }
            set { _angleB = value; }
        }
        public float AngleC
        {
            get { return _angleC; }
            set { _angleC = value; }
        }
        // Distances
        public float DistAB
        {
            get { return _distAB; }
            set { _distAB = value; }
        }
        public float DistBC
        {
            get { return _distBC; }
            set { _distBC = value; }
        }
        public float DistCA
        {
            get { return _distCA; }
            set { _distCA = value; }
        }
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public Triangle()
        {
            _a = Point.Zero;
            _b = Point.Zero;
            _c = Point.Zero;
        }

        /// <summary>
        /// Constructor for creating a new triangle at a given set of points
        /// </summary>
        /// <param name="a">Right angle point</param>
        /// <param name="b">Second point</param>
        /// <param name="c">Third point</param>
        public Triangle(Point a, Point b, Point c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        /// <summary>
        /// Initializes all of the values in the triangle
        /// </summary>
        public void Initialize()
        {
            _distAB = (float)Math.Sqrt(Math.Pow(_b.Y - _a.Y, 2) + Math.Pow(_b.X - _a.X, 2));
            _distBC = (float)Math.Sqrt(Math.Pow(_c.Y - _b.Y, 2) + Math.Pow(_c.X - _b.X, 2));
            _distCA = (float)Math.Sqrt(Math.Pow(_a.Y - _c.Y, 2) + Math.Pow(_a.X - _c.X, 2));

            _angleA = 90;
            _angleB = (float)Math.Sin(_distCA / _distBC);
            _angleC = 90 - _angleB;
        }

        /// <summary>
        /// Copies the values of an existing triangle to a new triangle, but updates the location.
        /// </summary>
        /// <param name="a">New right angle point</param>
        /// <param name="b">New second point</param>
        /// <param name="c">New third point</param>
        /// <returns>Copies triangle at a new location</returns>
        public Triangle CopyTo(Point a, Point b, Point c)
        {
            Triangle newTriangle = new Triangle();

            newTriangle.A = a;
            newTriangle.B = b;
            newTriangle.C = c;

            newTriangle.AngleA = _angleA;
            newTriangle.AngleB = _angleB;
            newTriangle.AngleC = _angleC;

            newTriangle.DistAB = _distAB;
            newTriangle.DistBC = _distBC;
            newTriangle.DistCA = _distCA;

            return newTriangle;
        }

        /// <summary>
        /// Checks whether or not a triangle contains a point, includes on lines
        /// Source: http://www.blackpawn.com/texts/pointinpoly/default.html
        /// </summary>
        /// <param name="loc">Point to check</param>
        /// <returns>True if the Triangle containts the given point</returns>
        public bool Contains(Point loc)
        {
            // Compute vectors        
            Vector2 v0 = new Vector2(C.X - A.X, C.Y - A.Y);
            Vector2 v1 = new Vector2(B.X - A.X, B.Y - A.Y);
            Vector2 v2 = new Vector2(loc.X - A.X, loc.Y - A.Y);

            // Compute dot products
            int dot00 = Dot(v0, v0);
            int dot01 = Dot(v0, v1);
            int dot02 = Dot(v0, v2);
            int dot11 = Dot(v1, v1);
            int dot12 = Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = (float)1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            return ((u >= 0) && (v >= 0) && (u + v < 1));
        }

        private int Dot(Vector2 v1, Vector2 v2) 
        {
            return (int)(v1.X * v2.X + v1.Y * v2.Y);
        }
    }
}
