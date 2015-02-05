// GameplayScreen.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BladesLib.Gameplay;
using BladesLib.Gameplay.Agents.Actors;
using BladesLib.Gameplay.Maps.Orthogonal;
using BladesLib.Input;
using BladesLib.Util;

namespace BladesLib.Screens
{
    /// <summary>
    /// Gameplay Screen represents the screen that the player "playes the game" in
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        private Player _player;
        private OrthogonalMap _map;

        public override void LoadContent()
        {
            base.LoadContent();

            // Load the player
            XmlManager<Player> playerLoader = new XmlManager<Player>();
            _player = playerLoader.Load("Load/Gameplay/Characters/Player.xml");
            _player.LoadContent();

            // Load the OrthogonalMap
            XmlManager<OrthogonalMap> MapLoader = new XmlManager<OrthogonalMap>();
            _map = MapLoader.Load("Load/Gameplay/Maps/Map1.xml");
            _map.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            _player.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update player and then map so that the players actions matter this frame
            _player.Update(gameTime);
            _map.Update(gameTime, ref _player);

            // Change screen on enter key
            if (InputManager.Instance.KeyPressed(Keys.Enter) && !ScreenManager.Instance.IsTransitioning)
            {
                //ScreenManager.Instance.ChangeScreens("CombatScreen");
            }               
 
            // Update the camera position to our new position
            ScreenManager.Instance.Camera.Position = _player.Image.Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw the underlaying tiles, the player, and then the overlaying tiles
            _map.Draw(spriteBatch, "Underlay");
            _player.Draw(spriteBatch);
            _map.Draw(spriteBatch, "Overlay");
        }
    }
}
