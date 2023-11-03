using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WrongHole
{
    public class Egg : DrawableModel
    {
        public Vector3 Position;

        public Egg(Game game, Matrix world, string modelName) : base(game, world, modelName)
        {
            Position = world.Translation;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 scale, translation;
            Quaternion rotation;
            this.effect.World.Decompose(out scale, out rotation, out translation);
            Debug.WriteLine($"X: {translation.X} Z: {translation.Z}");
            this.effect.World =
                Matrix.CreateScale(scale)
                * Matrix.CreateFromQuaternion(rotation)
                * Matrix.CreateTranslation(
                    Position.X,
                    ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * .5f) + .5f,
                    Position.Z);

            base.Update(gameTime);
        }
    }
}
