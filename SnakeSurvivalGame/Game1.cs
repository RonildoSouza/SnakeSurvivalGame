﻿using Curupira2D;
using Myra;
using SnakeSurvivalGame.Scenes;

namespace SnakeSurvivalGame
{
    public class Game1 : GameCore
    {
#if DEBUG
        public Game1() : base(360, 600, true, true) { }
#else
        public Game1() : base(360, 600, false, true) { }
#endif

        protected override void LoadContent()
        {
            // Myra
            MyraEnvironment.Game = this;

            SetScene<MenuScene>();

            base.LoadContent();
        }
    }
}
