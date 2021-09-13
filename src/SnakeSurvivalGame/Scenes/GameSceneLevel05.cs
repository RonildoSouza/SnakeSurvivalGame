using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;

namespace SnakeSurvivalGame.Scenes
{
    public sealed class GameSceneLevel05 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 05");
            SetNextGameSceneLevel(new GameSceneLevel06());

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 6.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 6.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 12.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 18.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 18.5f));

            base.LoadContent();
        }
    }
}
