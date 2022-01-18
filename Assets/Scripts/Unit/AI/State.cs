#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/State")]
    public class State : ScriptableObject
    {
        public Transition[] transitions;
        public Action[] actions;


        #region Private Methods
        public IEnumerator DoActions(StateController controller, StateMachine machine)
        {          
            for (int i = 0; i < actions.Length; i++)
            {
                controller.ActionDone = false;
                actions[i].Act(controller, machine);
                yield return new WaitUntil(() => controller.ActionDone == true || Input.GetKeyDown(KeyCode.M));             
            }
            yield return new WaitForSeconds(0.5f);
        }

        public void CheckTransitions(StateMachine controller)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(controller);

                if (decisionSucceeded)
                {
                    controller.TransitionToState(transitions[i].trueState);
                    //Debug.Log("state true");
                }
                else
                {
                    controller.TransitionToState(transitions[i].falseState);
                    //Debug.Log("state false");
                }
            }
        }
        #endregion
    }
}