class Human : Entity {
    public override Tile tile => Tile.Human;

    public Human(int x, int y) : base(x, y) {}
}
