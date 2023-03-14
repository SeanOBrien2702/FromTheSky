using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FTS.Core;


public class PlanetInteratable : MonoBehaviour
{
    bool overPlanet = false;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (overPlanet && Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.Log(hit.point);
                // Do something with the object that was hit by the raycast.
            }

            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }

    private void OnMouseEnter()
    {
        overPlanet = true;
    }

    private void OnMouseExit()
    {
        overPlanet = false;
    }
}
