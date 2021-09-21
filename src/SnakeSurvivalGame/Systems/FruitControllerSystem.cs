using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(FruitControllerSystem), typeof(SpriteComponent))]
    public sealed class FruitControllerSystem : Curupira2D.ECS.System, ILoadable, IUpdatable
    {
        readonly Lazy<Random> _random = new Lazy<Random>();
        Entity _fruitEntity;

        public event EventHandler SnakeEatFruit;

        public void LoadContent()
        {
            var fruitSource = SnakeSurvivalGameHelper.GetSnakeTextureSource(SnakeTexture.Fruit);
            var startPosition = new Vector2(SnakeSurvivalGameHelper.PixelSize * 10.5f, SnakeSurvivalGameHelper.PixelSize * 12.5f);

            _fruitEntity = Scene.CreateEntity(SnakeSurvivalGameHelper.FruitId, startPosition)
                .AddComponent(new SpriteComponent(SnakeSurvivalGameHelper.SnakeSurvivalGameTextures, sourceRectangle: fruitSource));
        }

        public void Update()
        {
            var snakeHeadEntity = Scene.GetEntity(SnakeSurvivalGameHelper.SnakeHeadId);

            // Did the snake eat fruit?
            if (snakeHeadEntity.Position == _fruitEntity.Position)
            {
                AddSnakePart();
                UpdateFruitPosition();

                SnakeEatFruit?.Invoke(this, null);
            }
        }

        void UpdateFruitPosition()
        {
            var x = SnakeSurvivalGameHelper.PixelSize *
                (_random.Value.Next(0, (Scene.ScreenWidth - 1) / (int)SnakeSurvivalGameHelper.PixelSize) + 0.5f);
            var y = SnakeSurvivalGameHelper.PixelSize *
                (_random.Value.Next(0, (Scene.ScreenHeight - 1) / (int)SnakeSurvivalGameHelper.PixelSize) + 0.5f);

            _fruitEntity.SetPosition(x, y);

            if (_fruitEntity.IsCollidedWithAny(Scene))
                UpdateFruitPosition();
        }

        void AddSnakePart()
        {
            // Get the last snake part
            var lastSnakePartEntity = Scene
                .GetEntities(_ => MatchComponents(_) && _.UniqueId.StartsWith(SnakeSurvivalGameHelper.SnakePartIdPrefix))
                .LastOrDefault(_ => !_.Children.Any());

            // Add new snake part as child of the last snake part
            var newSnakePartEntity = Scene.CreateSnakePart();
            lastSnakePartEntity.AddChild(newSnakePartEntity);
        }
    }
}
