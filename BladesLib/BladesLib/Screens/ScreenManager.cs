// ScreenManager.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Util;
using BladesLib.Images;

namespace BladesLib.Screens
{
    /// <summary>
    /// ScreenManager is a class to help facilitate screen transistions as well
    /// as manage the current screen
    /// </summary>
    public class ScreenManager
    {
        private static ScreenManager _instance;
        private XmlManager<GameScreen> _xmlGameScreenManager;
        private GameScreen _currentScreen, _newScreen;

        public Image Image;

        [XmlIgnore]
        public Camera Camera;

        [XmlIgnore]
        public Vector2 Dimensions { private set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        /// <summary>
        /// Singleton class instance
        /// </summary>
        public static ScreenManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    _instance = xml.Load("Load/Screens/ScreenManager.xml");
                    //instance = new ScreenManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScreenManager()
        {
            Dimensions = new Vector2(1024, 576);
            Camera = new Camera();
            //currentScreen = new SplashScreen();
            _currentScreen = new GameplayScreen();
            _xmlGameScreenManager = new XmlManager<GameScreen>();
            _xmlGameScreenManager.Type = _currentScreen.Type;
            //_currentScreen = _xmlGameScreenManager.Load("Load/Screens/SplashScreen.xml");
            IsTransitioning = false;
        }

        /// <summary>
        /// Handles a screen changes
        /// </summary>
        /// <param name="screenName">name of the class to load</param>
        public void ChangeScreens(string screenName)
        {
            _newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("BladesLib.Source.Screens." + screenName));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        /// <summary>
        /// Handles a screen transition with a Fade Effect
        /// </summary>
        /// <param name="gameTime"></param>
        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    _currentScreen.UnloadContent();
                    _currentScreen = _newScreen;
                    _xmlGameScreenManager.Type = _currentScreen.Type;
                    if(File.Exists(_currentScreen.XmlPath))
                        _currentScreen = _xmlGameScreenManager.Load(_currentScreen.XmlPath);
                    _currentScreen.LoadContent();
                }
                else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }

        /// <summary>
        /// Loads Content
        /// </summary>
        /// <param name="Content"></param>
        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            _currentScreen.LoadContent();
            Image.LoadContent();
        }

        /// <summary>
        /// Unloads Content
        /// </summary>
        public void UnloadContent()
        {
            _currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        /// <summary>
        /// Update the current screen, and if necessary updates the transition
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        /// <summary>
        /// Draws the current screen, and if necessary draws the transitition
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(spriteBatch);
            if (IsTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}

