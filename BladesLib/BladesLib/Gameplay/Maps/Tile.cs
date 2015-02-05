// Tile.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladesLib.Gameplay.Maps
{
    /// <summary>
    /// Base class for tiles in our tile maps
    /// </summary>
    public class Tile
    {
        protected Vector2 _position, _dimensions;
        protected Point _gridLocation;
        protected Rectangle _sourceRect;
        protected int _value1, _value2;

        #region Accessors
        public int Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        public int Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        public Rectangle SourceRect
        {
            get { return _sourceRect; }
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public Point GridLocation
        {
            get { return _gridLocation; }
            set { _gridLocation = value; }
        }
        public Vector2 Dimensions
        {
            get { return _dimensions; }
            set { _dimensions = value; }
        }
        #endregion

        public virtual void LoadContent(Vector2 position, Rectangle sourceRect)
        {
            _position = position;
            _sourceRect = sourceRect;
            _dimensions = new Vector2(sourceRect.Width, sourceRect.Height);
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
