using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Planet/Shape", fileName = "PlanetShape.asset")]
public class PlanetShape : ScriptableObject
{
    [SerializeField] public int radius;
    [SerializeField] public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool firstLayerMask;
        public NoiseSettings noiseSettings;
    }
}
