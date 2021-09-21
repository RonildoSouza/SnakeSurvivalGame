using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Helpers;
using SnakeSurvivalGame.Systems;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeSurvivalGame.Scenes
{
    public abstract class GameSceneLevelBase : Scene
    {
#if DEBUG
        readonly int _scoreToChangeLevel = 150;
#else
        readonly int _scoreToChangeLevel = 1000;
#endif
        FruitControllerSystem _fruitControllerSystem;
        ScoreControllerSystem _scoreControllerSystem;
        IList<Vector2> _blocksPosition;
        Entity _nextLevelEntity;

        // Myra
        Desktop _desktop;
        bool _escapePressed;

        public GameSceneLevelBase NextGameSceneLevel { get; private set; }

        public override void LoadContent()
        {
            AddSystem<SnakeInitializeSystem>();
            AddSystem<SnakeHeadControllerSystem>();
            AddSystem<SnakePartControllerSystem>();
            AddSystem(new BlockControllerSystem(_blocksPosition));

            _fruitControllerSystem = new FruitControllerSystem();
            _scoreControllerSystem = new ScoreControllerSystem();

            _fruitControllerSystem.SnakeEatFruit += _scoreControllerSystem.ChangeScore;
            _scoreControllerSystem.ScoreChange += ScoreControllerSystem_ScoreChange;

            AddSystem(_fruitControllerSystem);
            AddSystem(_scoreControllerSystem);

#if DEBUG
            AddSystem<DebugSystem>();
#endif

            var options = new List<(string, Action)>
            {
                ("Continue", () =>
                {
                    _escapePressed = false;
                    GameCore.IsMouseVisible = false;
                    _desktop.Root.Visible = false;
                }),
                ("Menu", () => { this.ShowMenuConfirmDialog(_desktop); }),
                ("Quit", () => { this.ShowQuitConfirmDialog(noAction: null, desktop: _desktop); }),
            };

            var menuOptions = this.MenuOptionsBuilder("Paused", options, Color.Gray * 0.8f);
            menuOptions.Visible = false;

            _desktop = new Desktop { Root = menuOptions };

            var spriteFont = this.GetGameFont("MainText");
            _nextLevelEntity = CreateEntity(nameof(_nextLevelEntity), ScreenCenter, isCollidable: false)
                .AddComponent(new SpriteComponent(GameCore.GraphicsDevice.CreateTextureRectangle(ScreenSize, Color.Gray * 0.7f), layerDepth: .9f))
                .AddComponent(new TextComponent(spriteFont, $"NEXT LEVEL...", color: Color.Black, drawInUICamera: false, layerDepth: 1f, scale: new Vector2(0.7f)))
                .SetActive(false);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (_nextLevelEntity.Active)
            {
                Thread.Sleep(3000);
                PauseUpdatableSystems = true;
                GameCore.SetScene(NextGameSceneLevel);
            }

            if (!_escapePressed && KeyboardInputManager.IsKeyPressed(Keys.Escape))
            {
                _escapePressed = true;
                GameCore.IsMouseVisible = true;
                _desktop.Root.Visible = true;
            }

            if (!_escapePressed)
                base.Update(gameTime);

            KeyboardInputManager.End();
        }

        public override void Draw()
        {
            base.Draw();
            _desktop.Render();
        }

        protected virtual void ScoreControllerSystem_ScoreChange(object sender, ScoreChangeEventArgs e)
        {
            if (NextGameSceneLevel == null || e.Score % _scoreToChangeLevel != 0)
                return;

            _nextLevelEntity.SetActive(true);
            GetEntity(SnakeSurvivalGameHelper.FruitId).SetActive(false);
        }

        protected void SetNextGameSceneLevel(GameSceneLevelBase gameSceneLevel)
        {
            NextGameSceneLevel = gameSceneLevel;
        }

        protected void AddBlockPosition(Vector2 position)
        {
            if (_blocksPosition == null)
            {
                _blocksPosition = new List<Vector2> { position };
                return;
            }

            _blocksPosition.Add(position);
        }

        public override void Dispose()
        {
            _fruitControllerSystem.SnakeEatFruit -= _scoreControllerSystem.ChangeScore;
            _scoreControllerSystem.ScoreChange -= ScoreControllerSystem_ScoreChange;

            base.Dispose();
        }
    }
}
