using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components;

namespace SnakeGame.Components
{
    public class SnakePartComponet : IComponent
    {
        public SnakePartComponet()
        {
            OffsetPosition = new Vector2(26f, 0f);
        }

        public SnakePartComponet(Vector2 offsetPosition)
        {
            OffsetPosition = offsetPosition;
        }

        public Vector2 OffsetPosition { get; set; }
    }
}
