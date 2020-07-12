using MonoGame.Helper;
using SnakeGame.Scenes;

namespace SnakeGame
{
    public class Game1 : GameCore
    {
        public Game1() : base(720, 480, true) { }

        protected override void Initialize()
        {
            SetScene<GameScene>();

            base.Initialize();
        }
    }
}
