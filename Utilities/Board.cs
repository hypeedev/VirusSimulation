public class Board
{
    Tile[,] grid;
    public List<Entity> entities = new List<Entity>();
    IRandom random;

    public int Width => grid.GetLength(0);
    public int Height => grid.GetLength(1);

    public float AwarenessLevel { get; set; }
    public bool LockdownEnabled { get; set; }

    public Board(int width, int height)
    {
        random = GameRandom.Create();
        var generator = new MapGenerator(width, height);
        grid = generator.Generate();
    }

    public Board(int width, int height, IRandom rng)
    {
        random = rng;
        var generator = new MapGenerator(width, height, rng);
        grid = generator.Generate();
    }

    public Board(int width, int height, bool useSimple)
    {
        random = GameRandom.Create();
        if (!useSimple)
        {
            var generator = new MapGenerator(width, height);
            grid = generator.Generate();
        }
        else
        {
            grid = new Tile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = Tile.Land;
                }
            }
        }
    }

    public Board(int width, int height, bool useSimple, IRandom rng)
    {
        random = rng;
        if (!useSimple)
        {
            var generator = new MapGenerator(width, height, rng);
            grid = generator.Generate();
        }
        else
        {
            grid = new Tile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = Tile.Land;
                }
            }
        }
    }

    public void addEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public void addRandomEntity(System.Func<int, int, Entity> factory)
    {
        while (true)
        {
            int x = random.Next(grid.GetLength(0));
            int y = random.Next(grid.GetLength(1));
            if (grid[x, y] is Tile.Land or Tile.Region0 or Tile.Region1 or Tile.Region2 or Tile.Region3)
            {
                addEntity(factory(x, y));
                return;
            }
        }
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
        return entities.FirstOrDefault(e =>
        {
            if (e.x != x || e.y != y)
                return false;

            if (e is Human h)
                return h.IsAlive;

            return true;
        });
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || y < 0 ||
            x >= grid.GetLength(0) ||
            y >= grid.GetLength(1))
        {
            return false;
        }

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
                if (human.IsAlive &&
                    human.IsInfected)
                {
                    result.Add(human);
                }
            }
        }

        return result;
    }

    public Tile GetRegionAt(int x, int y)
    {
        return grid[x, y];
    }
}
