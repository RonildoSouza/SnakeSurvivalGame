using Curupira2D.ECS;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Helpers;
using SnakeSurvivalGame.Infrastructure;
using SnakeSurvivalGame.Systems;
using System;

namespace SnakeSurvivalGame.Scenes
{
    public class RankingScene : Scene
    {
        // Myra
        Desktop _desktop;
        bool _canShowInputNameDialog;
        DynamicSpriteFont _dynamicSpriteFont;

        RankingService _rankingService;

        public RankingScene(bool canShowInputNameDialog = false)
        {
            _canShowInputNameDialog = canShowInputNameDialog;
        }

        public override void LoadContent()
        {
            GameCore.IsMouseVisible = true;
            _rankingService = new RankingService(this);
            _desktop = new Desktop();
            _dynamicSpriteFont = SnakeSurvivalGameHelper.SerpensRegularTTFFontSystem.GetFont(20);

            if (_canShowInputNameDialog)
                ShowInputNameDialog();
            else
            {
                ScoreControllerSystem.CleanScore();
                RankingGridBuilder();
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (!_canShowInputNameDialog && KeyboardInputManager.IsKeyPressed(Keys.Escape))
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
                TextVerticalAlignment = VerticalAlignment.Center,
                Font = SnakeSurvivalGameHelper.FreePixelTTFFontSystem.GetFont(20)
            };

            horizontalStackPanel.Widgets.Add(playerNameTextBox);

            // Create dialog
            var inputNameDialog = new Dialog
            {
                Title = $"Your Score: {ScoreControllerSystem.Score}",
                Content = horizontalStackPanel,
                TitleFont = SnakeSurvivalGameHelper.FreePixelTTFFontSystem.GetFont(20)
            };

            inputNameDialog.ButtonOk.Text = "Save";
            inputNameDialog.ButtonOk.Width = 100;
            inputNameDialog.ButtonOk.Height = 30;
            inputNameDialog.ButtonOk.ContentHorizontalAlignment = HorizontalAlignment.Center;
            inputNameDialog.ButtonOk.ContentVerticalAlignment = VerticalAlignment.Center;
            inputNameDialog.ButtonOk.Font = SnakeSurvivalGameHelper.FreePixelTTFFontSystem.GetFont(20);

            inputNameDialog.ButtonCancel.Visible = false;
            inputNameDialog.CloseButton.Visible = false;
            inputNameDialog.Width = Convert.ToInt32(ScreenWidth * 0.8f);
            inputNameDialog.Height = Convert.ToInt32(ScreenHeight * 0.2f);
            inputNameDialog.Padding = new Myra.Graphics2D.Thickness(10, 10);
            inputNameDialog.Content.Margin = new Myra.Graphics2D.Thickness(10, 10);
            inputNameDialog.Content.VerticalAlignment = VerticalAlignment.Center;
            inputNameDialog.CloseKey = Keys.None;
            inputNameDialog.DragDirection = DragDirection.None;

            inputNameDialog.Closed += (s, a) =>
            {
                if (!inputNameDialog.Result || string.IsNullOrEmpty(playerNameTextBox.Text?.Trim()))
                {
                    playerNameTextBox.HintText = "TYPE YOUR NAME HERE!";
                    inputNameDialog.Show(_desktop);
                    return;
                }

                _canShowInputNameDialog = false;
                _rankingService.Add(playerNameTextBox.Text.Trim(), ScoreControllerSystem.Score);

                ScoreControllerSystem.CleanScore();
                RankingGridBuilder();
            };

            inputNameDialog.Show(_desktop);
        }

        void RankingGridBuilder()
        {
            var rankings = _rankingService.GetAll();
            var grid = new Grid();

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 25));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 50));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 25));
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            var headerLabel = new Label
            {
                Text = "Ranking",
                TextColor = Color.Black,
                Font = SnakeSurvivalGameHelper.SerpensRegularTTFFontSystem.GetFont(32),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Myra.Graphics2D.Thickness(0, 20),
                GridColumnSpan = 3
            };

            grid.Widgets.Add(headerLabel);

            var verticalStackPanelCol0 = new VerticalStackPanel { GridRow = 1, GridColumn = 0, };
            var verticalStackPanelCol1 = new VerticalStackPanel { GridRow = 1, GridColumn = 1, };
            var verticalStackPanelCol2 = new VerticalStackPanel { GridRow = 1, GridColumn = 2, };

            // Build labels in vertical stack
            for (int pos = 1; pos <= rankings.Count; pos++)
            {
                var ranking = rankings[pos - 1];

                verticalStackPanelCol0.Widgets.Add(new Label()
                {
                    TextColor = Color.Black,
                    Text = $"{pos}",
                    Font = _dynamicSpriteFont,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                verticalStackPanelCol1.Widgets.Add(new Label()
                {
                    TextColor = Color.Black,
                    Text = ranking.PlayerName.Length > 15 ? ranking.PlayerName.Substring(0, 15) : ranking.PlayerName,
                    Font = _dynamicSpriteFont,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Wrap = true
                });

                verticalStackPanelCol2.Widgets.Add(new Label()
                {
                    TextColor = Color.Black,
                    Text = $"{ranking.PlayerScore}",
                    Font = _dynamicSpriteFont,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
            }

            grid.Widgets.Add(verticalStackPanelCol0);
            grid.Widgets.Add(verticalStackPanelCol1);
            grid.Widgets.Add(verticalStackPanelCol2);

            var footerLabel = new Label
            {
                Text = "Press [ESC] to Return Menu!",
                TextColor = Color.Black,
                Font = SnakeSurvivalGameHelper.FreePixelTTFFontSystem.GetFont(22),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Padding = new Myra.Graphics2D.Thickness(0, 40),
                GridColumnSpan = 3,
                GridRow = 2
            };

            grid.Widgets.Add(footerLabel);

            _desktop.Root = grid;
        }
    }
}
