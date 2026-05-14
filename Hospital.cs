class Hospital : Entity, IHealingAbility
{
    public override Tile tile => Tile.Hospital;

    public int Range => 2;

    public float Effectiveness => 1.0f;

    public Hospital(int x, int y) : base(x, y)
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
