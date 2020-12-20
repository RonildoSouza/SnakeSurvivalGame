using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using SnakeGame.Components;
using System;
using System.Linq;

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

        public static Texture2D GameTextures { get; private set; }

        public static void SetGameTextures(Texture2D gameTextures)
            => GameTextures = gameTextures;

        public static Entity CreateSnakePart(Scene scene)
        {
            if (GameTextures == null)
                throw new NullReferenceException($"Property {nameof(GameTextures)} can't be null!");

            var snakeEntityParts = scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakePartIdPrefix));
            var nextIndexSnakePart = snakeEntityParts.Count;

            if (snakeEntityParts.Any())
            {
                var lastSnakeEntityPart = snakeEntityParts[nextIndexSnakePart - 1];
                var snakeSpriteComponent = lastSnakeEntityPart.GetComponent<SpriteComponent>();
                var snakeBodySource = GetSnakeTextureSource(SnakeTexture.BodyHorizontal);
                snakeSpriteComponent.SourceRectangle = snakeBodySource;
            }

            var snakeTailSource = GetSnakeTextureSource(SnakeTexture.TailRight);
            return scene.CreateEntity($"{SnakePartIdPrefix}{nextIndexSnakePart}")
                .AddComponent(new SpriteComponent(GameTextures, sourceRectangle: snakeTailSource))
                .AddComponent(new SnakePartComponent(Vector2.Zero, Vector2.Zero));
        }

        public static Rectangle GetSnakeTextureSource(SnakeTexture snakeTexture)
        {
            switch (snakeTexture)
            {
                case SnakeTexture.HeadLeft:
                    return new Rectangle((int)SnakePixel * 3, (int)SnakePixel, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.HeadUp:
                    return new Rectangle((int)SnakePixel * 3, 0, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.HeadRight:
                    return new Rectangle((int)SnakePixel * 4, 0, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.HeadDown:
                    return new Rectangle((int)SnakePixel * 4, (int)SnakePixel, (int)SnakePixel, (int)SnakePixel);

                case SnakeTexture.BodyHorizontal:
                    return new Rectangle((int)SnakePixel * 1, 0, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.BodyVertical:
                    return new Rectangle((int)SnakePixel * 2, (int)SnakePixel * 1, (int)SnakePixel, (int)SnakePixel);

                case SnakeTexture.BodyLeftUp_DownRight:
                    return new Rectangle(0, (int)SnakePixel * 1, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.BodyDownLeft_RightUp:
                    return new Rectangle((int)SnakePixel * 2, (int)SnakePixel * 2, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.BodyUpRight_LeftDown:
                    return new Rectangle(0, 0, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.BodyRightDown_UpLeft:
                    return new Rectangle((int)SnakePixel * 2, 0, (int)SnakePixel, (int)SnakePixel);

                case SnakeTexture.TailLeft:
                    return new Rectangle((int)SnakePixel * 3, (int)SnakePixel * 3, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.TailUp:
                    return new Rectangle((int)SnakePixel * 3, (int)SnakePixel * 2, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.TailRight:
                    return new Rectangle((int)SnakePixel * 4, (int)SnakePixel * 2, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.TailDown:
                    return new Rectangle((int)SnakePixel * 4, (int)SnakePixel * 3, (int)SnakePixel, (int)SnakePixel);
                case SnakeTexture.Fruit:
                    return new Rectangle(0, (int)SnakePixel * 3, (int)SnakePixel, (int)SnakePixel);
                default:
                    return Rectangle.Empty;
            }
        }
    }

    public enum SnakeTexture
    {
        HeadLeft,
        HeadUp,
        HeadRight,
        HeadDown,

        BodyHorizontal,
        BodyVertical,

        /// <summary>
        /// <para>Snake Position</para>
        /// Last: Left - Down
        /// <para/>
        /// New  Up - Right
        /// </summary>
        BodyLeftUp_DownRight,
        /// <summary>
        /// <para>Snake Position</para>
        /// Last: Down - Right
        /// <para/>
        /// New: Right - Up
        /// </summary>
        BodyDownLeft_RightUp,
        /// <summary>
        /// <para>Snake Position</para>
        /// Last: Up - Left
        /// <para/>
        /// New: Right - Down
        /// </summary>
        BodyUpRight_LeftDown,
        /// <summary>
        /// <para>Snake Position</para>
        /// Last: Right - Up
        /// <para/>
        /// New: Down - Left
        /// </summary>
        BodyRightDown_UpLeft,

        TailLeft,
        TailUp,
        TailRight,
        TailDown,
        Fruit,
    }
}