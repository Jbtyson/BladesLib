// IsometricMap.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BladesLib.Gameplay.Maps.Isometric
{
    public class IsometricMap : Map
    {
        private IsometricMapInfo _surfaceInfo;

        [XmlElement("IsometricLayer")]
        public List<IsometricLayer> Layers;
        public Vector2 TileOffset;

        public IsometricMapInfo SurfaceInfo
        {
            get { return _surfaceInfo; }
        }

        public IsometricMap()
            : base()
        {
            Layers = new List<IsometricLayer>();
            _surfaceInfo = new IsometricMapInfo();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _surfaceInfo.LoadContent(LayerDimensions, TileDimensions);

            foreach (IsometricLayer l in Layers)
            {
                l.LoadContent(TileDimensions, TileOffset, MapDimensions);
                _surfaceInfo.AddLayer(l);
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            foreach (IsometricLayer l in Layers)
                l.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (IsometricLayer l in Layers)
                l.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, string drawType)
        {
            base.Draw(spriteBatch, drawType);

            foreach (IsometricLayer l in Layers)
                l.Draw(spriteBatch, drawType);
        }
    }
}
