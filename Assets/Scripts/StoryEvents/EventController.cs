using System.Collections;
using UnityEngine;

public enum EventType
{
    Exploration, Population, Resources, Special
}

namespace FTS.Events
{
    public class EventController : MonoBehaviour
    {
        [SerializeField] Animator transition;
        [SerializeField] UIController uIController;

        [SerializeField] Event[] repairEvents;
        [SerializeField] Event[] exploreEvents;
        [SerializeField] Event[] populationEvents;
        [SerializeField] Event[] specialEvents;
        [SerializeField] Event testEvent;


        float transitionTime = 0.5f;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void TriggerEvent(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Exploration:
                    break;
                case EventType.Population:
                    break;
                case EventType.Resources:
                    break;
                case EventType.Special:
                    break;
                default:
                    break;
            }
        }


        public Event GetEvent()
        {
            return testEvent;
        }
        public void ExplorationEvent()
        {
            StartCoroutine(Transition(true));
        }

        //public void DraftEvent()
        //{
        //    StartCoroutine(Transition(true));
            
        //}

        public void RepairEvent(int level)
        {
            StartCoroutine(Transition(true));
            //StartEvent();
        }

        private void StartEvent()
        {
            uIController.Event();

        }

        public void ReturnToGame()
        {
            StartCoroutine(Transition(false));
        }

        IEnumerator Transition(bool isEvent)
        {

            transition.SetTrigger("FadeIn");
            //transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            if (isEvent)
            {
                uIController.Event();
            }
            else
            {
                uIController.Game();
            }
            transition.SetTrigger("FadeOut");
            yield return new WaitForSeconds(transitionTime);
            transition.SetTrigger("Reset");

        }
    }
}