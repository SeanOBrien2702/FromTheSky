using UnityEngine;
using FTS.Grid;


namespace FTS.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Destination", fileName = "Destination.asset")]
    public class Destination : ScriptableObject
    {
        [SerializeField] string scenePath;
        [SerializeField] Sprite image;
        HexCoordinates location;

        public Sprite Image   // property
        {
            get { return image; }   // get method
            set { image = value; }  // set method
        }

        public string ScenePath   // property
        {
            get { return scenePath; }   // get method
            set { scenePath = value; }  // set method
        }

        public HexCoordinates Location   // property
        {
            get { return location; }   // get method
            set { location = value; }  // set method
        }
    }
}
