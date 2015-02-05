// TitleScreen.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Gui.Menus;

namespace BladesLib.Screens
{
    /// <summary>
    /// TitleScreen displays the title menu
    /// </summary>
    public class TitleScreen : GameScreen
    {
        private MenuManager _menuManager;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TitleScreen()
        {
            _menuManager = new MenuManager();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            // Load the title menu
            _menuManager.LoadContent("Load/Menus/TitleMenu.xml");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            _menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _menuManager.Draw(spriteBatch);
        }

    }
}
