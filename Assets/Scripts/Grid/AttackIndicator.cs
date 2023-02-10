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
        public List<HexCell> Line;

        public Character Attacker { get => attacker; set => attacker = value; }

        public AttackIndicator(Character attacker, HexDirection direction)
        {
            this.attacker = attacker;
            this.direction = direction;
        }

        public AttackIndicator(List<HexCell> line, HexDirection direction)
        {
            this.Line = line;
            this.direction = direction;
        }
    }
}