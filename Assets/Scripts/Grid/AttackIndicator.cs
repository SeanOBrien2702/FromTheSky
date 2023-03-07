using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace FTS.Grid
{
    public class AttackIndicator
    {
        private Character attacker;
        private HexDirection direction;
        private List<HexCell> line;

        public Character Attacker { get => attacker; set => attacker = value; }
        public HexDirection Direction { get => direction; set => direction = value; }
        public List<HexCell> Line { get => line; set => line = value; }

        public AttackIndicator(List<HexCell> line, HexDirection direction)
        {
            this.Line = line;
            this.direction = direction;
        }

        public AttackIndicator(HexCell position, HexDirection direction)
        {
            this.Line = new List<HexCell>() { position };
            this.direction = direction;
        }
    }
}