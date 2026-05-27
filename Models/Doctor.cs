public class Doctor : Human, ITile, IHealingAbility
{
    // ITile implementation
    public override char Symbol => 'D';

    public override string Description => "Doctor";
    public override int RenderPriority => 25;

    public int Range => 3;

    public float Effectiveness => 0.8f;

    public Doctor(int x, int y) : base(x, y, RandomAge(28, 65))
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
