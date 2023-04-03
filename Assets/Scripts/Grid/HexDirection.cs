namespace FTS.Grid
{
    public enum HexDirection
    {
        NE, E, SE, SW, W, NW, None
    }

    public static class HexDirectionExtensions
    {
        public static HexDirection Opposite(this HexDirection direction)
        {
            return (int)direction < 3 ? (direction + 3) : (direction - 3);
        }

        public static int Angle(this HexDirection direction)
        {
            return (int)direction * 60 + 30;
        }
    }
}