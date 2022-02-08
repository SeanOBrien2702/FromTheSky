#region Using Statements
using FTS.Characters;
using FTS.Cards;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FTS.Turns;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace FTS.Grid
{
    public class HexGridController : MonoBehaviour
    {
        [SerializeField] UI.CharacterInfo characterInfo;
        [SerializeField] CameraController cameraController;

        [SerializeField] Vector2Int placementArea;
        [SerializeField] Button startButton;

        TurnController turnController;

        HexGrid grid;
        UnitController unitController;
        CardController cardController;

        HexCell currentCell;

        Character selectedUnit;
        Mover mover;
        bool unitsPlaced = false;

        //List<AttackIndicator> attackIndicators = new List<AttackIndicator>();

        int maxNumUnits = 3;


        #region Properties
        public bool UnitsPlaced   // property
        {
            get { return unitsPlaced; }   // get method
            set { unitsPlaced = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            grid = GetComponent<HexGrid>();
            unitController = GetComponent<UnitController>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            UnitController.OnPlayerSelected += UnitController_OnPlayerSelected;
            TurnController.OnNewTurn += TurnController_OnNewTurn;
            TurnController.OnEndTurn += TurnController_OnEndTurn;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            TurnController.OnCombatStart += TurnController_OnCombatStart;
            grid.ShowPlacementArea(placementArea);
        }

        void Update()
        {
            if (unitsPlaced)
            {
                DoUnitControl();
            }
            else
            {
                DoPlaceUnits();
            }
        }

        private void OnDestroy()
        {
            UnitController.OnPlayerSelected -= UnitController_OnPlayerSelected;
            TurnController.OnNewTurn -= TurnController_OnNewTurn;
            TurnController.OnEndTurn -= TurnController_OnEndTurn; 
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            TurnController.OnCombatStart -= TurnController_OnCombatStart;
        }
        #endregion

        #region Private Methods
        //TODO: make unit placement its own class
        private void DoPlaceUnits()
        {
            if (Input.GetMouseButtonDown(0) && MouseOverGrid())
            {
                PlaceUnit();                  
            }
            else if (Input.GetMouseButtonDown(1) && MouseOverGrid())
            {
                RemoveUnit();
            }
        }

        private void RemoveUnit()
        {
            UpdateCurrentCell();
            if (currentCell && currentCell.Unit is Player 
                && currentCell.Unit.CharacterClass != CharacterClass.Vehicle)
            {
                currentCell.Unit.Die();
                currentCell.Unit = null;
                Debug.Log(unitController.NumberOfUnits);
                if (unitController.NumberOfPlayers <= 2)
                {
                    startButton.interactable = false;
                }
                characterInfo.DisableUI();
            }
        }

        private void PlaceUnit()
        {
            UpdateCurrentCell();
            if (currentCell && !currentCell.IsObstacle && !currentCell.Unit)
            {
                unitController.PlacePlayer(currentCell);
                if (unitController.NumberOfPlayers >= 3)
                {
                    startButton.interactable = true;
                }
            }
        }

        private void DoUnitControl()
        {
            if (CanControlUnit())
            {
                if (Input.GetMouseButtonDown(0) && MouseOverGrid())
                {
                    DoSelection();
                }
                if (mover)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        Debug.Log("Move");
                        DoMove();
                    }
                    else
                    {
                        if (cardController.CardSelected)
                        {

                            DoCardArea();
                        }
                        else
                        {
                            if (mover.CanMove)
                            {
                                DoPathfinding();
                            }
                        }
                    }
                }
            }
        }

        private bool CanControlUnit()
        {
            return EventSystem.current.IsPointerOverGameObject()
                && (turnController.TurnPhase == TurnPhases.PlayerTurn
                || turnController.TurnPhase == TurnPhases.VehicleAction);
        }

        private bool MouseOverGrid()
        {
            bool overGrid = true;
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (var item in raycastResults)
            {
                if (item.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    overGrid = false;
                }
            }
            return overGrid;
        }


        void DoSelection()
        {
            grid.ClearReachable();
            UpdateCurrentCell();
            if (currentCell.Unit && selectedUnit != currentCell.Unit)
            {
                //Debug.Log("Select");
                if (currentCell && currentCell.Unit)
                {
                    unitController.SetCurrentUnit(currentCell.Unit as Player);
                }
            }
            else
            {
                unitController.SetCurrentUnit(null);
                DeselectUnit();
            }
        }

        void DeselectUnit()
        {
            grid.ClearReachable();
            grid.ClearPath();
            selectedUnit = null;
            mover = null;
        }

        void DoPathfinding()
        {
            if (UpdateCurrentCell() && turnController.TurnPhase == TurnPhases.PlayerTurn)
            {
                if (currentCell && mover.IsValidDestination(currentCell))
                {
                    grid.FindPath(mover.Location, currentCell, mover.MovementLeft, true);
                }
                else
                {
                    grid.ClearPath(mover.MovementLeft);
                }
            }
        }

        private void DoCardArea()
        {
            if (UpdateCurrentCell())
            {
                Debug.Log("show area");
                grid.ClearReachable();
                //grid.ShowArea(mover.Location, unitController.CurrentPlayer.GetCardRange(cardController.CardSelected.Type));
                Player player = unitController.GetPlayerByClass(cardController.CardSelected.CharacterClass);
                grid.ShowArea(player.GetComponent<Mover>().Location, player.GetCardRange(cardController.CardSelected.Type));
            }
        }

        void DoMove()
        {
            if (grid.HasPath)
            {
                grid.ClearReachable();
                mover.Travel(grid.GetPath(mover.MovementLeft));
                grid.ClearPath(mover.MovementLeft);
                grid.ShowReachableHexes(mover.Location, mover.MovementLeft);
                characterInfo.UpdateUI(unitController.CurrentPlayer);
            }
        }

        bool UpdateCurrentCell()
        {
            HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != currentCell)
            {
                currentCell = cell;
                return true;
            }
            return false;
        }

        internal void Push(Character character, HexDirection direction)
        {
            Mover mover = character.GetComponent<Mover>();
            Debug.Log("Target to be moved " +mover.Location.Unit);
            HexCell neighbor = mover.Location.GetNeighbor(direction);
            if (neighbor != null)
            {
                if (!neighbor.IsObstacle && !neighbor.Unit)
                {
                    mover.Push(direction);
                }
                else
                {
                    character.Health -= 2;
                    if(neighbor.Unit)
                    {
                        neighbor.Unit.Health -= 2;
                    }
                }
            }
        }

        #endregion

        #region Public Methods
        public void SelectNextUnit()
        {
            DeselectUnit();
            selectedUnit = unitController.GetCurrentPlayer();
            mover = selectedUnit.GetComponent<Mover>();
            selectedUnit.StartRound();
            StartCoroutine(cameraController.MoveToPosition(selectedUnit.transform.localPosition));
            grid.ShowReachableHexes(mover.Location, mover.MovementLeft);
        }

        internal HexCell GetCardTarget()
        {
            UpdateCurrentCell();
            grid.ClearArea();
            grid.ShowReachableHexes(mover.Location, mover.MovementLeft);    
            return currentCell;
        }

        internal bool IsTargetValid(HexCell target, CardTargeting targeting)
        {
            bool isValidTarget = false;
            if (target != null)
            {
                if (targeting == CardTargeting.Unit)
                {
                    if (currentCell.Unit)
                    {
                        isValidTarget = true;
                    }
                }
                else if (targeting == CardTargeting.GroundOnly)
                {
                    if (!currentCell.Unit)
                    {
                        isValidTarget = true;
                    }
                }
                else if (targeting == CardTargeting.Ground || targeting == CardTargeting.FromPlayer)
                {

                    isValidTarget = true;
                    
                }
            }
            return isValidTarget;
        }

        internal int GetDistance(HexCell target)
        {
            return (Mathf.Abs(target.Location.X - mover.Location.Location.X)
                    + Mathf.Abs(target.Location.Y - mover.Location.Location.Y)
                    + Mathf.Abs(target.Location.Z - mover.Location.Location.Z)) / 2;
        }

        internal void TravelToTarget(Mover AIMover, int AIRange, HexCell target, Vector3 lookAt)
        {
            mover = AIMover;
            grid.FindPath(AIMover.Location, target, AIMover.MovementLeft, false);          
            mover.Travel(grid.GetPath(mover.MovementLeft), lookAt);
        }

        internal void Flee(Mover AIMover)
        {
            mover = AIMover;
            grid.FindPathAway(mover.Location, GetClosesPlayerDrection(mover), mover.MovementLeft);
            //mover.Travel(grid.GetPath(mover.MovementLeft), unitController.GetVehiclePosition().transform.position);
        }

        private HexDirection GetClosesPlayerDrection(Mover mover)
        {
            Mover closetPlayer = null;
            int closesDistance = 1000;
            int distanceBuffer = 0;
            foreach (var item in unitController.GetPlayerUnits())
            {
                Mover buffer = item.GetComponent<Mover>();
                distanceBuffer = mover.Location.Location.DistanceTo(buffer.Location.Location);
                if (closesDistance > distanceBuffer)
                {
                    closetPlayer = buffer;
                    closesDistance = distanceBuffer;
                }
            }

            return grid.GetDirection(closetPlayer.Location, mover.Location);
        }

 
        internal void TargetPush(Character target)
        {
            HexCell attacker = unitController.GetCurrentPlayer().GetComponent<Mover>().Location;
            Mover mover = target.GetComponent<Mover>();
            HexDirection direction = grid.GetDirection(attacker, mover.Location);
            Push(target, direction);
        }


        internal void AreaPush(HexCell target)
        {
            for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
            {
                HexCell neighbor = target.GetNeighbor(direction);
                if (neighbor.Unit)
                {
                    Push(neighbor.Unit, direction);
                }
            }
        }

        internal bool PlayerInRange(Enemy enemy)
        {
            bool playerInRange = false;
            foreach (var item in unitController.GetPlayerUnits())
            {
                //TODO: clean this up
                if(enemy.GetComponent<Mover>().Location.Location.DistanceTo(item.GetComponent<Mover>().Location.Location) <= enemy.Range - 1)
                {
                    playerInRange = true;
                    break;
                }
            }
            return playerInRange;
        }


        internal bool CanReachAttackRange(Enemy enemy, HexCell targetCell)
        {
            bool canReach = false;
            mover = enemy.GetComponent<Mover>();
            int distanceToEndpoint = targetCell.Location.DistanceTo(mover.Location.Location);
            int distanceToTarget = targetCell.Location.DistanceTo(enemy.Target.Location);
            if (distanceToTarget < enemy.Range && distanceToEndpoint <= mover.MovementLeft)
                canReach = true;

            return canReach;
        }

        internal HexCell GetNewEnemyPosition(Mover AIMover, EnemyTargeting targeting, Enemy enemy) //int range)
        {
            HexCell target = null;
            int shortestDistance = 1000;
            List<Player> players = unitController.GetPlayerUnits();
            foreach (var player in players)
            {              
                Mover bufferMover = player.GetComponent<Mover>();
                HexCell buffer = grid.FindPath(bufferMover.Location, AIMover.Location, AIMover.MovementLeft, enemy.Range - 1);
 
                int distance = grid.GetPathDistance();
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    target = buffer;
                    enemy.Target = bufferMover.Location;
                }
            }
            return target;
        }


        internal bool IsAttackNotBlocked(Enemy enemy)
        {
            bool isAttackNotBlocked = true;


            return isAttackNotBlocked;
        }
        #endregion

        #region Events
        private void TurnController_OnCombatStart()
        {
            grid.ClearArea();
            unitsPlaced = true;
        }

        private void UnitController_OnPlayerSelected()
        {
            SelectNextUnit();
        }

        private void TurnController_OnNewTurn()
        {
            SelectNextUnit();
        }


        private void TurnController_OnEnemyTurn()
        {
            grid.ClearArea();
            grid.ClearPath();
            grid.ClearReachable();
        }

        private void TurnController_OnEndTurn()
        {
            grid.ClearArea();
            grid.ClearPath();
            grid.ClearReachable();
        }
        #endregion

        #region Coroutines

        #endregion
    }
}