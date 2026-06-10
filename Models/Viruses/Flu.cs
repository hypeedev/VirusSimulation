public class Flu : IVirus
{
    public string Name { get; } = "Flu";

    public float Infectivity { get; } = 0.25f;

    public float Mortality { get; } = 0.005f;

    public int InfectionRange { get; } = 1;

    private readonly ISpreadLogic spreadLogic;

    public Flu()
    {
        spreadLogic = new UniformSpreadLogic();
    }

    public Flu(IRandom rng)
    {
        spreadLogic = new UniformSpreadLogic(rng);
    }

    public void Spread(Board board)
    {
        spreadLogic.Spread(this, board);
    }
}
