using UnityEngine;

public class SeedController : MonoBehaviour
{
    bool isRandom = true;
    int seed;

    public bool IsRandom { get => isRandom; set => isRandom = value; }
    public int Seed { get => seed; set => seed = value; }

    public void SetSeed()
    {
        if (isRandom)
        {
            seed = Random.Range(0, 999999);
        }
        Random.InitState(seed);
    }
}
