using UnityEngine;

namespace SP.Grid
{
    [System.Serializable]
    public struct HexCoordinates
    {

        [SerializeField]
        private int x, z;

        public HexCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        #region Properties
        public int X   // property
        {
            get { return x; }   // get method
            set { x = value; }  // set method
        }

        public int Z   // property
        {
            get { return z; }   // get method
            set { z = value; }  // set method
        }
        public int Y
        {
            get
            {
                return -x - z;
            }
        }
        #endregion

        #region Public Methods
        //conditional operator
        public int DistanceTo(HexCoordinates other)
        {
            return (CheckDistance(x, other.x) + CheckDistance(Y, other.Y) + CheckDistance(z, other.z)) / 2;
        }

        private int CheckDistance(int corrd, int axis)
        {
            if (corrd < axis)
            {
                return axis - corrd;
            }
            else
            {
                return corrd - axis;
            }
        }

        public static HexCoordinates FromOffsetCoordinates(int x, int z)
        {
            return new HexCoordinates(x - z / 2, z);
        }


        public override string ToString()
        {
            return "(" +
                x.ToString() + ", " + Y.ToString() + ", " + z.ToString() + ")";
        }

        public string ToStringOnSeparateLines()
        {
            return x.ToString() + "\n" + Y.ToString() + "\n" + z.ToString();
        }

        public static HexCoordinates FromPosition(Vector3 position)
        {
            float x = position.x / (HexMetrics.innerRadius * 2f);
            float y = -x;
            float offset = position.z / (HexMetrics.outerRadius * 3f);
            x -= offset;
            y -= offset;
            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);

            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x - y - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = -iY - iZ;
                }
                else if (dZ > dY)
                {
                    iZ = -iX - iY;
                }
            }

            return new HexCoordinates(iX, iZ);
        }
        #endregion
    }
}
