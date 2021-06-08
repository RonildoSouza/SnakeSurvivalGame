using Microsoft.Xna.Framework;

namespace SnakeGame.Scenes
{
    public sealed class GameSceneLevel06 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 06");

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 4.5f, SnakeGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 10.5f, SnakeGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 9.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 4.5f, SnakeGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 10.5f, SnakeGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 21.5f));

            base.LoadContent();
        }
    }
}
