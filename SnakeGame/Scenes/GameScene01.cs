using Curupira2D.ECS;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public sealed class GameScene01 : Scene
    {
        public override void LoadContent()
        {
            SetTitle($"{nameof(GameScene01)}");

            AddSystem<GameInitializeSystem>();
            AddSystem<SnakeInitializeSystem>();
            AddSystem<SnakeHeadControllerSystem>();
            AddSystem<SnakePartControllerSystem>();

            var fruitControllerSystem = new FruitControllerSystem();
            var scoreControllerSystem = new ScoreControllerSystem();

            fruitControllerSystem.SnakeEatFruit += scoreControllerSystem.ChangeScore;
            scoreControllerSystem.ScoreChange += ScoreControllerSystem_ScoreChange; ;

            AddSystem(fruitControllerSystem);
            AddSystem(scoreControllerSystem);

            base.LoadContent();
        }

        private void ScoreControllerSystem_ScoreChange(object sender, ScoreChangeEventArgs e)
        {
            if (!(e.Score % 100 == 0))
                return;

            GameCore.SetScene<GameScene02>(e.Score);
        }
    }
}
