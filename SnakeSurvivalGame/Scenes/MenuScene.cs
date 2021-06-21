using Curupira2D.ECS;
using SnakeSurvivalGame.Systems;

namespace SnakeSurvivalGame.Scenes
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
