public interface ITile
{
    char Symbol { get; }

    string Description { get; }

    int RenderPriority { get; }
}
