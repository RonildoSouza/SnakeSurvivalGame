using Curupira2D;
using SnakeGame.Scenes;

namespace SnakeGame
{
    public class Game1 : GameCore
    {
        public Game1() : base(360, 600, true) { }

        protected override void LoadContent()
        {
            SetScene<MenuScene>();

            base.LoadContent();
        }
    }
}
