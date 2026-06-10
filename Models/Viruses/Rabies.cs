public class Rabies : IVirus
{
    public string Name { get; } = "Rabies";

    public float Infectivity { get; } = 0.5f;

    public float Mortality { get; } = 0.3f;

    public int InfectionRange { get; } = 1;

    private readonly ISpreadLogic spreadLogic;

    public Rabies()
    {
        spreadLogic = new FocusedSpreadLogic();
    }

    public Rabies(IRandom rng)
    {
        spreadLogic = new FocusedSpreadLogic(rng);
    }

    public void Spread(Board board)
    {
        spreadLogic.Spread(this, board);
    }
}
