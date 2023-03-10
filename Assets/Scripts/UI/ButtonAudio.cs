using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] SFXObject hoverSound;
    [SerializeField] SFXObject pressedSound;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick); 
    }

    private void OnButtonClick()
    {
        SFXManager.Main.Play(pressedSound);
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        SFXManager.Main.Play(hoverSound);
        Debug.Log("Cursor Entering " + name + " GameObject");
    }
}
