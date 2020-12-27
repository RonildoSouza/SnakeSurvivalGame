using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SpriteComponent))]
    [RequiredComponent(typeof(SnakePartComponent))]
    public sealed class SnakePartControllerSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        static int index = 0;

        public void Update()
        {
            var snakePartEntities = Scene.GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeHelper.SnakePartIdPrefix));
            var snakePartEntity = snakePartEntities[index];

            var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponent>();

            if (snakePartEntity.Transform.Position != snakePartComponentParent.LastPosition)
            {
                var snakeSpriteComponentSnakePart = snakePartEntity.GetComponent<SpriteComponent>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponent>();
                var position = snakePartComponentParent.LastPosition;

                if ((snakePartComponentParent.LastDirection == SnakeHelper.UpDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyUpRight_LeftDown);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.UpDirection && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);
                    }
                }

                if ((snakePartComponentParent.LastDirection == SnakeHelper.RightDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.UpDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyRightDown_UpLeft);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.RightDirection && snakePartComponentParent.NewDirection == SnakeHelper.DownDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.UpDirection && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);
                    }
                }

                if ((snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.DownDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyLeftUp_DownRight);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.LeftDirection && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.DownDirection && snakePartComponentParent.NewDirection == SnakeHelper.RightDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);
                    }
                }

                if ((snakePartComponentParent.LastDirection == SnakeHelper.DownDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection)
                    ||
                    (snakePartComponentParent.LastDirection == SnakeHelper.RightDirection
                    && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection))
                {
                    if (snakePartEntity.Parent.UniqueId == SnakeHelper.SnakeHeadId)
                        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyDownLeft_RightUp);
                    else
                    {
                        var snakeSpriteParentComponentSnakePart = snakePartEntity.Parent.GetComponent<SpriteComponent>();
                        snakeSpriteComponentSnakePart.SourceRectangle = snakeSpriteParentComponentSnakePart.SourceRectangle;

                        if (snakePartComponentParent.LastDirection == SnakeHelper.DownDirection && snakePartComponentParent.NewDirection == SnakeHelper.LeftDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyHorizontal);

                        if (snakePartComponentParent.LastDirection == SnakeHelper.RightDirection && snakePartComponentParent.NewDirection == SnakeHelper.UpDirection)
                            snakeSpriteParentComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.BodyVertical);
                    }
                }

                //// Tail
                //if (!snakePartEntity.Children.Any())
                //{
                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.LeftDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.RightDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailLeft);

                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.UpDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.DownDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailUp);

                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.RightDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.LeftDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailRight);

                //    if (snakePartComponentSnakePart.NewDirection == SnakeHelper.DownDirection && snakePartComponentSnakePart.LastDirection != SnakeHelper.UpDirection)
                //        snakeSpriteComponentSnakePart.SourceRectangle = SnakeHelper.GetSnakeTextureSource(SnakeTexture.TailDown);
                //}

                snakePartComponentSnakePart.SetDirection(snakePartComponentParent.NewDirection);
                snakePartComponentSnakePart.LastPosition = snakePartEntity.Transform.Position;
                snakePartEntity.SetPosition(position);

                index++;

                if (index > snakePartEntities.Count - 1)
                    index = 0;
            }
        }
    }
}