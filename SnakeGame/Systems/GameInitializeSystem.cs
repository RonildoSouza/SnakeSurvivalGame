using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Systems;

namespace SnakeGame.Systems
{
    public sealed class GameInitializeSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            var gameTextures = Scene.GameCore.Content.Load<Texture2D>("SnakeGameTextures");
            SnakeGameHelper.SetGameTextures(gameTextures);
        }
    }
}