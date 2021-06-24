using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Systems;
using System.Collections.Generic;

namespace SnakeSurvivalGame.Scenes
{
    public abstract class GameSceneLevelBase : Scene
    {
        readonly int _scoreToChangeLevel = 1000;
        FruitControllerSystem _fruitControllerSystem;
        ScoreControllerSystem _scoreControllerSystem;
        IList<Vector2> _blocksPosition;

        // Myra
        Desktop _desktop;
        bool _escapePressed;

        public GameSceneLevelBase NextGameSceneLevel { get; private set; }
        protected int Score { get; set; }

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

            SnakeSurvivalGameHelper.CleanBlockEntityPositions();
#if DEBUG
            AddSystem<DebugSystem>();
#endif

            _desktop = new Desktop();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (!_escapePressed && KeyboardInputManager.IsKeyPressed(Keys.Escape))
            {
                _escapePressed = true;
                GameCore.IsMouseVisible = true;

                this.ShowConfirmDialog(
                    title: "Menu?",
                    message: "Would you like to return to menu?",
                    yesAction: () => GameCore.SetScene<MenuScene>(),
                    noAction: () =>
                    {
                        _escapePressed = false;
                        GameCore.IsMouseVisible = false;
                    },
                    desktop: _desktop);
            }

            KeyboardInputManager.End();

            if (!_escapePressed)
                base.Update(gameTime);
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

            NextGameSceneLevel.Score = e.Score;
            GameCore.SetScene(NextGameSceneLevel);
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
