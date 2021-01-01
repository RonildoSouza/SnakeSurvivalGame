using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.ECS.Systems.Attributes;
using SnakeGame.Components;
using System;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SnakePartControllerSystem), new Type[] { typeof(SpriteComponent), typeof(SnakePartComponent) })]
    public sealed class SnakePartControllerSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        public void Update()
        {
            var snakePartEntities = Scene.GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeGameHelper.SnakePartIdPrefix));

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