using Curupira2D.ECS;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Scenes;
using System;
using System.Collections.Generic;

namespace SnakeSurvivalGame.Extensions
{
    internal static class MyraExtensions
    {
        internal static void ShowQuitConfirmDialog(this Scene scene, Action noAction, Desktop desktop)
            => scene.ShowConfirmDialog("Quit?", "Would you like to quit?", () => scene.GameCore.Exit(), noAction, desktop);

        internal static void ShowMenuConfirmDialog(this Scene scene, Desktop desktop)
            => scene.ShowConfirmDialog("Menu?", "Would you like to return to menu?", () => scene.GameCore.SetScene<MenuScene>(), null, desktop);

        static void ShowConfirmDialog(this Scene scene, string title, string message, Action yesAction, Action noAction, Desktop desktop)
        {
            var dialog = Dialog.CreateMessageBox(title, message);
            dialog.ButtonOk.Text = "Yes";
            dialog.ButtonOk.Width = 100;
            dialog.ButtonOk.Height = 30;
            dialog.ButtonOk.ContentHorizontalAlignment = HorizontalAlignment.Center;
            dialog.ButtonOk.ContentVerticalAlignment = VerticalAlignment.Center;

            dialog.ButtonCancel.Text = "No";
            dialog.ButtonCancel.Width = 100;
            dialog.ButtonCancel.Height = 30;
            dialog.ButtonCancel.ContentHorizontalAlignment = HorizontalAlignment.Center;
            dialog.ButtonCancel.ContentVerticalAlignment = VerticalAlignment.Center;

            dialog.CloseButton.Visible = false;
            dialog.Width = Convert.ToInt32(scene.ScreenWidth * 0.8f);
            dialog.Height = Convert.ToInt32(scene.ScreenHeight * 0.2f);
            dialog.Padding = new Myra.Graphics2D.Thickness(10, 10);
            dialog.Content.Margin = new Myra.Graphics2D.Thickness(10, 10);
            dialog.Content.VerticalAlignment = VerticalAlignment.Center;

            dialog.Closed += (s, a) =>
            {
                // Escape or "Cancel"
                if (!dialog.Result)
                {
                    noAction?.Invoke();
                    return;
                }

                yesAction?.Invoke();
            };

            dialog.ShowModal(desktop);
        }

        internal static Panel MenuOptionsBuilder(this Scene scene, string title, IList<(string OptionText, Action OptionClicked)> options, Color backgroundColor)
        {
            // Ordinary DynamicSpriteFont
            var ordinaryFontSystem = FontSystemFactory.Create(scene.GameCore.GraphicsDevice);
            ordinaryFontSystem.AddFont(SnakeSurvivalGameHelper.SerpensRegularTTFData);

            var verticalMenu = new VerticalMenu
            {
                Width = scene.ScreenWidth,
                BorderThickness = Myra.Graphics2D.Thickness.Zero,
                Background = new SolidBrush(Color.Transparent),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                LabelHorizontalAlignment = HorizontalAlignment.Center,
                LabelColor = Color.Black,
                LabelFont = ordinaryFontSystem.GetFont(72),
                SelectionBackground = new SolidBrush(Color.Gray),
                SelectionHoverBackground = new SolidBrush(Color.Gray)
            };

            // builde menu items
            for (int i = 0; i < options.Count; i++)
            {
                var (OptionText, OptionClicked) = options[i];
                var menuItem = new MenuItem($"{i}", OptionText);
                menuItem.Selected += (s, a) => { OptionClicked?.Invoke(); };

                verticalMenu.Items.Add(menuItem);
            }

            var titleLabel = new Label
            {
                Text = title,
                TextColor = Color.Black,
                Font = ordinaryFontSystem.GetFont(32),
                HorizontalAlignment = HorizontalAlignment.Center,
                Top = 10,
            };

            var versionLabel = new Label
            {
                Text = $"Version {scene.GameCore.GetVersion()}",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Left = -10,
                Top = -10,
            };

            var panel = new Panel { Background = new SolidBrush(backgroundColor) };

            if (!string.IsNullOrEmpty(title))
                panel.Widgets.Add(titleLabel);

            panel.Widgets.Add(versionLabel);
            panel.Widgets.Add(verticalMenu);

            return panel;
        }
    }
}
