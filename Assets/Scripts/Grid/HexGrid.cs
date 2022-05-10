#region Using Statements
using FTS.Cards;
using FTS.Core;
using FTS.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
#endregion

namespace FTS.Grid
{
    public class HexGrid : MonoBehaviour
    {
        [Header("Grid size")]
        [Range(5, 20)]
        [SerializeField] int width = 5;
        [Range(5, 20)]
        [SerializeField] int height = 5;
        [Header("Hex prefabs")]
        [SerializeField] HexCell cellPrefab;
        [SerializeField] GameObject startHex;
        //[SerializeField] GameObject grassland;
        //[SerializeField] GameObject water;
        [SerializeField] GameObject mountainModel;
        [SerializeField] Material grassland;
        [SerializeField] Material water;
        [SerializeField] Material mountain;
        [SerializeField] GameObject DestinationUI;
        [SerializeField] Text cellLabel;
        DestinationController destinationController;

        int hexSides = 6;
        int heuristicModifier = 1;
        int startingPosModifier = 3; 
        HexCellPriorityQueue searchFrontier;
        int searchFrontierPhase;

        int numOfCells;
        HexCell[] cells;
        List<HexCell> currentPath = new List<HexCell>();
        List<HexCell> reachable = new List<HexCell>();
        List<HexCell> currentArea = new List<HexCell>();

        List<HexCell> vehicleDestinations = new List<HexCell>();

        HexCell currentPathFrom, currentPathTo, vehicleStart;
        bool currentPathExists;

        #region Properties
        public bool HasPath
        {
            get { return currentPathExists; }
        }
        public HexCell VehicleStart
        {
            get { return vehicleStart; }
        }
        public int Width   // property
        {
            get { return width; }   // get method
            set { width = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        /*
        * FUNCTION    : Awake()
        * DESCRIPTION : Awake is called when the script instance is being loaded.
        *               Create hex grid and path for the vehicle to travel.
        * PARAMETERS  : void
        * RETURNS     : void
        */
        private void Awake()
        {
            
            destinationController = GetComponent<DestinationController>();
            CreateGrid();
            numOfCells = cells.Length;
            SetVehicleDestinations();
        }

        void Start()
        {
            //hexMesh.Triangulate(cells);
        }
        #endregion


        #region Private Methods
        /*
        * FUNCTION    : CreateGrid()
        * DESCRIPTION : Create grid based on the height and width dimensions 
        *               determined in the inspector
        * PARAMETERS  : void
        * RETURNS     : void
        */
        void CreateGrid()
        {
            cells = new HexCell[height * width];
            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }         
        }

        /*
        * FUNCTION    : CreateCell(int x, int z, int i)
        * DESCRIPTION : Instantiate a new hex cell. Set the position of the hex and
        *               determine randomly the type cell used. Each neighbor is stored
        *               for later uses.
        * PARAMETERS  : int x - x coordinate position
        *               int z - x coordinate position
        *               int i - index in the position in the collection of hex cells
        * RETURNS     : void
        */
        void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            HexCell cell;
            //Create cell position
            cell = cells[i] = Instantiate<HexCell>(cellPrefab);
            int randomNumber = UnityEngine.Random.Range(0, 80);

            if (randomNumber <= 70)
            {
                cell.SetMeshMaterial(grassland);
                cell.MovementCost = 1;
            }
            else if (randomNumber > 70 && randomNumber <= 90)
            {
                cell.SetMeshMaterial(water);
                cell.MovementCost = 2;
            }
            else
            {
                cell.SetMeshMaterial(mountain);
                cell.SetModel(mountainModel);
                cell.IsObstacle = true;
            }

            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.Location = HexCoordinates.FromOffsetCoordinates(x, z);

            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, cells[i - 1]);
            }
            if (z > 0)
            {
                if ((z & 1) == 0)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                    }
                }
                else
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                    if (x < width - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                    }
                }
            }
        }

        /*
        * FUNCTION    : SetVehicleDestinations()
        * DESCRIPTION : Set the 2-3 destinations 
        * PARAMETERS  : void
        * RETURNS     : void
        */
        private void SetVehicleDestinations()
        {
            //TODO: set different effects when reaching the end of the level. Currently just draft for testing purposes
            //int numDestinations = UnityEngine.Random.Range(2, 3);
            HexCell fromCell = FindGridEdge(true);           
            vehicleStart = fromCell;

            //for (int i = 0; i < numDestinations; i++)
            //{
            //    HexCell toCell = FindGridEdge(false);
            //    vehicleDestinations.Add(toCell);
            //}

            //destinationController.SetDestination(vehicleDestinations);
            destinationController.SetDestination(FindGridEdge(false));
        }

        /*
        * FUNCTION    : FindGridEdge(bool bottom)
        * DESCRIPTION : Randomly find a hex at the top and bottom of the map for 
        *               the vehicles path. Bottom position is set within a range 
        *               center of the map. Top position can be anywhere along the top.
        *               Function is called recursively until a hex that is not an obstacle is found
        * PARAMETERS  : bool bottom - finding bottom position if true
        * RETURNS     : HexCell cell - The position found
        */
        private HexCell FindGridEdge(bool bottom)
        {
            HexCell cell;
            int randomNumber;
            if (bottom)
            {
                randomNumber = UnityEngine.Random.Range(width / 2 - startingPosModifier, width / 2 + startingPosModifier); 
            }
            else
            {
                randomNumber = UnityEngine.Random.Range(numOfCells - width, numOfCells);
            }
            cell = cells[randomNumber];
            if (cell.IsObstacle && !vehicleDestinations.Contains(cell))
            {
                cell = FindGridEdge(bottom);
            }

            return cell;
        }

        /*
        * FUNCTION    : FindGridEdge(bool bottom)
        * DESCRIPTION : Randomly find a hex at the top and bottom of the map for 
        *               the vehicles path. Bottom position is set within a range 
        *               center of the map. Top position can be anywhere along the top.
        *               Function is called recursively until a hex that is not an obstacle is found
        * PARAMETERS  : bool bottom - finding bottom position if true
        * RETURNS     : HexCell cell - The position found
        */
        public HexCell FindGridEdge()
        {
            HexCell cell;
            int randomNumber = UnityEngine.Random.Range(0, width * 2 + height * 2 - 3);
            if (randomNumber > width -1)
            {
                if(randomNumber < width + height * 2 - 2)
                {
                    if (randomNumber % 2 == 0)
                    {
                        randomNumber = (randomNumber - width) / 2 * width + width;
                    }
                    else
                    {
                        randomNumber = (randomNumber - width) / 2 * width + width * 2 - 1;
                    }
                }
                else
                {
                    randomNumber += (width - 2) * (height - 2) - 1; 
                }
            }
            cell = cells[randomNumber];

            if (cell.IsObstacle || cell.Unit || cell.IsDestination)
            {
                cell = FindGridEdge();
            }
            return cell;
        }




        /*
        * FUNCTION    : Search(HexCell fromCell, HexCell toCell)
        * DESCRIPTION : A* (A Star) pathfinding algorithm is used to find the cost
        *               effective path to take.
        * PARAMETERS  : HexCell fromCell - starting position of the unit
        *               HexCell toCell - destination for the algorithm 
        * RETURNS     : bool - true if the path exist
        */
        bool Search(HexCell fromCell, HexCell toCell)
        {
            searchFrontierPhase += 2;
            bool pathExists = false;
            if (searchFrontier == null)
            {
                searchFrontier = new HexCellPriorityQueue();
            }
            else
            {
                searchFrontier.Clear();
            }
            fromCell.SearchPhase = searchFrontierPhase;
            fromCell.Distance = 0;
            searchFrontier.Enqueue(fromCell);
            HexCell current = null;
            while (searchFrontier.Count > 0)
            {
                current = searchFrontier.Dequeue();
                current.SearchPhase += 1;
                if (current == toCell)
                {
                    pathExists = true;
                    break;
                }

                for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
                {
                    HexCell neighbor = current.GetNeighbor(d);
                    if (neighbor == null ||
                    neighbor.SearchPhase > searchFrontierPhase)
                    {
                        continue;
                    }
                    if (neighbor.IsObstacle || neighbor.Unit)
                    {
                        continue;
                    }

                    int distance = current.Distance + neighbor.MovementCost;
                    if (neighbor.SearchPhase < searchFrontierPhase)
                    {
                        neighbor.SearchPhase = searchFrontierPhase;
                        neighbor.Distance = distance;
                        neighbor.PathFrom = current;
                        neighbor.SearchHeuristic = neighbor.Location.DistanceTo(toCell.Location) * heuristicModifier;
                        searchFrontier.Enqueue(neighbor);
                    }
                    else if (distance < neighbor.Distance)
                    {
                        int oldPriority = neighbor.SearchPriority;
                        neighbor.Distance = distance;
                        neighbor.PathFrom = current;
                        searchFrontier.Change(neighbor, oldPriority);
                    }
                }
            }
            if(pathExists)
            {
                Debug.Log("path exists " + current.Location);
                toCell.SetHighlight(HighlightIndex.Highlight, true);
            }
            else
            {
                Debug.Log("path does not exists " + current.Location + " to cell "+ toCell.Location);
                Debug.Log("path does not exists " + current.Unit + " to cell " + toCell.Unit);
                toCell.SetHighlight(HighlightIndex.CantReach, true);
            }
            return pathExists;
        }

        /*
        * FUNCTION    : ShowPath(int movement)
        * DESCRIPTION : Highlight on the hex grid the current path. If the unit does not
        *               have the movement to reach the distance on the path than the path
        *               will be highlighted red
        * PARAMETERS  : int movement - how for the unit can move along the path
        * RETURNS     : void
        */
        void ShowPath(int movement)
        {
            if (currentPathExists)
            {
                HexCell current = currentPathTo;
                while (current != currentPathFrom)
                {
                    current.SetLabel(current.Distance);
                    if (current.Distance <= movement)
                    {
                        current.SetHighlight(HighlightIndex.CanReach);
                    }
                    else
                    {
                        current.SetHighlight(HighlightIndex.CantReach);
                    }
                    current = current.PathFrom;
                }
            }
            //highlight start
            currentPathFrom.SetHighlight(HighlightIndex.Highlight);
        }

        ///*
        //* FUNCTION    : GetRangeOffset(HexCell targetCell, int range)
        //* DESCRIPTION : Find the position enemy units need to reach for them to
        //*               be within range of attacking
        //* PARAMETERS  : HexCell targetCell - target enemy is attacking
        //*               int range - range of the enemies attack
        //* RETURNS     : HexCell - Position for attacking
        //*/
        //private HexCell GetRangeOffset(HexCell fromCell, HexCell targetCell, int range)
        //{
        //    List<HexCell> offSets = new List<HexCell>();

        //    for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        //    {
        //        HexCell neighbor = targetCell.GetNeighbor(d);
        //        if (neighbor != null && !neighbor.IsObstacle)
        //            if (IsCellAvailable(neighbor))
        //            {
        //                for (int i = 0; i < range; i++)
        //                {
        //                    if (!IsCellAvailable(neighbor))
        //                        if (neighbor.GetNeighbor(d) == null || neighbor.GetNeighbor(d).IsObstacle || neighbor.GetNeighbor(d).Unit)
        //                        {
        //                            break;
        //                        }
        //                    neighbor = neighbor.GetNeighbor(d);
        //                }
        //                offSets.Add(neighbor);
        //            }
        //    }

        //    return ClosesToUnit(fromCell, targetCell, offSets);
        //}

        /*
        * FUNCTION    : GetRangeOffset(HexCell targetCell, int range)
        * DESCRIPTION : Find the position enemy units need to reach for them to
        *               be within range of attacking
        * PARAMETERS  : HexCell targetCell - target enemy is attacking
        *               int range - range of the enemies attack
        * RETURNS     : HexCell - Position for attacking
        */
        private HexCell GetRangeOffset(HexCell playerCell, HexCell enemyCell, int range)
        {
            List<HexCell> ring = new List<HexCell>();

            ring = GetRing(playerCell, range);

            return ClosesToUnit(playerCell, enemyCell, ring);
        }

        private HexCell ClosesToUnit(HexCell playerCell, HexCell enemyCell, List<HexCell> offSets)
        {
            HexCell target = null;
            List<HexCell> shortestPath = new List<HexCell>();
            int shortestDistance = 1000;
            foreach (var offSet in offSets)
            {
                FindPath(enemyCell, offSet, 10, false);

                int distance = GetPathDistance();
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    target = offSet;
                }
            }
            return target;
        }


        bool IsCellAvailable(HexCell cell)
        {
            return (cell != null
                && !cell.IsObstacle
                && cell.Unit != null);
        }
        #endregion


        #region Public Methods
        public HexCell GetCell(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return GetCell(hit.point);
            }
            return null;
        }

        public HexCell GetCell(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            if (index < 0)
            {
                return null;
            }
            return cells[index];
        }

        public HexCell GetCell(HexCoordinates coordinates)
        {
            int z = coordinates.Z;
            if (z < 0 || z >= height)
            {
                return null;
            }
            int x = coordinates.X + z / 2;
            if (x < 0 || x >= width)
            {
                return null;
            }
            return cells[x + z * width];
        }

        //Temporary code for testing on units 
        public HexCell[] GetGrid()
        {
            return cells;
        }
        public Vector3 GetLastCellPosition()
        {
            return cells.Last().transform.position;
        }
        
        public bool FindPath(HexCell fromCell, HexCell toCell, int speed , bool isPlayer)
        {
            if (isPlayer)
            {
                ClearPath(speed);
            }
            currentPathFrom = fromCell;
            currentPathTo = toCell;
            currentPathExists = Search(fromCell, toCell);
            if (isPlayer)
            {
                ShowPath(speed);
            }
            return currentPathExists;
        }

        //need to fix the parameters for what is passed into this function.
        //distance is not checked from the enemy
        //public HexCell FindPath(HexCell fromCell, HexCell targetCell, int speed, int range)
        public HexCell FindPath(HexCell playerCell, HexCell enemyCell, int speed, int range)
        {
            
            currentPathFrom = enemyCell;
            currentPathTo = GetRangeOffset(playerCell, enemyCell, range);
            currentPathExists = Search(enemyCell, currentPathTo);

            return currentPathTo;
        }
        internal bool FindPathAway(HexCell fromCell, HexDirection direction , int movementLeft)
        {
            currentPathFrom = fromCell;
            for (int i = 0; i < movementLeft; i++)
            {
                if (currentPathTo.GetNeighbor(direction) != null)
                {
                    currentPathTo = currentPathTo.GetNeighbor(direction);
                }
            }
            
            currentPathExists = false;
            while (!currentPathExists)
            {
                currentPathExists = Search(fromCell, currentPathTo);
            }
                        
            return currentPathTo;
        }

        internal void ShowPlacementArea(Vector2Int dimentions)
        {
            vehicleStart.SetHighlight(HighlightIndex.CantReach);

            HexCoordinates startPos = vehicleStart.Location;
            for (int x = -dimentions.x; x <= dimentions.x; x++)
            {
                for (int z = 0; z <= dimentions.y; z++)
                {
                    HexCoordinates coordinates = new HexCoordinates(x, z);
                    coordinates.X += startPos.X - z / 2;
                    coordinates.Z += startPos.Z;
                    HexCell cell = GetCell(coordinates);
                    if (cell != null)
                    {
                        cell.SetHighlight(HighlightIndex.CantReach);
                        currentArea.Add(cell);
                    }
                }
            }
        }

        internal void ShowReachableHexes(HexCell cell, int speed)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].Distance = int.MaxValue;
            }

            Queue<HexCell> frontier = new Queue<HexCell>();
            cell.Distance = 0;
            frontier.Enqueue(cell);
            while (frontier.Count > 0)
            {
                HexCell current = frontier.Dequeue();
                for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
                {

                    HexCell neighbor = current.GetNeighbor(d);

                    if (neighbor != null)
                    {
                        if (neighbor.IsObstacle)
                        {
                            continue;
                        }
                        if (current.Distance < neighbor.Distance)
                        {
                            neighbor.Distance = current.Distance + neighbor.MovementCost;

                            if (neighbor.Distance <= speed && current.Distance < neighbor.Distance)
                            {
                                frontier.Enqueue(neighbor);
                                neighbor.SetHighlight(HighlightIndex.Highlight);
                                reachable.Add(neighbor);
                            }
                        }
                    }
                }
            }
        }

        internal void ClearReachable()
        {
            if (currentArea != null)
            {
                foreach (HexCell cell in reachable)
                {
                    cell.SetHighlight(HighlightIndex.Default);
                }
            }
        }


        public List<HexCell> GetRing(HexCell fromCell, int radius)
        {
            List<HexCell> ring = new List<HexCell>();
            HexCoordinates startPos = fromCell.Location;
            for (int x = -radius; x <= radius; x++)
            {
                for (int z = Mathf.Max(-radius, -x - radius); z <= Mathf.Min(radius, -x + radius); z++)
                {
                    HexCoordinates coordinates = new HexCoordinates(x, z);
                    coordinates.X += startPos.X;
                    coordinates.Z += startPos.Z;
                    HexCell cell = GetCell(coordinates);
                    if (cell != null)
                    {
                        if (cell.Location.DistanceTo(fromCell.Location) == radius &&
                            !cell.IsObstacle)  
                        {
                            //Debug.Log("add to ring");
                            ring.Add(cell);
                        }
                    }                   
                }
            }
            return ring;
        }



        public List<HexCell> GetArea(HexCell fromCell, int radius)
        {
            List<HexCell> area = new List<HexCell>();
            fromCell.SetHighlight(HighlightIndex.CantReach);

            HexCoordinates startPos = fromCell.Location;
            for (int x = -radius; x <= radius; x++)
            {
                for (int z = Mathf.Max(-radius, -x - radius); z <= Mathf.Min(radius, -x + radius); z++)
                {
                    HexCoordinates coordinates = new HexCoordinates(x, z);
                    coordinates.X += startPos.X;
                    coordinates.Z += startPos.Z;
                    HexCell cell = GetCell(coordinates);
                    if (cell != null)
                    {
                        //cell.SetHighlight(HighlightIndex.CantReachColour);
                        Debug.Log("Cell found: " + cell.Location);
                        area.Add(cell);
                    }
                }
            }
            return area;
        }

        internal List<HexCell> GetLine(Character player, HexCell target, int length)
        {
            HexCell buffer = player.GetComponent<Mover>().Location;
            HexDirection direction = GetDirection(buffer, target);
            List<HexCell> line = new List<HexCell>();

            for (int i = 0; i < length; i++)
            {
                if(buffer.GetNeighbor(direction) == null)
                {
                    break;
                }
                line.Add(buffer.GetNeighbor(direction));
                buffer = buffer.GetNeighbor(direction);        
            }
            return line;
        }


        public void ShowArea(HexCell fromCell, int range)
        {
            HexCoordinates startPos = fromCell.Location;
            for (int x = -range; x <= range; x++)
            {
                for (int z = Mathf.Max(-range, -x - range); z <= Mathf.Min(range, -x + range); z++)
                {
                    HexCoordinates coordinates = new HexCoordinates(x, z);
                    coordinates.X += startPos.X;
                    coordinates.Z += startPos.Z;
                    HexCell cell = GetCell(coordinates);
                    if (cell != null)
                    {
                        cell.SetHighlight(HighlightIndex.Attack);
                        currentArea.Add(cell);
                    }
                }
            }
        }

        public void ClearArea()
        {
            if (currentArea != null)
            {
                foreach (HexCell cell in currentArea)
                {
                    cell.SetHighlight(HighlightIndex.Default);
                }
            }
        }

        public List<HexCell> GetPath(int speed)
        {
            currentPath.Clear();
            if (!currentPathExists)
            {
                return null;
            }
            for (HexCell cell = currentPathTo; cell != currentPathFrom; cell = cell.PathFrom)
            {
                if (cell.Distance <= speed)
                    currentPath.Add(cell);
            }
            currentPath.Add(currentPathFrom);
            currentPath.Reverse();
            Debug.Log("path length: " + currentPath.Count);
            return currentPath;
        }

        public int GetPathDistance()
        {
            GetPath(1000);
            int distance = 0;
            if (currentPathExists)
            {
                foreach (var item in currentPath)
                {
                    distance += item.MovementCost;
                }
            }
            return distance;
        }
        

        public void ClearPath(int speed)
        {
            if (currentPathExists)
            {
                HexCell current = currentPathTo;
                while (current != null && current != currentPathFrom)
                {                
                    if (current.Distance <= speed)
                    {
                        current.SetHighlight(HighlightIndex.Highlight);
                    }
                    else
                    {
                        current.SetHighlight(HighlightIndex.Default);
                    }
                    current.SetLabel(0);
                    current = current.PathFrom;
                }
                current.SetHighlight(HighlightIndex.Default);
                currentPathExists = false;
            }
            else if (currentPathFrom && currentPathTo)
            {
                currentPathFrom.SetHighlight(HighlightIndex.Default);
                currentPathTo.SetHighlight(HighlightIndex.Default);
            }
            currentPathFrom = currentPathTo = null;
        }
        public void ClearPath()
        {
            if (currentPathExists)
            {
                HexCell current = currentPathTo;
                while (current != null && current != currentPathFrom)
                {
                    current.SetHighlight(HighlightIndex.Default);
                    current.SetLabel(0);
                    current = current.PathFrom;
                }
                current.SetHighlight(HighlightIndex.Default);
                currentPathExists = false;
            }
            else if (currentPathFrom && currentPathTo)
            {
                currentPathFrom.SetHighlight(HighlightIndex.Default);
                currentPathTo.SetHighlight(HighlightIndex.Default);
            }
            currentPathFrom = currentPathTo = null;
        }

        public HexDirection GetDirection(HexCell pivot, HexCell target)
        {
            HexDirection direction = HexDirection.NW;
            int x = pivot.Location.X - target.Location.X;
            int y = pivot.Location.Y - target.Location.Y;
            int z = pivot.Location.Z - target.Location.Z;
            if(y == 0)
            {
                if (x > 0)
                {
                    direction = HexDirection.NW;
                }
                else
                {
                    direction = HexDirection.SE;
                }
            }
            else if (x == 0)
            {
                if (y > 0)
                {
                    direction = HexDirection.NE;
                }
                else
                {
                    direction = HexDirection.SW;
                }
            }
            else if (z == 0)
            {
                if (y > 0)
                {
                    direction = HexDirection.E;
                }
                else
                {
                    direction = HexDirection.W;
                }
            }
            return direction;
        }

        #endregion
    }
}

