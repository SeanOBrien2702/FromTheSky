using FTS.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/ProjectileAttack")]
    public class ProjectileAttackDecision : InAttackRangeDecision
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

                    while (neighbor && neighbor.IsCellAvailable())
                    {
                        buffer++;   
                        neighbor = neighbor.GetNeighbor(d);
                    }

                    if (neighbor)
                    {
                        if (neighbor.IsFrendlyUnit(machine.enemy))
                        {
                            buffer += 3;
                            if(neighbor.Unit == machine.enemy.Target)
                            {
                                buffer -= 2;
                            }

                            buffer += neighbor.Unit.MaxHealth - neighbor.Unit.Health;
                            if (neighbor.Unit is Player)
                            {
                                while (neighbor)
                                {
                                    neighbor = neighbor.GetNeighbor(d);
                                    if (neighbor && neighbor.IsFrendlyUnit(machine.enemy))
                                    {
                                        buffer += 4;
                                    }
                                }
                            }
                        }
                        else
                        {
                            buffer = 0;
                        }

                        if (buffer > highestDirection)
                        {
                            highestDirection = buffer;
                            position.Direction = d;
                            position.Target = bufferTarget;
                        }
                    }
                }
                position.Score += highestDirection;
            }
        }
    }
}
