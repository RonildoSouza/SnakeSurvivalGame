using Curupira2D;
using Curupira2D.GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    public class Game1 : GameCore
    {
        public Game1() : base(360, 600, true) { }

        protected override void Initialize()
        {
            SetScene<GameSceneLevel01>();

#if DEBUG
            Components.Add(new LineGridComponent(this, new Vector2(24), Color.Red * 0.3f));
#endif

            base.Initialize();
        }

#if DEBUG
        Microsoft.Xna.Framework.Input.KeyboardState _oldKeyState;
        readonly Stack<GameSceneLevelBase> _prevGameSceneLevels = new Stack<GameSceneLevelBase>();

        protected override void Update(GameTime gameTime)
        {
            var keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemPlus) && _oldKeyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.OemPlus))
            {
                var gameSceneLevel = GetCurrentScene<GameSceneLevelBase>();

                if (gameSceneLevel == null)
                    return;

                _prevGameSceneLevels.Push(gameSceneLevel);

                if (gameSceneLevel.NextGameSceneLevel != null)
                    SetScene(gameSceneLevel.NextGameSceneLevel);
                else
                    SetScene<GameSceneLevel01>();
            }

            if (_prevGameSceneLevels.Any() && keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemMinus) && _oldKeyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                SetScene(_prevGameSceneLevels.Pop());

            _oldKeyState = keyState;

            base.Update(gameTime);
        }
#endif
    }
}
