using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Components;
using System;

namespace SnakeGame
{
    public static class SnakeGameHelper
    {
        public const string SnakeHeadId = "snakeHead";
        public const string SnakePartIdPrefix = "snakePart";
        public const string FruitId = "fruit";
        public const float PixelSize = 24;
        public const float PixelSizeHalf = PixelSize * 0.5f;

        public static Vector2 LeftDirection => new Vector2(-PixelSize, 0f);
        public static Vector2 UpDirection => new Vector2(0f, -PixelSize);
        public static Vector2 RightDirection => new Vector2(PixelSize, 0f);
        public static Vector2 DownDirection => new Vector2(0f, PixelSize);

        public static Texture2D SnakeGameTextures { get; private set; }

        public static void SetGameTextures(Texture2D gameTextures)
            => SnakeGameTextures = gameTextures;

        public static Entity CreateSnakePart(Scene scene)
        {
            if (SnakeGameTextures == null)
                throw new NullReferenceException($"Property {nameof(SnakeGameTextures)} can't be null!");

            var snakeEntityParts = scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakePartIdPrefix));
            var nextIndexSnakePart = snakeEntityParts.Count;

            var snakeTailSource = GetSnakeTextureSource(SnakeTexture.Body);
            return scene.CreateEntity($"{SnakePartIdPrefix}{nextIndexSnakePart}")
                .AddComponent(new SpriteComponent(SnakeGameTextures, sourceRectangle: snakeTailSource))
                .AddComponent(new SnakePartComponent(Vector2.Zero, Vector2.Zero))
                .SetPosition(new Vector2(-PixelSize));
        }

        public static Rectangle GetSnakeTextureSource(SnakeTexture snakeTexture)
        {
            return snakeTexture switch
            {
                SnakeTexture.Head => new Rectangle(0, 0, (int)PixelSize, (int)PixelSize),
                SnakeTexture.Body => new Rectangle(0, 0, (int)PixelSize, (int)PixelSize),
                SnakeTexture.Fruit => new Rectangle((int)PixelSize, 0, (int)PixelSize, (int)PixelSize),
                SnakeTexture.Mouse => new Rectangle((int)PixelSize * 2, 0, (int)PixelSize, (int)PixelSize),
                SnakeTexture.Block => new Rectangle(0, (int)PixelSize, (int)PixelSize * 3, (int)PixelSize * 3),
                _ => Rectangle.Empty,
            };
        }
    }

    public enum SnakeTexture
    {
        Head,
        Body,
        Fruit,
        Mouse,
        Block
    }
}