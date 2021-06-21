using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeSurvivalGame.Systems
{
    [RequiredComponent(typeof(BlockControllerSystem), typeof(SpriteComponent))]
    public sealed class BlockControllerSystem : Curupira2D.ECS.System, ILoadable
    {
        readonly IEnumerable<Vector2> _blocksPosition;

        public BlockControllerSystem(IEnumerable<Vector2> blocksPosition)
        {
            _blocksPosition = blocksPosition;
        }

        public void LoadContent()
        {
            if (!_blocksPosition?.Any() ?? true)
                return;

            var blockSource = SnakeSurvivalGameHelper.GetSnakeTextureSource(SnakeTexture.Block);

            foreach (var block in _blocksPosition)
                Scene.CreateEntity($"{Guid.NewGuid()}", SnakeSurvivalGameHelper.BlockGroupName)
                    .SetPosition(block)
                    .AddComponent(new SpriteComponent(SnakeSurvivalGameHelper.SnakeSurvivalGameTextures, sourceRectangle: blockSource));
        }
    }
}
