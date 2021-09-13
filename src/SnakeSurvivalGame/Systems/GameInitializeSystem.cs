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
            var SnakeSurvivalGameTextures = Scene.GameCore.Content.Load<Texture2D>("Textures/SnakeSurvivalGame");
            var serpensRegularTTFData = File.ReadAllBytes($"{Scene.GameCore.Content.RootDirectory}/Fonts/SerpensRegular.ttf");

            SnakeSurvivalGameHelper.SetSnakeSurvivalGameTextures(SnakeSurvivalGameTextures);
            SnakeSurvivalGameHelper.SetSerpensRegularTTFData(Scene, serpensRegularTTFData);
        }
    }
}