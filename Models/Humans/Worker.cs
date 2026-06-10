public class Worker : Human
{
    public override char Symbol => 'W';

    public override string Description =>
        $"Worker{(IsInfected ? " (Infected)" : "")}";

    public Worker(int x, int y)
        : base(x, y, GameRandom.Create().Next(23, 61), GameRandom.Create())
    {

    }

    public Worker(int x, int y, IRandom rng)
        : base(x, y, rng.Next(23, 61), rng)
    {

    }

    public override void Move(Board board)
    {
        base.Move(board);
        base.Move(board);
    }
}
