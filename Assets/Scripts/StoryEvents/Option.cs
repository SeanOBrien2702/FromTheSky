using UnityEngine;

namespace FTS.Events
{
    [System.Serializable]
    public abstract class Option : ScriptableObject
    {
        [SerializeField] string buttonText;


        public string ButtonText   // property
        {
            get { return buttonText; }   // get method
            set { buttonText = value; }  // set method
        }

        public virtual void SelectEvent()
        {
            Debug.Log("Event selected");
        }
    }
}