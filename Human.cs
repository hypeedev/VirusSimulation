class Human : Entity
{
    public override Tile tile => Tile.Human;

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
