using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField] Track mainMenuTrack;
    [SerializeField] Playlist combatTrack;
    void Start()
    {
        MusicManager.Main.Play(mainMenuTrack);
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log(oldScene.name + " to " + newScene.name);
        if(newScene.name == "GameScene")
        {
            Debug.Log("combat music?");
            PlaylistManager.Main.Play(combatTrack);
            //MusicManager.Main.Pause();
        }
        else
        {
            MusicManager.Main.Play(mainMenuTrack, 0.5f, 1);
        }
    }
}
