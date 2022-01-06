using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Lighting Preset", order = 1)]
public class LigthingPreset : ScriptableObject
{
    public Gradient ambientColour;
    public Gradient directionalColour;
    public Gradient fogColour;
}
