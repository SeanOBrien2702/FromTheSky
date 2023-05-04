using FTS.Core;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    bool isRandom = true;
    int seed;
    int tutorialSeed = 7;

    public bool IsRandom { get => isRandom; set => isRandom = value; }
    public int Seed { get => seed; set => seed = value; }

    public void SetSeed()
    {
        if(!TutorialController.Instance.IsTutorialComplete)
        {
            seed = tutorialSeed;
        }
        else if (isRandom)
        {
            seed = Random.Range(0, 999999);
        }

        Random.InitState(seed);
    }
}
