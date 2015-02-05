// SpriteSheetEffect.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BladesLib.Images.Effects
{
    /// <summary>
    /// SpriteSheetEffect animates basic spritesheets
    /// </summary>
    public class SpriteSheetEffect : ImageEffect
    {
        public int FrameCounter;
        public int SwitchFrame;
        public Vector2 CurrentFrame;
        public Vector2 AmountOfFrames;

        public int FrameWidth
        {
            get
            {
                if (_image.Texture != null)
                    return _image.Texture.Width / (int)AmountOfFrames.X;
                else
                    return 0;
            }
        }

        public int FrameHeight
        {
            get
            {
                if (_image.Texture != null)
                    return _image.Texture.Height / (int)AmountOfFrames.Y;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SpriteSheetEffect()
        {
            AmountOfFrames = new Vector2(3, 4);
            CurrentFrame = new Vector2(1, 0);
            SwitchFrame = 100;
            FrameCounter = 0;
        }

        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Check for the effect to be active
            if (_image.IsActive)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                // Update animation
                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    CurrentFrame.X++;
                    // Prevent overflow
                    if (CurrentFrame.X * FrameWidth >= _image.Texture.Width)
                        CurrentFrame.X = 0;
                }
            }
            // Reset animation if inactive
            else
                CurrentFrame.X = 1;
            
            // Set the size of the image
            _image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth, (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
