using Curupira2D.ECS;
using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Systems
{
    public sealed class GameInitializeSystem : Curupira2D.ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var gameTextures = Scene.GameCore.Content.Load<Texture2D>("SnakeGameTextures");
            SnakeGameHelper.SetGameTextures(gameTextures);
        }
    }
}