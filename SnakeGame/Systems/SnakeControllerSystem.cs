using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SpriteComponent))]
    public sealed class SnakeControllerSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            var snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            snakeTexture.SetData(new Color[] { Color.Red });

            var snakePart1Entity = Scene.CreateEntity("snakePart1")
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 25, 25)))
                .AddComponent<SnakePartComponet>();

            var snakePart2Entity = Scene.CreateEntity("snakePart2").SetPosition(148, 200)
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 25, 25)))
                .AddComponent<SnakePartComponet>();

            var startPosition = new Vector2(Scene.ScreenWidth / 2f, Scene.ScreenHeight / 2f);
            var snakeEntity = Scene.CreateEntity("snakePart0").SetPosition(startPosition);
            snakeEntity.AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 25, 25)));

            snakePart1Entity.AddChild(snakePart2Entity);
            snakeEntity.AddChild(snakePart1Entity);
        }
    }
}
