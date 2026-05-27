public class Entity : ITile
{
    public int x { get; set; }
    public int y { get; set; }

    // ITile implementation
    public virtual char Symbol => 'E';
    public virtual string Description => GetType().Name;
    public virtual int RenderPriority => 10;

    public virtual void Update(Board board)
    {

    }

    public Entity(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
