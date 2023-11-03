using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrongHole
{
    /// <summary>
    /// A class representing a crate
    /// </summary>
    public class DrawableModel
    {
        public BasicEffect effect;
        protected Model model;

        /// <summary>
        /// Creates a new crate instance
        /// </summary>
        /// <param name="game">The game this crate belongs to</param>
        /// <param name="world">The position and orientation of the crate in the world</param>
        public DrawableModel(Game game, Matrix world, string modelName, bool lightEnabled = true)
        {
            model = game.Content.Load<Model>(modelName);

            effect = new BasicEffect(game.GraphicsDevice);
            effect.View = Matrix.CreateLookAt(
                new Vector3(8, 9, 12), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                         // The field-of-view
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance
                100.0f // The far plane distance
            );
            effect.TextureEnabled = true;

            // Turn on lighting
            effect.LightingEnabled = lightEnabled;
            // Set up light 0
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = new Vector3(.7f, -1f, 0f);
            effect.DirectionalLight0.DiffuseColor = new Vector3(.5f, .5f, 1f);
            effect.DirectionalLight0.SpecularColor = Vector3.Zero;
            effect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.2f);

            effect.World = world;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the crate
        /// </summary>
        /// <param name="camera">The camera to use to draw the crate</param>
        public void Draw(ICamera camera, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    var effect = this.effect;

                    if (part.PrimitiveCount > 0)
                    {
                        BasicEffect be = effect as BasicEffect;
                        be.Texture = (part.Effect as BasicEffect).Texture;

                        if (be != null)
                        {
                            be.View = camera.View;
                            be.Projection = camera.Projection;

                            be.Alpha = 1;
                        }
                        else effect.Parameters["WorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);

                        graphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        graphicsDevice.Indices = part.IndexBuffer;

                        for (int i = 0; i < effect.CurrentTechnique.Passes.Count; ++i)
                        {
                            effect.CurrentTechnique.Passes[i].Apply();
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                    }
                }
            }
        }
    }
}
