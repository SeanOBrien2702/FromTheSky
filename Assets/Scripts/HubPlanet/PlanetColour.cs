using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Planet/Colour", fileName = "PlanetColour.asset")]
public class PlanetColour : ScriptableObject
{
    //[SerializeField] public Gradient gradient;
    [SerializeField] public Material planetMaterial;
    public BiomeColourSettings biomeColourSettings;

    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmout;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0, 1)]
            public float startHeight;
            [Range(0, 1)]
            public float tintPercent;
        }
    }
}
