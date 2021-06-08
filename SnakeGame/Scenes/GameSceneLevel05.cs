using Microsoft.Xna.Framework;

namespace SnakeGame.Scenes
{
    public sealed class GameSceneLevel05 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 05");
            SetNextGameSceneLevel(new GameSceneLevel06());

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 4.5f, SnakeGameHelper.PixelSize * 6.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 10.5f, SnakeGameHelper.PixelSize * 6.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 12.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 4.5f, SnakeGameHelper.PixelSize * 18.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 10.5f, SnakeGameHelper.PixelSize * 18.5f));

            base.LoadContent();
        }
    }
}
