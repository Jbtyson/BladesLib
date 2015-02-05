// MenuManager.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BladesLib.Input;
using BladesLib.Screens;
using BladesLib.Util;

namespace BladesLib.Gui.Menus
{
    /// <summary>
    /// MenuManager helps facilite menu transitions
    /// </summary>
    public class MenuManager
    {
        private Menu _menu;
        private bool _isTransitioning;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MenuManager()
        {
            _menu = new Menu();
            _menu.OnMenuChange += menu_OnMenuChange;
        }

        /// <summary>
        /// Handles a transition between menus
        /// </summary>
        /// <param name="gameTime"></param>
        void Transition(GameTime gameTime)
        {
            if (_isTransitioning)
            {
                for (int i = 0; i < _menu.Items.Count; i++)
                {
                    // Update all of the items, and then grab the first and last alpha values
                    _menu.Items[i].Image.Update(gameTime);
                    float first = _menu.Items[0].Image.Alpha;
                    float last = _menu.Items[_menu.Items.Count - 1].Image.Alpha;

                    // If the transition has faded out the original menu
                    if (first == 0.0f && last == 0.0f)
                        _menu.ID = _menu.Items[_menu.ItemNumber].LinkID;
                    // If the transition has faded in the linked menu
                    else if (first == 1.0f && last == 1.0f)
                    {
                        _isTransitioning = false;
                        foreach (MenuItem item in _menu.Items)
                            item.Image.RestoreEffects();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the onMenuChance event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menu_OnMenuChange(object sender, EventArgs e)
        {
            // Unload the old menu, and load in the new menu
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            _menu.UnloadContent();
            _menu = xmlMenuManager.Load(_menu.ID);
            _menu.LoadContent();
            _menu.OnMenuChange += menu_OnMenuChange;
            _menu.Transition(0.0f);

            // Add a fade effect to all of the images
            foreach (MenuItem item in _menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        public void LoadContent(string menuPath)
        {
            if (menuPath != String.Empty)
                _menu.ID = menuPath;
        }

        public void UnloadContent()
        {
            _menu.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if(!_isTransitioning)
                _menu.Update(gameTime);
            if (InputManager.Instance.KeyPressed(Keys.Enter) && !_isTransitioning)
            {
                // Linked to screen
                if (_menu.Items[_menu.ItemNumber].LinkType == "Screen")
                    ScreenManager.Instance.ChangeScreens(_menu.Items[_menu.ItemNumber].LinkID);
                // Linked to Menu
                else
                {
                    _isTransitioning = true;
                    _menu.Transition(1.0f);
                    foreach (MenuItem item in _menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
                }
            }
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
        }
    }
}
