public class Human : Entity, ITile
{
    public override char Symbol => 'H';

    public override string Description => $"Human{(IsInfected ? " (Infected)" : "")}";
    public override int RenderPriority => 20;

    public bool IsAlive { get; private set; } = true;

    public IVirus? Virus { get; private set; }

    public bool IsInfected => Virus != null;

    public int HealingTicks { get; set; }

    public int Age { get; }

    public Tile HomeRegion { get; set; }

    public int MigrationCooldown { get; set; }

    protected IRandom rng;

    const float BaseMigrationChance = 0.03f;
    const float MinMigrationChance = 0.001f;
    const float MaxMigrationChance = 0.02f;

    public Human(int x, int y) : base(x, y)
    {
        rng = GameRandom.Create();
        Age = rng.Next(18, 70);
        HomeRegion = Tile.Region0;
    }

    public Human(int x, int y, IRandom rng) : base(x, y)
    {
        this.rng = rng;
        Age = rng.Next(18, 70);
        HomeRegion = Tile.Region0;
    }

    public Human(int x, int y, int age) : this(x, y)
    {
        Age = age;
    }

    protected Human(int x, int y, int age, IRandom rng) : base(x, y)
    {
        this.rng = rng;
        Age = age;
        HomeRegion = Tile.Region0;
    }

    public override void Update(Board board)
    {
        if (!IsAlive)
            return;

        Move(board);
    }

    public virtual void Move(Board board)
    {
        float movementChance =
            1f - board.AwarenessLevel;

        if (rng.NextSingle() > movementChance)
        {
            return;
        }

        if (MigrationCooldown > 0)
        {
            MigrationCooldown--;
        }

        if (rng.NextSingle() < GetMigrationChance() &&
            MigrationCooldown == 0)
        {
            LongDistanceMigration(board);
            return;
        }

        int dx = rng.Next(-1, 2);
        int dy = rng.Next(-1, 2);

        int nx = x + dx;
        int ny = y + dy;

        if (board.IsWalkable(nx, ny))
        {
            if (board.LockdownEnabled)
            {
                Tile currentRegion =
                    board.GetRegionAt(x, y);

                Tile nextRegion =
                    board.GetRegionAt(nx, ny);

                if (currentRegion != nextRegion)
                {
                    return;
                }
            }

            x = nx;
            y = ny;
        }
    }

    private void LongDistanceMigration(Board board)
    {
        for (int i = 0; i < 100; i++)
        {
            int nx = rng.Next(board.Width);
            int ny = rng.Next(board.Height);

            if (!board.IsWalkable(nx, ny))
                continue;

            Tile targetRegion =
                board.GetRegionAt(nx, ny);

            if (targetRegion != HomeRegion)
            {
                x = nx;
                y = ny;

                HomeRegion = targetRegion;

                MigrationCooldown = 15;

                break;
            }
        }
    }

    private float GetMigrationChance()
    {
        float chance =
            BaseMigrationChance *
            GetOccupationMultiplier() *
            GetAgeMultiplier();

        return Clamp(
            chance,
            MinMigrationChance,
            MaxMigrationChance);
    }

    private float GetOccupationMultiplier()
    {
        if (this is Student)
            return 1.4f;

        if (this is Worker)
            return 1.1f;

        if (this is Doctor)
            return 1.0f;

        if (this is Elder)
            return 0.7f;

        return 1.0f;
    }

    private float GetAgeMultiplier()
    {
        float ageFactor =
            1.4f - (Age / 100f);

        return Clamp(ageFactor, 0.6f, 1.4f);
    }

    private float Clamp(float value, float min, float max)
    {
        if (value < min)
            return min;

        if (value > max)
            return max;

        return value;
    }

    public virtual void Infect(IVirus virus)
    {
        if (Virus != null)
            return;

        Virus = virus;
    }

    public void Heal()
    {
        Virus = null;
        HealingTicks = 0;
    }

    public void Die()
    {
        IsAlive = false;
        Virus = null;
    }
}
