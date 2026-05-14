class Doctor : Human, IHealingAbility
{
    public override Tile tile => Tile.Doctor;

    public int Range => 3;

    public float Effectiveness => 0.8f;

    public Doctor(int x, int y) : base(x, y)
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
