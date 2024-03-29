﻿using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;
using System;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(ScoreControllerSystem), typeof(TextComponent))]
    public sealed class ScoreControllerSystem : Curupira2D.ECS.System, ILoadable
    {
        TextComponent _scoreTextComponent;

        public static int Score { get; private set; } = 0;

        public event EventHandler<ScoreChangeEventArgs> ScoreChange;

        public void LoadContent()
        {
            _scoreTextComponent = new TextComponent(SnakeSurvivalGameHelper.ScoreFont, GetScoreText(), color: Color.Black, layerDepth: 1f)
            {
                Position = new Vector2(Scene.ScreenCenter.X * 1.35f, SnakeSurvivalGameHelper.PixelSize * 25.8f)
            };

            var scoreTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Scene.ScreenWidth, SnakeSurvivalGameHelper.PixelSize * 2f, Color.Gray * 0.5f);
            Scene.CreateEntity("score", Scene.ScreenCenter.X, Scene.ScreenHeight - SnakeSurvivalGameHelper.PixelSize)
                .AddComponent(new SpriteComponent(scoreTexture))
                .AddComponent(_scoreTextComponent);
        }

        public void ChangeScore(object sender, EventArgs e)
        {
            Score += 50;
            _scoreTextComponent.Text = GetScoreText();
            ScoreChange?.Invoke(this, new ScoreChangeEventArgs(Score));
        }

        internal static void CleanScore() => Score = 0;

        private static string GetScoreText() => string.Format("Score: {0}", $"{Score}".PadLeft(6, '0'));
    }

    public class ScoreChangeEventArgs : EventArgs
    {
        public ScoreChangeEventArgs(int score)
        {
            Score = score;
        }

        public int Score { get; }
    }
}
