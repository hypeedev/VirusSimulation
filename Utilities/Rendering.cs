using System;

public class Renderer
{
    private readonly Board board;
    private readonly ColorScheme colorScheme = ColorScheme.Default();

    public Renderer(Board board)
    {
        this.board = board;
    }

    public void Render()
    {
        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                RenderCell(x, y);
            }
            Console.WriteLine();
        }
    }

    private void RenderCell(int x, int y)
    {
        var entity = board.GetEntityAt(x, y);

        if (entity != null)
        {
            RenderEntity(entity);
        }
        else
        {
            RenderTile(board.GetTileAt(x, y));
        }

        Console.Write(" ");
    }

    private void RenderEntity(Entity entity)
    {
        var color = colorScheme.GetEntityColor(entity);
        var symbol = GetEntitySymbol(entity);
        WriteColored(symbol.ToString(), color);
    }

    private void RenderTile(Tile tile)
    {
        var color = colorScheme.GetTileColor(tile);
        var symbol = tile.Symbol();
        WriteColored(symbol.ToString(), color);
    }

    private void WriteColored(string text, AnsiColor color)
    {
        if (color.Code == AnsiColor.None.Code)
        {
            Console.Write(text);
        }
        else
        {
            Console.Write(color.Code + text + AnsiColor.Reset.Code);
        }
    }

    private static char GetEntitySymbol(Entity entity)
    {
        return entity.Symbol;
    }
}

public readonly struct AnsiColor
{
    public readonly string Code;

    private AnsiColor(string code)
    {
        Code = code;
    }

    // Common ANSI color codes
    public static readonly AnsiColor None = new("");
    public static readonly AnsiColor Reset = new("\u001b[0m");

    // Foreground colors
    public static readonly AnsiColor Red = new("\u001b[31m");
    public static readonly AnsiColor Green = new("\u001b[32m");
    public static readonly AnsiColor Yellow = new("\u001b[33m");
    public static readonly AnsiColor Blue = new("\u001b[34m");
    public static readonly AnsiColor Magenta = new("\u001b[35m");
    public static readonly AnsiColor Cyan = new("\u001b[36m");
    public static readonly AnsiColor White = new("\u001b[37m");
    public static readonly AnsiColor Black = new("\u001b[30m");

    // Bright colors
    public static readonly AnsiColor BrightRed = new("\u001b[91m");
    public static readonly AnsiColor BrightGreen = new("\u001b[92m");
    public static readonly AnsiColor BrightYellow = new("\u001b[93m");
    public static readonly AnsiColor BrightBlue = new("\u001b[94m");
    public static readonly AnsiColor BrightMagenta = new("\u001b[95m");
    public static readonly AnsiColor BrightCyan = new("\u001b[96m");
    public static readonly AnsiColor BrightWhite = new("\u001b[97m");

    public static AnsiColor Custom(string ansiCode)
    {
        return new(ansiCode);
    }

    public static AnsiColor FromPalette(int colorIndex)
    {
        if (colorIndex < 0 || colorIndex > 255)
            throw new ArgumentException("Color index must be between 0 and 255");
        return new($"\u001b[38;5;{colorIndex}m");
    }

    public static AnsiColor FromRGB(int r, int g, int b)
    {
        if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            throw new ArgumentException("RGB values must be between 0 and 255");
        return new($"\u001b[38;2;{r};{g};{b}m");
    }
}

public class ColorScheme
{
    private Dictionary<Tile, AnsiColor> tileColors;
    private Dictionary<Type, AnsiColor> entityColors;

    public ColorScheme()
    {
        tileColors = new();
        entityColors = new();
    }

    public static ColorScheme Default()
    {
        var scheme = new ColorScheme();

        // Tile colors
        scheme.SetTileColor(Tile.Water, AnsiColor.BrightBlue);
        scheme.SetTileColor(Tile.Land, AnsiColor.Green);
        scheme.SetTileColor(Tile.Region0, AnsiColor.Green);
        scheme.SetTileColor(Tile.Region1, AnsiColor.Green);
        scheme.SetTileColor(Tile.Region2, AnsiColor.Green);
        scheme.SetTileColor(Tile.Region3, AnsiColor.Green);

        // Entity colors
        scheme.SetEntityColor(typeof(Human), AnsiColor.White);
        scheme.SetEntityColor(typeof(Doctor), AnsiColor.BrightGreen);
        scheme.SetEntityColor(typeof(Hospital), AnsiColor.Yellow);

        return scheme;
    }

    public void SetTileColor(Tile tile, AnsiColor color)
    {
        tileColors[tile] = color;
    }

    public void SetEntityColor(Type entityType, AnsiColor color)
    {
        entityColors[entityType] = color;
    }

    public AnsiColor GetTileColor(Tile tile)
    {
        return tileColors.TryGetValue(tile, out var color) ? color : AnsiColor.None;
    }

    public AnsiColor GetEntityColor(Entity entity)
    {
        // Check if human or doctor is infected
        if (entity is Human human && human.IsInfected)
            return AnsiColor.Red;

        var type = entity.GetType();

        if (entityColors.TryGetValue(type, out var color))
            return color;

        // Check base types
        type = type.BaseType;
        while (type != null && type != typeof(object))
        {
            if (entityColors.TryGetValue(type, out color))
                return color;
            type = type.BaseType;
        }

        return AnsiColor.None;
    }
}
