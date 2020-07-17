using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using SnakeGame.Components;

namespace SnakeGame
{
    public static class SnakeHelper
    {
        public const string SnakeHeadId = "snakeHead";
        public const string SnakePartIdPrefix = "snakePart";
        public const float SnakePixel = 24;

        public static Vector2 LeftDirection => new Vector2(-SnakePixel, 0f);
        public static Vector2 UpDirection => new Vector2(0f, -SnakePixel);
        public static Vector2 RightDirection => new Vector2(SnakePixel, 0f);
        public static Vector2 DownDirection => new Vector2(0f, SnakePixel);

        public static Entity CreateSnakePart(Scene scene, Texture2D snakeTexture)
        {
            var nextIndexSnakePart = scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakePartIdPrefix)).Count;

            return scene.CreateEntity($"{SnakePartIdPrefix}{nextIndexSnakePart}")
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 24, 24)))
                .AddComponent<SnakePartComponet>();
        }
    }
}