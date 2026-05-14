class Board {
    Tile[,] grid;
    List<Entity> entities = new List<Entity>();
    Random random = new Random();

    public Board(int width, int height) {
        var generator = new MapGenerator(width, height);
        grid = generator.Generate();
    }

    public Board(int width, int height, bool useSimple) {
        if (!useSimple) {
            var generator = new MapGenerator(width, height);
            grid = generator.Generate();
        } else {
            grid = new Tile[width, height];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    grid[x, y] = Tile.Land;
                }
            }
        }
    }

    public void addEntity(Entity entity) {
        entities.Add(entity);
        grid[entity.x, entity.y] = entity.tile;
    }

    // TODO: This can get into an infinite loop if the board is full of entities.
    public void addRandomEntity(System.Func<int, int, Entity> factory) {
        int x = random.Next(grid.GetLength(0));
        int y = random.Next(grid.GetLength(1));
        if (grid[x, y] != Tile.Land && grid[x, y] != Tile.Region0 && grid[x, y] != Tile.Region1 && grid[x, y] != Tile.Region2 && grid[x, y] != Tile.Region3) {
            addRandomEntity(factory);
            return;
        }
        addEntity(factory(x, y));
    }

    public void print() {
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                Tile tile = grid[x, y];
                Console.Write(tile.Symbol() + " ");
            }
            Console.WriteLine();
        }
    }
}
