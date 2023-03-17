using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Turns;
using FTS.Grid;
using System.Xml;
using System.Linq;

namespace FTS.Characters
{
    public class InAttackRangeDecision : Decision
    {
        protected List<Positions> possiblePositions = new List<Positions>();

        public override bool Decide(StateMachine machine)
        {
            CalculatePosition(machine);
            Positions position = GetRandomBestPosition();
            machine.newEnemyPosition = position.Position;
            machine.enemy.Direction = position.Direction;
            machine.enemy.Target = position.Target;

            return CanReach(machine);
        }

        private Positions GetRandomBestPosition()
        {
            int bestScore = possiblePositions.Max(x => x.Score);
            possiblePositions.RemoveAll(x => x.Score < bestScore);
            return possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
        }

        //private HexCell GetClosestBestPosition()
        //{
        //    int bestScore = possiblePositions.Max(x => x.Score);
        //    possiblePositions.RemoveAll(x => x.Score < bestScore);
        //    foreach (Positions position in possiblePositions)
        //    {

        //    }
        //}

        protected virtual void CalculatePosition(StateMachine machine)
        {
            
        }

        private bool CanReach(StateMachine machine)
        {
            bool canReach = machine.gridController.CanReachAttackRange(machine.enemy, machine.newEnemyPosition);
            return canReach;
        }
    }

    public class Positions
    {
        public int Score { get; set; }
        public HexCell Position { get; set; }
        public Unit Target { get; set; }
        public HexDirection Direction { get; set; }

        public Positions() { }

        public Positions(HexCell position)
        {
            Position = position;
        }

        public Positions(int score, HexCell position, Unit target, HexDirection direction)
        {
            Score = score;
            Position = position;
            Target = target;
            Direction = direction;
        }
    }
}
