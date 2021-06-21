using Microsoft.Xna.Framework;

namespace SnakeSurvivalGame.Scenes
{
    public sealed class GameSceneLevel03 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 03");
            SetNextGameSceneLevel(new GameSceneLevel04());

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 6.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 12.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 7.5f, SnakeSurvivalGameHelper.PixelSize * 18.5f));

            base.LoadContent();
        }
    }
}
