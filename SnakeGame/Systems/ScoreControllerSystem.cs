using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.ECS.Systems.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(ScoreControllerSystem), typeof(TextComponent))]
    public sealed class ScoreControllerSystem : TextSystem, ILoadable
    {
        Entity _scoreEntity;
        static int _score = 0;
        const string scoreFormatText = "Score: {0}";
        SpriteFont _spriteFont;

        public event EventHandler<ScoreChangeEventArgs> ScoreChange;

        public void LoadContent()
        {
            _spriteFont = Scene.GameCore.Content.Load<SpriteFont>("Font");
            var scoreText = string.Format(scoreFormatText, _score);

            _scoreEntity = Scene.CreateEntity("score")
                .SetPosition(new Vector2(GetPositionX(_spriteFont, scoreText), SnakeGameHelper.PixelSize))
                .AddComponent(new TextComponent(_spriteFont, scoreText, color: Color.Black));
        }

        public void ChangeScore(object sender, EventArgs e)
        {
            _score += 20;

            var scoreTextComponent = _scoreEntity.GetComponent<TextComponent>();
            var scoreText = string.Format(scoreFormatText, _score);

            scoreTextComponent.Text = scoreText;

            _scoreEntity.SetPositionX(GetPositionX(_spriteFont, scoreText));

            ScoreChange?.Invoke(this, new ScoreChangeEventArgs(_score));
        }

        public float GetPositionX(SpriteFont spriteFont, string scoreText)
        {
            var measure = spriteFont.MeasureString(scoreText);
            return measure.X;
        }

        public void SetScore(int score) => _score = score;
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
