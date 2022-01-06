using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SP.Grid
{
    public class AttackIndicator : INotifyCollectionChanged, INotifyPropertyChanged
    {
        private Character attacker;
        private HexCell attackerCell;
        private HexDirection direction;
        private HexCell targetCell;
        private Mover mover;
        //List<HexCell> attackArea = new List<HexCell>();
        private ObservableCollection<HexCell> attackArea = new ObservableCollection<HexCell>();

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Character Attacker { get => attacker; set => attacker = value; }

        public AttackIndicator(Character attacker, HexDirection direction)
        {
            this.attacker = attacker;
            this.direction = direction;
            //UpdateGrid();
        }

        public AttackIndicator(HexCell attackerCell, HexCell targetCell, HexDirection direction)
        {
            this.attacker = attackerCell.Unit;
            this.attackerCell = attackerCell;
            this.targetCell = targetCell;
            this.direction = direction;
            this.mover = attacker.GetComponent<Mover>();
            CollectionChanged += AttackIndicator_CollectionChanged;
            PropertyChanged += AttackIndicator_PropertyChanged;
            Update();
            Debug.Log("start attack indicator");
        }

        private void AttackIndicator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Log("property changed");
        }

        private void AttackIndicator_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.Log("Collection changed");
        }

        public void Update()
        {
            ClearArea();
            HexCell neighbor = mover.Location.GetNeighbor(direction);
            while (neighbor != null)
            {
                HighlightArea(neighbor);
                if (neighbor.Unit)
                {
                    break;
                }
                neighbor = neighbor.GetNeighbor(direction);
            }
        }

        void HighlightArea(HexCell cell)
        {
            
            //cell.SetIndicator(true);
            attackArea.Add(cell);
        }

        public void ClearArea()
        {
            foreach (HexCell cell in attackArea)
            {
                //cell.SetIndicator(false);
            }
            attackArea.Clear();
        }

        //internal void Attack()
        //{
        //    int damage = attacker.Stats.GetStat(Stat.Damage, attacker.CharacterClass);
        //    Debug.Log("enemy damage: " + damage);
        //    foreach (var area in attackArea)
        //    {
        //        if (area.Unit)
        //        {
        //            area.Unit.CalculateDamageTaken(damage);
        //            break;
        //        }
        //    }
        //    ClearArea();
        //    attackArea.Clear();
        //}
    }
}
