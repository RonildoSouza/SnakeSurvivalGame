using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeSurvivalGame.Components;
using System;
using System.Linq;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(SnakeHeadControllerSystem), typeof(SnakePartComponent))]
    public sealed class SnakeHeadControllerSystem : Curupira2D.ECS.System, ILoadable, IUpdatable
    {
        TimeSpan _sleepTime = TimeSpan.Zero;
        KeyboardState _oldKeyboardState = new KeyboardState();
        Vector2 direction = SnakeSurvivalGameHelper.RightDirection;
        readonly TimeSpan _snakeSpeed = TimeSpan.FromMilliseconds(100);
        bool _start;

        public void LoadContent()
        {
            var startBackgroundEntity = Scene.CreateEntity($"{nameof(_start)}Background")
                .SetPosition(Scene.ScreenCenter)
                .AddComponent(new SpriteComponent(Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Scene.ScreenSize, Color.Gray * 0.8f), layerDepth: .9f));

            Scene.CreateEntity(nameof(_start))
                .SetPosition(Scene.ScreenCenter)
                .AddComponent(new TextComponent(Scene.GetGameFont("MainText"), $"{Scene.Title}\nPress\nSPACE\nto\nStart!", color: Color.Black, drawInUICamera: false, layerDepth: 1f))
                .AddChild(startBackgroundEntity);
        }

        public void Update()
        {
            var keyboardState = Keyboard.GetState();

            if (!_start)
            {
                if (keyboardState.IsKeyDown(Keys.Space) && !_oldKeyboardState.IsKeyDown(Keys.Space))
                {
                    _start = true;
                    Scene.RemoveEntity(nameof(_start));
                }

                return;
            }

            var snakeHeadEntity = Scene.GetEntity(SnakeSurvivalGameHelper.SnakeHeadId);
            var snakePartComponentSnakeHead = snakeHeadEntity.GetComponent<SnakePartComponent>();

            if (keyboardState.IsKeyDown(Keys.Left) && !_oldKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = SnakeSurvivalGameHelper.LeftDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_oldKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = SnakeSurvivalGameHelper.UpDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !_oldKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = SnakeSurvivalGameHelper.RightDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = SnakeSurvivalGameHelper.DownDirection;
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
                    .GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeSurvivalGameHelper.SnakePartIdPrefix))
                    .Select(_ => _.Transform.Position);

                var snakeBlockPositions = Scene
                    .GetEntities(SnakeSurvivalGameHelper.BlockGroupName)
                    .Select(_ => _.Transform.Position);

                if (snakePartPositions.Any(_ => Vector2.Distance(_, snakeHeadEntity.Transform.Position) <= 0f)
                    || Scene.PositionIntersectWithAnyBlockEntity(snakeHeadEntity.Transform.Position))
                    Scene.SetCleanColor(Color.Red);
                else
                    Scene.SetCleanColor(Color.LightGray);

                #region Out of screen
                if (snakeHeadEntity.Transform.Position.X > Scene.ScreenWidth)
                    snakeHeadEntity.SetPosition(SnakeSurvivalGameHelper.PixelSizeHalf, snakeHeadEntity.Transform.Position.Y);

                if (snakeHeadEntity.Transform.Position.X <= -SnakeSurvivalGameHelper.PixelSizeHalf)
                    snakeHeadEntity.SetPosition(Scene.ScreenWidth - SnakeSurvivalGameHelper.PixelSizeHalf, snakeHeadEntity.Transform.Position.Y);

                if (snakeHeadEntity.Transform.Position.Y > Scene.ScreenHeight)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, SnakeSurvivalGameHelper.PixelSizeHalf);

                if (snakeHeadEntity.Transform.Position.Y <= -SnakeSurvivalGameHelper.PixelSizeHalf)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, Scene.ScreenHeight - SnakeSurvivalGameHelper.PixelSizeHalf);
                #endregion

                _sleepTime = TimeSpan.Zero;
            }
        }
    }
}
