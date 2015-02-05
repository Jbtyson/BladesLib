// Menu.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BladesLib.Screens;
using BladesLib.Input;

namespace BladesLib.Gui.Menus
{
    /// <summary>
    /// Menu provides basic menu functionality
    /// </summary>
    public class Menu
    {
        private int _itemNumber;
        private string _id;

        /// <summary>
        /// Event that handles a menu being changed
        /// </summary>
        public event EventHandler OnMenuChange;

        [XmlElement("Item")]
        public List<MenuItem> Items;
        public string Axis;
        public string Effects;

        public int ItemNumber
        {
            get { return _itemNumber; }
        }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnMenuChange(this, null);
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Menu()
        {
            _id = String.Empty;
            _itemNumber = 0;
            Effects = String.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        /// <summary>
        /// Handle a menu transition
        /// </summary>
        /// <param name="alpha"></param>
        public void Transition(float alpha)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                if (alpha == 0.0f)
                    item.Image.FadeEffect.Increase = true;
                else
                    item.Image.FadeEffect.Increase = false;
            }
        }

        /// <summary>
        /// Aligns the menu items centered along an axis
        /// </summary>
        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            // Add the size of each item to the dimensions and find the middle
            foreach (MenuItem item in Items)
                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X - dimensions.X) / 2,
                (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2);

            // Place each item in its correct spot based on the axis of alignment
            foreach (MenuItem item in Items)
            {
                if (Axis == "X")
                    item.Image.Position = new Vector2(dimensions.X,
                        (ScreenManager.Instance.Dimensions.Y - item.Image.SourceRect.Height) / 2);
                else if (Axis == "Y")
                    item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X - 
                        item.Image.SourceRect.Width) / 2, dimensions.Y);
                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }

        }

        public void LoadContent()
        {
            // Load all of the effects
            string[] split = Effects.Split(':');
            foreach (MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach (string s in split)
                    item.Image.ActivateEffect(s);
            }
            AlignMenuItems();
        }

        public void UnloadContent()
        {
            foreach (MenuItem item in Items)
                item.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            // Change the item number based on input
            if (Axis == "X")
            {
                if (InputManager.Instance.KeyPressed(Keys.Right))
                    _itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.Left))
                    _itemNumber--;
            }
            else if (Axis == "Y")
            {
                if (InputManager.Instance.KeyPressed(Keys.Down))
                    _itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.Up))
                    _itemNumber--;
            }

            // Clamp the menu selection
            if (_itemNumber < 0)
                _itemNumber = 0;
            else if (_itemNumber > Items.Count - 1)
                _itemNumber = Items.Count - 1;

            // Highlight the item currently selected
            for (int i = 0; i < Items.Count; i++)
            {
                if (i == _itemNumber)
                    Items[i].Image.IsActive = true;
                else
                    Items[i].Image.IsActive = false;

                Items[i].Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
                item.Image.Draw(spriteBatch);
        }
    }
}
