namespace Core.ScoreCounting
{
    public interface IScoreCounter
    {
        public float CalculateScore(float time);
        public float CalculateMaxBonus();
    }
}
