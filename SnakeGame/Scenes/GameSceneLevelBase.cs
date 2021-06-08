using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using SnakeGame.Systems;
using System.Collections.Generic;

namespace SnakeGame.Scenes
{
    public abstract class GameSceneLevelBase : Scene
    {
        readonly int _scoreToChangeLevel = 100;
        FruitControllerSystem _fruitControllerSystem;
        ScoreControllerSystem _scoreControllerSystem;
        GameSceneLevelBase _nextGameSceneLevel;
        IList<Vector2> _blocksPosition;

        protected int Score { get; set; }

        public override void LoadContent()
        {
            AddSystem<GameInitializeSystem>();
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

            base.LoadContent();
        }

        protected virtual void ScoreControllerSystem_ScoreChange(object sender, ScoreChangeEventArgs e)
        {
            if (_nextGameSceneLevel == null || e.Score % _scoreToChangeLevel != 0)
                return;

            _nextGameSceneLevel.Score = e.Score;
            GameCore.SetScene(_nextGameSceneLevel);
        }

        protected void SetNextGameSceneLevel(GameSceneLevelBase gameSceneLevel)
        {
            _nextGameSceneLevel = gameSceneLevel;
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
