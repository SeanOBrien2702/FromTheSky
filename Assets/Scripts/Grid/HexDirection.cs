using System.Diagnostics;

namespace FTS.Grid
{
    public enum HexDirection
    {
        NE, E, SE, SW, W, NW, None
    }

    public enum AttackDirections
    {
        Forward, ForwardRight, BackwardsRight, Backwards, BackwardsLeft, ForawrdLeft
    }

    public static class HexDirectionExtensions
    {
        static int hexSides = 6;

        public static HexDirection Opposite(this HexDirection direction)
        {
            return (int)direction < 3 ? (direction + 3) : (direction - 3);
        }

        public static int Angle(this HexDirection direction)
        {
            return (int)direction * 60 + 30;
        }

        public static HexDirection LocalDirection(this HexDirection directionFacing, AttackDirections attackDirection)
        {
            int direction = (int)directionFacing + (int)attackDirection;
            if(direction >= hexSides) 
            {
                direction -= hexSides;
            }
            
            return (HexDirection)(direction);
        }
    }
}