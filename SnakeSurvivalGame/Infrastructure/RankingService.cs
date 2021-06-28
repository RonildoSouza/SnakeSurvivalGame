﻿using Curupira2D.ECS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SnakeSurvivalGame.Infrastructure
{
    internal sealed class RankingService
    {
        const int MaxRankings = 23;

        readonly Scene _scene;
        static IList<Ranking> _rankings = new List<Ranking>(MaxRankings);

        public RankingService(Scene scene)
        {
            _scene = scene;

            // Initialize
            _rankings = GetAll().ToArray();
        }

        public IReadOnlyList<Ranking> Add(string playerName, int playerScore)
        {
            var ranking = new Ranking(playerName, playerScore);

            if (_rankings.Count == MaxRankings)
            {
                var minScore = _rankings.Min(_ => _.PlayerScore);
                var lastRanking = _rankings.LastOrDefault(_ => _.PlayerScore == minScore);

                _rankings.Remove(lastRanking);
            }

            _rankings.Add(ranking);

            var rankingsJson = JsonConvert.SerializeObject(_rankings);
            File.WriteAllText(GetRankingFilePath(), Convert.ToBase64String(Encoding.UTF8.GetBytes(rankingsJson)));

            return _rankings.OrderByDescending(_ => _.PlayerScore).Take(MaxRankings).ToList();
        }

        public IReadOnlyList<Ranking> GetAll()
        {
            var rankingsJson = File.ReadAllText(GetRankingFilePath());

            if (string.IsNullOrEmpty(rankingsJson))
                return _rankings.ToList();

            _rankings = JsonConvert.DeserializeObject<List<Ranking>>(
                Encoding.UTF8.GetString(Convert.FromBase64String(rankingsJson)));

            return _rankings.OrderByDescending(_ => _.PlayerScore).Take(MaxRankings).ToList();
        }

        string GetRankingFilePath() => $"{_scene.GameCore.Content.RootDirectory}/Infra/ranking.ss";
    }
}
