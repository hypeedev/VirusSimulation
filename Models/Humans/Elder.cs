public class Elder : Human
{
    public override char Symbol => 'O';

    public Elder(int x, int y)
        : base(x, y, GameRandom.Create().Next(65, 91), GameRandom.Create())
    {

    }

    public Elder(int x, int y, IRandom rng)
        : base(x, y, rng.Next(65, 91), rng)
    {

    }

    public override void Update(Board board)
    {
        if (rng.NextSingle() < 0.5f)
        {
            base.Update(board);
        }
    }
}
