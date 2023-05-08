#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.UI;
using System;
using UnityEngine.EventSystems;
#endregion

namespace FTS.Cards
{
    public class HandPrefab
    { 
        public GameObject CardGameObject;
        //public GameObject Zoom;
        public RectTransform ZoomArea;
        public Draggable draggable;
        public string CardID;
        public Vector3 StartPosition;
        public Vector3 ZoomPosition;
        public int Tilt;
        public int SiblingIndex;

        public HandPrefab(GameObject gameObject, string cardID)
        {
            CardGameObject = gameObject;
            //Zoom = collider;
            CardID = cardID;
            ZoomArea = gameObject.GetComponent<RectTransform>();
            draggable = gameObject.GetComponent<Draggable>();
            ZoomPosition = Vector3.zero;
            StartPosition = Vector3.zero;
            Tilt = 0;
            SiblingIndex = 0;
        }
    }

    public class HandController : MonoBehaviour
    {
        public static event Action<string> OnCardSelected = delegate { };

        [SerializeField] CardController cardController;
        [SerializeField] UnitController unitController;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] Transform targetingPosition;

        [Header("Hand Position")]
        [SerializeField] Transform deckPosition;
        [SerializeField] Transform discardPosition;
        [SerializeField] GameObject handPosition;
        [SerializeField] GameObject zoomPoistion;

        [Header("Zoom Settings")]
        [SerializeField] int zoomHeight = 300;
        [Range(1f, 1.5f)]
        [SerializeField] float zoomInScaling = 1.1f;
        [SerializeField] float zoomInSpeed = 0.5f;
        [SerializeField] float zoomoutSpeed = 1f;
        Vector3 zoomScale = new Vector3(1.25f, 1.25f, 1);
        Vector3 handScale = new Vector3(1f, 1f, 1);
        Vector3 discardScale = new Vector3(0.05f, 0.05f, 1);
        bool isTargetingZoom = false;
        float startZoom;
        float endZoom;
        int currentZoom = -1;

        [Header("Draw Settings")]
        [SerializeField] float drawInterval;
        [SerializeField] float duration = 1f;
        [SerializeField] SFXObject drawSound;
        int spacingIncrement = 600;
        int spacingAdjustment = 35;       
        int rotationIncrement = 6;
        bool isDrawing = false;
        bool lerping = false;

        public static event System.Action OnCardsSpaced = delegate { };
        List<HandPrefab> handPrefabs = new List<HandPrefab>();
        List<CardUI> cardUI = new List<CardUI>();
        Queue<HandPrefab> cardsToBeDrawn = new Queue<HandPrefab>();
        string selectedCard = null;

        private KeyCode[] keyCodes = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
                                                    KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
                                                    KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

        #region Properties
        public bool LERPing  // property
        {
            get { return lerping; }   // get method
            set { lerping = value; }  // set method
        }

        public string SelectedCard { get => selectedCard; set => selectedCard = value; }
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            UnitController.OnSelectPlayer += UnitController_OnSelectPlayer;
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            UnitController.OnEnergyChanged += UnitController_OnEnergyChanged; 
            CardController.OnCardDrawn += CardController_OnCardDrawn;
        }

        private void OnDestroy()
        {
            UnitController.OnSelectPlayer += UnitController_OnSelectPlayer;
            UnitController.OnSelectUnit -= UnitController_OnSelectUnit;
            UnitController.OnEnergyChanged -= UnitController_OnEnergyChanged;
            CardController.OnCardDrawn -= CardController_OnCardDrawn;
        }

        private void Update()
        {         
            if (!unitController.CurrentPlayer)
            {
                return;
            }
            if (UpdateCurrentZoom())
            {
                if (currentZoom >= 0)
                    ZoomIn(currentZoom);
            }

            for (int i = 0; i < handPrefabs.Count; ++i)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    int positionSelected = handPrefabs.Count - i - 1;
                    foreach (var item in handPrefabs)
                    {
                        item.draggable.IsDragging = false;
                    }
                    if (selectedCard == handPrefabs[positionSelected].CardID)
                    {
                        handPrefabs[positionSelected].draggable.IsDragging = false;
                        selectedCard = null;
                    }
                    else
                    {
                        handPrefabs[positionSelected].draggable.IsDragging = true;
                        selectedCard = handPrefabs[positionSelected].CardID;
                    }
                    SelectCard(selectedCard);
                }
            }

            if (selectedCard != null && Input.GetMouseButtonDown(0))
            {
                cardController.PlayCard(selectedCard);
                selectedCard = null;
                OnCardSelected?.Invoke(selectedCard);
            }

            if (selectedCard != null && Input.GetMouseButtonDown(1))
            {               
                handPrefabs.Find(item => item.CardID == selectedCard).draggable.IsDragging = false;
                selectedCard = null;
                SpaceHand();                   
                OnCardSelected?.Invoke(selectedCard);
            }
        }
        #endregion

        #region Private Methods
        private void SpaceHand(string ignoredCard = null)
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
            if (!transform.gameObject.activeSelf)
            {
                return;
            }
            foreach (HandPrefab child in handPrefabs)
            {              
                Vector3 startPosition = new Vector3(position, -Mathf.Abs(position) / 10, 0);
                child.StartPosition = startPosition;
                child.ZoomPosition = new Vector3(position, zoomHeight, 0);
                child.Tilt = rotation;
                child.CardGameObject.transform.SetAsFirstSibling();
                if (child.CardID != ignoredCard)
                {
                    StartCoroutine(LerpCard(child.CardGameObject,
                                           startPosition,
                                           handScale,
                                           Quaternion.Euler(0, 0, rotation),
                                           duration));

                }
                rotation += rotationIncrement;
                position -= increment;                              
            }                          
        }

        private void CalculateHandPixelWidth()
        {
            if (handPrefabs.Count > 0)
            {
                Vector3[] v = new Vector3[4];
                handPrefabs[0].ZoomArea.GetWorldCorners(v);
                endZoom = v[2].x;
                handPrefabs[handPrefabs.Count - 1].ZoomArea.GetWorldCorners(v);
                startZoom = v[1].x;
            }
            else
            {
                endZoom = 0;
                startZoom = 0;  
            }
        }

        private void UpdateHighlight()
        {
            if (cardUI.Count <= 0)
            {
                return;
            }
            DisableHighlight();

            foreach (var item in cardUI)
            {
                Card card = cardController.GetCard(item.CardID);
                if (cardController.CanPlay(card))
                {
                    item.HighlightCard(true);
                }
            }
            
        }

        private void DisableHighlight()
        {    
            if (cardUI.Count > 0)
            {
                foreach (var item in cardUI)
                {
                    item.HighlightCard(false);
                }
            }
        }

        public void SelectCard(string cardID)
        {
            OnCardSelected?.Invoke(cardID);
            SpaceHand(cardID);
            if (cardID != null)
                Targeting(cardID);
        }

        internal void ZoomOut(int index)
        {
            if (!isTargetingZoom)
            {
                HandPrefab handPrefab = handPrefabs[index];
                handPrefab.CardGameObject.transform.SetSiblingIndex(handPrefab.SiblingIndex);
                StartCoroutine(LerpCard(handPrefab.CardGameObject,
                                          handPrefab.StartPosition,
                                          handScale,
                                          Quaternion.Euler(0, 0, handPrefab.Tilt),
                                          zoomoutSpeed));
            }
        }

        internal void ZoomIn(int index)
        {
            if (!isTargetingZoom)
            {
                HandPrefab handPrefab = handPrefabs[index];
                handPrefab.SiblingIndex = handPrefab.CardGameObject.transform.GetSiblingIndex();
                handPrefab.CardGameObject.transform.SetAsLastSibling();
                StartCoroutine(LerpCard(handPrefab.CardGameObject,
                                          handPrefab.ZoomPosition,
                                          zoomScale,
                                          Quaternion.Euler(Vector3.zero),
                                          zoomInSpeed));
            }
        }

        internal void Targeting(string cardID)
        {
            isTargetingZoom = true;
            currentZoom = -1;
            HandPrefab handPrefab = handPrefabs.Find(item => item.CardID == cardID);
            handPrefab.SiblingIndex = handPrefab.CardGameObject.transform.GetSiblingIndex();
            handPrefab.CardGameObject.transform.SetAsLastSibling();
            StartCoroutine(LerpCard(handPrefab.CardGameObject,
                          targetingPosition.position,
                          zoomScale,
                          Quaternion.Euler(Vector3.zero),
                          zoomInSpeed));
        }


        private bool UpdateCurrentZoom()
        {
            int zoom = MouseOverZoom();
            if (zoom != currentZoom)
            {
                if (currentZoom >= 0)
                {
                    ZoomOut(currentZoom);
                }
                currentZoom = zoom;
                return true;
            }
            return false;
        }

        private int MouseOverZoom()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            int index = -1;
            if (pointerData.position.y < 150 &&
                pointerData.position.x > startZoom &&
                pointerData.position.x < endZoom)
            {
                float position = (int)pointerData.position.x - endZoom;
                float increment = (endZoom - startZoom) / handPrefabs.Count;
                index = (int)Math.Floor(-position / increment);
            }
            return index;
        }
        void SpaceHandStager()
        {
            if (!isDrawing)
                StartCoroutine(SpaceHandStagerCoroutine());

        }

        IEnumerator SpaceHandStagerCoroutine()
        {
            isDrawing = true;
            while (cardsToBeDrawn.Count > 0)
            {
                handPrefabs.Add(cardsToBeDrawn.Dequeue());
                SFXManager.Main.Play(drawSound);
                SpaceHand();
                yield return new WaitForSeconds(drawInterval);
            }
            isDrawing = false;
        }
        #endregion

        #region Public Methods
        public List<CardUI> GetCardInfos()
        {
            return cardUI;
        }


        public void RemoveCard(Card card)
        {
            HandPrefab handPrefab = handPrefabs.Find(item => item.CardID == card.Id);
            if(handPrefab == null)
            {
                return;
            }
            StartCoroutine(LerpToDiscard(handPrefab.CardGameObject,
                                         discardPosition.position,
                                         discardScale,
                                         Quaternion.Euler(Vector3.zero)));
            cardUI.RemoveAll(item => item.CardID == card.Id);
            handPrefabs.Remove(handPrefab);
            SpaceHand();
        }

        public void DrawAnimation(Card card)
        {
            GameObject drawnCard = Instantiate<GameObject>(cardPrefab);
            drawnCard.transform.SetParent(handPosition.transform, false);
            drawnCard.transform.SetAsFirstSibling();
            drawnCard.transform.position = deckPosition.position;
            drawnCard.transform.localScale = discardScale;

            CardUI newCardUI = drawnCard.GetComponent<CardUI>();
            newCardUI.SaveCardData(card);
            newCardUI.FillCardUI(card);           
            cardUI.Add(newCardUI);

            HandPrefab handPrefab = new HandPrefab(drawnCard, newCardUI.CardID);
            cardsToBeDrawn.Enqueue(handPrefab);
            //handPrefabs.Add(handPrefab);
            SpaceHandStager();
        }

        internal void DiscardHand()
        {
            foreach (var item in handPrefabs)
            {
                StartCoroutine(LerpToDiscard(item.CardGameObject,
                                             discardPosition.position,
                                             discardScale,
                                             Quaternion.Euler(Vector3.zero)));
            }
            cardUI.Clear();
            handPrefabs.Clear();
        }
        internal void ReadjustHand()
        {
            SpaceHand();
        }

        public void SetTagetingZoom(bool zoom)
        {
            isTargetingZoom = zoom;
        }

        public void SetCard(string cardID)
        {
            SelectCard(cardID);
        }
        #endregion

        #region Coroutines
        IEnumerator LerpCard(GameObject gameObject,
                               Vector3 targetPosition,
                               Vector3 targetScale,
                               Quaternion targetRotation,
                               float duration)
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
            CalculateHandPixelWidth();
        }

        IEnumerator LerpToDiscard(GameObject gameObject,
                                   Vector3 targetPosition,
                                   Vector3 targetScale,
                                   Quaternion targetRotation)
        {
            yield return LerpCard(gameObject,
                                   targetPosition,
                                   targetScale,
                                   targetRotation,
                                   duration);
            Destroy(gameObject);
        }

        #endregion

        #region Events

        private void UnitController_OnSelectPlayer(Player player)
        {
            if (unitController.CurrentPlayer == player)
            {
                UpdateHighlight();
            }
        }

        private void UnitController_OnEnergyChanged(Player player, int energy)
        {
            if(unitController.CurrentPlayer == player)
            {
                UpdateHighlight();
            }
        }

        private void CardController_OnCardDrawn()
        {
            UpdateHighlight();
        }

        private void UnitController_OnSelectUnit(Unit unit)
        {
            DisableHighlight();
        }
        #endregion
    }
}