using Microsoft.Xna.Framework;
using MonoGame.Helper;
using MonoGame.Helper.GameComponents;
using SnakeGame.Scenes;

namespace SnakeGame
{
    public class Game1 : GameCore
    {
        public Game1() : base(360, 600, true) { }

        protected override void Initialize()
        {
            SetScene<GameScene>();

            Components.Add(new LineGridComponent(this, new Vector2(24), Color.Red * 0.3f));

            base.Initialize();
        }
    }
}
