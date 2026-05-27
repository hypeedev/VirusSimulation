public class DistanceWeightedSpreadLogic : SpreadLogicBase
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

        float maxDistance =
            MathF.Max(1f, virus.InfectionRange);

        foreach (var target in nearby)
        {
            int dx = target.x - source.x;
            int dy = target.y - source.y;

            float distance =
                MathF.Sqrt(dx * dx + dy * dy);

            float weight =
                1f - (distance / (maxDistance + 1f));

            float chance = virus.Infectivity * weight;

            TryInfect(virus, source, target, chance);
        }
    }
}
