// GameScreen.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Input;

namespace BladesLib.Screens
{
    /// <summary>
    /// GameScreen represents a base class for all screens
    /// </summary>
    public class GameScreen
    {
        protected ContentManager _content;

        [XmlIgnore]
        public Type Type;
        public string XmlPath;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameScreen()
        {
            Type = this.GetType();
            XmlPath = "Load/" +Type.ToString().Replace("BladesLib.", "") + ".xml";
        }

        public virtual void LoadContent()
        {
            _content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            _content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

