using AeLa.EasyFeedback.APIs;
using FTS.Cards;
using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DeckUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Transform contentPanel;

    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] CardController cardController;
    int currentLocation = -1;

    [SerializeField] CardUI cardPrefab;
    List<CardUI> cards = new List<CardUI>();

    void Start()
    {
        foreach (Card card in cardController.GetDeck())
        {
            CardUI go = Instantiate(cardPrefab);
            go.transform.SetParent(contentPanel, false);
            go.FillCardUI(card);
            Debug.Log("card.Id " + go.CardID);
            cards.Add(go);
        }
    }

    public void OpenPanel(int cardLocation)
    {
        if(panel.activeSelf && currentLocation == cardLocation)
        {
            panel.SetActive(false);
            return;
        }
        currentLocation = cardLocation;
        CardLocation location = (CardLocation)cardLocation;
        panel.SetActive(true);
        headerText.text = location.ToString();

        foreach (CardUI card in cards)
        {
            if (card.CardInfos.Location == location)
            {
                card.gameObject.SetActive(true);
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
