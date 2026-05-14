public enum Tile {
    Water = ',',
    Land = '.',
    Entity = 'E',
    Human = 'H',
    Region0 = '0',
    Region1 = '1',
    Region2 = '2',
    Region3 = '3'
}

public static class TileExtensions {
    public static char Symbol(this Tile tile) {
        return (char)tile;
    }
}
