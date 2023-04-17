using AeLa.EasyFeedback.APIs;
using FTS.Cards;
using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Transform contentPanel;

    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] CardController cardController;

    [SerializeField] GameObject cardPrefab;
    Dictionary<string, GameObject> cards = new Dictionary<string, GameObject>();

    void Start()
    {
        foreach (Card card in cardController.GetDeck())
        {
            GameObject go = (GameObject)Instantiate(cardPrefab);
            go.transform.SetParent(contentPanel, false);
            go.GetComponentInChildren<CardUI>().SaveCardData(card);
            cards.Add(card.Id, go);
            Debug.Log(card);
        }
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel(int cardLocation)
    {
        CardLocation location = (CardLocation)cardLocation;
        panel.SetActive(true);
        headerText.text = location.ToString();


        foreach (Card card in cardController.GetDeck())
        {
            if(card.Location == location)
            {
                cards[card.Id].SetActive(true);
            }
            else
            {
                cards[card.Id].SetActive(false);
            }
        }

    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
