﻿using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeSurvivalGame.Components;
using System;

namespace SnakeSurvivalGame.Helpers
{
    internal static class SnakeSurvivalGameHelper
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

        internal static Texture2D SnakeSurvivalGameTextures { get; private set; }
        internal static Texture2D ControlsTexture { get; private set; }
        internal static FontSystem SerpensRegularTTFFontSystem { get; private set; }
        internal static FontSystem FreePixelTTFFontSystem { get; private set; }
        internal static SpriteFont MainTextFont { get; private set; }
        internal static SpriteFont ScoreFont { get; private set; }

        internal static void SetSnakeSurvivalGameTextures(Texture2D gameTextures) => SnakeSurvivalGameTextures = gameTextures;

        internal static void SetControlsTexture(Texture2D controlsTexture) => ControlsTexture = controlsTexture;

        internal static void SetSerpensRegularTTFData(byte[] serpensRegularTTFData)
        {
            SerpensRegularTTFFontSystem = new FontSystem();
            SerpensRegularTTFFontSystem.AddFont(serpensRegularTTFData);
        }

        internal static void SetFreePixelTTFData(byte[] freePixelTTFData)
        {
            FreePixelTTFFontSystem = new FontSystem();
            FreePixelTTFFontSystem.AddFont(freePixelTTFData);
        }

        internal static void SetMainFont(SpriteFont spriteFont) => MainTextFont = spriteFont;

        internal static void SetScoreFont(SpriteFont spriteFont) => ScoreFont = spriteFont;

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

        #region Extension Methods
        internal static Entity CreateSnakePart(this Scene scene)
        {
            if (SnakeSurvivalGameTextures == null)
                throw new NullReferenceException($"Property {nameof(SnakeSurvivalGameTextures)} can't be null!");

            var snakeEntityParts = scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakePartIdPrefix));
            var nextIndexSnakePart = snakeEntityParts.Count;

            var snakeTailSource = GetSnakeTextureSource(SnakeTexture.Body);
            return scene.CreateEntity($"{SnakePartIdPrefix}{nextIndexSnakePart}", new Vector2(-PixelSize), SnakeGroupName)
                .AddComponent(new SpriteComponent(SnakeSurvivalGameTextures, sourceRectangle: snakeTailSource))
                .AddComponent(new SnakePartComponent(Vector2.Zero, Vector2.Zero));
        }
        #endregion
    }

    internal enum SnakeTexture
    {
        Head,
        Body,
        Fruit,
        Mouse,
        Block
    }
}