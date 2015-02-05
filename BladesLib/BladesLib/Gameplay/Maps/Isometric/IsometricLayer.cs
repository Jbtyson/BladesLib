// IsometricLayer.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BladesLib.Gameplay.Maps.Isometric
{
    public class IsometricLayer : Layer
    {
        public Vector2 TileOffset;
        public int Height;

        public IsometricLayer() 
            : base()
        {
            TileOffset = Vector2.Zero;
        }

        public void LoadContent(Vector2 tileDimensions, Vector2 tileOffset, Vector2 mapDimensions)
        {
            if (Image.Path != String.Empty)
                Image.LoadContent();
            Vector2 position = -tileDimensions;
            position.X += mapDimensions.X;
            position.Y -= Height * tileDimensions.X / 8;
            TileOffset = tileOffset;

            // Save the dimensions of the layer
            LayerDimensions.Y = TMap.Row.Count;
            LayerDimensions.X = TMap.Row[0].Split('[').Length - 1;

            int numTile = 0; int numRows = 0;
            foreach (string row in TMap.Row)
            {
                // Get the tiles
                string[] split = row.Split(']');
                position.X = -tileDimensions.X - tileOffset.X * numRows + mapDimensions.X; ;
                position.Y += tileDimensions.X / 2 - tileOffset.Y * numTile - tileOffset.Y; // tile dimensions.x/2 is a hack to get the sample to work

                // Loop through all of the tiles and load their content from the tile sheet based on val1 and val2
                numTile = 0;
                foreach (string s in split)
                {
                    if (s != String.Empty)
                    {
                        position += tileOffset;
                        if (!s.Contains("x"))
                        {
                            // Create a new tile and add it to the list
                            IsometricTile t = new IsometricTile();
                            string str = s.Replace("[", String.Empty);
                            t.Value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            t.Value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));
                            t.GridLocation = new Point(numTile, numRows);
                            t.Height = Height;
                            t.LoadContent(position, new Rectangle(t.Value1 * (int)tileDimensions.X, t.Value2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y));

                            if (OverlayTiles.Contains("[" + t.Value1.ToString() + ":" + t.Value2.ToString() + "]"))
                                _overlayTiles.Add(t);
                            else
                                _underlayTiles.Add(t);
                        }
                        numTile++;
                    }
                }
                numRows++;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (IsometricTile tile in _underlayTiles)
                tile.Update(gameTime);

            foreach (IsometricTile tile in _overlayTiles)
                tile.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            List<Tile> tiles;
            if (drawType == "Underlay")
                tiles = _underlayTiles;
            else
                tiles = _overlayTiles;


            foreach (IsometricTile tile in tiles)
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
        public IsometricTile[,] GetTileArray2D()
        {
            IsometricTile[,] data = new IsometricTile[(int)LayerDimensions.X, (int)LayerDimensions.Y];

            foreach (IsometricTile t in _underlayTiles)
                data[t.GridLocation.X, t.GridLocation.Y] = t;

            return data;
        }
    }
}
