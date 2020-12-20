using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Systems;

namespace SnakeGame.Systems
{
    public sealed class GameInitializeSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            //var snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            //snakeTexture.SetData(new Color[] { Color.Black });

            var gameTextures = Scene.GameCore.Content.Load<Texture2D>("GameTextures");
            SnakeHelper.SetGameTextures(gameTextures);
        }
    }
}