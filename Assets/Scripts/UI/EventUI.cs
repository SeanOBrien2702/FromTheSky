#region Using Statements
using FTS.Events;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class EventUI : MonoBehaviour
{
    EventController eventController;
    [SerializeField] Image image;

    [SerializeField] Text title;

    [SerializeField] Button option1;
    [SerializeField] Button option2;
    [SerializeField] Button option3;
    FTS.Events.Event newEvent;

    #region MonoBehaviour Callbacks
    void Awake()
    {
        Debug.Log("Is event controller enabled?");
        eventController = FindObjectOfType<EventController>().GetComponent<EventController>();
        if (eventController)
        {
            Debug.Log(eventController.name);
        }
        else
        {
            Debug.Log("No game object called eventController found");
        }
    }

    private void OnEnable()
    {
        Debug.Log("Event enabled");
        newEvent = eventController.GetEvent();
        BuildEvent();
    }
    #endregion

    #region Public Methods
    public void BuildEvent()
    {
        //image.sprite = storyEvent.Image;
        title.text = newEvent.Title;
        option1.GetComponentInChildren<Text>().text = newEvent.options[0].ButtonText;
        option2.GetComponentInChildren<Text>().text = newEvent.options[1].ButtonText;
        //foreach (Option options in storyEvent.options)
        //{
        //    //Debug.Log(options.buttonText); 
        //}
    }

    public void Option1()
    {
        newEvent.options[0].SelectEvent();
        eventController.ReturnToGame();
    }

    public void Option2()
    {
        newEvent.options[1].SelectEvent();
        eventController.ReturnToGame();
    }
    #endregion
}
