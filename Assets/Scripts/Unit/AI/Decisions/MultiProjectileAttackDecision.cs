using FTS.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/MultiProjectileAttack")]
    public class MultiProjectileAttackDecision : InAttackRangeDecision
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

                    foreach (AttackDirections direction in machine.enemy.AttackDirections)
                    {                    
                        HexDirection localDirection = HexDirectionExtensions.LocalDirection(d, direction);
                        int dirValue = CalculateDirectionValue(machine, cell, localDirection);
                        if(dirValue > 0 && direction == AttackDirections.Forward)
                        {
                            dirValue++;
                        }
                        buffer += dirValue;
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

        int CalculateDirectionValue(StateMachine machine, HexCell cell, HexDirection d)
        {
            int value = 0;
            HexCell forward = cell.GetNeighbor(d);

            while (forward && forward.IsCellAvailable())
            {
                //buffer++;   
                forward = forward.GetNeighbor(d);
            }

            if (forward)
            {
                if (forward.IsFrendlyUnit(machine.enemy))
                {
                    value += 3;
                    if (forward.Unit == machine.enemy.Target)
                    {
                        value -= 2;
                    }

                    //value += forward.Unit.MaxHealth - forward.Unit.Health;
                    if (forward.Unit is Player)
                    {
                        while (forward)
                        {
                            forward = forward.GetNeighbor(d);
                            if (forward && forward.IsFrendlyUnit(machine.enemy))
                            {
                                value += 4;
                            }
                        }
                    }
                }
                else
                {
                    value = 0;
                }

            }
            return value;
        }
    }
}
