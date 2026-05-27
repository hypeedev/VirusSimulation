public class UniformSpreadLogic : SpreadLogicBase
{
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
