using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.ECS.Systems.Drawables;
using Microsoft.Xna.Framework;
using System;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(ScoreControllerSystem), typeof(TextComponent))]
    public sealed class ScoreControllerSystem : TextSystem, ILoadable
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
                .SetPosition(new Vector2(Scene.ScreenCenter.X * 1.5f, SnakeGameHelper.PixelSize * 23.5f))
                .AddComponent(_scoreTextComponent);
        }

        public void ChangeScore(object sender, EventArgs e)
        {
            _score += 40;
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
