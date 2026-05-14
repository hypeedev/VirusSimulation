class Entity {
    public int x { get; set; }
    public int y { get; set; }
    public virtual Tile tile => Tile.Entity;
    public virtual void Update(Board board)
    {

    }
    public Entity(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
