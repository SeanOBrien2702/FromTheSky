using System.Collections.Generic;
using UnityEngine;
using FTS.Core;

namespace FTS.UI
{
    public class ObjectiveUIController : MonoBehaviour
    {
        [SerializeField] ObjectiveUI objectiveLabel;
        [SerializeField] Transform panel;
        //[SerializeField] List<Objective> objectivesBuffer;
        Dictionary<Objective, ObjectiveUI> objectiveList = new Dictionary<Objective, ObjectiveUI>();

        public void CreateObjectiveText(List<Objective> objectives)
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                ObjectiveUI objectiveUI = Instantiate(objectiveLabel);
                objectiveUI.UpdateObjective(objectives[i]); 
                objectiveUI.transform.SetParent(panel, false);

                objectiveList.Add(objectives[i], objectiveUI);
            }

            foreach (var item in objectives)
            {
                UpdateUI(item);
            }
        }
        
        public void UpdateUI(List<Objective> objectives)
        {
            foreach (var objective in objectives)
            {
                objectiveList[objective].UpdateObjective(objective);
            }
        }

        public void UpdateUI(Objective objective)
        {
            objectiveList[objective].UpdateObjective(objective);       
        }
    }
}
