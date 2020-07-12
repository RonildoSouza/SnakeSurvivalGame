using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using SnakeGame.Components;

namespace SnakeGame
{
    public static class SnakeHelper
    {
        public const float SnakePixel = 24;
        public static Vector2 LeftDirection => new Vector2(-SnakePixel, 0f);
        public static Vector2 UpDirection => new Vector2(0f, -SnakePixel);
        public static Vector2 RightDirection => new Vector2(SnakePixel, 0f);
        public static Vector2 DownDirection => new Vector2(0f, SnakePixel);

        public static Entity CreateSnakePart(Scene scene)
        {
            var snakeTexture = new Texture2D(scene.GameCore.GraphicsDevice, 1, 1);
            snakeTexture.SetData(new Color[] { Color.Black });

            var nextIndexSnakePart = scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith("snakePart")).Count;

            return scene.CreateEntity($"snakePart{nextIndexSnakePart}")
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 24, 24)))
                .AddComponent<SnakePartComponet>();
        }
    }
}