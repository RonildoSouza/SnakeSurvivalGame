using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;

namespace SnakeSurvivalGame.Scenes
{
    public sealed class GameSceneLevel06 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 06");
            SetNextGameSceneLevel(new GameSceneLevel07());

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 9.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 21.5f));

            base.LoadContent();
        }
    }
}
