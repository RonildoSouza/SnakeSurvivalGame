using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using System;

namespace SnakeSurvivalGame.Scenes
{
    public class RankingScene : Scene
    {
        // Myra
        Desktop _desktop;
        bool _showInputNameDialog;
        readonly int? _score;

        public RankingScene(bool showInputNameDialog, int? score = null)
        {
            _showInputNameDialog = showInputNameDialog;
            _score = score;
        }

        public override void LoadContent()
        {
            GameCore.IsMouseVisible = true;

            if (_showInputNameDialog)
                ShowInputNameDialog();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (!_showInputNameDialog && KeyboardInputManager.IsKeyPressed(Keys.Escape))
                GameCore.SetScene<MenuScene>();

            KeyboardInputManager.End();

            base.Update(gameTime);
        }

        public override void Draw()
        {
            _desktop.Render();
            base.Draw();
        }

        void ShowInputNameDialog()
        {
            _desktop = new Desktop();

            // Create dialog content
            var horizontalStackPanel = new HorizontalStackPanel { Spacing = 8 };
            horizontalStackPanel.Proportions.Add(new Proportion(ProportionType.Fill));

            var nameTextBox = new TextBox
            {
                HintText = "Enter your name here!",
                Height = 50,
                TextVerticalAlignment = VerticalAlignment.Center
            };

            horizontalStackPanel.Widgets.Add(nameTextBox);

            // Create dialog
            var inputNameDialog = new Dialog();

            if (_score.HasValue)
                inputNameDialog.Title = $"Your Score: {_score}";

            inputNameDialog.Content = horizontalStackPanel;

            inputNameDialog.ButtonOk.Text = "Save";
            inputNameDialog.ButtonOk.Width = 100;
            inputNameDialog.ButtonOk.Height = 30;
            inputNameDialog.ButtonOk.ContentHorizontalAlignment = HorizontalAlignment.Center;
            inputNameDialog.ButtonOk.ContentVerticalAlignment = VerticalAlignment.Center;

            inputNameDialog.ButtonCancel.Visible = false;
            inputNameDialog.CloseButton.Visible = false;
            inputNameDialog.Width = Convert.ToInt32(ScreenWidth * 0.8f);
            inputNameDialog.Height = Convert.ToInt32(ScreenHeight * 0.2f);
            inputNameDialog.Padding = new Myra.Graphics2D.Thickness(10, 10);
            inputNameDialog.Content.Margin = new Myra.Graphics2D.Thickness(10, 10);
            inputNameDialog.Content.VerticalAlignment = VerticalAlignment.Center;

            inputNameDialog.Closed += (s, a) =>
            {
                _showInputNameDialog = false;
            };

            inputNameDialog.ShowModal(_desktop);
        }
    }
}
