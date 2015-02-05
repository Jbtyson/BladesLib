// OrthogonalMap.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Gameplay.Agents.Actors;

namespace BladesLib.Gameplay.Maps.Orthogonal
{
    /// <summary>
    /// OrthogonalMap represents an orthogonal (top down view) map
    /// </summary>
    public class OrthogonalMap : Map
    {
        [XmlElement("OrthogonalLayer")]
        public List<OrthogonalLayer> Layers;

        public OrthogonalMap() : 
            base()
        {
            Layers = new List<OrthogonalLayer>();
        }

        public override void LoadContent()
        {
            foreach (OrthogonalLayer l in Layers)
            {
                l.LoadContent(TileDimensions, MapDimensions);
            }
        }

        public override void UnloadContent()
        {
            foreach (OrthogonalLayer l in Layers)
            {
                l.UnloadContent();
            }
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (OrthogonalLayer l in Layers)
            {
                l.Update(gameTime, ref player);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (OrthogonalLayer l in Layers)
            {
                l.Draw(spriteBatch, drawType);
            }
        }
    }
}
