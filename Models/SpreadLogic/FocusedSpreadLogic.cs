public class FocusedSpreadLogic : SpreadLogicBase
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

        List<Human> candidates = new();

        foreach (var target in nearby)
        {
            if (target == source)
                continue;

            if (target.IsInfected)
                continue;

            candidates.Add(target);
        }

        if (candidates.Count == 0)
            return;

        var chosen =
            candidates[rng.Next(candidates.Count)];

        TryInfect(virus, source, chosen, virus.Infectivity);
    }
}
