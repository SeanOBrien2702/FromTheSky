#region Using Statements
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FTS.Characters;
using FTS.UI;
using System;
using UnityEngine.EventSystems;
using AMPInternal;
using FTS.Grid;
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
            if(UpdateCurrentZoom())
            {
                if(currentZoom >= 0)
                ZoomIn(currentZoom);
            }
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
                foreach (HandPrefab child in handPrefabs)
                {
                    Vector3 startPosition = new Vector3(position, -Mathf.Abs(position) / 10, 0);
                    child.StartPosition = startPosition;
                    child.ZoomPosition = new Vector3(position, zoomHeight, 0);
                    child.Tilt = rotation;
                    child.CardGameObject.transform.SetAsFirstSibling();
                    StartCoroutine(LerpCard(child.CardGameObject,
                                              startPosition,
                                              handScale,
                                              Quaternion.Euler(0, 0, rotation)));

                    rotation += rotationIncrement;
                    position -= increment;                              
                }               
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
            if (cardUI.Count > 0)
            {
                DisableHighlight();

                foreach (var item in cardUI)
                {
                    Card card = cardController.GetCard(item.CardID);
                    //item.FillCardUI(player, card);
                    //item.FillCardUI(card);
                    if (cardController.CanPlay(card))
                    {
                        item.HighlightCard(true);
                    }
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

        internal void ZoomOut(int index)
        {
            if (!isTargetingZoom)
            {
                HandPrefab handPrefab = handPrefabs[index];
                handPrefab.CardGameObject.transform.SetSiblingIndex(handPrefab.SiblingIndex);
                StartCoroutine(LerpCard(handPrefab.CardGameObject,
                                          handPrefab.StartPosition,
                                          handScale,
                                          Quaternion.Euler(0, 0, handPrefab.Tilt)));
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
                                          Quaternion.Euler(Vector3.zero)));
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
                          Quaternion.Euler(Vector3.zero)));
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
            StartCoroutine(LerpToDiscard(handPrefab.CardGameObject,
                                         discardPosition.position,
                                         discardScale,
                                         Quaternion.Euler(Vector3.zero)));
            cardUI.RemoveAll(item => item.CardID == card.Id);
            handPrefabs.Remove(handPrefab);
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
        #endregion

        #region Coroutines
        IEnumerator LerpCard(GameObject gameObject,
                               Vector3 targetPosition,
                               Vector3 targetScale,
                               Quaternion targetRotation)
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
                                   targetRotation);
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