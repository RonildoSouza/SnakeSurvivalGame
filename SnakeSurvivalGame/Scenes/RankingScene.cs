using Curupira2D.ECS;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Infrastructure;
using SnakeSurvivalGame.Systems;
using System;

namespace SnakeSurvivalGame.Scenes
{
    public class RankingScene : Scene
    {
        // Myra
        Desktop _desktop;
        bool _showInputNameDialog;
        readonly int? _score;
        DynamicSpriteFont _dynamicSpriteFont;

        RankingService _rankingService;

        public RankingScene(bool showInputNameDialog, int? score = null)
        {
            _showInputNameDialog = showInputNameDialog;
            _score = score;
        }

        public override void LoadContent()
        {
            GameCore.IsMouseVisible = true;
            _rankingService = new RankingService(this);
            _desktop = new Desktop();
            _dynamicSpriteFont = SnakeSurvivalGameHelper.SerpensRegularTTFFontSystem.GetFont(20);

            var rankings = _rankingService.GetAll();

            var grid = new Grid();

            // Set partitioning configuration
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 25));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 50));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 25));
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            var titleLabel = new Label
            {
                Text = "Ranking",
                TextColor = Color.Black,
                Font = SnakeSurvivalGameHelper.SerpensRegularTTFFontSystem.GetFont(32),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Myra.Graphics2D.Thickness(0, 20),
                GridColumnSpan = 3
            };

            grid.Widgets.Add(titleLabel);

            var verticalStackPanel0 = new VerticalStackPanel
            {
                GridRow = 1,
                GridColumn = 0,
            };
            var verticalStackPanel1 = new VerticalStackPanel
            {
                GridRow = 1,
                GridColumn = 1,
            };
            var verticalStackPanel2 = new VerticalStackPanel
            {
                GridRow = 1,
                GridColumn = 2,
            };

            for (int pos = 1; pos <= rankings.Count; pos++)
            {
                var ranking = rankings[pos - 1];

                verticalStackPanel0.Widgets.Add(new Label()
                {
                    TextColor = Color.Black,
                    Text = $"{pos}",
                    Font = _dynamicSpriteFont,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                verticalStackPanel1.Widgets.Add(new Label()
                {
                    TextColor = Color.Black,
                    Text = $"{ranking.PlayerName}",
                    Font = _dynamicSpriteFont,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                verticalStackPanel2.Widgets.Add(new Label()
                {
                    TextColor = Color.Black,
                    Text = $"{ranking.PlayerScore}",
                    Font = _dynamicSpriteFont,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
            }

            grid.Widgets.Add(verticalStackPanel0);
            grid.Widgets.Add(verticalStackPanel1);
            grid.Widgets.Add(verticalStackPanel2);

            _desktop.Root = grid;

            if (_showInputNameDialog)
                ShowInputNameDialog();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (!_showInputNameDialog && KeyboardInputManager.IsKeyPressed(Keys.Escape))
                GameCore.SetScene<MenuScene>();

            base.Update(gameTime);

            KeyboardInputManager.End();
        }

        public override void Draw()
        {
            _desktop.Render();
            base.Draw();
        }

        void ShowInputNameDialog()
        {
            // Create dialog content
            var horizontalStackPanel = new HorizontalStackPanel { Spacing = 8 };
            horizontalStackPanel.Proportions.Add(new Proportion(ProportionType.Fill));

            var playerNameTextBox = new TextBox
            {
                HintText = "Enter your name here!",
                Height = 50,
                TextVerticalAlignment = VerticalAlignment.Center
            };

            horizontalStackPanel.Widgets.Add(playerNameTextBox);

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
                if (!inputNameDialog.Result)
                    return;

                _showInputNameDialog = false;

                _rankingService.Add(playerNameTextBox.Text, _score.Value);

                ScoreControllerSystem.CleanScore();
            };

            inputNameDialog.ShowModal(_desktop);
        }
    }
}
