// Camera.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BladesLib.Screens
{
    /// <summary>
    /// Camera.cs provides base functionality to a camera
    /// </summary>
    public class Camera
    {
        private Matrix _transform;

        public Vector2 Position;
        public float Rotation;
        private float _zoom;
        public Point WorldPosition;
        public Vector2 WorldChange;

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }
        public Matrix Transform { get { return _transform; } }

        /// <summary>
        /// Default Constrcutor
        /// </summary>
        public Camera()
        {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            WorldPosition = Point.Zero;
        }

        /// <summary>
        /// Moves the camera by a Vector2D
        /// </summary>
        /// <param name="amount">Vector to move camera along</param>
        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        /// <summary>
        /// Sets the top left corner of the screen to the world position
        /// </summary>
        /// <param name="position"></param>
        public void SetWorldPosition(Vector2 position) 
        {
            Position.X = ScreenManager.Instance.Dimensions.X / 2 + position.Y;
            Position.Y = ScreenManager.Instance.Dimensions.Y / 2 + position.X;
            WorldPosition = new Point((int)position.X, (int)position.Y);    // changing this to position from Position fixed the ui panel bug...may cause problems later we'll see
        }

        /// <summary>
        /// Gets the camera transformation
        /// </summary>
        /// <param name="graphicsDevice">Graphics Device</param>
        /// <returns>Camera Transformation Matrix</returns>
        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
             _transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(ScreenManager.Instance.Dimensions.X * 0.5f, ScreenManager.Instance.Dimensions.Y * 0.5f, 0));
            _transform.M41 = (float)(Math.Round(_transform.M41)); // removes horizontal white stripes betwwen tiles
            _transform.M42 = (float)(Math.Round(_transform.M42)); // removes vertical white stripes between tiles
            
            // Update the world position
            WorldChange = new Vector2((int)_transform.M41 - WorldPosition.X, (int)_transform.M42 - WorldPosition.Y);
            WorldPosition.X = (int)_transform.M41;
            WorldPosition.Y = (int)_transform.M42;
            
            return _transform;
        }
    }
}
