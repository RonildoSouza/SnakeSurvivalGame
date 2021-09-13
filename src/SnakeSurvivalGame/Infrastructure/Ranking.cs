namespace SnakeSurvivalGame.Infrastructure
{
    internal sealed class Ranking
    {
        public Ranking(string playerName, int playerScore)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
        }

        public string PlayerName { get; }
        public int PlayerScore { get; }
    }
}
