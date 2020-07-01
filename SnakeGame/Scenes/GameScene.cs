using MonoGame.Helper.ECS;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public sealed class GameScene : Scene
    {
        public override void Initialize()
        {
            AddSystem<SnakeControllerSystem>();
            AddSystem<SnakePartSystem>();

            base.Initialize();
        }
    }
}
