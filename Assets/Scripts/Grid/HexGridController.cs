#region Using Statements
using FTS.Cards;
using FTS.Characters;
using FTS.Core;
using FTS.Turns;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
#endregion

namespace FTS.Grid
{
    public class HexGridController : MonoBehaviour
    {
        [SerializeField] UI.CharacterInfo characterInfo;
        [SerializeField] CameraController cameraController;

        [SerializeField] Vector2Int placementArea;
        [SerializeField] Button startButton;
        [SerializeField] int collisionDamage = 1;

        TurnController turnController;

        HexGrid grid;
        UnitController unitController;
        CardController cardController;

        HexCell currentCell;

        Unit currentUnit;
        //Character selectedUnit;
        Mover mover;
        bool unitsPlaced = false;
        List<HexCell> targetArea = new List<HexCell>();

        int projectileRange = 9999;

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
            UnitController.OnSelectPlayer += UnitController_OnSelectPlayer;
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            TurnController.OnCombatStart += TurnController_OnCombatStart;
            Mover.OnMoved += Mover_OnMoved;
            
            grid.ShowPlacementArea(placementArea);
            
            if(!TutorialController.Instance.IsTutorialComplete)
            {
                unitsPlaced = true;
                currentUnit = unitController.PlacePlayer(grid.GetCell(new HexCoordinates(2, 2)));
            }
            else
            {
                currentUnit = unitController.PlacePlayer(grid.GetCell(new HexCoordinates(grid.Width / 2, 0)));
            }
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
            UnitController.OnSelectPlayer -= UnitController_OnSelectPlayer;
            UnitController.OnSelectUnit -= UnitController_OnSelectUnit;
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            TurnController.OnCombatStart -= TurnController_OnCombatStart; 
            Mover.OnMoved -= Mover_OnMoved;
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
        }

        private void PlaceUnit()
        {
            UpdateCurrentCell();
            if (currentCell && !currentCell.IsObstacle && !currentCell.Unit && grid.InCurrentArea(currentCell))
            {
                currentUnit.GetComponent<Mover>().Location = currentCell;
            }
        }

        private void DoUnitControl()
        {
            if (CanControlUnit())
            {
                if (Input.GetMouseButtonDown(0) && MouseOverGrid())
                {
                    DoSelection();
                    Debug.Log(mover);
                }
                if (mover)
                {
                    if (Input.GetMouseButtonDown(0) && MouseOverGrid())
                    {
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
                                if (MouseOverGrid())
                                {
                                    DoPathfinding();
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                unitController.SetCurrentUnit(null);
            }
        }

        private bool CanControlUnit()
        {
            return EventSystem.current.IsPointerOverGameObject()
                && turnController.TurnPhase == TurnPhases.PlayerTurn;//unitController.IsPlayer();
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
            UpdateCurrentCell();
            if (currentCell.Unit && currentUnit != currentCell.Unit)
            {
                unitController.SetCurrentUnit(currentCell.Unit);
            }
        }

        void DeselectUnit()
        {
            grid.ClearReachable();
            grid.ClearPath();
            currentUnit = null;
            mover = null;
        }

        void DoPathfinding()
        {
            if (UpdateCurrentCell())
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
                grid.ClearReachable();
                grid.ClearArea();
                ClearTargetArea();
                
                Card card = cardController.CardSelected;
                if (cardController.IsFreeAim())
                {
                    grid.ShowArea(currentUnit.Location, card.Range, HighlightIndex.CardRange);

                    if (GetDistance(currentCell) <= card.Range)
                    {
                        currentCell.SetHighlight(HighlightIndex.Attack);
                    }
                    if (card.Type == CardType.Attack)
                    {
                        grid.ShowAvalibleTargets(currentUnit.Location, cardController.CardSelected.Range);
                    }               
                }
                else
                {
                    if (card.Targeting == CardTargeting.Projectile)
                    {
                        grid.ShowLines(currentUnit.Location, projectileRange, true);
                    }
                    else
                    {
                        grid.ShowLines(currentUnit.Location, card.Range, false);
                    }
                    HexDirection direction = grid.GetDirection(currentUnit.Location, currentCell);
                    if (direction != HexDirection.None)
                    {
                        mover.LookAt(direction);
                        if (card.Targeting == CardTargeting.Projectile)
                        {
                            targetArea = grid.ShowLine(currentUnit.Location, direction, projectileRange, true);
                        }
                        else
                        {
                            targetArea = grid.ShowLine(currentUnit.Location, direction, card.Range, false);
                        }
                        foreach (var cell in targetArea)
                        {
                            if (cell.Unit)
                            {
                                cell.Unit.ShowDamage(cardController.GetDamage(cell));
                            }
                        }
                    }
                }
                              
                //if (cardController.CardSelected.Area > 0 &&
                //    GetDistance(currentCell) <= cardController.CardSelected.Range)
                //{ 
                //    grid.ShowArea(currentCell, cardController.CardSelected.Area, HighlightIndex.CanReach);
                //}
            }
        }

        void DoMove()
        {
            if (grid.HasPath)
            {
                Travel(mover);
                grid.ClearPath(mover.MovementLeft);
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

        void ClearTargetArea()
        {
            foreach (var cell in targetArea)
            {
                if(cell.Unit)
                {
                    cell.Unit.ShowDamage(0);
                }
            }
        }

        internal void Push(Character character, HexDirection direction, int distance)
        {
            if(distance == 0)
            {
                distance = projectileRange;
            }

            HexCell buffer = character.Location;
            HexCell destination = buffer;
            for (int i = 0; i < distance; i++)
            {
                buffer = buffer.GetNeighbor(direction);
                if (buffer)
                {                  
                    if (buffer.IsCellAvailable())
                    {
                        destination = buffer;
                    }
                    else
                    {
                        if (distance < projectileRange)
                        {
                            character.Health -= collisionDamage;
                            if (buffer.Unit)
                            {
                                buffer.Unit.Health -= collisionDamage;
                            }
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            character.Mover.Push(destination);
        }

        void Travel(Mover mover)
        {
            mover.Travel(grid.GetPath(mover.MovementLeft));
        }
        #endregion

        #region Public Methods
        public void SelectNextUnit()
        {
            DeselectUnit();
            currentUnit = unitController.CurrentUnit;
            mover = currentUnit.GetComponent<Mover>();
            StartCoroutine(cameraController.MoveToPosition(currentUnit.transform.localPosition));
            grid.ClearReachable();
            if (currentUnit is Player)
            {
                grid.ShowReachableHexes(mover.Location, mover.MovementLeft);
            }
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
                else if (targeting == CardTargeting.Ground)
                {
                    if (currentCell.IsCellAvailable())
                    {
                        isValidTarget = true;
                    }
                }
                else
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

        internal void TravelToTarget(Mover AIMover, int AIRange, HexCell target)
        {
            mover = AIMover;
            if (grid.FindPath(AIMover.Location, target, AIMover.MovementLeft, false))
            {
                Travel(AIMover);
            }
            else
            {
                Debug.Log("path not found");
            }
        }

        internal void Flee(Mover AIMover)
        {
            mover = AIMover;
            grid.FindPathAway(mover.Location, GetClosesPlayerDrection(mover), mover.MovementLeft);
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

        internal HexDirection GetValidDirection(HexCell location)
        {
            HexDirection direction = HexDirection.None;
            foreach (var item in unitController.GetTargetableUnits())
            {
                direction = grid.GetDirection(location, item.Location);
                if(direction != HexDirection.None)
                {
                    break;
                }
                 
            }
            return direction;
        }

        internal Unit GetClosestPlayer(Mover mover)
        {
            Unit closetUnit = null;
            int closesDistance = 1000;
            int distanceBuffer = 0;
            foreach (var item in unitController.GetTargetableUnits())
            {
                Unit buffer = item;
                distanceBuffer = mover.Location.Location.DistanceTo(item.Location.Location);
                if (closesDistance > distanceBuffer)
                {
                    closetUnit = buffer;
                    closesDistance = distanceBuffer;
                }
            }

            return closetUnit;
        }

        internal void TargetPush(Character target, int distance, bool isPull)
        {
            HexCell attacker = unitController.CurrentUnit.GetComponent<Mover>().Location;
            HexDirection direction = grid.GetDirection(attacker, target.Mover.Location);
            if(isPull)
            {
                direction = HexDirectionExtensions.Opposite(direction);
            }
            Push(target, direction, distance);            
        }

        internal void AreaPush(HexCell target, int distance)
        {
            for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
            {
                HexCell neighbor = target.GetNeighbor(direction);
                if (neighbor.Unit is Character)
                {
                    Push((Character)neighbor.Unit, direction, distance);
                }
            }
        }

        internal bool PlayerInRange(Enemy enemy)
        {
            bool playerInRange = false;
            foreach (Unit item in unitController.GetTargetableUnits())
            {
                if (enemy.Location.Location.DistanceTo(item.Location.Location) <= enemy.Range - 1)
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
            if (distanceToEndpoint <= mover.MovementLeft)
                canReach = true;

            return canReach;
        }

        internal HexCell GetNewEnemyPosition(Enemy enemy, Unit enemyTarget) //int range)
        {
            HexCell target = null;
            int shortestDistance = 1000;
            HexCell buffer = grid.FindPath(enemyTarget.Location, enemy.Location, enemy.GetComponent<Mover>().MovementLeft, enemy.Range - 1);

            int distance = grid.GetPathDistance();
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                target = buffer;                    
            }
            return target;
        }

        public List<HexCell> GetReachable(HexCell location, int movementLeft)
        {
            return grid.GetReachable(location, movementLeft).Distinct().ToList();
        }

        internal bool IsAttackNotBlocked(Enemy enemy)
        {
            bool isAttackNotBlocked = true;


            return isAttackNotBlocked;
        }

        public void UpdateReachable()
        {
            grid.ClearReachable();
            grid.ShowReachableHexes(mover.Location, mover.MovementLeft);
        }

        public void OutOfBounds()
        {
            if (mover != null)
            {
                grid.ClearPath(mover.MovementLeft);
            }
            currentCell = null;
        }
        #endregion

        #region Events

        private void UnitController_OnSelectPlayer(Player player)
        {
            currentUnit = player;
            SelectNextUnit();
        }

        private void UnitController_OnSelectUnit(Unit unit)
        {
            DeselectUnit();
        }

        private void TurnController_OnCombatStart()
        {
            grid.ClearArea();
            unitsPlaced = true;
        }

        private void TurnController_OnNewTurn()
        { 
            SelectNextUnit();
        }

        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            grid.ClearArea();
            grid.ClearPath();
            grid.ClearReachable();
        }

        private void Mover_OnMoved(HexCell cell, HexCell newCell)
        {
            if (turnController.TurnPhase == TurnPhases.PlayerTurn)
            {
                UpdateReachable();
            }
        }
        #endregion
    }
}