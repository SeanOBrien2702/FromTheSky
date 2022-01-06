using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.simple:
                return new SimpleNoiseFilter(settings.simpleNoiseSettings);
            case NoiseSettings.FilterType.ridgid:
                return new RidgidNoiseFilter(settings.ridgidNoiseSettings);
            //case NoiseSettings.FilterType.biome:
            //    return new RidgidNoiseFilter(settings.);
            default:
                return null;
        }
    }
}
