using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components;

namespace SnakeGame.Components
{
    public class SnakePartComponet : IComponent
    {
        public Vector2 LastPosition { get; set; }
    }
}
