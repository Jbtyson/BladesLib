// Diamond.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BladesLib.Util.Shapes
{
    /// <summary>
    /// Diamond represents a diamond geometric shape
    /// </summary>
    public class Diamond
    {        
        private Point _left, _top, _right, _bottom;
        private Rectangle _rect;
        private Triangle _topTri, _bottomTri;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Diamond()
        {
            _left = Point.Zero;
            _top = Point.Zero;
            _right = Point.Zero;
            _bottom = Point.Zero;
            _rect = Rectangle.Empty;
            _topTri = new Triangle();
            _bottomTri = new Triangle();
        }
        
        /// <summary>
        /// Creates a diamond from a rectangle, using halfway points as vertices
        /// </summary>
        /// <param name="rect">Rectangle to create the diamond from</param>
        public Diamond(Rectangle rect)
        {
            _rect = rect;

            _left = new Point(rect.X, rect.Y + rect.Height / 2);
            _right = new Point(rect.X + rect.Width, rect.Y + rect.Height / 2);
            _top = new Point(rect.X + rect.Width / 2, rect.Y);
            _bottom = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height);

            _topTri = new Triangle(_left, _top, _right);
            _bottomTri = new Triangle(_left, _bottom, _right);
        }

        /// <summary>
        /// Creates a diamond from a left point and a width and height
        /// </summary>
        /// <param name="left">Leftmost point of the diamond</param>
        /// <param name="width">Width of the diamond</param>
        /// <param name="height">Height of the diamond</param>
        public Diamond(Point left, int width, int height)
        {
            _left = left;
            _right = new Point(left.X + width, left.Y);
            _top = new Point(left.X + width / 2, left.Y - height / 2);
            _top = new Point(left.X + width / 2, left.Y + height / 2);

            _rect = new Rectangle(left.X, left.Y - height / 2, width, height);

            _topTri = new Triangle(_left, _top, _right);
            _bottomTri = new Triangle(_left, _bottom, _right);
        }

        /// <summary>
        /// Checks for a point to be within the diamond
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool Contains(Point loc)
        {
            if(_rect.Contains(loc))
                return (_topTri.Contains(loc) || _bottomTri.Contains(loc));
            return false;
        }
    }
}
