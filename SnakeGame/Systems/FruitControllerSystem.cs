using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using SnakeGame.Components;
using System;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(SnakePartComponet))]
    public sealed class FruitControllerSystem : MonoGame.Helper.ECS.System, IInitializable, IUpdatable
    {
        readonly Lazy<Random> _random = new Lazy<Random>();
        readonly Rectangle _fruitSize = new Rectangle(0, 0, 24, 24);
        Vector2 _fruitHalf;
        Entity _fruitEntity;

        public void Initialize()
        {
            _fruitHalf = _fruitSize.Size.ToVector2() / 2f;

            var fruitTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            fruitTexture.SetData(new Color[] { Color.Red });

            var startPosition = new Vector2(
                Scene.ScreenWidth * 0.6f - _fruitHalf.X,
                Scene.ScreenHeight / 2f - _fruitHalf.Y);

            _fruitEntity = Scene.CreateEntity("fruit")
                .SetPosition(startPosition)
                .AddComponent(new SpriteComponent(fruitTexture, sourceRectangle: _fruitSize));
        }

        KeyboardState oldKS = new KeyboardState();
        public void Update()
        {
            //var snakeHeadEntity = Scene.GetEntity("snakeHead");

            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Space) && oldKS.IsKeyUp(Keys.Space))
            {
                _fruitEntity.SetPosition(GetNewPosition());
            }

            oldKS = ks;
        }

        Vector2 GetNewPosition()
        {
            var x = GetX();
            var y = GetY();

            return new Vector2(x, y);
        }

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
