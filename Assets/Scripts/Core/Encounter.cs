using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Core
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Encounter", fileName = "Encounter.asset")]
    public class Encounter : ScriptableObject
    {
        [SerializeField] string encounterName;
        [SerializeField] Sprite encounterSprite;
        [SerializeField] Scenes nextScene;
        [SerializeField] bool isSceneAddative = false;

        public string EncounterName { get => encounterName; set => encounterName = value; }
        public Sprite EncounterSprite { get => encounterSprite; set => encounterSprite = value; }
        public Scenes NextScene { get => nextScene; set => nextScene = value; }
        public bool IsSceneAddative { get => isSceneAddative; set => isSceneAddative = value; }
    }
}
