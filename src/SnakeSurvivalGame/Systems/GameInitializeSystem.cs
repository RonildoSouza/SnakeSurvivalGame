using Curupira2D.ECS;
using Curupira2D.ECS.Systems;
using Microsoft.Xna.Framework.Graphics;
using SnakeSurvivalGame.Helpers;
using System.IO;

namespace SnakeSurvivalGame.Systems
{
    public sealed class GameInitializeSystem : Curupira2D.ECS.System, ILoadable
    {
        public void LoadContent()
        {
            SnakeSurvivalGameHelper.SetSnakeSurvivalGameTextures(Scene.GameCore.Content.Load<Texture2D>("Textures/SnakeSurvivalGame"));
            SnakeSurvivalGameHelper.SetControlsTexture(Scene.GameCore.Content.Load<Texture2D>("Textures/Controls"));
            SnakeSurvivalGameHelper.SetSerpensRegularTTFData(File.ReadAllBytes($"{Scene.GameCore.Content.RootDirectory}/Fonts/SerpensRegular.ttf"));
            SnakeSurvivalGameHelper.SetFreePixelTTFData(File.ReadAllBytes($"{Scene.GameCore.Content.RootDirectory}/Fonts/FreePixel.ttf"));
            SnakeSurvivalGameHelper.SetMainFont(Scene.GameCore.Content.Load<SpriteFont>("Fonts/MainText"));
            SnakeSurvivalGameHelper.SetScoreFont(Scene.GameCore.Content.Load<SpriteFont>("Fonts/Score"));
        }
    }
}