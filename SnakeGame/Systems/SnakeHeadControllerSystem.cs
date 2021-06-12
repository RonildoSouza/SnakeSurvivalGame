using Curupira2D.ECS;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components;
using System;
using System.Linq;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SnakeHeadControllerSystem), typeof(SnakePartComponent))]
    public sealed class SnakeHeadControllerSystem : Curupira2D.ECS.System, IUpdatable
    {
        TimeSpan _sleepTime = TimeSpan.Zero;
        KeyboardState _oldKeyboardState = new KeyboardState();
        Vector2 direction = SnakeGameHelper.RightDirection;
        readonly TimeSpan _snakeSpeed = TimeSpan.FromMilliseconds(100);

        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity(SnakeGameHelper.SnakeHeadId);
            var snakePartComponentSnakeHead = snakeHeadEntity.GetComponent<SnakePartComponent>();
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) && !_oldKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = SnakeGameHelper.LeftDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_oldKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = SnakeGameHelper.UpDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !_oldKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = SnakeGameHelper.RightDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = SnakeGameHelper.DownDirection;
                _oldKeyboardState = keyboardState;
            }

            _sleepTime += Scene.GameTime.ElapsedGameTime;
            if (_sleepTime >= _snakeSpeed)
            {
                snakePartComponentSnakeHead.SetDirection(direction);
                snakePartComponentSnakeHead.LastPosition = snakeHeadEntity.Transform.Position;

                var position = snakeHeadEntity.Transform.Position + direction;
                snakeHeadEntity.SetPosition(position);

                // Get snake part positions
                var snakePartPositions = Scene
                    .GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeGameHelper.SnakePartIdPrefix))
                    .Select(_ => _.Transform.Position);

                var snakeBlockPositions = Scene
                    .GetEntities(SnakeGameHelper.BlockGroupName)
                    .Select(_ => _.Transform.Position);

                if (snakePartPositions.Any(_ => Vector2.Distance(_, snakeHeadEntity.Transform.Position) <= 0f)
                    || Scene.PositionIntersectWithAnyBlockEntity(snakeHeadEntity.Transform.Position))
                    Scene.SetCleanColor(Color.Red);
                else
                    Scene.SetCleanColor(Color.LightGray);

                #region Out of screen
                if (snakeHeadEntity.Transform.Position.X > Scene.ScreenWidth)
                    snakeHeadEntity.SetPosition(SnakeGameHelper.PixelSizeHalf, snakeHeadEntity.Transform.Position.Y);

                if (snakeHeadEntity.Transform.Position.X <= -SnakeGameHelper.PixelSizeHalf)
                    snakeHeadEntity.SetPosition(Scene.ScreenWidth - SnakeGameHelper.PixelSizeHalf, snakeHeadEntity.Transform.Position.Y);

                if (snakeHeadEntity.Transform.Position.Y > Scene.ScreenHeight)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, SnakeGameHelper.PixelSizeHalf);

                if (snakeHeadEntity.Transform.Position.Y <= -SnakeGameHelper.PixelSizeHalf)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, Scene.ScreenHeight - SnakeGameHelper.PixelSizeHalf);
                #endregion

                _sleepTime = TimeSpan.Zero;
            }
        }
    }
}
