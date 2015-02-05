// Image.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BladesLib.Screens;
using BladesLib.Images.Effects;

namespace BladesLib.Images
{
    /// <summary>
    /// Image allows us to manipulate and add effects to images
    /// </summary>
    public class Image
    {
        private Vector2 _origin;
        private ContentManager _content;
        private RenderTarget2D _renderTarget;
        private SpriteFont _font;
        private Dictionary<string, ImageEffect> _effectList;

        private Texture2D _texture;
        private float _alpha;
        private bool _isActive;
        private string _text, _fontName, _path, _effects;
        private Vector2 _position, _scale;
        private Rectangle _sourceRect;
        
        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;

        #region Accessors
        [XmlIgnore]
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        public float Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        } 
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        } 
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        } 
        public string Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        public Rectangle SourceRect
        {
            get { return _sourceRect; }
            set { _sourceRect = value; }
        }
        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Image()
        {
            Path = Text = Effects = string.Empty;
            FontName = "Fonts/Font";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            _effectList = new Dictionary<string, ImageEffect>();
        }

        /// <summary>
        /// Sets an effect to an image
        /// </summary>
        /// <typeparam name="T">Type of the effect</typeparam>
        /// <param name="effect">Effect being added</param>
        void SetEffect<T>(ref T effect)
        {
            // Instantiate the effect if necessary
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            // Load the effect
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            // Add it to the list of effects
            _effectList.Add(effect.GetType().ToString().Replace("BladesLib.Source.Images.Effects.", ""), (effect as ImageEffect));
        }

        /// <summary>
        /// Activate a given effect
        /// </summary>
        /// <param name="effect">Id of the effect to activate</param>
        public void ActivateEffect(string effect)
        {
            // Check for the effect to exist
            if (_effectList.ContainsKey(effect))
            {
                _effectList[effect].IsActive = true;
                var obj = this;
                _effectList[effect].LoadContent(ref obj);
            }
        }

        /// <summary>
        /// Deactivate a given effect
        /// </summary>
        /// <param name="effect">Id of the effect to deactivate</param>
        public void DeactivateEffect(string effect)
        {
            if (_effectList.ContainsKey(effect))
            {
                _effectList[effect].IsActive = false;
                _effectList[effect].UnloadContent();
            }
        }

        /// <summary>
        /// Store all of the effects on an image
        /// </summary>
        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach (var effect in _effectList)
            {
                if (effect.Value.IsActive)
                    Effects += effect.Key + ":";
            }

            if(Effects != String.Empty)
                Effects.Remove(Effects.Length - 1);
        }

        /// <summary>
        /// Restore all of the effects on an image
        /// </summary>
        public void RestoreEffects()
        {
            foreach (var effect in _effectList)
            {
                DeactivateEffect(effect.Key);
            }

            string[] split = Effects.Split(':');
            foreach (string s in split)
                ActivateEffect(s);
        }

        public void LoadContent()
        {
            // Get the content
            _content = ScreenManager.Instance.Content;

            // Load the texture
            if (Path != string.Empty)
                Texture = _content.Load<Texture2D>(Path);

            // Load the font
            _font = _content.Load<SpriteFont>(FontName);

            // Get the dimensions
            Vector2 dimensions = Vector2.Zero;
            if (Texture != null)
                dimensions.X += Texture.Width;
            dimensions.X += _font.MeasureString(Text).X;
            if(Texture != null)
                dimensions.Y = Math.Max(Texture.Height, _font.MeasureString(Text).Y);
            else
                dimensions.Y = _font.MeasureString(Text).Y;

            // Get the source rect of the image
            if(SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            // Do graphics magic to draw things
            _renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(_renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if(Texture != null)
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.DrawString(_font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            // Store the render target and then reset it
            Texture = _renderTarget;
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            // Handle effects
            SetEffect<FadeEffect>(ref FadeEffect);
            SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

            // Activate image effects
            if (Effects != String.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (string item in split)
                    ActivateEffect(item);
            }
        }

        public void UnloadContent()
        {
            _content.Unload();
            foreach (var effect in _effectList)
                DeactivateEffect(effect.Key);
        }

        public void Update(GameTime gametime)
        {
            foreach (var effect in _effectList)
            {
                if(effect.Value.IsActive)
                    effect.Value.Update(gametime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            spriteBatch.Draw(Texture, Position + _origin, SourceRect, Color.White * Alpha, 0.0f, _origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}