using Microsoft.Xna.Framework;

namespace WrongHole
{
    /// <summary>
    /// A camera that circles the origin
    /// </summary>
    public class MenuCamera : ICamera
    {
        // The view matrix
        private Matrix view;

        // The projection matrix
        private Matrix projection;

        /// <summary>
        /// Constructs a new camera that circles the origin
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="position">The initial position of the camera</param>
        public MenuCamera(Game game, Vector3 position)
        {
            this.projection =
                Matrix.CreatePerspectiveFieldOfView(
                    1.1f,
                    game.GraphicsDevice.Viewport.AspectRatio,
                    1,
                    1000
                );
            this.view = Matrix.CreateLookAt(
                new Vector3(-8, 10, 0),
                new Vector3(10, 13, 0),
                Vector3.Forward
            )
                * Matrix.CreateRotationZ(MathHelper.ToRadians(90));
        }

        /// <summary>
        /// The camera's view matrix
        /// </summary>
        public Matrix View => view;

        /// <summary>
        /// The camera's projection matrix
        /// </summary>
        public Matrix Projection => projection;
    }
}
