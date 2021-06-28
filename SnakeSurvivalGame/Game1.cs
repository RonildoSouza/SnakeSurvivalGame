using Curupira2D;
using Myra;
using SnakeSurvivalGame.Scenes;

namespace SnakeSurvivalGame
{
    public class Game1 : GameCore
    {
        public Game1()
            : base(
                  width: 360,
                  height: 600,
#if DEBUG
                  debugActive: true,
#else
                  debugActive: false, 
#endif
                  disabledExit: true)
        {
            Window.IsBorderless = false;
        }

        protected override void LoadContent()
        {
            // Myra
            MyraEnvironment.Game = this;

            SetScene<MenuScene>();

            base.LoadContent();
        }
    }
}
