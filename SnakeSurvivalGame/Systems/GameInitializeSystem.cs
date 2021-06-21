using Curupira2D.ECS;
using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeSurvivalGame.Systems
{
    public sealed class GameInitializeSystem : Curupira2D.ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var SnakeSurvivalGameTextures = Scene.GameCore.Content.Load<Texture2D>("Textures/SnakeSurvivalGame");
            var mouseCursorTextures = Scene.GameCore.Content.Load<Texture2D>("Textures/MouseCursors");

            SnakeSurvivalGameHelper.SetSnakeSurvivalGameTextures(SnakeSurvivalGameTextures);
            SnakeSurvivalGameHelper.SetMouseCursorTextures(mouseCursorTextures);
        }
    }
}