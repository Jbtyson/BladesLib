// SplashScreen.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using BladesLib.Images;
using BladesLib.Input;

namespace BladesLib.Screens
{
    /// <summary>
    /// SplashScreen is a screen that simply displays an image at the start of the game while it loads
    /// </summary>
    public class SplashScreen : GameScreen
    {
        [XmlElement("Image")]
        public Image image;

        public override void LoadContent()
        {
            base.LoadContent();
            image.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            image.Update(gameTime);

            // Transition to title screen on enter key
            // TODO: In the future make this automatic
            if (InputManager.Instance.KeyPressed(Keys.Enter) && !ScreenManager.Instance.IsTransitioning)
                ScreenManager.Instance.ChangeScreens("TitleScreen");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }
    }
}
