using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WrongHole
{
    /// <summary>
    /// A camera controlled by WASD + Mouse
    /// </summary>
    public class FPSCamera : ICamera
    {
        // The camera's position in the world
        public Vector3 position;

        // The angle of rotation about the Y-axis
        private float horizontalAngle;

        // The angle of rotation about the X-axis
        private float verticalAngle;

        // The state of the mouse in the prior frame
        private MouseState oldMouseState;

        // The Game this camera belongs to
        private Game game;

        /// <summary>
        /// Constructs a new FPS Camera
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="position">The player's initial position</param>
        public FPSCamera(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position;

            this.horizontalAngle = 0;
            this.verticalAngle = 0;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(2f, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);

            //Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }

        /// <summary>
        /// The view matrix for this camera
        /// </summary>
        public Matrix View { get; protected set; }

        /// <summary>
        /// The projection matrix for this camera
        /// </summary>
        public Matrix Projection { get; protected set; }

        /// <summary>
        /// The sensitivity of the mouse when aiming
        /// </summary>
        public float Sensitivity { get; set; } = 0.0018f;

        /// <summary>
        /// The speed of the player while moving
        /// </summary>
        public float Speed { get; set; } = 0.5f;

        /// <summary>
        /// Updates the camera
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            // Get the direction the player is currently facing
            var facing = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(horizontalAngle));

            var newPosition = position;

            // Forward and backward movement
            if (keyboard.IsKeyDown(Keys.W)) newPosition += facing * Speed;
            if (keyboard.IsKeyDown(Keys.S)) newPosition -= facing * Speed;

            // Strifing movement
            if (keyboard.IsKeyDown(Keys.A)) newPosition += Vector3.Cross(Vector3.Up, facing) * Speed;
            if (keyboard.IsKeyDown(Keys.D)) newPosition -= Vector3.Cross(Vector3.Up, facing) * Speed;

            // Adjust horizontal angle
            horizontalAngle += Sensitivity * (oldMouseState.X - newMouseState.X);

            // Adjust vertical angle
            verticalAngle += Sensitivity * (oldMouseState.Y - newMouseState.Y);

            var direction = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(verticalAngle) * Matrix.CreateRotationY(horizontalAngle));

            // create the veiw matrix
            if (newPosition.X < 47 && newPosition.X > -47 && newPosition.Z < 47 && newPosition.Z > -47)
            {
                position = newPosition;
                View = Matrix.CreateLookAt(newPosition, newPosition + direction, Vector3.Up);
            }

            // Reset mouse state
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }
    }
}
