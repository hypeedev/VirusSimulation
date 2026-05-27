public class Worker : Human
{
    public override char Symbol => 'W';

    public override string Description =>
        $"Worker{(IsInfected ? " (Infected)" : "")}";

    public Worker(int x, int y)
        : base(x, y, RandomAge(23, 60))
    {

    }

    public override void Move(Board board)
    {
        base.Move(board);
        base.Move(board);
    }
}
