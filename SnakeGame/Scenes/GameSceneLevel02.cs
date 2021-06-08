using Microsoft.Xna.Framework;

namespace SnakeGame.Scenes
{
    public sealed class GameSceneLevel02 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 02");
            SetNextGameSceneLevel(new GameSceneLevel03());

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 7.5f));
            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 17.5f));

            base.LoadContent();
        }
    }
}
