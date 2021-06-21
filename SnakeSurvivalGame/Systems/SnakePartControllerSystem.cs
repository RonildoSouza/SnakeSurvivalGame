using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using SnakeSurvivalGame.Components;
using System;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(SnakePartControllerSystem), new Type[] { typeof(SpriteComponent), typeof(SnakePartComponent) })]
    public sealed class SnakePartControllerSystem : Curupira2D.ECS.System, IUpdatable
    {
        public void Update()
        {
            var snakePartEntities = Scene.GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeSurvivalGameHelper.SnakePartIdPrefix));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponent>();

                if (snakePartEntity.Transform.Position != snakePartComponentParent.LastPosition)
                {
                    var snakeSpriteComponentSnakePart = snakePartEntity.GetComponent<SpriteComponent>();
                    var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponent>();
                    var position = snakePartComponentParent.LastPosition;

                    snakePartComponentSnakePart.SetDirection(snakePartComponentParent.NewDirection);
                    snakePartComponentSnakePart.LastPosition = snakePartEntity.Transform.Position;
                    snakePartEntity.SetPosition(position);
                }
            }
        }
    }
}