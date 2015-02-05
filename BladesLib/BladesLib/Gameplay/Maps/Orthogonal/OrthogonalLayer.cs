// OrthogonalLayer.cs
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
    /// OrthogonalLayer represents one layer of a map
    /// </summary>
    public class OrthogonalLayer : Layer
    {
        public string SolidTiles;

        public OrthogonalLayer() 
            : base()
        {
            SolidTiles = string.Empty;
        }

        public void LoadContent(Vector2 tileDimensions, Vector2 mapDimensions) 
        {
            if(Image.Path != String.Empty)
                Image.LoadContent();
            Vector2 position = -tileDimensions;
            position.X += mapDimensions.X;

            // Save the dimensions of the layer
            LayerDimensions.Y = TMap.Row.Count;
            LayerDimensions.X = TMap.Row[0].Split('[').Length-1;

            foreach (string row in TMap.Row)
            {
                // Get the tiles
                string[] split = row.Split(']');
                position.X = -tileDimensions.X + mapDimensions.X; ;
                position.Y += tileDimensions.Y;

                // Loop through all of the tiles and load their content from the tile sheet based on val1 and val2
                foreach (string s in split)
                {
                    if (s != String.Empty)
                    {
                        _state = "Passive";
                        position.X += tileDimensions.X;
                        if (!s.Contains("x"))
                        {
                            // Create a new tile and add it to the list
                            OrthogonalTile t = new OrthogonalTile();
                            string str = s.Replace("[", String.Empty);
                            t.Value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            t.Value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));
                            if (SolidTiles.Contains("[" + t.Value1.ToString() + ":" + t.Value2.ToString() + "]"))
                                _state = "Solid";
                            t.LoadContent(_state, position, new Rectangle(t.Value1 * (int)tileDimensions.X, t.Value2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y));

                            if (OverlayTiles.Contains("[" + t.Value1.ToString() + ":" + t.Value2.ToString() + "]"))
                                _overlayTiles.Add(t);
                            else
                                _underlayTiles.Add(t);
                        }
                    }
                }
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (OrthogonalTile tile in _underlayTiles)
                tile.Update(gameTime, ref player);

            foreach (OrthogonalTile tile in _overlayTiles)
                tile.Update(gameTime, ref player);
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            base.Draw(spriteBatch);

            List<Tile> tiles;
            if (drawType == "Underlay")
                tiles = _underlayTiles;
            else
                tiles = _overlayTiles;
            
            
            foreach (OrthogonalTile tile in tiles)
            {
                Image.Position = tile.Position;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Converts the tilemap into a two dimensional Tile array
        /// </summary>
        /// <returns>2D Tile array</returns>
        public OrthogonalTile[,] GetTileArray2D()
        {
            OrthogonalTile[,] data = new OrthogonalTile[(int)LayerDimensions.X, (int)LayerDimensions.Y];
            int x = 0; int y = 0;
            foreach (OrthogonalTile t in _underlayTiles)
            {
                data[x,y] = t;
                x++;
                if (x > LayerDimensions.X - 1)
                {
                    x = 0;
                    y++;
                }
            }
            return data;
        }
    }
}
