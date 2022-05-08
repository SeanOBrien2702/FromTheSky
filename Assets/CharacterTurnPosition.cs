using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FTS.Characters;

public class CharacterTurnPosition : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI positionText;
    [SerializeField] Image profilePicture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPositionText(int position)
    {
        positionText.text = position.ToString();
    }

    public void SetPortrait(Sprite profile)
    {
        profilePicture.sprite = profile;
    }

}
