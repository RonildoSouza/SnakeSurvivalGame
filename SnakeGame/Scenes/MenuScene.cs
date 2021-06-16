﻿using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Systems;

namespace SnakeGame.Scenes
{
    public class MenuScene : Scene
    {
        public override void LoadContent()
        {
            AddSystem<GameInitializeSystem>();

            CreateEntity("menu")
                .SetPosition(ScreenCenter)
                .AddComponent(new TextComponent(this.GetGameFont(), "Press ENTER to Start!", color: Color.Red));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (KeyboardInputManager.IsKeyReleased(Keys.Enter))
                GameCore.SetScene<GameSceneLevel01>();

            KeyboardInputManager.End();

            base.Update(gameTime);
        }
    }
}
