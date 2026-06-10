public class Doctor : Human, ITile, IHealingAbility
{
    public override char Symbol => 'D';

    public override string Description => "Doctor";
    public override int RenderPriority => 25;

    public int Range => 3;

    public float Effectiveness => 0.8f;

    public Doctor(int x, int y)
        : base(x, y, GameRandom.Create().Next(28, 66), GameRandom.Create())
    {

    }

    public Doctor(int x, int y, IRandom rng)
        : base(x, y, rng.Next(28, 66), rng)
    {

    }

    public void Heal(Human target)
    {
        if (!target.IsInfected)
            return;

        target.HealingTicks++;

        if (target.HealingTicks >= 5)
        {
            target.Heal();
        }
    }

    public override void Update(Board board)
    {
        base.Update(board);

        var nearby = board.GetHumansInRange(x, y, Range);

        foreach (var human in nearby)
        {
            if (human.IsInfected)
            {
                Heal(human);
            }
        }
    }
}
