using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Components;
using SnakeSurvivalGame.Helpers;
using System.Collections.Generic;

namespace SnakeSurvivalGame.Systems
{
    public sealed class SnakeInitializeSystem : Curupira2D.ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var snakeHeadEntity = CreateSnakeHead();
            var snakePartEntities = new List<Entity> { snakeHeadEntity };

            for (int i = 0; i < 2; i++)
            {
                var snakePartEntity = Scene.CreateSnakePart();

                snakePartEntities[i].AddChild(snakePartEntity);
                snakePartEntities.Add(snakePartEntity);
            }

            InitializeSnakePartPosition();
        }

        Entity CreateSnakeHead()
        {
            var snakeHeadSource = SnakeSurvivalGameHelper.GetSnakeTextureSource(SnakeTexture.Head);
            var startPosition = new Vector2(SnakeSurvivalGameHelper.PixelSize * 4.5f, SnakeSurvivalGameHelper.PixelSize * 23.5f);
            var lastPosition = startPosition - new Vector2(SnakeSurvivalGameHelper.PixelSize, 0f);

            return Scene.CreateEntity(SnakeSurvivalGameHelper.SnakeHeadId, startPosition, SnakeSurvivalGameHelper.SnakeGroupName)
                .AddComponent(new SpriteComponent(SnakeSurvivalGameHelper.SnakeSurvivalGameTextures, sourceRectangle: snakeHeadSource))
                .AddComponent(new SnakePartComponent(lastPosition, SnakeSurvivalGameHelper.RightDirection));
        }

        void InitializeSnakePartPosition()
        {
            var snakePartEntities = Scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakeSurvivalGameHelper.SnakePartIdPrefix));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponent>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponent>();
                var position = snakePartComponentParent.LastPosition;

                snakePartComponentSnakePart.LastPosition = position - new Vector2(SnakeSurvivalGameHelper.PixelSize, 0f);
                snakePartEntity.SetPosition(position);
            }
        }
    }
}