using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        protected override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.R))
                SetScene<GameScene>();

            base.Update(gameTime);
        }
    }
}
