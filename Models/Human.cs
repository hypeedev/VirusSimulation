public class Human : Entity, ITile
{
    // ITile implementation
    public override char Symbol => 'H';

    public override string Description => $"Human{(IsInfected ? " (Infected)" : "")}";
    public override int RenderPriority => 20;

    public bool IsAlive { get; private set; } = true;

    public Virus? Virus { get; private set; }

    public bool IsInfected => Virus != null;

    public int HealingTicks { get; set; }

    Random rng = new();

    public Human(int x, int y) : base(x, y)
    {

    }

    public override void Update(Board board)
    {
        Move(board);
    }

    public virtual void Move(Board board)
    {
        int dx = rng.Next(-1, 2);
        int dy = rng.Next(-1, 2);

        int nx = x + dx;
        int ny = y + dy;

        if (board.IsWalkable(nx, ny))
        {
            x = nx;
            y = ny;
        }
    }

    public void Infect(Virus virus)
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
    }
}
