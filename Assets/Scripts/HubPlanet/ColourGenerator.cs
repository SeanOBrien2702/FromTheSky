using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    PlanetColour settings;
    Texture2D texture;
    const int textureResolution = 58;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(PlanetColour settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution, settings.biomeColourSettings.biomes.Length);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noiseSettings);
    }

    public void UpdateElevation(PlanetMinMax minMax)
    {
        settings.planetMaterial.SetVector("_minMax", new Vector4(minMax.Min, minMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUintSphere)
    {
        float heightPercent = (pointOnUintSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUintSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColourSettings.biomes.Length;
        float blendRrange = settings.biomeColourSettings.blendAmout / 2f + 0.001f;

        for (int i = 0; i < numBiomes; i++)
        {
            float distance = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRrange, blendRrange, distance);
            biomeIndex *= (i - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, biomeIndex - 1);
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[texture.width * texture.height];
        int colourIndex = 0;
        foreach (var biome in settings.biomeColourSettings.biomes)
        {
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientColour = biome.gradient.Evaluate(i / (textureResolution - 1f));
                Color tintColour = biome.tint;
                colours[colourIndex] = gradientColour * (1 - biome.tintPercent) + tintColour * biome.tintPercent;
                colourIndex++;
            }
        }

        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
