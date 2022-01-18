#region Using Statements
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FTS.Characters;
#endregion

namespace FTS.Cards
{
    public class HandController : MonoBehaviour
    {
        [SerializeField] CardController cardController;
        [SerializeField] UnitController unitController;
        [SerializeField] GameObject cardPrefab;

        [Header("Hand Position")]
        [SerializeField] Transform deckPosition;
        [SerializeField] Transform discardPosition;
        [SerializeField] GameObject handPosition;


        Dictionary<string, GameObject> handPrefabs = new Dictionary<string, GameObject>();
        List<CardUI> cardUI = new List<CardUI>();
        int spacingIncrement = 600;
        int spacingAdjustment = 35;
        float duration = 0.5f;
        int rotationIncrement = 6;
        float scaleModifier = 1;
        Vector3 handScale = new Vector3(1f, 1f, 1);
        Vector3 discardScale = new Vector3(0.25f, 0.25f, 1);
        bool lerping = false;



        #region Properties
        public bool LERPing  // property
        {
            get { return lerping; }   // get method
            set { lerping = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {   
            UnitController.OnPlayerSelected += UnitController_OnPlayerSelected;
            CardController.OnEnergyChanged += CardController_OnEnergyChanged;
            CardController.OnCardDrawn += CardController_OnCardDrawn;
        }

        private void OnDestroy()
        {
            UnitController.OnPlayerSelected -= UnitController_OnPlayerSelected;
            CardController.OnEnergyChanged -= CardController_OnEnergyChanged;
            CardController.OnCardDrawn -= CardController_OnCardDrawn;
        }

        #endregion

        #region Private Methods
        private void SpaceHand()
        {
            int handSize = handPrefabs.Count;
            //Set position
            int increment = spacingIncrement - handSize * spacingAdjustment;
            int position = 0;
            if (handSize % 2 == 0)
            {
                position = increment * (handSize - 1) / 2;
            }
            else
            {
                position = increment * handSize / 2 - increment / 2;
            }

            int rotation = 0;
            if (handSize % 2 == 0)
            {
                rotation = -rotationIncrement * (handSize - 1) / 2;
            }
            else
            {
                rotation = -rotationIncrement * handSize / 2 + rotationIncrement / 2;
            }
            if (transform.gameObject.activeSelf)
            {
                foreach (GameObject child in handPrefabs.Values)
                {
                    StartCoroutine(LerpToHand(child, new Vector3(position, -Mathf.Abs(position) / 10, 0), handScale, Quaternion.Euler(0, 0, rotation)));
                    child.transform.SetAsFirstSibling();
                    rotation += rotationIncrement;
                    position -= increment;
                }
            }
        }

        private void UpdateHighlight()
        {
            Player player = unitController.GetCurrentPlayer();
            if (player != null && cardUI.Count > 0)
            {
                CharacterClass characterClass = player.CharacterClass;
                if (characterClass != CharacterClass.Vehicle)
                {
                    foreach (var item in cardUI)
                    {
                        item.HighlightCard(false);
                    }

                    foreach (var item in cardUI)
                    {
                        Card card = cardController.GetCard(item.CardID);
                        item.FillCardUI(player, card);
                        if (cardController.CanPlay(card, characterClass))
                        {
                            item.HighlightCard(true);
                        }

                    }
                }
            }
        }
        #endregion

        #region Public Methods
        public List<CardUI> GetCardInfos()
        {
            return cardUI;
        }


        public void RemoveCard(Card card)
        {
            StartCoroutine(LerpToDiscard(handPrefabs[card.Id], discardPosition.position, discardScale));
            cardUI.RemoveAll(item => item.CardID == card.Id);
            handPrefabs.Remove(card.Id);
        }

        public void AddCard(Card card)
        {
            GameObject drawnCard = Instantiate<GameObject>(cardPrefab);
            drawnCard.transform.SetParent(handPosition.transform, false);
            drawnCard.transform.SetAsFirstSibling();
            drawnCard.transform.position = deckPosition.position;
            drawnCard.transform.localScale = discardScale;

            CardUI newCardUI = drawnCard.GetComponent<CardUI>();
            newCardUI.SaveCardData(card);
            newCardUI.FillCardUI(unitController.CurrentPlayer, card);
            
            cardUI.Add(newCardUI);
            handPrefabs.Add(newCardUI.CardID, drawnCard);
            SpaceHand();
        }

        internal void DiscardHand()
        {
            foreach (var item in handPrefabs)
            {
                StartCoroutine(LerpToDiscard(item.Value, discardPosition.position, discardScale));
            }
            cardUI.Clear();
            handPrefabs.Clear();
        }

        public void ReadjustHand()
        {
            SpaceHand();
        }
        #endregion

        #region Coroutines
        IEnumerator LerpToHand(GameObject gameObject, Vector3 targetPosition, Vector3 targetScale, Quaternion targetRotation)
        {
            lerping = true;
            float time = 0;
            Vector3 startPosition = gameObject.transform.localPosition;
            Vector3 startScale = gameObject.transform.localScale;
            Quaternion startRotation = gameObject.transform.rotation;

            while (time < duration)
            {
                gameObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                gameObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
                gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
                time += UnityEngine.Time.deltaTime;
                yield return null;
            }
            gameObject.transform.localPosition = targetPosition;
            gameObject.transform.rotation = targetRotation;
            gameObject.transform.localScale = targetScale;

            lerping = false;
        }
        IEnumerator LerpToDiscard(GameObject gameObject, Vector3 targetPosition, Vector3 targetScale)
        {

            lerping = true;
            float time = 0;
            Vector3 startPosition = gameObject.transform.position;
            Vector3 startScale = gameObject.transform.localScale;

            while (time < duration)
            {
                gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
                time += UnityEngine.Time.deltaTime;
                yield return null;
            }

            gameObject.transform.position = targetPosition;
            gameObject.transform.localScale = targetScale;
            Destroy(gameObject);
            lerping = false;
        }
        #endregion

        #region Events
        private void UnitController_OnPlayerSelected()
        {
            UpdateHighlight();

        }

        private void CardController_OnEnergyChanged()
        {
            UpdateHighlight();
        }

        private void CardController_OnCardDrawn()
        {
            UpdateHighlight();
        }
        #endregion
    }
}