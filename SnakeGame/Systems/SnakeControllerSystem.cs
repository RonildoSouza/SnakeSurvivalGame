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
    [RequiredComponent(typeof(SpriteComponent))]
    [RequiredComponent(typeof(SnakePartComponet))]
    public sealed class SnakeControllerSystem : MonoGame.Helper.ECS.System, IInitializable, IUpdatable
    {
        TimeSpan _sleepTime = TimeSpan.Zero;
        KeyboardState _oldKeyboardState = new KeyboardState();
        Vector2 direction = new Vector2(25f, 0f);

        public void Initialize()
        {
            var snakeHeadEntity = CreateSnakeHead();
            var snakePartEntity1 = CreateSnakePart();
            var snakePartEntity2 = CreateSnakePart();

            var snakePartEntity3 = CreateSnakePart();
            var snakePartEntity4 = CreateSnakePart();
            var snakePartEntity5 = CreateSnakePart();

            snakePartEntity4.AddChild(snakePartEntity5);
            snakePartEntity3.AddChild(snakePartEntity4);
            snakePartEntity2.AddChild(snakePartEntity3);

            snakePartEntity1.AddChild(snakePartEntity2);
            snakeHeadEntity.AddChild(snakePartEntity1);

            InitializeSnakePartPosition();
        }

        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity("snakeHead");
            var snakePartComponentSnakeHead = snakeHeadEntity.GetComponent<SnakePartComponet>();
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) && !_oldKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = new Vector2(-25f, 0f);
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_oldKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = new Vector2(0f, -25f);
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !_oldKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = new Vector2(25f, 0f);
                _oldKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = new Vector2(0f, 25f);
                _oldKeyboardState = keyboardState;
            }

            //System.Diagnostics.Debug.WriteLine($"{_oldKS.GetPressedKeys().LastOrDefault()}");

            _sleepTime += Scene.GameTime.ElapsedGameTime;
            if (_sleepTime >= TimeSpan.FromSeconds(0.5))
            {
                snakePartComponentSnakeHead.LastPosition = snakeHeadEntity.Transform.Position;

                var position = snakeHeadEntity.Transform.Position + direction;
                snakeHeadEntity.SetPosition(position);

                _sleepTime = TimeSpan.Zero;
            }
        }

        Entity CreateSnakeHead()
        {
            var snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            snakeTexture.SetData(new Color[] { Color.Red });

            var startPosition = new Vector2(100f/*Scene.ScreenWidth / 2f*/, 50f/*Scene.ScreenHeight / 2f*/);

            return Scene.CreateEntity("snakeHead").SetPosition(startPosition)
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 25, 25)))
                .AddComponent(new SnakePartComponet { LastPosition = startPosition - new Vector2(25f, 0f) });
        }

        Entity CreateSnakePart()
        {
            var snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            snakeTexture.SetData(new Color[] { Color.Red });

            var nextIndexSnakePart = Scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith("snakePart")).Count;

            return Scene.CreateEntity($"snakePart{nextIndexSnakePart}")
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 25, 25)))
                .AddComponent<SnakePartComponet>();
        }

        void InitializeSnakePartPosition()
        {
            var snakePartEntities = Scene.GetEntities(_ => _.Active && _.UniqueId.StartsWith("snakePart"));

            foreach (var snakePartEntity in snakePartEntities)
            {
                var snakePartComponentParent = snakePartEntity.Parent.GetComponent<SnakePartComponet>();
                var snakePartComponentSnakePart = snakePartEntity.GetComponent<SnakePartComponet>();
                var position = snakePartComponentParent.LastPosition;// - snakePartComponentSnakePart.OffsetPosition;

                snakePartComponentSnakePart.LastPosition = position - new Vector2(25f, 0f);
                snakePartEntity.SetPosition(position);
            }
        }
    }
}
