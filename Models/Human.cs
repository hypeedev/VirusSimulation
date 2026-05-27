public class Human : Entity, ITile
{
    // ITile implementation
    public override char Symbol => 'H';

    public override string Description => $"Human{(IsInfected ? " (Infected)" : "")}";
    public override int RenderPriority => 20;

    public bool IsAlive { get; private set; } = true;

    public IVirus? Virus { get; private set; }

    public bool IsInfected => Virus != null;

    public int HealingTicks { get; set; }

    public Tile HomeRegion { get; set; }

    public int MigrationCooldown { get; set; }

    Random rng = new();

    public Human(int x, int y) : base(x, y)
    {
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

        if (rng.NextSingle() < 0.01f &&
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

                // nie można opuścić regionu
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

                MigrationCooldown = 50;

                break;
            }
        }
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
