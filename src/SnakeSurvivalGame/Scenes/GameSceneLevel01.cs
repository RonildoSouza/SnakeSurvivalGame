using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;

namespace SnakeSurvivalGame.Scenes
{

    public sealed class GameSceneLevel01 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 01");
            SetNextGameSceneLevel(new GameSceneLevel02());

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 12.5f));

            base.LoadContent();
        }
    }
}
