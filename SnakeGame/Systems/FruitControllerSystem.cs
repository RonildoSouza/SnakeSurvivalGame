using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;
using System;
using System.Linq;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SnakePartComponet))]
    public sealed class FruitControllerSystem : MonoGame.Helper.ECS.System, IInitializable, IUpdatable
    {
        readonly Lazy<Random> _random = new Lazy<Random>();
        readonly Rectangle _fruitSize = new Rectangle(0, 0, 24, 24);
        Vector2 _fruitHalf;
        Entity _fruitEntity;
        Texture2D _snakeTexture;

        public void Initialize()
        {
            _fruitHalf = _fruitSize.Size.ToVector2() / 2f;

            _snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            _snakeTexture.SetData(new Color[] { Color.Black });

            var fruitTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            fruitTexture.SetData(new Color[] { Color.Red });

            var startPosition = new Vector2(
                Scene.ScreenWidth * 0.6f - _fruitHalf.X,
                Scene.ScreenHeight / 2f - _fruitHalf.Y);

            _fruitEntity = Scene.CreateEntity("fruit")
                .SetPosition(startPosition)
                .AddComponent(new SpriteComponent(fruitTexture, sourceRectangle: _fruitSize));
        }

        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity(SnakeHelper.SnakeHeadId);

            // Did the snake eat fruit?
            if (Vector2.Distance(snakeHeadEntity.Transform.Position, _fruitEntity.Transform.Position) <= 12)
            {
                // Update fruit position
                _fruitEntity.SetPosition(GetNewPosition());

                // Get the last snake part
                var lastSnakePartEntity = Scene
                    .GetEntities(_ => Matches(_) && _.UniqueId.StartsWith(SnakeHelper.SnakePartIdPrefix))
                    .FirstOrDefault(_ => !_.Children.Any());

                // Add new snake part as child of the last snake part
                var newSnakePartEntity = SnakeHelper.CreateSnakePart(Scene, _snakeTexture);
                lastSnakePartEntity.AddChild(newSnakePartEntity);
            }
        }

        Vector2 GetNewPosition()
        {
            return new Vector2(GetX(), GetY());

            int GetX()
            {
                var x = _random.Value.Next((int)_fruitHalf.X, Scene.ScreenWidth - (int)_fruitHalf.X);

                if (x % _fruitSize.Width != 0)
                    x = GetX();

                return x;
            }

            int GetY()
            {
                var y = _random.Value.Next((int)_fruitHalf.Y, Scene.ScreenHeight - (int)_fruitHalf.Y);

                if (y % _fruitSize.Height != 0)
                    y = GetY();

                return y;
            }
        }
    }
}
