public class Board {
    Tile[,] grid;
    public List<Entity> entities = new List<Entity>();
    Random random = new Random();

    public int Width => grid.GetLength(0);
    public int Height => grid.GetLength(1);

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

    public void print()
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Entity? entity =
                    entities.FirstOrDefault(
                        e => e.x == x && e.y == y);

                if (entity != null)
                {
                    Console.Write(entity.Symbol + " ");
                }
                else
                {
                    Console.Write(grid[x, y].Symbol() + " ");
                }
            }

            Console.WriteLine();
        }
    }
    
    public Tile GetTileAt(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return Tile.Water;
        return grid[x, y];
    }

    public Entity? GetEntityAt(int x, int y)
    {
        return entities.FirstOrDefault(e => e.x == x && e.y == y);
    }
    
    public bool IsWalkable(int x, int y)
    {
        // poza mapa
        if (x < 0 || y < 0 ||
            x >= grid.GetLength(0) ||
            y >= grid.GetLength(1))
        {
            return false;
        }

        // woda
        if (grid[x, y] == Tile.Water)
        {
            return false;
        }

        return true;
    }
    
    public List<Human> GetHumansInRange(
        int cx,
        int cy,
        int range)
    {
        List<Human> result = new();

        foreach (var entity in entities)
        {
            if (entity is Human human)
            {
                int dx = human.x - cx;
                int dy = human.y - cy;

                if (dx * dx + dy * dy <= range * range)
                {
                    result.Add(human);
                }
            }
        }

        return result;
    }
    
    public List<Human> GetInfectedHumans()
    {
        List<Human> result = new();

        foreach (var entity in entities)
        {
            if (entity is Human human)
            {
                if (human.IsInfected)
                {
                    result.Add(human);
                }
            }
        }

        return result;
    }
}
