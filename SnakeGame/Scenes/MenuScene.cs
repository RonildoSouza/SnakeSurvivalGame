using Curupira2D.ECS;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public class MenuScene : Scene
    {
        public override void LoadContent()
        {
            AddSystem<GameInitializeSystem>();
            AddSystem<MenuSystem>();

            base.LoadContent();
        }
    }
}
