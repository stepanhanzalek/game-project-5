using Microsoft.Xna.Framework;

namespace WrongHole
{
    /// <summary>
    /// A camera that circles the origin
    /// </summary>
    public class TopDownCamera : ICamera
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
        public TopDownCamera(Game game, Vector3 position)
        {
            this.projection =
                Matrix.CreatePerspectiveFieldOfView(
                    .1f,
                    game.GraphicsDevice.Viewport.AspectRatio,
                    900,
                    1200
                )
                * Matrix.CreateScale(.25f)
                * Matrix.CreateTranslation(.8f, .7f, 0);
            this.view = Matrix.CreateLookAt(
                position,
                Vector3.Zero,
                Vector3.Forward
            );
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
