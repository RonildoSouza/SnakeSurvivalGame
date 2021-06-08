using Microsoft.Xna.Framework;

namespace SnakeGame.Scenes
{
    public sealed class GameSceneLevel09 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 09");
            SetNextGameSceneLevel(new GameSceneLevel10());

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 3.5f));

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 9.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 9.5f));


            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 12.5f));


            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 15.5f));

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 21.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 21.5f));

            base.LoadContent();
        }
    }
}
