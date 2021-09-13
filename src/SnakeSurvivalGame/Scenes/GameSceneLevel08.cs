using Microsoft.Xna.Framework;
using SnakeSurvivalGame.Helpers;

namespace SnakeSurvivalGame.Scenes
{
    public sealed class GameSceneLevel08 : GameSceneLevelBase
    {
        public override void LoadContent()
        {
            SetTitle("Level 08");
            SetNextGameSceneLevel(new GameSceneLevel09());

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 3.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 3.5f));

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 9.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 9.5f));


            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 15.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 15.5f));

            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 3.5f, SnakeSurvivalGameHelper.PixelSize * 21.5f));
            AddBlockPosition(new Vector2(SnakeSurvivalGameHelper.PixelSize * 11.5f, SnakeSurvivalGameHelper.PixelSize * 21.5f));

            base.LoadContent();
        }
    }
}
