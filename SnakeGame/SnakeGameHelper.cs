using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    internal static class SnakeGameHelper
    {
        internal const string SnakeHeadId = "snakeHead";
        internal const string SnakePartIdPrefix = "snakePart";
        internal const string FruitId = "fruit";
        internal const float PixelSize = 24;
        internal const float PixelSizeHalf = PixelSize * 0.5f;
        internal const string BlockGroupName = "blocks";
        internal const string SnakeGroupName = "snakeParts";

        internal static Vector2 LeftDirection => new Vector2(-PixelSize, 0f);
        internal static Vector2 UpDirection => new Vector2(0f, PixelSize);
        internal static Vector2 RightDirection => new Vector2(PixelSize, 0f);
        internal static Vector2 DownDirection => new Vector2(0f, -PixelSize);

        static IEnumerable<Vector2> _blockEntityPositions;

        internal static Texture2D SnakeGameTextures { get; private set; }

        internal static void SetGameTextures(Texture2D gameTextures) => SnakeGameTextures = gameTextures;

        internal static Rectangle GetSnakeTextureSource(SnakeTexture snakeTexture)
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

        internal static void CleanBlockEntityPositions() => _blockEntityPositions = null;

        #region Extension Methods
        internal static Entity CreateSnakePart(this Scene scene)
        {
            if (SnakeGameTextures == null)
                throw new NullReferenceException($"Property {nameof(SnakeGameTextures)} can't be null!");

            var snakeEntityParts = scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakePartIdPrefix));
            var nextIndexSnakePart = snakeEntityParts.Count;

            var snakeTailSource = GetSnakeTextureSource(SnakeTexture.Body);
            return scene.CreateEntity($"{SnakePartIdPrefix}{nextIndexSnakePart}", SnakeGroupName)
                .AddComponent(new SpriteComponent(SnakeGameTextures, sourceRectangle: snakeTailSource))
                .AddComponent(new SnakePartComponent(Vector2.Zero, Vector2.Zero))
                .SetPosition(new Vector2(-PixelSize));
        }

        internal static bool PositionIntersectWithAnyBlockEntity(this Scene scene, Vector2 position)
        {
            if (_blockEntityPositions == null)
                _blockEntityPositions = scene.GetEntities(BlockGroupName).Select(_ => _.Transform.Position);

            return _blockEntityPositions.Any(_ =>
            {
                var blockRectangle = new Rectangle(_.ToPoint(), new Point((int)(PixelSize * 3f)));
                blockRectangle.Offset(-PixelSize, -PixelSize);

                var otherRectangle = new Rectangle(position.ToPoint(), new Point((int)PixelSize));

                return otherRectangle.Intersects(blockRectangle);
            });
        }

        internal static SpriteFont GetGameFont(this Scene scene, string fontName = "Font")
        {
            var spriteFont = scene.GameCore.Content.Load<SpriteFont>(fontName);
            return spriteFont;
        }
        #endregion
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