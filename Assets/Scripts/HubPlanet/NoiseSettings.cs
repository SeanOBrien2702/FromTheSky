using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(menuName = "Planet/Noise", fileName = "PlanetNoise.asset")]
public class NoiseSettings
{
    public enum FilterType {simple, ridgid, biome };
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RidgidNoiseSettings ridgidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1, 8)]
        public int numLayers = 1;
        public float baseRoughness = 1;
        [Tooltip("Determines distance between points sampled on shpere")]
        public float roughness = 2;
        //how much the amplitued is reduced each layer
        [Tooltip("How much the amplitued is reduced each layer")]
        public float persistence = 0.5f;
        public Vector3 center;
        [Tooltip("Minimue distance between center of shpere and points")]
        public float minValue;
    }

    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }
}