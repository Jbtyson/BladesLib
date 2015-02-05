// Player.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BladesLib.Input;

namespace BladesLib.Gameplay.Agents.Actors
{
    /// <summary>
    /// Player represents the player object in the world
    /// </summary>
    public class Player : GameObject
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Player()
        {
            
        }

        public override void LoadContent()
        {
            base.LoadContent();
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

            // Set the image.isActive to false if the player is standing still so that the correct sprite is displayed
            if (_velocity.X == 0 && _velocity.Y == 0)
                _image.IsActive = false;

            // Update our image
            _image.Update(gameTime);

            // Update our position in the world
            _image.Position += _velocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _image.Draw(spriteBatch);
        }
    }
}
