using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WrongHole
{
    public class BadEgger : DrawableModel
    {
        public Vector2 Position;

        public Quaternion Rotation;

        public BadEgger(Game game, Matrix world, string modelName) : base(game, world, modelName)
        {
            Quaternion rotation;
            world.Decompose(out _, out rotation, out _);
            Position = new Vector2(world.Translation.X, world.Translation.Z);
            Rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 scale, translation;
            this.effect.World.Decompose(out scale, out _, out translation);
            Debug.WriteLine($"X: {translation.X} Z: {translation.Z}");
            this.effect.World =
                Matrix.CreateScale(scale)
                * Matrix.CreateFromQuaternion(Rotation)
                * Matrix.CreateTranslation(
                    Position.X,
                    ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * .5f) + .5f,
                    Position.Y);

            base.Update(gameTime);
        }
    }
}
