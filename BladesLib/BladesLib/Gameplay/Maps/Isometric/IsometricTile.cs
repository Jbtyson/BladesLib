// IsometricTile.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Util.Shapes;

namespace BladesLib.Gameplay.Maps.Isometric
{
    /// <summary>
    /// IsometricTile holds the data associated with an individual tile in an isometric layer
    /// </summary>
    public class IsometricTile : Tile
    {
        private int _height;
        private bool _isSurface;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public bool IsSurface
        {
            get { return _isSurface; }
            set { _isSurface = value; }
        }

        public IsometricTile() 
            : base()
        {

        }

        public override void LoadContent(Vector2 position, Rectangle sourceRect)
        {
            base.LoadContent(position, sourceRect);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
