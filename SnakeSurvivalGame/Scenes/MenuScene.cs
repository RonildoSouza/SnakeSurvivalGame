using Curupira2D.ECS;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Systems;

namespace SnakeSurvivalGame.Scenes
{
    public class MenuScene : Scene
    {
        Desktop _desktop;

        public override void LoadContent()
        {
            _desktop = new Desktop();

            AddSystem<GameInitializeSystem>();
            AddSystem(new MenuSystem(_desktop));

            base.LoadContent();
        }

        public override void Draw()
        {
            base.Draw();
            _desktop.Render();
        }
    }
}
