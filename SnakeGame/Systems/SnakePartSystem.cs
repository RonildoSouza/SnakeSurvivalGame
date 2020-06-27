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
            SceneMatchEntitiesIteration(entity =>
            {
                var snakePartComponent = entity.GetComponent<SnakePartComponet>();
                var position = entity.Parent.Transform.Position - snakePartComponent.OffsetPosition;

                entity.SetPosition(position);
            });
        }
    }
}
