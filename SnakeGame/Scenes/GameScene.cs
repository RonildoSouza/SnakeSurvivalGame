using MonoGame.Helper.ECS;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public sealed class GameScene : Scene
    {
        public override void Initialize()
        {
            //SetCleanColor(new Color(94, 90, 90));

            AddSystem<GameInitializeSystem>();
            AddSystem<SnakeInitializeSystem>();
            AddSystem<SnakeHeadControllerSystem>();
            AddSystem<SnakePartControllerSystem>();
            AddSystem<FruitControllerSystem>();

            base.Initialize();
        }
    }
}
