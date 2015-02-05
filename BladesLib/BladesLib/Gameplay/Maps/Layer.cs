// Layer.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Images;

namespace BladesLib.Gameplay.Maps
{
    /// <summary>
    /// Base class for both orthogonal and isometric layers
    /// </summary>
    public class Layer
    {
        protected List<Tile> _underlayTiles, _overlayTiles;
        protected string _state;

        /// <summary>
        /// TileMap is a grid of tiles to represent the world
        /// </summary>
        public class TileMap
        {
            [XmlElement("Row")]
            public List<string> Row;

            /// <summary>
            /// Default constructor
            /// </summary>
            public TileMap()
            {
                Row = new List<string>();
            }
        }

        [XmlElement("TileMap")]
        public TileMap TMap;
        public string OverlayTiles;
        public Image Image;

        public Vector2 TileDimensions, LayerDimensions;

        public Layer()
        {
            Image = new Image();
            _overlayTiles = new List<Tile>();
            _underlayTiles = new List<Tile>();
            OverlayTiles = String.Empty;
            TileDimensions = Vector2.Zero;
            LayerDimensions = Vector2.Zero;
        }

        public virtual void LoadContent()
        {
            Image.UnloadContent();
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
