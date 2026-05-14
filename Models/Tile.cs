public enum Tile {
    Water = '~',
    Land = 'L',
    Region0 = '.',
    Region1 = '.',
    Region2 = '.',
    Region3 = '.'
}

public static class TileExtensions {
    public static char Symbol(this Tile tile) {
        return (char)tile;
    }
}
