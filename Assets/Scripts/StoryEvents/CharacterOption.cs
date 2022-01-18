using UnityEngine;

namespace FTS.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Events/Option/Character", fileName = "CharacterOption.asset")]
    public class CharacterOption : Option
    {

        //private void OnEnable()
        private void OnEnable()
        {

            //Debug.Log("population option");
            //Debug.Log("Number of people: " + population.GetAvblPopulation());
        }

        public override void SelectEvent()
        {

        }

    }
}