using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;

namespace SnakeSurvivalGame.Scenes
{
    public sealed class GameSceneLevel02 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 02");
            SetNextGameSceneLevel(new GameSceneLevel03());

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 7.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 17.5f));

            base.LoadContent();
        }
    }
}
