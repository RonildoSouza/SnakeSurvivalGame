using Microsoft.Xna.Framework;
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

#if DEBUG
            //var snakePartEntity4 = SnakeHelper.CreateSnakePart(Scene);
            //var snakePartEntity5 = SnakeHelper.CreateSnakePart(Scene);
            //var snakePartEntity6 = SnakeHelper.CreateSnakePart(Scene);
            //var snakePartEntity7 = SnakeHelper.CreateSnakePart(Scene);
            //var snakePartEntity8 = SnakeHelper.CreateSnakePart(Scene);
            //var snakePartEntity9 = SnakeHelper.CreateSnakePart(Scene);

            //snakePartEntity8.AddChild(snakePartEntity9);
            //snakePartEntity7.AddChild(snakePartEntity8);
            //snakePartEntity6.AddChild(snakePartEntity7);
            //snakePartEntity5.AddChild(snakePartEntity6);
            //snakePartEntity4.AddChild(snakePartEntity5);
            //snakePartEntity3.AddChild(snakePartEntity4);
#endif

            snakePartEntity2.AddChild(snakePartEntity3);
            snakePartEntity1.AddChild(snakePartEntity2);
            snakeHeadEntity.AddChild(snakePartEntity1);

            InitializeSnakePartPosition();
        }

        Entity CreateSnakeHead()
        {
            var snakeHeadSource = SnakeHelper.GetSnakeTextureSource(SnakeTexture.HeadRight);
            var startPosition = new Vector2(
                Scene.ScreenWidth / 2f - snakeHeadSource.Width / 2f,
                Scene.ScreenHeight / 2f - snakeHeadSource.Height / 2f);

            return Scene.CreateEntity(SnakeHelper.SnakeHeadId)
                .SetPosition(startPosition)
                .AddComponent(new SpriteComponent(SnakeHelper.GameTextures, sourceRectangle: snakeHeadSource))
                .AddComponent(new SnakePartComponent(startPosition - new Vector2(SnakeHelper.SnakePixel, 0f), SnakeHelper.RightDirection));
        }

        void InitializeSnakePartPosition()
        {
            var snakePartEntities = Scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith(SnakeHelper.SnakePartIdPrefix));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponent>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponent>();
                var position = snakePartComponentParent.LastPosition;

                snakePartComponentSnakePart.LastPosition = position - new Vector2(SnakeHelper.SnakePixel, 0f);
                snakePartEntity.SetPosition(position);
            }
        }
    }
}