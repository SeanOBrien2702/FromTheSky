using FTS.Cards;
using FTS.Turns;
using UnityEngine;

namespace FTS.UI
{
    public class CardList : MonoBehaviour
    {
        [SerializeField] private GameObject image;
        [SerializeField] private CardController cardController;
        Canvas canvas;

        private void Start()
        {
            canvas = GameObject.FindGameObjectWithTag("DeckUI").GetComponent<Canvas>();
            TurnController.OnPlayerTurn += Populate;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= Populate;
        }

        public void ToggleUI()
        {
            if (!canvas.enabled)
            {
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
            }

        }

        public void Populate()
        {
            //foreach (Transform child in this.transform)
            //{
            //    Destroy(child.gameObject);
            //}
            ////ToggleUI();
            //foreach (Card card in cardController.GetDeck())
            //{
            //    //instantiate item
            //    GameObject label = Instantiate(image);
            //    //setParent
            //    label.transform.SetParent(transform, false);
            //    label.GetComponent<PersonLabel>().SetLabel(card.CardName,
            //                                               card.RulesText,
            //                                               "Cards!!");
            //}
        }
    }
}