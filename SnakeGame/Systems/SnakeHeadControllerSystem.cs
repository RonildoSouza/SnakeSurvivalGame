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
        Rectangle? snakeTextureSource;

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
                snakeTextureSource = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadLeft);
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_oldKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = SnakeHelper.UpDirection;
                _oldKeyboardState = keyboardState;
                snakeTextureSource = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadUp);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !_oldKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = SnakeHelper.RightDirection;
                _oldKeyboardState = keyboardState;
                snakeTextureSource = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadRight);
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = SnakeHelper.DownDirection;
                _oldKeyboardState = keyboardState;
                snakeTextureSource = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadDown);
            }

            _sleepTime += Scene.GameTime.ElapsedGameTime;
            if (_sleepTime >= _snakeSpeed)
            {
                if (snakeTextureSource != null)
                    snakeSpriteComponentSnakeHead.SourceRectangle = snakeTextureSource;

                UpdatePart();

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

            if (index == 0)
                UpdatePart();
        }

        static int index = 0;
        public void UpdatePart()
        {
            var snakePartEntities = Scene.GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeHelper.SnakePartIdPrefix));
            var snakePartEntity = snakePartEntities[index];

            var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponent>();

            if (snakePartEntity.Transform.Position != snakePartComponentParent.LastPosition)
            {
                var snakeSpriteComponentSnakePart = snakePartEntity.GetComponent<SpriteComponent>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponent>();
                var position = snakePartComponentParent.LastPosition;

                if ((snakePartComponentParent.LastDirection == SnakeHelper.UpDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyUpRight_LeftDown);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.UpDirection && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);
                    }
                }

                if ((snakePartComponentParent.LastDirection == SnakeHelper.RightDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.UpDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyRightDown_UpLeft);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.RightDirection && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.UpDirection && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);
                    }
                }

                if ((snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.DownDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyLeftUp_DownRight);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.DownDirection && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);
                    }
                }

                if ((snakePartComponentParent.LastDirection == SnakeHelper.DownDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.RightDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyDownLeft_RightUp);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.DownDirection && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.RightDirection && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);
                    }
                }

                //// Tail
                //if (!snakePartEntity.Children.Any())
                //{
                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.LeftDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.RightDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailLeft);

                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.UpDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.DownDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailUp);

                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.RightDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.LeftDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailRight);

                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.DownDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.UpDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailDown);
                //}

                snakePartComponentSnakePart.SetDirection(snakePartComponentParent.NewDirection);
                snakePartComponentSnakePart.LastPosition = snakePartEntity.Transform.Position;
                snakePartEntity.SetPosition(position);

                index++;

                if (index > snakePartEntities.Count - 1)
                    index = 0;
            }
        }
    }
}
