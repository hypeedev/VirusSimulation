public class UniformSpreadLogic : SpreadLogicBase
{
    public UniformSpreadLogic() : base()
    {
    }

    public UniformSpreadLogic(IRandom rng) : base(rng)
    {
    }

    protected override void SpreadFromSource(
        IVirus virus,
        Board board,
        Human source)
    {
        var nearby =
            board.GetHumansInRange(
                source.x,
                source.y,
                virus.InfectionRange);

        foreach (var target in nearby)
        {
            TryInfect(virus, source, target, virus.Infectivity);
        }
    }
}
