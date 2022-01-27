using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Core
{
    public class ObjectiveGenerator : MonoBehaviour
    {
        [SerializeField] MapObjective mapObjective;
        [SerializeField] int numObjectives;
        [SerializeField] Planet planet;
        [SerializeField] GameObject startingLocationLbl;
        [SerializeField] List<MapObjective> mapObjectives = new List<MapObjective>();
        int currentIndex = 0;

        public int CurrentIndex
        {
            get
            {
                if (currentIndex > mapObjectives.Count - 1)
                {
                    currentIndex = 0;
                }
                if (currentIndex < 0)
                {
                    currentIndex = mapObjectives.Count - 1;
                }
                return currentIndex;
            }
            set { currentIndex = value; }
        }
        public MapObjective NextObjective
        {
            get { currentIndex++; return mapObjectives[CurrentIndex]; }
        }

        public MapObjective PreviousObjective
        {
            get { currentIndex--; return mapObjectives[CurrentIndex]; }
        }
        public MapObjective CurrentObjective
        {
            get { return mapObjectives[CurrentIndex]; }
        }
    }
}
