// CombatMap.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Gameplay.Maps.Isometric;
using BladesLib.Images;

namespace BladesLib.Gameplay.Maps.Isometric.Combat
{
    /// <summary>
    /// CombatMap stores map data relevant to combat, such as tiles that are impassable, height of tiles, and so on (stored as CombatTiles).
    /// </summary>
    public class CombatMap : ICombatGrid
    {
        private Image _image;
        private CombatTile[,] _grid;
        private HashSet<Point> _moveOverlays;
        private Rectangle _moveEffectRect;
        private Vector2 _tileDimensions, _gridDimensions;

        #region Accessors
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public CombatTile[,] Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }
        public HashSet<Point> MoveOverlays
        {
            get { return _moveOverlays; }
            set { _moveOverlays = value; }
        }
        public Rectangle MoveEffectRect
        {
            get { return _moveEffectRect; }
            set { _moveEffectRect = value; }
        }
        public Vector2 TileDimensions
        {
            get { return _tileDimensions; }
            set { _tileDimensions = value; }
        }
        public Vector2 GridDimensions
        {
            get { return _gridDimensions; }
            set { _gridDimensions = value; }
        }
        #endregion

        public delegate void SetCursor(Vector2 loc);
        [XmlIgnore]
        public SetCursor SetCursorPosition;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CombatMap()
        {
            _grid = new CombatTile[0, 0];
            _tileDimensions = Vector2.Zero;
            _image = new Image();
            _moveEffectRect = new Rectangle();
            _moveOverlays = new HashSet<Point>();
        }

        public void LoadContent(IsometricMapInfo mapInfo, SetCursor setCursor)
        {
            SetCursorPosition = setCursor;

            _tileDimensions = mapInfo.TileDimensions;
            _gridDimensions = mapInfo.LayerDimensions;
            _grid = new CombatTile[(int)GridDimensions.X, (int)GridDimensions.Y];

            // Load the content for each combat tile and store it in the grid
            for (int y = 0; y < GridDimensions.Y; y++)
            {
                for (int x = 0; x < GridDimensions.X; x++)
                {
                    _grid[x, y] = new CombatTile();
                    _grid[x, y].LoadContent(mapInfo.Tiles[x, y]);
                    _grid[x, y].GridPosition = new Point(x, y);
                }
            }

            // Laod the tilesheet
            _image.Path = "Gameplay/TileSheets/ground";
            _image.LoadContent();
            _moveEffectRect = new Rectangle(0, 0, 32, 32); // Hardcoded, remove later
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            foreach (CombatTile tile in _grid)
            {
                tile.Update(gameTime);
                if (tile.HasCursor)
                    SetCursorPosition(tile.SourceTile.Position);
            }
        }

        /// <summary>
        /// Draws the move range of an agent
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMoveRange(SpriteBatch spriteBatch)
        {
            foreach (Point p in MoveOverlays)
            {
                _image.Position = new Vector2(p.X*32, p.Y*32);
                _image.SourceRect = MoveEffectRect;
                _image.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Adds the move range of an agent to the list of overlays to draw
        /// </summary>
        /// <param name="range">The range of the agent</param>
        /// <param name="location">Location of the agent</param>
        public void DisplayMoveRange(int range, Point location)
        {
            for (int y = 0; y < range; y++)
            {
                for (int x = 0; x < range; x++)
                {
                    if (x + y < range)
                    {
                        _moveOverlays.Add(new Point((int)location.X + x, (int)location.Y + y));
                        _moveOverlays.Add(new Point((int)location.X - x, (int)location.Y - y));
                        _moveOverlays.Add(new Point((int)location.X + x, (int)location.Y - y));
                        _moveOverlays.Add(new Point((int)location.X - x, (int)location.Y + y));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a tile at a given location, can return null
        /// </summary>
        /// <param name="loc">Location to look for a tile</param>
        /// <returns>The tile containing the point</returns>
        public CombatTile GetTileAtPosition(Point loc, int height)
        {
            foreach (CombatTile tile in _grid)
            {
                if (tile.Contains(loc) && tile.Height == height)
                    return tile;
            }
            return null;
        }

        /// <summary>
        /// Returns a tile at a given location
        /// </summary>
        /// <param name="loc">Location to look for a tile</param>
        /// <returns>The tile containing the point</returns>
        public CombatTile GetTileAtPosition(Vector2 loc, int height)
        {
            return GetTileAtPosition(new Point((int)loc.X, (int)loc.Y), height);
        }

        /// <summary>
        /// Returns the grid
        /// </summary>
        /// <returns></returns>
        public CombatTile[,] GetGrid()
        {
            return _grid;
        }

        public CombatTile this[int x, int y]
        {
            get { return _grid[x, y]; }
            set { _grid[x, y] = value; }
        }
    }
}
