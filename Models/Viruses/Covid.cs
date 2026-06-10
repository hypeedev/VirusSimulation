public class Covid : IVirus
{
    public string Name { get; } = "Covid";

    public float Infectivity { get; } = 0.65f;

    public float Mortality { get; } = 0.02f;

    public int InfectionRange { get; } = 2;

    private readonly ISpreadLogic spreadLogic;

    public Covid()
    {
        spreadLogic = new DistanceWeightedSpreadLogic();
    }

    public Covid(IRandom rng)
    {
        spreadLogic = new DistanceWeightedSpreadLogic(rng);
    }

    public void Spread(Board board)
    {
        spreadLogic.Spread(this, board);
    }
}
