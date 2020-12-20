using MonoGame.Helper.ECS;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public sealed class GameScene : Scene
    {
        public override void Initialize()
        {
            SetTitle("Snake Black Power");

            AddSystem<GameInitializeSystem>();
            AddSystem<SnakeInitializeSystem>();
            //AddSystem<FruitControllerSystem>();
            AddSystem<SnakeHeadControllerSystem>();
            AddSystem<SnakePartControllerSystem>();

            base.Initialize();
        }
    }
}
