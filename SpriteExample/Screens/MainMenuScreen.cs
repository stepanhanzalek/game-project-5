using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WrongHole.StateManagement;

namespace WrongHole.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        private List<DrawableModel> models;

        private MenuCamera camera;

        public MainMenuScreen() : base("Egger")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += ConfirmExit;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Activate()
        {
            models = new List<DrawableModel>
            {
                new DrawableModel(ScreenManager.Game, Matrix.CreateScale(2f), "sky", false),
                new DrawableModel(ScreenManager.Game, Matrix.CreateScale(2f), "loading_map"),
                new DrawableModel(ScreenManager.Game, Matrix.CreateScale(2f), "loading_eggs"),
                new BadEgger(ScreenManager.Game, Matrix.CreateScale(4f) * Matrix.CreateTranslation(new Vector3(50, 0, 6)) * Matrix.CreateRotationY(MathHelper.ToRadians(45)), "egger"),
                new DrawableModel(ScreenManager.Game, Matrix.CreateScale(60f) * Matrix.CreateTranslation(new Vector3(150, -40, 0)) * Matrix.CreateRotationY(MathHelper.ToRadians(-15)), "egger_bad"),
            };

            camera = new MenuCamera(ScreenManager.Game, new Vector3(0, 10, 10));

            base.Activate();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (var model in models) model.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var model in models) model.Draw(camera, ScreenManager.GraphicsDevice);
            base.Draw(gameTime);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "How to play?" +
                                    "\n- Try to collect most eggs" +
                                    "\n- Do not get eaten by the" +
                                    "\nevil eggers!!!" +
                                    "\n- Be quick";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += PlayGame;

            ScreenManager.AddScreen(confirmExitMessageBox, null);
        }

        private void PlayGame(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LevelScreen(ScreenManager.Game), null);
            ExitScreen();
        }

        private void ConfirmExit(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
