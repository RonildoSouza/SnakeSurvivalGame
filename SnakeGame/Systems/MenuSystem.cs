using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.ECS.Systems.Drawables;
using Microsoft.Xna.Framework;
using SnakeGame.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Systems
{
    [RequiredComponent(typeof(MenuSystem), typeof(TextComponent))]
    public sealed class MenuSystem : TextSystem, ILoadable, IUpdatable
    {
        IDictionary<MenuButton, Rectangle> _buttonBoundingBoxes;
        Rectangle _sourceRectanglePointerCursor;
        Rectangle _sourceRectangleHandCursor;
        SpriteComponent _mouseCursorSpriteComponent;
        Entity _mouseCursorEntity;

        public void LoadContent()
        {
            // Mouse cursor entities
            _sourceRectanglePointerCursor = SnakeGameHelper.GetMouseCursorTextureSource(MouseCursorTexture.Pointer);
            _sourceRectangleHandCursor = SnakeGameHelper.GetMouseCursorTextureSource(MouseCursorTexture.Hand);
            _mouseCursorSpriteComponent = new SpriteComponent(SnakeGameHelper.MouseCursorTextures, sourceRectangle: _sourceRectanglePointerCursor);

            _mouseCursorEntity = Scene.CreateEntity("MouseCursor").AddComponent(_mouseCursorSpriteComponent);

            // Button entities
            var btnStartTextComponent = TextComponentBuilder("Start");
            Scene.CreateEntity(nameof(MenuButton.Start)).SetPosition(Scene.ScreenCenter.X, Scene.ScreenCenter.Y + 50f).AddComponent(btnStartTextComponent);

            var btnRankingTextComponent = TextComponentBuilder("Ranking");
            Scene.CreateEntity(nameof(MenuButton.Ranking)).SetPosition(Scene.ScreenCenter.X, Scene.ScreenCenter.Y - 50f).AddComponent(btnRankingTextComponent);

            // Button bounding boxes
            _buttonBoundingBoxes = new Dictionary<MenuButton, Rectangle>
            {
                { MenuButton.Start, new Rectangle(Scene.GetEntityPosition(nameof(MenuButton.Start)).ToPoint(), btnStartTextComponent.TextSize.ToPoint()) },
                { MenuButton.Ranking, new Rectangle(Scene.GetEntityPosition(nameof(MenuButton.Ranking)).ToPoint(), btnRankingTextComponent.TextSize.ToPoint()) },
            };
        }

        public void Update()
        {
            if (IsMenuButtonPressed(MenuButton.Start, Scene.MouseInputManager.GetPosition()))
                Scene.GameCore.SetScene<GameSceneLevel01>();

            if (IsMenuButtonPressed(MenuButton.Ranking, Scene.MouseInputManager.GetPosition()))
                Scene.SetCleanColor(Color.Green);

            UpdateMouseCursor(Scene.MouseInputManager.GetPosition());
        }

        TextComponent TextComponentBuilder(string text) => new TextComponent(Scene.GetGameFont("MainText"), text, color: Color.Black);

        bool IsMenuButtonPressed(MenuButton menuButton, Point mousePosition)
        {
            var boundingBoxMouse = new Rectangle(mousePosition, Point.Zero);

            return _buttonBoundingBoxes.Any(_ => boundingBoxMouse.Intersects(_.Value) && _.Key == menuButton)
                  && Scene.MouseInputManager.IsMouseButtonPressed(Curupira2D.Input.MouseButton.Left);
        }

        void UpdateMouseCursor(Point mousePosition)
        {
            var boundingBoxMouse = new Rectangle(mousePosition, Point.Zero);

            _mouseCursorEntity.SetPosition(Scene.PositionToScene(mousePosition));

            if (_buttonBoundingBoxes.Any(_ => boundingBoxMouse.Intersects(_.Value)))
                _mouseCursorSpriteComponent.SourceRectangle = _sourceRectangleHandCursor;
            else
                _mouseCursorSpriteComponent.SourceRectangle = _sourceRectanglePointerCursor;
        }

        internal enum MenuButton
        {
            Start,
            Ranking,
        }
    }
}
