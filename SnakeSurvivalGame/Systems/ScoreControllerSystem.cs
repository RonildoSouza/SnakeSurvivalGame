﻿using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using System;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(ScoreControllerSystem), typeof(TextComponent))]
    public sealed class ScoreControllerSystem : Curupira2D.ECS.System, ILoadable
    {
        static int _score = 0;
        const string scoreFormatText = "Score: {0}";
        TextComponent _scoreTextComponent;

        public event EventHandler<ScoreChangeEventArgs> ScoreChange;

        public void LoadContent()
        {
            var scoreText = string.Format(scoreFormatText, _score);

            _scoreTextComponent = new TextComponent(Scene.GetGameFont("Score"), scoreText, color: Color.Black);

            Scene.CreateEntity("score")
                .SetPosition(new Vector2(Scene.ScreenCenter.X * 1.5f, SnakeSurvivalGameHelper.PixelSize * 23.5f))
                .AddComponent(_scoreTextComponent);
        }

        public void ChangeScore(object sender, EventArgs e)
        {
            _score += 50;
            var scoreText = string.Format(scoreFormatText, _score);

            _scoreTextComponent.Text = scoreText;

            ScoreChange?.Invoke(this, new ScoreChangeEventArgs(_score));
        }
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