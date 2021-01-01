﻿using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;
using System.Collections.Generic;

namespace SnakeGame.Systems
{
    public sealed class SnakeInitializeSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            var snakeHeadEntity = CreateSnakeHead();
            var snakePartEntities = new List<Entity> { snakeHeadEntity };

            for (int i = 0; i < 2; i++)
            {
                var snakePartEntity = SnakeGameHelper.CreateSnakePart(Scene);

                snakePartEntities[i].AddChild(snakePartEntity);
                snakePartEntities.Add(snakePartEntity);
            }

            InitializeSnakePartPosition();
        }

        Entity CreateSnakeHead()
        {
            var snakeHeadSource = SnakeGameHelper.GetSnakeTextureSource(SnakeTexture.Head);
            var startPosition = new Vector2(SnakeGameHelper.PixelSize * 8.5f, SnakeGameHelper.PixelSize * 12.5f);
            var lastPosition = startPosition - new Vector2(SnakeGameHelper.PixelSize, 0f);

            return Scene.CreateEntity(SnakeGameHelper.SnakeHeadId)
                .SetPosition(startPosition)
                .AddComponent(new SpriteComponent(SnakeGameHelper.SnakeGameTextures, sourceRectangle: snakeHeadSource))
                .AddComponent(new SnakePartComponent(lastPosition, SnakeGameHelper.RightDirection));
        }

        void InitializeSnakePartPosition()
        {
            var snakePartEntities = Scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakeGameHelper.SnakePartIdPrefix));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponent>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponent>();
                var position = snakePartComponentParent.LastPosition;

                snakePartComponentSnakePart.LastPosition = position - new Vector2(SnakeGameHelper.PixelSize, 0f);
                snakePartEntity.SetPosition(position);
            }
        }
    }
}