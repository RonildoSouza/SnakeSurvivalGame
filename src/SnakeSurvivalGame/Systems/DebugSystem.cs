#if DEBUG

using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeSurvivalGame.Helpers;
using SnakeSurvivalGame.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(DebugSystem), typeof(TextComponent))]
    public sealed class DebugSystem : Curupira2D.ECS.System, ILoadable, IUpdatable
    {
        Entity _debugEntity;
        TextComponent _debugTextComponent;
        static readonly Stack<GameSceneLevelBase> _prevGameSceneLevels = new Stack<GameSceneLevelBase>();
        static bool _enabled = false;
        LineGridComponent _lineGridComponent;

        public void LoadContent()
        {
            _debugTextComponent = new TextComponent(SnakeSurvivalGameHelper.ScoreFont, string.Empty, color: Color.Black, layerDepth: 1f, scale: new Vector2(0.5f));

            _debugEntity = Scene.CreateEntity("debug", new Vector2(120f, 120f), isCollidable: false)
                .AddComponent(_debugTextComponent);

            _lineGridComponent = new LineGridComponent(Scene.GameCore, new Vector2(24), Color.Red * 0.3f);
        }

        public void Update()
        {
            if (Scene.KeyboardInputManager.IsKeyPressed(Keys.F1))
            {
                _enabled = !_enabled;
                Scene.AddGameComponent(_lineGridComponent);
            }

            if (_enabled)
            {
                var totalSnakeParts = Scene.GetEntities(SnakeSurvivalGameHelper.SnakeGroupName).Count;
                var fruitEntity = Scene.GetEntity(SnakeSurvivalGameHelper.FruitId);

                _debugTextComponent.Text = $"***DEBUG PANEL***" +
                                           $"\nSnake Size: {totalSnakeParts}" +
                                           $"\nFruit Position: {fruitEntity?.Position}";

                _debugEntity.SetPositionY(_debugTextComponent.TextSize.Y);

                ChangeToNextLevel();
            }
            else
            {
                _debugTextComponent.Text = string.Empty;
                Scene.RemoveGameComponent(_lineGridComponent);
            }
        }

        private void ChangeToNextLevel()
        {
            if (Scene.KeyboardInputManager.IsKeyPressed(Keys.OemPlus))
            {
                if (!(Scene is GameSceneLevelBase gameSceneLevel))
                    return;

                _prevGameSceneLevels.Push(gameSceneLevel);

                if (gameSceneLevel.NextGameSceneLevel != null)
                    Scene.GameCore.SetScene(gameSceneLevel.NextGameSceneLevel);
                else
                    Scene.GameCore.SetScene<GameSceneLevel01>();
            }

            if (_prevGameSceneLevels.Any() && Scene.KeyboardInputManager.IsKeyPressed(Keys.OemMinus))
                Scene.GameCore.SetScene(_prevGameSceneLevels.Pop());
        }
    }
}

#endif