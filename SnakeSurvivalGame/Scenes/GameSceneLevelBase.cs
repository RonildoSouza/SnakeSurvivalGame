using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameCore.SetScene<MenuScene>();

            base.Update(gameTime);
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
