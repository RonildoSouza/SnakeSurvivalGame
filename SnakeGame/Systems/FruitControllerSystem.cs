﻿using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(FruitControllerSystem), typeof(SpriteComponent))]
    public sealed class FruitControllerSystem : Curupira2D.ECS.System, ILoadable, IUpdatable
    {
        readonly Lazy<Random> _random = new Lazy<Random>();
        Entity _fruitEntity;

        public event EventHandler SnakeEatFruit;

        public void LoadContent()
        {
            var fruitSource = SnakeGameHelper.GetSnakeTextureSource(SnakeTexture.Fruit);
            var startPosition = new Vector2(SnakeGameHelper.PixelSize * 10.5f, SnakeGameHelper.PixelSize * 13.5f);

            _fruitEntity = Scene.CreateEntity(SnakeGameHelper.FruitId)
                .SetPosition(startPosition)
                .AddComponent(new SpriteComponent(SnakeGameHelper.SnakeGameTextures, sourceRectangle: fruitSource));
        }

        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity(SnakeGameHelper.SnakeHeadId);

            // Did the snake eat fruit?
            if (snakeHeadEntity.Transform.Position == _fruitEntity.Transform.Position)
            {
                UpdateFruitPosition();
                AddSnakePart();

                SnakeEatFruit?.Invoke(this, null);
            }
        }

        void UpdateFruitPosition()
        {
            // Get all snake part positions
            var snakePartPositions = Scene
                .GetEntities(_ => MatchComponents(_))
                .Select(_ => _.Transform.Position);

            _fruitEntity.SetPosition(GetX(), GetY());

            float GetX()
            {
                var x = SnakeGameHelper.PixelSize *
                    (_random.Value.Next(0, (Scene.ScreenWidth - 1) / (int)SnakeGameHelper.PixelSize) + 0.5f);

                if (snakePartPositions.Any(_ => _.X == x))
                    Task.Factory.StartNew(() => x = GetX());

                return x;
            }

            float GetY()
            {
                var y = SnakeGameHelper.PixelSize *
                    (_random.Value.Next(0, (Scene.ScreenHeight - 1) / (int)SnakeGameHelper.PixelSize) + 0.5f);

                if (snakePartPositions.Any(_ => _.Y == y))
                    Task.Factory.StartNew(() => y = GetY());

                return y;
            }
        }

        void AddSnakePart()
        {
            // Get the last snake part
            var lastSnakePartEntity = Scene
                .GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeGameHelper.SnakePartIdPrefix))
                .LastOrDefault(_ => !_.Children.Any());

            // Add new snake part as child of the last snake part
            var newSnakePartEntity = SnakeGameHelper.CreateSnakePart(Scene);
            lastSnakePartEntity.AddChild(newSnakePartEntity);
        }
    }
}
