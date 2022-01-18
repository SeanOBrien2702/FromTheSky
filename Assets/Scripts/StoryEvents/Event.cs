using UnityEngine;

public enum EventRarity
{
    common, uncommon, rare
}
namespace FTS.Events
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Events/Event", fileName = "Event.asset")]
    public class Event : ScriptableObject
    {

        [SerializeField] string title;
        [SerializeField] string description;
        [SerializeField] Sprite image;
        [SerializeField] EventRarity rarity;
        [SerializeField] EventType type;



        [SerializeField] public Option[] options;

        public Sprite Image   // property
        {
            get { return image; }   // get method
            set { image = value; }  // set method
        }

        public string Title   // property
        {
            get { return title; }   // get method
            set { title = value; }  // set method
        }
    }
}
