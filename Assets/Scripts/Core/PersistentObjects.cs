using UnityEngine;

namespace FTS.Core
{
    public class PersistentObjects : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;
        /*
        * FUNCTION    : Awake()
        * DESCRIPTION : Singleton design pattern for spawning persistent game objects 
        * PARAMETERS  :
        *		VOID
        * RETURNS     :
        *		VOID
        */
        private void Awake()
        {
            if (!hasSpawned)
            {
                SpawnPersistentObjects();
                hasSpawned = true;
            }
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}