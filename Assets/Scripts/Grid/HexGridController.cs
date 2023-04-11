#region Using Statements
using FTS.Cards;
using FTS.Characters;
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
        Dictionary<Enemy, AttackIndicator> attackIndicators = new Dictionary<Enemy, AttackIndicator>();

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
            UnitController.OnPlayerSelected += UnitController_OnPlayerSelected;
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            TurnController.OnCombatStart += TurnController_OnCombatStart;
            Unit.OnHover += Unit_OnHover;
            Unit.OnHoverExit += Unit_OnHoverExit;
            grid.ShowPlacementArea(placementArea);
            currentUnit = unitController.PlacePlayer(grid.GetCell(new HexCoordinates(grid.Width / 2, 0)));
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
            UnitController.OnPlayerSelected += UnitController_OnPlayerSelected;
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            TurnController.OnCombatStart -= TurnController_OnCombatStart;
            Unit.OnHover += Unit_OnHover;
            Unit.OnHoverExit += Unit_OnHoverExit;
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
            //else if (Input.GetMouseButtonDown(1) && MouseOverGrid())
            //{
            //    RemoveUnit();
            //}
        }

        //private void RemoveUnit()
        //{
        //    UpdateCurrentCell();
        //    if (currentCell && currentCell.Unit is Player)
        //    {
        //        currentCell.Unit.Die();
        //        currentCell.Unit = null;
        //        Debug.Log(unitController.NumberOfUnits);
        //        if (unitController.NumberOfPlayers < maxNumPLayers)
        //        {
        //            startButton.interactable = false;
        //        }
        //        characterInfo.DisableUI();
        //    }
        //}

        //private void PlaceUnit()
        //{
        //    UpdateCurrentCell();
        //    if (currentCell && !currentCell.IsObstacle && !currentCell.Unit)
        //    {
        //        unitController.PlacePlayer(currentCell);
        //        if (unitController.NumberOfPlayers >= maxNumPLayers)
        //        {
        //            startButton.interactable = true;
        //        }
        //    }
        //}

        private void PlaceUnit()
        {
            UpdateCurrentCell();
            if (currentCell && !currentCell.IsObstacle && !currentCell.Unit && grid.InCurrentArea(currentCell))
            {
                currentUnit.GetComponent<Mover>().Location = currentCell;
                //unitController.PlacePlayer(currentCell);
                //if (unitController.NumberOfPlayers >= maxNumPLayers)
                //{
                //    startButton.interactable = true;
                //}
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
                    if (Input.GetMouseButtonDown(0))
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
                                DoPathfinding();
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                DeselectUnit();
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
            //grid.ClearReachable();
            UpdateCurrentCell();
            if (currentCell.Unit)
            {
                characterInfo.EnableUI(currentCell.Unit);
            }
            else
            {
                characterInfo.EnableUI(unitController.GetCurrentUnit());

            }
            if (currentCell.Unit && currentUnit != currentCell.Unit)
            {
                Debug.Log("Select");
                if (currentCell && currentCell.Unit is Player)
                {
                    unitController.SetCurrentUnit(currentCell.Unit as Player);
                }
            }
            //else
            //{
            //    unitController.SetCurrentUnit(null);
            //    DeselectUnit();
            //}
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
                                Debug.Log(cell.Unit.name + " " + cardController.GetDamage());
                                cell.Unit.ShowDamage(cardController.GetDamage());
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
                grid.ClearReachable();
                Travel(mover);                
                grid.ClearPath(mover.MovementLeft);
                grid.ShowReachableHexes(mover.Location, mover.MovementLeft);
                //characterInfo.UpdateUI(unitController.CurrentPlayer);
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

        internal void Push(Character character, HexDirection direction)
        {
            Mover mover = character.GetComponent<Mover>();
            Debug.Log("Target to be moved " + mover.Location.Unit);
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
                    if (neighbor.Unit)
                    {
                        neighbor.Unit.Health -= 2;
                    }
                }
            }
        }

        public void UpdateIndicators(Enemy enemy)
        {
            UpdateLine(enemy, attackIndicators[enemy]);
        }

        public void UpdateIndicators(HexCell oldLocation, HexCell newLocation)
        {
            foreach (var indicator in attackIndicators)
            {
                if (indicator.Value.Line.Contains(oldLocation) ||
                    indicator.Value.Line.Contains(newLocation))
                {
                    UpdateLine(indicator.Key, indicator.Value);
                }          
            }
        }

        private void UpdateLine(Enemy enemy, AttackIndicator indicator)
        {
            foreach (HexCell cell in indicator.Line)
            {
                cell.SetDangerIndicator(false);
                cell.SetDangerous(false);
            }

            if (enemy.IsPiercieing())
            {
                indicator.Line = grid.GetLine(enemy.Location, indicator.Direction, enemy.Range, enemy.IsPiercieing());
                foreach (HexCell cell in indicator.Line)
                {
                    cell.SetDangerIndicator(true);
                }
            }
            else
            {
                indicator.Line = grid.GetLine(enemy.Location, indicator.Direction, projectileRange, enemy.IsPiercieing());
                indicator.Line.Last().SetDangerIndicator(true);
            }
            foreach (HexCell cell in indicator.Line)
            {
                cell.SetDangerous(true);
            }
        }

        void Travel(Mover mover)
        {
            UpdateIndicators(mover.Location, mover.Travel(grid.GetPath(mover.MovementLeft)));
        }
        #endregion

        #region Public Methods
        public void SelectNextUnit()
        {
            DeselectUnit();
            currentUnit = unitController.GetCurrentPlayer();
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

        internal void TargetPush(Character target)
        {
            HexCell attacker = unitController.GetCurrentPlayer().GetComponent<Mover>().Location;
            Mover mover = target.GetComponent<Mover>();
            HexDirection direction = grid.GetDirection(attacker, mover.Location);
            Push(target, direction);
            if(target is Enemy)
                UpdateIndicators((Enemy)target);
        }


        internal void AreaPush(HexCell target)
        {
            for (HexDirection direction = HexDirection.NE; direction <= HexDirection.NW; direction++)
            {
                HexCell neighbor = target.GetNeighbor(direction);
                if (neighbor.Unit is Character)
                {
                    Push((Character)neighbor.Unit, direction);
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

        public void TelegraphTrajectoryAttack(Enemy enemy)
        {
            AttackIndicator indicator = new AttackIndicator(enemy.Target.Location, enemy.Direction);

            enemy.Target.Location.SetDangerIndicator(true);


            attackIndicators.Add(enemy, indicator);
        }

        public void TelegraphAttack(Enemy enemy)
        {
            AttackIndicator indicator;
            if (enemy.IsPiercieing())
            {
                indicator = new AttackIndicator(grid.GetLine(enemy.Location, enemy.Direction, enemy.Range, false), enemy.Direction);
                foreach (HexCell cell in indicator.Line)
                {
                    cell.SetDangerIndicator(true);
                }
            }
            else
            {
                indicator = new AttackIndicator(grid.GetLine(enemy.Location, enemy.Direction, projectileRange, true), enemy.Direction);
                indicator.Line.Last().SetDangerIndicator(true);
            }

            foreach (HexCell cell in indicator.Line)
            {
                cell.SetDangerous(true);
            }
            attackIndicators.Add(enemy, indicator);
        }

        internal void RemoveIndicator(Enemy unit)
        {
            foreach (HexCell cell in attackIndicators[unit].Line.ToList())
            {
                cell.SetDangerIndicator(false);
            }
            attackIndicators.Remove(unit);
        }

        public void Attack(Enemy enemy)
        {
            if(!attackIndicators.ContainsKey(enemy))
            {
                return;
            }
            foreach (HexCell cell in attackIndicators[enemy].Line.ToList())
            {
                cell.SetDangerIndicator(false);
                if(cell.Unit)
                {
                    int damage = enemy.Stats.GetStat(Stat.Damage, enemy.CharacterClass);
                    cell.Unit.CalculateDamageTaken(damage);
                }
            } 
            attackIndicators.Remove(enemy);
        }

        public void UpdateReachable()
        {
            grid.ClearReachable();
            grid.ShowReachableHexes(mover.Location, mover.MovementLeft);
        }
        #endregion

        #region Events
        private void UnitController_OnPlayerSelected(Player player)
        {
            currentUnit = unitController.CurrentPlayer;
            SelectNextUnit();
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

        private void Unit_OnHoverExit(Unit unit)
        {
            if(unit is Enemy)
            {
                Enemy enemy = (Enemy)unit;
                if (cardController.CardSelected)
                {
                    enemy.ShowDamage(0);
                }
                else if (attackIndicators.ContainsKey(enemy))
                {
                    foreach (HexCell cell in attackIndicators[enemy].Line.ToList())
                    {
                        if (cell.Unit)
                        {
                            cell.Unit.ShowDamage(0);
                        }
                    }
                }
            }                         
        }

        private void Unit_OnHover(Unit unit)
        {
            if (unit is Enemy)
            {
                Enemy enemy = (Enemy)unit;
                if(cardController.CardSelected &&
                   cardController.CardSelected.Targeting == CardTargeting.Unit)
                {
                    enemy.ShowDamage(cardController.GetDamage());
                }
                else if (attackIndicators.ContainsKey(enemy))
                {
                    foreach (HexCell cell in attackIndicators[enemy].Line.ToList())
                    {
                        if (cell.Unit)
                        {
                            int damage = enemy.Stats.GetStat(Stat.Damage, enemy.CharacterClass);
                            cell.Unit.ShowDamage(damage);
                        }
                    }
                }
            }       
        }
        #endregion
    }
}