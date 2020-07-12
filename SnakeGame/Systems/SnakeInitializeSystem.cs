using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;

namespace SnakeGame.Systems
{
    public sealed class SnakeInitializeSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            var snakeHeadEntity = CreateSnakeHead();
            var snakePartEntity1 = SnakeHelper.CreateSnakePart(Scene);
            var snakePartEntity2 = SnakeHelper.CreateSnakePart(Scene);

            var snakePartEntity3 = SnakeHelper.CreateSnakePart(Scene);
            var snakePartEntity4 = SnakeHelper.CreateSnakePart(Scene);
            var snakePartEntity5 = SnakeHelper.CreateSnakePart(Scene);

            snakePartEntity4.AddChild(snakePartEntity5);
            snakePartEntity3.AddChild(snakePartEntity4);
            snakePartEntity2.AddChild(snakePartEntity3);

            snakePartEntity1.AddChild(snakePartEntity2);
            snakeHeadEntity.AddChild(snakePartEntity1);

            InitializeSnakePartPosition();
        }

        Entity CreateSnakeHead()
        {
            var snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            snakeTexture.SetData(new Color[] { Color.Black });

            var snakeHeadSize = new Rectangle(0, 0, 24, 24);
            var startPosition = new Vector2(
                Scene.ScreenWidth / 2f - snakeHeadSize.Width / 2f,
                Scene.ScreenHeight / 2f - snakeHeadSize.Height / 2f);

            return Scene.CreateEntity("snakeHead").SetPosition(startPosition)
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: snakeHeadSize))
                .AddComponent(new SnakePartComponet { LastPosition = startPosition - new Vector2(SnakeHelper.SnakePixel, 0f) });
        }

        void InitializeSnakePartPosition()
        {
            var snakePartEntities = Scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith("snakePart"));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponet>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponet>();
                var position = snakePartComponentParent.LastPosition;

                snakePartComponentSnakePart.LastPosition = position - new Vector2(SnakeHelper.SnakePixel, 0f);
                snakePartEntity.SetPosition(position);
            }
        }
    }
}