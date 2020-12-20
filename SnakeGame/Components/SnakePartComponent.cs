using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components;

namespace SnakeGame.Components
{
    public class SnakePartComponent : IComponent
    {
        public SnakePartComponent(Vector2 lastPosition, Vector2 newDirection)
        {
            LastPosition = lastPosition;
            SetDirection(newDirection);
        }

        public Vector2 LastPosition { get; set; }
        public Vector2 NewDirection { get; private set; }
        public Vector2 LastDirection { get; private set; }

        public void SetDirection(Vector2 direction)
        {
            if (NewDirection == direction)
                return;

            LastDirection = NewDirection;
            NewDirection = direction;
        }
    }
}
