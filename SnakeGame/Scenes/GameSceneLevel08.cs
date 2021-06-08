using Microsoft.Xna.Framework;

namespace SnakeGame.Scenes
{
    public sealed class GameSceneLevel08 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 08");
            SetNextGameSceneLevel(new GameSceneLevel09());

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 3.5f));

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 9.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 9.5f));


            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 15.5f));

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 3.5f, SnakeGameHelper.PixelSize * 21.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 11.5f, SnakeGameHelper.PixelSize * 21.5f));

            base.LoadContent();
        }
    }
}
