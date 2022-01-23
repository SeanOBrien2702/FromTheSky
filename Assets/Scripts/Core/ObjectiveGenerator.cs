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
        List<MapObjective> mapObjectives = new List<MapObjective>();
        // Start is called before the first frame update
        void Start()
        {
            MapObjective buffer = mapObjective;
            for (int i = 0; i < numObjectives; i++)
            {
                MapObjective mo = Instantiate(mapObjective);
                mo.transform.position = planet.GetMissionPosition();
                mo.transform.LookAt(Vector3.zero, Vector3.forward);
                mo.SetObjectives();
                mapObjectives.Add(mo);
            }
        }


        public void GetCurrentObjective()
        {

        }
    }
}
