using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RidgidNoiseSettings settings;

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings planetNoise)
    {
        this.settings = planetNoise;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float nv = 1-Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
            nv *= nv;
            nv *= weight;
            weight = Mathf.Clamp01(nv * settings.weightMultiplier);

            noiseValue += nv * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
