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
        KeyboardState _oldKS = new KeyboardState();
        bool _keyDownPress;

        public void Initialize()
        {
            var snakeHeadEntity = CreateSnakePart(true);
            var snakePartEntity1 = CreateSnakePart();
            var snakePartEntity2 = CreateSnakePart();

            snakePartEntity1.AddChild(snakePartEntity2);
            snakeHeadEntity.AddChild(snakePartEntity1);

            InitializeSnakePartPosition();
        }



        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity("snakeHead");
            var snakePartComponentSnakeHead = snakeHeadEntity.GetComponent<SnakePartComponet>();
            var ks = Keyboard.GetState();


            if (ks.IsKeyDown(Keys.Down) && _oldKS.IsKeyUp(Keys.Down))
                _keyDownPress = true;

            _oldKS = ks;



            _sleepTime += Scene.GameTime.ElapsedGameTime;
            if (_sleepTime >= TimeSpan.FromSeconds(1))
            {
                snakePartComponentSnakeHead.LastPosition = snakeHeadEntity.Transform.Position;

                if (_keyDownPress)
                {
                    var posY = snakeHeadEntity.Transform.Position.Y + 25f;
                    snakeHeadEntity.SetPosition(snakeHeadEntity.Transform.Position.X, posY);
                }
                else
                {
                    var posX = snakeHeadEntity.Transform.Position.X + 25f;
                    snakeHeadEntity.SetPosition(posX, snakeHeadEntity.Transform.Position.Y);
                }


                _sleepTime = TimeSpan.Zero;
            }
        }

        Entity CreateSnakePart(bool isHead = false)
        {
            var snakeTexture = new Texture2D(Scene.GameCore.GraphicsDevice, 1, 1);
            snakeTexture.SetData(new Color[] { Color.Red });

            if (isHead)
            {
                var startPosition = new Vector2(100f/*Scene.ScreenWidth / 2f*/, 50f/*Scene.ScreenHeight / 2f*/);

                return Scene.CreateEntity("snakeHead").SetPosition(startPosition)
                .AddComponent(new SpriteComponent(snakeTexture, sourceRectangle: new Rectangle(0, 0, 25, 25)))
                .AddComponent(new SnakePartComponet { LastPosition = startPosition - new Vector2(25f, 0f) });
            }

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
