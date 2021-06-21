using Curupira2D.ECS;
using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Systems
{
    public sealed class GameInitializeSystem : Curupira2D.ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var snakeGameTextures = Scene.GameCore.Content.Load<Texture2D>("Textures/SnakeGame");
            var mouseCursorTextures = Scene.GameCore.Content.Load<Texture2D>("Textures/MouseCursors");

            SnakeGameHelper.SetSnakeGameTextures(snakeGameTextures);
            SnakeGameHelper.SetMouseCursorTextures(mouseCursorTextures);
        }
    }
}