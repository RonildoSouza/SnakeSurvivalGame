using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Extensions;
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
                ("Ranking", () => { GameCore.SetScene(new RankingScene(false)); }),
                ("Quit", () =>{ this.ShowQuitConfirmDialog(noAction: null, desktop: _desktop); }),
            };

            _desktop.Root = this.MenuOptionsBuilder("Snake Survival Game", options, Color.Transparent);
        }

        public override void Draw()
        {
            base.Draw();
            _desktop.Render();
        }
    }
}
