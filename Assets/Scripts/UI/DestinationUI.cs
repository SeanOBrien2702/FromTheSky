#region Using Statements
using SP.Core;
using System;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class DestinationUI : MonoBehaviour
{
    [SerializeField] Image nextEventImage;
    Camera cam;

    int maxHealth = 0;
    #region MonoBehaviour Callbacks
    void Start()
    {
        cam = Camera.main;
    }

    public void FixedUpdate()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);

    }
    #endregion

    #region Private Methods

    #endregion

    #region Public Methods
    public void SetImage(Image image)
    {
        nextEventImage = image;
    }

    internal void SetType(Destination destination)
    {
        nextEventImage.sprite = destination.Image;
    }

    #endregion
}

