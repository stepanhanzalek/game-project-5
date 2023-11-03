using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ParticleSystemExample;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    public class LevelScreen : GameScreen
    {
        private List<DrawableModel> models;

        private List<DrawableModel> minimap;

        private List<BadEgger> badEggers;

        private BadEgger egger;

        private Egg egg;

        private FPSCamera camera;

        private TopDownCamera miniMapCamera;

        private int score = 0;

        private bool isOver = false;

        private SoundEffect coin;

        private SoundEffect hurt;

        public LevelScreen(Game game, bool isLast = false)
        {
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            this.egg = new Egg(ScreenManager.Game, Matrix.CreateTranslation(RandomHelper.Next(90) - 45f, 0, RandomHelper.Next(90) - 45f), "egg");

            var drawables = new List<DrawableModel>
            {
                this.egg,
                new DrawableModel(ScreenManager.Game, Matrix.CreateScale(2f), "map"),
            };

            models = new List<DrawableModel>
            {
                new DrawableModel(ScreenManager.Game, Matrix.CreateScale(2f), "sky", false),
            };
            models.AddRange(drawables);

            this.egger = new BadEgger(ScreenManager.Game, Matrix.CreateTranslation(0, 0, 0), "egger");

            minimap = new List<DrawableModel>
            {
                this.egger,
            };
            minimap.AddRange(drawables);

            badEggers = new List<BadEgger> { };

            camera = new FPSCamera(ScreenManager.Game, new Vector3(0, 3, 0));
            miniMapCamera = new TopDownCamera(ScreenManager.Game, new Vector3(0, 1000, 0));

            coin = ScreenManager.Game.Content.Load<SoundEffect>("pickupCoin");
            hurt = ScreenManager.Game.Content.Load<SoundEffect>("hitHurt");
            base.Activate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (isOver) return;

            foreach (var model in models) model.Update(gameTime);
            camera.Update(gameTime);

            if (UpdateAI() && !isOver) Die();

            if (Vector3.Distance(camera.position, egg.Position) < 4)
            {
                coin.Play();
                ++score;
                egg.Position = new Vector3(RandomHelper.Next(90) - 45f, 0, RandomHelper.Next(90) - 45f);
                var badEgger = new BadEgger(ScreenManager.Game, Matrix.CreateTranslation(RandomHelper.Next(90) - 45f, 0, RandomHelper.Next(90) - 45f), "egger_bad");
                badEggers.Add(badEgger);
                models.Add(badEgger);
                minimap.Add(badEgger);
            }

            Quaternion rotation;
            Vector3 translation;
            camera.View.Decompose(out _, out rotation, out translation);

            egger.Position = new Vector2(camera.position.X, camera.position.Z);
            //egger.Rotation = rotation;
            egger.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var model in models) model.Draw(camera, ScreenManager.GraphicsDevice);
            //Draw the minimap
            foreach (var model in minimap) model.Draw(miniMapCamera, ScreenManager.GraphicsDevice);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontSmall, $"score: {score}", Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        protected bool UpdateAI()
        {
            foreach (var be in badEggers)
            {
                Vector3 posEgger = new Vector3(be.Position.X, 0, be.Position.Y);

                var m = Matrix.CreateLookAt(posEgger, camera.position, Vector3.Down);

                // Calculate the direction vector between the two points
                Vector3 direction = posEgger - new Vector3(camera.position.X, 0, camera.position.Z);

                // Calculate the Y-axis rotation (yaw) using the arctangent function
                float rotationAngle = (float)Math.Atan2(direction.X, direction.Z) - MathHelper.ToRadians(90);

                be.Position += new Vector2(m.Forward.X, m.Forward.Z) / 10;
                be.Rotation = Quaternion.CreateFromYawPitchRoll(rotationAngle, 0, 0);

                if (Vector3.Distance(posEgger, camera.position) < 3.5f) return true;
            }

            return false;
        }

        private void PlayGame(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LevelScreen(ScreenManager.Game), null);
            ExitScreen();
        }

        private void MainMenu(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen(), null);
            ExitScreen();
        }

        private void Die()
        {
            hurt.Play();

            SpriteExampleGame.bestScore = Math.Max(SpriteExampleGame.bestScore, score);

            isOver = true;
            string message = $"Score: {score}" +
                                $"\nBest score: {SpriteExampleGame.bestScore}" +
                                "\n\nEnter -> play again" +
                                "\nBackspace -> main menu";

            var confirmExitMessageBox = new MessageBoxScreen(message, false);

            confirmExitMessageBox.Accepted += PlayGame;
            confirmExitMessageBox.Cancelled += MainMenu;

            ScreenManager.AddScreen(confirmExitMessageBox, null);
        }
    }
}
