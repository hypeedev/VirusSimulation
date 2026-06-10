public abstract class SpreadLogicBase : ISpreadLogic
{
    protected readonly IRandom rng;

    protected SpreadLogicBase()
    {
        rng = GameRandom.Create();
    }

    protected SpreadLogicBase(IRandom rng)
    {
        this.rng = rng;
    }

    public void Spread(IVirus virus, Board board)
    {
        var infected = board.GetInfectedHumans();

        foreach (var source in infected)
        {
            if (!source.IsAlive)
                continue;

            SpreadFromSource(virus, board, source);

            if (!source.IsAlive)
                continue;

            if (rng.NextSingle() <= virus.Mortality)
            {
                source.Die();
            }
        }
    }

    protected abstract void SpreadFromSource(
        IVirus virus,
        Board board,
        Human source);

    protected void TryInfect(
        IVirus virus,
        Human source,
        Human target,
        float chance)
    {
        if (target == source)
            return;

        if (target.IsInfected)
            return;

        float clampedChance =
            MathF.Min(1f, MathF.Max(0f, chance));

        if (rng.NextSingle() <= clampedChance)
        {
            target.Infect(virus);
        }
    }
}
