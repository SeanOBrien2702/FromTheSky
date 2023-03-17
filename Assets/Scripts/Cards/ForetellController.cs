using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.Cards
{
    public class ForetellController : MonoBehaviour
    {
        bool isForetelling = false;
        CardController cardController;
        GameUI gameUI;

        #region Properties
        public bool IsForetelling
        {
            get { return isForetelling; }
            set { isForetelling = value; }
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            cardController = GetComponent<CardController>();
            gameUI = FindObjectOfType<GameUI>().GetComponent<GameUI>();
        }

        private void Update()
        {
            if (isForetelling && Input.GetMouseButtonDown(0))
            {
                Debug.Log("try to select foretell");
                string cardID = MouseOverCard();
                if(cardID != "")
                {
                    Debug.Log("place on top " +cardID);
                    isForetelling = false;
                    cardController.PlaceOnTopOfDeck(cardID);
                    gameUI.ToggleForetell(null);

                }
            }
        }
        #endregion

        #region Private Methods

        private string MouseOverCard()
        {
            string cardID = null;
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            if (raycastResults.Count > 0 && raycastResults[0].gameObject.tag == "Card")
            {
                cardID = raycastResults[0].gameObject.GetComponentInParent<CardUI>().CardID;
                Debug.Log(raycastResults[0].gameObject.GetComponentInParent<CardUI>().Name);
            }
            return cardID;
        }
        #endregion

        #region Public Methods
        internal void Foretell(int foretellAmount)
        {
            var inDeck = cardController.GetDeck().FindAll(item => item.Location == CardLocation.Deck).Take(foretellAmount);
            List<Card> buffer = new List<Card>();

            buffer.AddRange(inDeck);

            gameUI.ToggleForetell(buffer);
            isForetelling = true;
        }
        #endregion
    }
}
