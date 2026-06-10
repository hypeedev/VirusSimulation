public enum Tile
{
    Water,
    Land,
    Region0,
    Region1,
    Region2,
    Region3
}

public static class TileExtensions
{
    public static char Symbol(this Tile tile)
    {
        return tile switch
        {
            Tile.Water => '~',
            Tile.Land => 'L',
            Tile.Region0 or Tile.Region1 or Tile.Region2 or Tile.Region3 => '.',
            _ => '?'
        };
    }
}
