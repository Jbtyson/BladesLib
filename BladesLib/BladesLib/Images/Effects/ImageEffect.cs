// ImageEffect.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BladesLib.Images.Effects
{
    /// <summary>
    /// ImageEffect is a base class for all image effects
    /// </summary>
    public class ImageEffect
    {
        protected Image _image;

        public bool IsActive;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageEffect()
        {
            IsActive = false;
        }

        public virtual void LoadContent(ref Image image)
        {
            this._image = image;
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
