using FTS.Characters;
using FTS.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/TrajectoryAttack")]
    public class TrajectoryAttackDecision : InAttackRangeDecision
    {
        protected override void CalculatePosition(StateMachine machine)
        {
            possiblePositions.Clear();
            foreach (HexCell cell in machine.gridController.GetReachable(machine.enemy.Location, machine.mover.MovementLeft))
            {
                possiblePositions.Add(new Positions(cell));
            }

            foreach (Positions position in possiblePositions)
            {
                HexCell cell = position.Position;
                if (cell.IsDangerous)
                {
                    position.Score -= 5;
                }

                if (cell.IsEdge)
                {
                    position.Score -= 6;
                }

                int highestDirection = -1;
                int buffer = 0;
                Unit bufferTarget = null;
                for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
                {
                    buffer = 0;
                    bufferTarget = null;
                    HexCell neighbor = cell.GetNeighbor(d);
                    if(neighbor)
                    {
                        neighbor = neighbor.GetNeighbor(d);
                    }
                    if (neighbor)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (neighbor.IsFrendlyUnit(machine.enemy))
                            {
                                buffer += 5;
                                bufferTarget = neighbor.Unit;
                            }

                            neighbor = neighbor.GetNeighbor(d);
                            if (!neighbor)
                            {
                                break;
                            }
                        }
                    }

                    if (buffer > highestDirection)
                    {
                        highestDirection = buffer;
                        position.Direction = d;
                        position.Target = bufferTarget;
                    }
                }
                position.Score += highestDirection;
            }
        }
    }
}
