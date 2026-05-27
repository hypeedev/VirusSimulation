public static class SpreadLogicFactory
{
    private static readonly ISpreadLogic uniform =
        new UniformSpreadLogic();
    private static readonly ISpreadLogic distanceWeighted =
        new DistanceWeightedSpreadLogic();
    private static readonly ISpreadLogic focused =
        new FocusedSpreadLogic();

    public static ISpreadLogic Create(SpreadLogicType type)
    {
        return type switch
        {
            SpreadLogicType.Uniform => uniform,
            SpreadLogicType.DistanceWeighted => distanceWeighted,
            SpreadLogicType.Focused => focused,
            _ => throw new ArgumentOutOfRangeException(
                nameof(type),
                type,
                "Unsupported spread logic type.")
        };
    }
}
