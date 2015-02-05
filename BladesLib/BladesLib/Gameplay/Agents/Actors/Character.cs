// Character.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BladesLib.Gameplay.Maps;
using BladesLib.Gameplay.Agents.Behaviours;
using BladesLib.Gameplay.Maps.Isometric.Combat;
using BladesLib.Input;

namespace BladesLib.Gameplay.Agents.Actors
{
    /// <summary>
    /// Character is a base class for all players/enemies in a combat scenario
    /// </summary>
    public class Character : GameObject
    {
        protected CombatTile _currentTile, _destinationTile;
        protected ICombatGrid _combatGrid;
        protected Vector2 _dimensions, _isometricOffset;
        protected Vector2 _destination;
        protected Point _location;
        protected bool _moving, _busy, _myTurn, _aligned, _reachedNode, _displayMoveRange;
        protected List<Point> _movementNodes;
        protected int _moveRange;
        protected Activity _currentAction;
        protected Direction _movingDirection;

        #region Accessors
        public Vector2 Dimensions
        {
            get { return _dimensions; }
            set { _dimensions = value; }
        }
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public bool Moving
        {
            get { return _moving; }
            set { _moving = value; }
        }
        public bool Busy
        {
            get { return _busy; }
            set { _busy = value; }
        }
        public bool MyTurn
        {
            get { return _myTurn; }
            set { _myTurn = value; }
        }
        public bool Aligned
        {
            get { return _aligned; }
            set { _aligned = value; }
        }
        public bool ReachedNode
        {
            get { return _reachedNode; }
            set { _reachedNode = value; }
        }
        public bool DisplayMoveRange
        {
            get { return _displayMoveRange; }
            set { _displayMoveRange = value; }
        }
        public List<Point> MovementNodes
        {
            get { return _movementNodes; }
            set { _movementNodes = value; }
        }
        public int MoveRange
        {
            get { return _moveRange; }
            set { _moveRange = value; }
        }
        [XmlIgnore]
        public Activity CurrentAction
        {
            get { return _currentAction; }
            set { _currentAction = value; }
        }
        public Direction MovingDirection
        {
            get { return _movingDirection; }
            set { _movingDirection = value; }
        }
        #endregion

        public delegate void TurnChange();
        [XmlIgnore]
        public TurnChange TurnIsOver;

        public enum Direction { None, Northwest, Northeast, Southeast, Southwest }
        

        /// <summary>
        /// Default constructor
        /// </summary>
        public Character()
        {
            _destination = _dimensions =_isometricOffset = Vector2.Zero;
            _location = Point.Zero;
            _currentTile = _destinationTile = new CombatTile();
            _movementNodes = new List<Point>();
            _currentAction = Activity.None;
        }

        /// <summary>
        /// Loads content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Unloads content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates based on gametime
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_myTurn)
            {
                if (_moving)
                {
                    this.Move(gameTime);
                    _displayMoveRange = false;
                }
                
                if (!Busy)
                {
                    if (InputManager.Instance.KeyPressed(Keys.M))
                        _displayMoveRange = !_displayMoveRange;
                }
            }
        }

        /// <summary>
        /// Draws the current spritebatch to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Calculates a path to and potentially moves an agent to a new destination on a grid
        /// </summary>
        /// <param name="newLoc">New location to move the agent to</param>
        /// <param name="beginMove">If true, immediately begins executing the move</param>
        public virtual void GetPathTo(Point newLoc, bool beginMove)
        {
            // Find the horizontal node to match x positions and then move to the end location
            if (newLoc.X != Location.X)
            {
                Point horNode = new Point((int)newLoc.X, (int)Location.Y);
                _movementNodes.Add(horNode);
            }
            if (newLoc.Y != Location.Y)
            {
                Point endNode = new Point((int)newLoc.X, (int)newLoc.Y);
                _movementNodes.Add(endNode);
            }

            if (beginMove)
            {
                this.ExecuteMove();
            }
        }

        /// <summary>
        /// Handles player position during a move
        /// </summary>
        /// <param name="gameTime"></param>
        public void Move(GameTime gameTime)
        {
            // Get our new position and location, and update our current tile if our location has changed
            _image.Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Point newLoc = _combatGrid.GetTileAtPosition(_image.Position + _dimensions / 2, _currentTile.Height).GridPosition;
            if (!_reachedNode && _location != newLoc)
            {
                _location = newLoc;
                _currentTile = _combatGrid.GetTileAtPosition(_image.Position + _dimensions / 2, _currentTile.Height);
            }

            // Check if we reaced a node
            if (_reachedNode || _currentTile == _destinationTile)
            {
                _reachedNode = true;

                // Now we need to make sure that our image is moving to align with the top left corner of the square
                // Left up
                if (_movingDirection == Direction.Northwest)
                {
                    if (_image.Position.X <= _destinationTile.Position.X + _isometricOffset.X)
                    {
                        _image.Position = _destinationTile.Position + _isometricOffset;
                        _aligned = true;
                    }
                }
                // Right up
                else if (_movingDirection == Direction.Northeast)
                {
                    if (_image.Position.X >= _destinationTile.Position.X + _isometricOffset.X)
                    {
                        _image.Position = _destinationTile.Position + _isometricOffset;
                        _aligned = true;
                    }
                }
                // Right down
                else if (_movingDirection == Direction.Southeast)
                {
                    if (_image.Position.X >= _destinationTile.Position.X + _isometricOffset.X)
                    {
                        _image.Position = _destinationTile.Position + _isometricOffset;
                        _aligned = true;
                    }
                }
                // Left down
                else if (_movingDirection == Direction.Southwest)
                {
                    if (_image.Position.X <= _destinationTile.Position.X + _isometricOffset.X)
                    {
                        _image.Position = _destinationTile.Position + _isometricOffset;
                        _aligned = true;
                    }
                }

                if (_aligned)
                {
                    _movementNodes.RemoveAt(0);
                    Console.WriteLine(">> Reached Node <<");

                    // Check if we reached the last node
                    if (_movementNodes.Count == 0)
                    {
                        _velocity = Vector2.Zero;
                        _moving = false;
                        _busy = false;
                        Console.WriteLine(">>> Movement complete <<<");
                        this.TurnIsOver();
                    }
                    else
                    {
                        this.ExecuteMove();
                    }
                    _aligned = false;
                    _reachedNode = false;
                }
            }
        }

        /// <summary>
        /// Executes a move to the next node in the node list
        /// </summary>
        public void ExecuteMove()
        {
            if (_movementNodes.Count == 0)
                return;

            _moving = true;
            _busy = true;
            _aligned = false;

            // Calculate the velocity
            Vector2 diff = new Vector2(MovementNodes[0].X - (int)_location.X, _movementNodes[0].Y - (int)_location.Y);
            
            if (diff.X > 0)
            {
                _velocity.X = _moveSpeed;
                _velocity.Y = _moveSpeed / 2;
                _image.SpriteSheetEffect.CurrentFrame.Y = 2;
                _movingDirection = Direction.Southeast;
            }
            else if (diff.X < 0)
            {
                _velocity.X = -_moveSpeed;
                _velocity.Y = -_moveSpeed / 2;
                _image.SpriteSheetEffect.CurrentFrame.Y = 1;
                _movingDirection = Direction.Northwest;
            }
            else if (diff.Y > 0)
            {
                _velocity.X = -_moveSpeed;
                _velocity.Y = _moveSpeed / 2;
                _image.SpriteSheetEffect.CurrentFrame.Y = 0;
                _movingDirection = Direction.Southwest;
            }
            else if (diff.Y < 0)
            {
                _velocity.X = _moveSpeed;
                _velocity.Y = -_moveSpeed / 2;
                _image.SpriteSheetEffect.CurrentFrame.Y = 3;
                _movingDirection = Direction.Northeast;
            }

            // Save our destination tile
            _destinationTile = _combatGrid[_movementNodes[0].X, _movementNodes[0].Y];
        }
    }
}
