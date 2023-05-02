using UnityEngine;
using FTS.Characters;

namespace FTS.Core
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "TutorialInfo", fileName = "TutorialInfo.asset")]
    public class TutorialInfo : ScriptableObject
    {
        [SerializeField] Vector2 windowPos;
        [SerializeField] Vector2 windowSize;
        [SerializeField] string text;

        public Vector2 WindowPos { get => windowPos; set => windowPos = value; }
        public Vector2 WindowSize { get => windowSize; set => windowSize = value; }
        public string Text { get => text; set => text = value; }
    }
}
