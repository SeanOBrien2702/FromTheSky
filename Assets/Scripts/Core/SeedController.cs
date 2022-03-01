using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    bool isRandom = true;
    int seed;

    public bool IsRandom { get => isRandom; set => isRandom = value; }
    public int Seed { get => seed; set => seed = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetSeed()
    {
        if (isRandom)
        {
            seed = Random.Range(0, 999999);
        }
        Debug.Log("set seed " + seed);
        Random.InitState(seed);
    }
}
