using Curupira2D.ECS;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public sealed class GameScene02 : Scene
    {
        private readonly int _score;

        public GameScene02(int score)
        {
            _score = score;
        }

        public override void LoadContent()
        {
            SetTitle($"{nameof(GameScene02)}");

            AddSystem<GameInitializeSystem>();
            AddSystem<SnakeInitializeSystem>();
            AddSystem<SnakeHeadControllerSystem>();
            AddSystem<SnakePartControllerSystem>();

            var fruitControllerSystem = new FruitControllerSystem();
            var scoreControllerSystem = new ScoreControllerSystem();

            fruitControllerSystem.SnakeEatFruit += scoreControllerSystem.ChangeScore;
            scoreControllerSystem.SetScore(_score);

            AddSystem(fruitControllerSystem);
            AddSystem(scoreControllerSystem);

            base.LoadContent();
        }
    }
}
