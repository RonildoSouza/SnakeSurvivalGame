using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using SnakeSurvivalGame.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(MenuSystem), typeof(SpriteComponent))]
    [RequiredComponent(typeof(MenuSystem), typeof(TextComponent))]
    public sealed class MenuSystem : Curupira2D.ECS.System, ILoadable, IUpdatable
    {
        IList<(MenuButton MenuButton, Rectangle BoundingBox, TextComponent Component)> _buttonsData;
        Rectangle _sourceRectanglePointerCursor;
        Rectangle _sourceRectangleHandCursor;
        SpriteComponent _mouseCursorSpriteComponent;
        Entity _mouseCursorEntity;

        // Myra
        readonly Desktop _desktop;
        bool _quitClicked;

        public MenuSystem(Desktop desktop)
        {
            _desktop = desktop;
        }

        public void LoadContent()
        {
            // Mouse cursor entities
            _sourceRectanglePointerCursor = SnakeSurvivalGameHelper.GetMouseCursorTextureSource(MouseCursorTexture.Pointer);
            _sourceRectangleHandCursor = SnakeSurvivalGameHelper.GetMouseCursorTextureSource(MouseCursorTexture.Hand);
            _mouseCursorSpriteComponent = new SpriteComponent(SnakeSurvivalGameHelper.MouseCursorTextures, sourceRectangle: _sourceRectanglePointerCursor, drawInUICamera: true, layerDepth: 1f);

            _mouseCursorEntity = Scene.CreateEntity("MouseCursor").AddComponent(_mouseCursorSpriteComponent);

            // Button entities
            var btnStartTextComponent = TextComponentBuilder("Start");
            Scene.CreateEntity(nameof(MenuButton.Start)).SetPosition(Scene.ScreenCenter.X, Scene.ScreenCenter.Y * 1.3f).AddComponent(btnStartTextComponent);

            var btnRankingTextComponent = TextComponentBuilder("Ranking");
            Scene.CreateEntity(nameof(MenuButton.Ranking)).SetPosition(Scene.ScreenCenter).AddComponent(btnRankingTextComponent);

            var btnQuitTextComponent = TextComponentBuilder("Quit");
            Scene.CreateEntity(nameof(MenuButton.Quit)).SetPosition(Scene.ScreenCenter.X, Scene.ScreenCenter.Y * 0.7f).AddComponent(btnQuitTextComponent);

            // Buttons datas
            _buttonsData = new List<(MenuButton, Rectangle, TextComponent)>
            {
                ( MenuButton.Start, new Rectangle(Scene.GetEntityPosition(nameof(MenuButton.Start)).ToPoint(), btnStartTextComponent.TextSize.ToPoint()), btnStartTextComponent ),
                ( MenuButton.Ranking, new Rectangle(Scene.GetEntityPosition(nameof(MenuButton.Ranking)).ToPoint(), btnRankingTextComponent.TextSize.ToPoint()), btnRankingTextComponent ),
                ( MenuButton.Quit, new Rectangle(Scene.GetEntityPosition(nameof(MenuButton.Quit)).ToPoint(), btnQuitTextComponent.TextSize.ToPoint()), btnQuitTextComponent ),
            };
        }

        public void Update()
        {
            if (_quitClicked)
                return;

            var mousePosition = Scene.MouseInputManager.GetPosition();

            UpdateButtons(mousePosition);
            UpdateMouseCursor(mousePosition);
        }

        TextComponent TextComponentBuilder(string text) => new TextComponent(Scene.GetGameFont("MainText"), text, color: Color.Black);

        bool IsMenuButtonIntersects(MenuButton menuButton, Point mousePosition)
        {
            var boundingBoxMouse = new Rectangle(mousePosition, Point.Zero);
            return _buttonsData.Any(_ => boundingBoxMouse.Intersects(_.BoundingBox) && _.MenuButton == menuButton);
        }

        bool IsMenuButtonPressed(MenuButton menuButton, Point mousePosition)
            => IsMenuButtonIntersects(menuButton, mousePosition) && Scene.MouseInputManager.IsMouseButtonPressed(Curupira2D.Input.MouseButton.Left);

        void UpdateButtons(Point mousePosition)
        {
            // Actions
            if (IsMenuButtonPressed(MenuButton.Start, mousePosition))
                Scene.GameCore.SetScene<GameSceneLevel01>();

            if (IsMenuButtonPressed(MenuButton.Ranking, mousePosition))
                Scene.GameCore.SetScene(new RankingScene(true, 10000));

            if (!_quitClicked && IsMenuButtonPressed(MenuButton.Quit, mousePosition))
            {
                _quitClicked = true;
                Scene.GameCore.IsMouseVisible = true;
                _mouseCursorEntity.SetActive(false);

                Scene.ShowConfirmDialog(
                    title: "Quit?",
                    message: "Would you like to quit?",
                    yesAction: () => Scene.GameCore.Exit(),
                    noAction: () =>
                    {
                        _quitClicked = false;
                        Scene.GameCore.IsMouseVisible = false;
                        _mouseCursorEntity.SetActive(true);
                    },
                    desktop: _desktop);
            }

            // Animations
            foreach (var buttonData in _buttonsData)
            {
                if (IsMenuButtonIntersects(buttonData.MenuButton, mousePosition))
                    buttonData.Component.Scale = new Vector2(1.1f);
                else
                    buttonData.Component.Scale = Vector2.One;
            }
        }

        void UpdateMouseCursor(Point mousePosition)
        {
            var boundingBoxMouse = new Rectangle(mousePosition, Point.Zero);
            _mouseCursorEntity.SetPosition(Scene.PositionToScene(mousePosition));

            if (_buttonsData.Any(_ => boundingBoxMouse.Intersects(_.BoundingBox)))
                _mouseCursorSpriteComponent.SourceRectangle = _sourceRectangleHandCursor;
            else
                _mouseCursorSpriteComponent.SourceRectangle = _sourceRectanglePointerCursor;
        }

        internal enum MenuButton
        {
            Start,
            Ranking,
            Quit,
        }
    }
}
