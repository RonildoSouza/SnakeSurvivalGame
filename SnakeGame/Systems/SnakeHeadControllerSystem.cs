using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;
using System;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SpriteComponent))]
    [RequiredComponent(typeof(SnakePartComponent))]
    public sealed class SnakeHeadControllerSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        TimeSpan _sleepTime = TimeSpan.Zero;
        KeyboardState _oldKeyboardState = new KeyboardState();
        Vector2 direction = SnakeHelper.RightDirection;
        readonly TimeSpan _snakeSpeed = TimeSpan.FromMilliseconds(1000);

        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity(SnakeHelper.SnakeHeadId);
            var snakeSpriteComponentSnakeHead = snakeHeadEntity.GetComponent<SpriteComponent>();
            var snakePartComponentSnakeHead = snakeHeadEntity.GetComponent<SnakePartComponent>();
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) && !_oldKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = SnakeHelper.LeftDirection;
                _oldKeyboardState = keyboardState;
                snakeSpriteComponentSnakeHead.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadLeft);
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_oldKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = SnakeHelper.UpDirection;
                _oldKeyboardState = keyboardState;
                snakeSpriteComponentSnakeHead.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadUp);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !_oldKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = SnakeHelper.RightDirection;
                _oldKeyboardState = keyboardState;
                snakeSpriteComponentSnakeHead.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadRight);
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = SnakeHelper.DownDirection;
                _oldKeyboardState = keyboardState;
                snakeSpriteComponentSnakeHead.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadDown);
            }

            _sleepTime += Scene.GameTime.ElapsedGameTime;
            if (_sleepTime >= _snakeSpeed)
            {
                snakePartComponentSnakeHead.SetDirection(direction);
                snakePartComponentSnakeHead.LastPosition = snakeHeadEntity.Transform.Position;

                var position = snakeHeadEntity.Transform.Position + direction;
                snakeHeadEntity.SetPosition(position);

                #region Out of screen
                if (snakeHeadEntity.Transform.Position.X > Scene.ScreenWidth)
                    snakeHeadEntity.SetPosition(0f, snakeHeadEntity.Transform.Position.Y);
                if (snakeHeadEntity.Transform.Position.X < -SnakeHelper.SnakePixel)
                    snakeHeadEntity.SetPosition(Scene.ScreenWidth, snakeHeadEntity.Transform.Position.Y);
                if (snakeHeadEntity.Transform.Position.Y > Scene.ScreenHeight)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, 0f);
                if (snakeHeadEntity.Transform.Position.Y < -SnakeHelper.SnakePixel)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, Scene.ScreenHeight);
                #endregion

                _sleepTime = TimeSpan.Zero;
            }
        }
    }
}
