// IsometricMapInfo.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace BladesLib.Gameplay.Maps.Isometric
{
    /// <summary>
    /// IsometricMapInfo holds information about
    /// </summary>
    public class IsometricMapInfo
    {
        private IsometricTile[,] _tiles;
        private Vector2 _tileDimensions, _layerDimensions;

        public IsometricTile[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }
        public Vector2 TileDimensions
        {
            get { return _tileDimensions; }
            set { _tileDimensions = value; }
        }
        public Vector2 LayerDimensions
        {
            get { return _layerDimensions; }
            set { _layerDimensions = value; }
        }

        public IsometricMapInfo()
        {
            _tiles = new IsometricTile[0,0];
            _layerDimensions = _tileDimensions = Vector2.Zero;
        }

        public void LoadContent(Vector2 layerDimensions, Vector2 tileDimensions) 
        {
            _layerDimensions = layerDimensions;
            _tileDimensions = tileDimensions;
            _tiles = new IsometricTile[(int)layerDimensions.X, (int)layerDimensions.Y];

            for (int y = 0; y < layerDimensions.Y; y++)
            {
                for (int x = 0; x < layerDimensions.X; x++)
                {
                    _tiles[x, y] = new IsometricTile();
                }
            }
        }

        /// <summary>
        /// Adds an isometric layer to the map info, so that the new layer will be considered the surface
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayer(IsometricLayer layer)
        {
            IsometricTile[,] layerArray = layer.GetTileArray2D();

            // Assign each tile in the layer so that if there is a tile above the previous one, it will overwrite it
            foreach (IsometricTile tile in layerArray)
            {
                if(tile != null)
                    _tiles[tile.GridLocation.X, tile.GridLocation.Y] = tile;
            }
        }
    }
}
