using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Helpers;
using SnakeSurvivalGame.Infrastructure;
using SnakeSurvivalGame.Systems;
using System;
using System.Collections.Generic;

namespace SnakeSurvivalGame.Scenes
{
    public class MenuScene : Scene
    {
        // Myra
        Desktop _desktop;

        public override void LoadContent()
        {
            GameCore.IsMouseVisible = true;
            _desktop = new Desktop();

            AddSystem<GameInitializeSystem>();

            base.LoadContent();

            // Build menu options
            var options = new List<(string, Action)>
            {
                ("Start", () =>
                {
                    GameCore.SetScene<GameSceneLevel01>();
                    GameCore.IsMouseVisible = false;
                }),
                ("Controls", () => { ShowControlsDialog(); }),
                ("Ranking", () => { GameCore.SetScene(new RankingScene()); }),
                ("License", () => { ShowLicenseDialog(); }),
                ("Quit", () =>{ this.ShowQuitConfirmDialog(noAction: null, desktop: _desktop); }),
            };

            _desktop.Root = this.MenuOptionsBuilder("Snake Survival Game", options, Color.Transparent);
        }

        public override void Draw()
        {
            base.Draw();
            _desktop.Render();
        }

        void ShowLicenseDialog()
        {
            var dialog = new Dialog
            {
                Width = ScreenWidth,
                Height = ScreenHeight,
                DragDirection = DragDirection.None
            };

            dialog.ButtonOk.Visible = false;
            dialog.ButtonCancel.Visible = false;
            dialog.CloseButton.Visible = true;

            var verticalStackPanel = new VerticalStackPanel();

            var headerLabel = new Label
            {
                Text = "License",
                TextColor = Color.White,
                Font = SnakeSurvivalGameHelper.SerpensRegularTTFFontSystem.GetFont(32),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Myra.Graphics2D.Thickness(0, 10)
            };

            verticalStackPanel.Widgets.Add(headerLabel);

            foreach (var softwareLicense in SoftwareLicenseService.GetAll())
            {
                var titleLabel = new Label
                {
                    Text = softwareLicense.Title,
                    Wrap = true,
                    Padding = new Myra.Graphics2D.Thickness(0, 20, 0, 5),
                };

                var licenseLabel = new Label
                {
                    Text = softwareLicense.License,
                    Wrap = true,
                    Padding = new Myra.Graphics2D.Thickness(20, 10),
                    Background = new SolidBrush(Color.Black),
                };

                verticalStackPanel.Widgets.Add(titleLabel);
                verticalStackPanel.Widgets.Add(licenseLabel);
            }

            var scrollViewer = new ScrollViewer
            {
                Content = verticalStackPanel
            };

            dialog.Content = scrollViewer;
            dialog.Content.Margin = new Myra.Graphics2D.Thickness(10, 10);
            dialog.Content.VerticalAlignment = VerticalAlignment.Center;

            dialog.ShowModal(_desktop);
        }

        void ShowControlsDialog()
        {
            var dialog = new Dialog
            {
                Width = ScreenWidth,
                Height = ScreenHeight,
                DragDirection = DragDirection.None,
            };

            dialog.ButtonOk.Visible = false;
            dialog.ButtonCancel.Visible = false;
            dialog.CloseButton.Visible = true;

            var verticalStackPanel = new VerticalStackPanel()
            {
                Width = ScreenWidth,
                Height = ScreenHeight,
            };

            var headerLabel = new Label
            {
                Text = "Controls",
                TextColor = Color.White,
                Font = SnakeSurvivalGameHelper.SerpensRegularTTFFontSystem.GetFont(32),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Myra.Graphics2D.Thickness(0, 10)
            };

            var image = new Image
            {
                Renderable = new TextureRegion(SnakeSurvivalGameHelper.ControlsTexture),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Myra.Graphics2D.Thickness(0, 100)
            };

            verticalStackPanel.Widgets.Add(headerLabel);
            verticalStackPanel.Widgets.Add(image);

            dialog.Content = verticalStackPanel;
            dialog.Content.Margin = new Myra.Graphics2D.Thickness(10, 10);
            dialog.Content.VerticalAlignment = VerticalAlignment.Center;

            dialog.ShowModal(_desktop);
        }
    }
}
