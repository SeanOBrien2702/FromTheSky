using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMinMax
{
    private float min;
    private float max;
    public float Min { get => min; set => min = value; }
    public float Max { get => max; set => max = value; }

    public PlanetMinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float value)
    {
        if(value > Max)
        {
            Max = value;
        }
        if(value < Min)
        {
            Min = value;
        }
    }
}
