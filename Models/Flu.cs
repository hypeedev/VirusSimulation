public class Flu : IVirus
{
    public string Name { get; } = "Flu";

    public float Infectivity { get; } = 0.15f;

    public float Mortality { get; } = 0.005f;

    public int InfectionRange { get; } = 1;

    private readonly ISpreadLogic spreadLogic =
        SpreadLogicFactory.Create(
            SpreadLogicType.Uniform);

    public void Spread(Board board)
    {
        spreadLogic.Spread(this, board);
    }
}
