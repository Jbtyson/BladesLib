// Ally.cs
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

using BladesLib.Gameplay.Agents.Behaviours;
using BladesLib.Gameplay.Maps;
using BladesLib.Input;
using BladesLib.Gameplay.Maps.Isometric;
using BladesLib.Gameplay.Maps.Isometric.Combat;

namespace BladesLib.Gameplay.Agents.Actors
{
    public class Ally : Character
    {
        private bool _isActive;
        private bool _isPlayerControlled;
        private List<Behavior> _behaviors;
        private List<string> _behaviorLoadList;

        #region Accessors
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        public bool IsPlayerControlled
        {
            get { return _isPlayerControlled; }
            set { _isPlayerControlled = value; }
        }
        public List<Behavior> Behaviors
        {
            get { return _behaviors; }
            set { _behaviors = value; }
        }
        [XmlElement("Behavior")]
        public List<string> BehaviorLoadList
        {
            get { return _behaviorLoadList; }
            set { _behaviorLoadList = value; }
        }
        #endregion
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Ally()
            : base() 
        {
            _behaviorLoadList = new List<string>();
        }

        public void LoadContent()
        {
            base.LoadContent();

            // Load behaviors
            foreach (string s in BehaviorLoadList)
            {
                Type t = Type.GetType("BladesLib.Source.Gameplay.Combat.Behaviors." + s);
                _behaviors.Add((Behavior)Activator.CreateInstance(t));
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Set the image to active for now, we will set to false at the end of the update loop if necessary
            _image.IsActive = true;

            // Allow for player control, if player controlled
            if (_isPlayerControlled)
            {
                // Vertical movement
                if (InputManager.Instance.KeyDown(Keys.Down))
                {
                    _velocity.Y = _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _image.SpriteSheetEffect.CurrentFrame.Y = 0;
                }
                else if (InputManager.Instance.KeyDown(Keys.Up))
                {
                    _velocity.Y = -_moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _image.SpriteSheetEffect.CurrentFrame.Y = 3;
                }
                else
                    _velocity.Y = 0;

                //  Horizontal movement
                if (InputManager.Instance.KeyDown(Keys.Right))
                {
                    _velocity.X = _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _image.SpriteSheetEffect.CurrentFrame.Y = 2;
                }
                else if (InputManager.Instance.KeyDown(Keys.Left))
                {
                    _velocity.X = -_moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _image.SpriteSheetEffect.CurrentFrame.Y = 1;
                }
                else
                    _velocity.X = 0;
            }
            // If not player controlled, update the AI behavior
            else
            {
                foreach (Behavior behavior in _behaviors)
                    behavior.Update(gameTime, this);
            }

            // Set the image.isActive to false if the player is standing still so that the correct sprite is displayed
            if (Velocity.X == 0 && Velocity.Y == 0)
                _image.IsActive = false;

            // Update our image
            _image.Update(gameTime);

            // Update our position in the world
            //Image.Position += Velocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _image.Draw(spriteBatch);
        }

        public void CenterIsometric(CombatTile tile)
        {
            _currentTile = tile;

            // Center the duders on the tiles
            _isometricOffset.X = (_currentTile.SourceTile.Dimensions.X - _dimensions.X) / 2; //64 - 32 / 2
            _isometricOffset.Y = -(_currentTile.SourceTile.Dimensions.Y - _dimensions.Y);

            _image.Position = _currentTile.SourceTile.Position + _isometricOffset;
        }
    }
}

