namespace Checkers.MVVM.Model
{
    public class Result
    {
        public Player Winner { get; }

        public Result(Player winner)
        {
            Winner = winner;
        }

        public static Result Win(Player winner)
        {
            if (winner == Player.White)
                return new Result(winner);
            else
                return new Result(winner);
        }
    }
}
