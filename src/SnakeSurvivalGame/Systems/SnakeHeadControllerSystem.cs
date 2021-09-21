using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeSurvivalGame.Components;
using SnakeSurvivalGame.Helpers;
using SnakeSurvivalGame.Infrastructure;
using SnakeSurvivalGame.Scenes;
using System;
using System.Threading;

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
        Entity _youDieEntity;
        Lazy<RankingService> _rankingService;

        public void LoadContent()
        {
            var spriteFont = Scene.GetGameFont("MainText");
            var spriteComponent = new SpriteComponent(Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Scene.ScreenSize, Color.Gray * 0.7f), layerDepth: .9f);

            Scene.CreateEntity($"{nameof(_start)}1", Scene.ScreenCenter, isCollidable: false)
                .AddComponent(spriteComponent)
                .AddComponent(new TextComponent(spriteFont, $"{Scene.Title}", color: Color.Black, drawInUICamera: false, layerDepth: 1f)
                {
                    Position = new Vector2(Scene.ScreenCenter.X, Scene.ScreenCenter.Y * 1.1f)
                });

            Scene.CreateEntity($"{nameof(_start)}2", Scene.ScreenCenter, isCollidable: false)
                .AddComponent(spriteComponent)
                .AddComponent(new TextComponent(spriteFont, "Press .SPACE. to Start!", color: Color.Black, drawInUICamera: false, layerDepth: 1f, scale: new Vector2(0.4f))
                {
                    Position = new Vector2(Scene.ScreenCenter.X, Scene.ScreenCenter.Y * 0.9f)
                });

            _youDieEntity = Scene.CreateEntity(nameof(_youDieEntity), Scene.ScreenCenter, isCollidable: false)
                .AddComponent(new TextComponent(spriteFont, $"YOU DIE!", color: Color.Red, drawInUICamera: false, layerDepth: 1f))
                .SetActive(false);

            _rankingService = new Lazy<RankingService>(new RankingService(Scene));
        }

        public void Update()
        {
            var keyboardState = Keyboard.GetState();

            if (!_start && keyboardState.IsKeyDown(Keys.Space) && !_oldKeyboardState.IsKeyDown(Keys.Space))
                _start = true;

            if (!_start)
                return;

            if (_start && Scene.ExistsEntities(_ => _.UniqueId == $"{nameof(_start)}1" || _.UniqueId == $"{nameof(_start)}2"))
            {
                Scene.RemoveEntity($"{nameof(_start)}1");
                Scene.RemoveEntity($"{nameof(_start)}2");
            }

            // YOU DIE!
            if (_youDieEntity.Active)
            {
                var canRegisterScore = ScoreControllerSystem.Score > _rankingService.Value.GetMinPlayerScore();

                Thread.Sleep(3000);
                Scene.PauseUpdatableSystems = true;
                Scene.GameCore.SetScene(new RankingScene(canRegisterScore));

                return;
            }

            var snakeHeadEntity = Scene.GetEntity(SnakeSurvivalGameHelper.SnakeHeadId);
            var snakePartComponentSnakeHead = snakeHeadEntity.GetComponent<SnakePartComponent>();

            if (keyboardState.IsKeyDown(Keys.Left) && !_oldKeyboardState.IsKeyDown(Keys.Right) && snakePartComponentSnakeHead.NewDirection != SnakeSurvivalGameHelper.RightDirection)
            {
                direction = SnakeSurvivalGameHelper.LeftDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_oldKeyboardState.IsKeyDown(Keys.Down) && snakePartComponentSnakeHead.NewDirection != SnakeSurvivalGameHelper.DownDirection)
            {
                direction = SnakeSurvivalGameHelper.UpDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !_oldKeyboardState.IsKeyDown(Keys.Left) && snakePartComponentSnakeHead.NewDirection != SnakeSurvivalGameHelper.LeftDirection)
            {
                direction = SnakeSurvivalGameHelper.RightDirection;
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Up) && snakePartComponentSnakeHead.NewDirection != SnakeSurvivalGameHelper.UpDirection)
            {
                direction = SnakeSurvivalGameHelper.DownDirection;
                _oldKeyboardState = keyboardState;
            }

            _sleepTime += Scene.GameTime.ElapsedGameTime;
            if (_sleepTime >= _snakeSpeed)
            {
                snakePartComponentSnakeHead.SetDirection(direction);
                snakePartComponentSnakeHead.LastPosition = snakeHeadEntity.Position;

                var position = snakeHeadEntity.Position + direction;
                snakeHeadEntity.SetPosition(position);

                // YOU DIE?
                if (snakeHeadEntity.IsCollidedWithAny(Scene, SnakeSurvivalGameHelper.SnakeGroupName)
                    || snakeHeadEntity.IsCollidedWithAny(Scene, SnakeSurvivalGameHelper.BlockGroupName))
                {
                    _youDieEntity.SetActive(true);
                }

                #region Out of screen
                if (snakeHeadEntity.Position.X > Scene.ScreenWidth)
                    snakeHeadEntity.SetPosition(SnakeSurvivalGameHelper.PixelSizeHalf, snakeHeadEntity.Position.Y);

                if (snakeHeadEntity.Position.X <= -SnakeSurvivalGameHelper.PixelSizeHalf)
                    snakeHeadEntity.SetPosition(Scene.ScreenWidth - SnakeSurvivalGameHelper.PixelSizeHalf, snakeHeadEntity.Position.Y);

                if (snakeHeadEntity.Position.Y > (Scene.ScreenHeight - SnakeSurvivalGameHelper.PixelSize * 2))
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Position.X, SnakeSurvivalGameHelper.PixelSizeHalf);

                if (snakeHeadEntity.Position.Y <= -SnakeSurvivalGameHelper.PixelSizeHalf)
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Position.X, Scene.ScreenHeight - SnakeSurvivalGameHelper.PixelSize * 2.5f);
                #endregion

                _sleepTime = TimeSpan.Zero;
            }
        }
    }
}
