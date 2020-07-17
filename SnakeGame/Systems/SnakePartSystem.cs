using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SnakePartComponet))]
    public sealed class SnakePartSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        public void Update()
        {
            var snakePartEntities = Scene.GetEntities(_ => Matches(_) && _.UniqueId.StartsWith("snakePart"));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponet>();

                if (snakePartEntity.Transform.Position != snakePartComponentParent.LastPosition)
                {
                    var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponet>();
                    var position = snakePartComponentParent.LastPosition;

                    snakePartComponentSnakePart.LastPosition = snakePartEntity.Transform.Position;
                    snakePartEntity.SetPosition(position);
                }
            }
        }
    }
}