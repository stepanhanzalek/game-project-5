using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using WrongHole.Screens;
using WrongHole.StateManagement;

namespace WrongHole
{
    /// <summary>
    /// A game demonstrating the use of sprites
    /// </summary>
    public class SpriteExampleGame : Game
    {
        public static int bestScore = 0;
        private readonly ScreenManager screenManager;
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public SpriteExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            graphics.ApplyChanges();

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            this.screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            MediaPlayer.IsRepeating = true;
            bestScore = int.Parse(File.ReadAllText(Constants.SCORE_PATH).Trim());
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            File.WriteAllText(Constants.SCORE_PATH, bestScore.ToString());

            base.UnloadContent();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new MainMenuScreen(), null);
        }
    }
}
