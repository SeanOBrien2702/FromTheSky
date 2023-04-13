using FTS.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Grid
{
    public class GridBackground : MonoBehaviour
    {
        [SerializeField] HexGridController grid;

        private void OnMouseEnter()
        {
            grid.OutOfBounds();
        }
    }
}
