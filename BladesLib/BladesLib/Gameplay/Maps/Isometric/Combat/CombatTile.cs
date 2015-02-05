// CombatTile.cs
// Ben Stegeman
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
using BladesLib.Gameplay.Maps;
using BladesLib.Gameplay.Maps.Isometric;
using BladesLib.Util.Shapes;
using BladesLib.Gameplay.Agents.Actors;
using BladesLib.Input;

namespace BladesLib.Gameplay.Maps.Isometric.Combat
{
    /// <summary>
    /// CombatTile holds map data relevant to one specific tile on a map, such as whether it's impassable, height of the tile, etc.
    /// </summary>
    public class CombatTile
    {
        private IsometricTile _sourceTile;
        private Diamond _hitbox;
        private Character _agentOccupying;
        private Point _gridPosition;
        private Vector2 _position;
        private bool _hasCursor, _occupiedByAgent;
        private int _type, _height;

        //Just examples of potential tile properties. We'll need some more obvs.
        private bool _walkable;
        private bool _damaging;

        #region Accessors
        public IsometricTile SourceTile
        {
            get { return _sourceTile; }
            set { _sourceTile = value; }
        }
        public Point GridPosition
        {
            get { return _gridPosition; }
            set { _gridPosition = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public bool HasCursor
        {
            get { return _hasCursor; }
            set { _hasCursor = value; }
        }
        public bool OccupiedByAgent
        {
            get { return _occupiedByAgent; }
            set { _occupiedByAgent = value; }
        }
        public Character AgentOccupying
        {
            get { return _agentOccupying; }
            set { _agentOccupying = value; }
        }
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public bool Walkable
        {
            get { return _walkable; }
            set { _walkable = value; }
        }
        public bool Damaging
        {
            get { return _damaging; }
            set { _damaging = value; }
        }
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public CombatTile() {
            _hitbox = new Diamond();
            _sourceTile = new IsometricTile();
            _agentOccupying = null; // need to find a way to make this not null
            _gridPosition = Point.Zero;
            _position = Vector2.Zero;
        }

        public void LoadContent(IsometricTile tile)
        {
            _sourceTile = tile;
            _hitbox = new Diamond(new Rectangle((int)tile.Position.X, (int)tile.Position.Y, tile.SourceRect.Width, tile.SourceRect.Height));
            _type = tile.Value1;
            _height = tile.Height;
            _position = tile.Position;

            //We can come up with many types, not just 3. (Double digit ints and negative ints can be read as well.)
            switch (Type)
            {
                case 1:
                    _walkable = true;
                    _damaging = false;
                    break;
                case 2:
                    _walkable = true;
                    _damaging = false;
                    break;
                case 3:
                    _walkable = true;
                    _damaging = false;
                    break;
                default:
                    _walkable = false;
                    _damaging = false;
                    break;
            }
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if (Contains(InputManager.Instance.MousePosition))
                _hasCursor = true;
            else
                _hasCursor = false;
        }

        public bool Contains(Point loc)
        {
            return _hitbox.Contains(loc);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
