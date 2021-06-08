using Microsoft.Xna.Framework;

namespace SnakeGame.Scenes
{

    public sealed class GameSceneLevel01 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 01");
            SetNextGameSceneLevel(new GameSceneLevel02());

            AddBlockPosition(new Vector2(SnakeGameHelper.PixelSize * 7.5f, SnakeGameHelper.PixelSize * 12.5f));

            base.LoadContent();
        }
    }
}
