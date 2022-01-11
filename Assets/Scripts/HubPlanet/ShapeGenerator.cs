using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
    public PlanetShape shape;
    INoiseFilter[] noiseFilters;
    public PlanetMinMax planetMinMax;

    public void UpdateSettings(PlanetShape shpae)
    {
        this.shape = shpae;
        noiseFilters = new INoiseFilter[shpae.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(shpae.noiseLayers[i].noiseSettings);
        }
        planetMinMax = new PlanetMinMax();
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(shape.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (shape.noiseLayers[i].enabled)
            {
                float mask = (shape.noiseLayers[i].firstLayerMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        planetMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0, unscaledElevation);
        elevation = shape.radius * (1 + elevation);
        return elevation;
    }
}
