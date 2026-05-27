public class Elder : Human
{
    public override char Symbol => 'O';

    public Elder(int x, int y)
        : base(x, y, RandomAge(65, 90))
    {

    }

    public override void Update(Board board)
    {
        // wolniejszy ruch
        if (Random.Shared.NextSingle() < 0.5f)
        {
            base.Update(board);
        }
    }
}
