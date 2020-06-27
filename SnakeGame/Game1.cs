using MonoGame.Helper;
using SnakeGame.Scenes;

namespace SnakeGame
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            SetScene<GameScene>();

            base.Initialize();
        }
    }
}
