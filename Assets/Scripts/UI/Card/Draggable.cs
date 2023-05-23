#region Using Statements
using FTS.Cards;
using FTS.Characters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#endregion

namespace FTS.UI
{
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [Header("Drag Properties")]
        [SerializeField] bool dragOnSurfaces = true;
        CardController cardController;
        HandController handController;
        UnitController unitController;
        CardUI cardInfo;
        Transform cardPrefab;
        RectTransform draggingPlane;
        RectTransform rectTransform;

        [Header("Arrow Properties")]
        [Range(0,1)]
        [SerializeField] float arrowScale = 1f;
        [SerializeField] int arrowNodeNum = 8;
        [SerializeField] GameObject arrowHead;
        [SerializeField] GameObject arrowNode;
        [SerializeField] Transform arrowStart;

        List<RectTransform> arrowNodes = new List<RectTransform>();
        List<Vector2> controlPoints = new List<Vector2>();       
        readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.15f, 0.4f), new Vector2(0.05f, 0.7f) };

        Vector2 startPosition = new Vector2(Screen.height /2, Screen.width /2);
        PointerEventData pointerData;
        string cardId;
        bool isDragging;
        bool select = false;

        #region Properties
        public bool IsDragging { get => isDragging;
            set 
            { 
                isDragging = value; 
                if(!isDragging)
                    HideArrow();
            } 
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            cardId = GetComponent<CardUI>().CardID;
            cardPrefab = GetComponent<RectTransform>();
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            handController = FindObjectOfType<HandController>().GetComponent<HandController>();
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            cardInfo = GetComponent<CardUI>();
            pointerData = new PointerEventData(EventSystem.current);
            rectTransform = GetComponent<RectTransform>();

            for (int i = 0; i < arrowNodeNum; i++)
            {
                arrowNodes.Add(Instantiate(arrowNode, transform).GetComponent<RectTransform>());
            }
            arrowNodes.Add(Instantiate(arrowHead, transform).GetComponent<RectTransform>());

            arrowNodes.ForEach(item => item.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

            for (int i = 0; i < arrowNodeNum + 1; i++)
            {
                controlPoints.Add(Vector2.zero);
            }
        }

        private void Update()
        {
            if(isDragging)
            {
                Dragging();
                if(Input.GetMouseButtonDown(1))
                {
                    isDragging = false;
                    EndDrag(false);
                }
            }
        }
        #endregion

        #region Private Methods
        void HideArrow()
        {
            foreach (var arrowNode in arrowNodes)
            {
                arrowNode.position = new Vector2(-1000, -1000);
            }
        }

        private void DrawTargetLine()
        {
            controlPoints[0] = new Vector2(arrowStart.position.x, arrowStart.position.y);
            controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //Debug.Log("Draw line. start position " + controlPoints[0] +" end position: " + controlPoints[3]);
            controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[0];
            controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[1];

            for (int i = 0; i < arrowNodes.Count; i++)
            {
                float t = Mathf.Log(1f * i / (arrowNodes.Count - 1) + 1f, 2f);

                arrowNodes[i].position = Mathf.Pow(1 - t, 3) * controlPoints[0] +
                    3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1] +
                    3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                    Mathf.Pow(t, 3) * controlPoints[3];

                if(i > 0)
                {
                    var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, arrowNodes[i].position - arrowNodes[i - 1].position));
                    arrowNodes[i].rotation = Quaternion.Euler(euler);
                }

                var scale = arrowScale * (1f - 0.03f * (arrowNodes.Count - i - i));
                arrowNodes[i].localScale = new Vector3(scale, scale, 1f);

            }

            arrowNodes[0].transform.rotation = arrowNodes[1].transform.rotation;
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            //https://www.codegrepper.com/code-examples/csharp/how+to+get+2D+object+drag+with+mouse+unity
            if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
                draggingPlane = data.pointerEnter.transform as RectTransform;

            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out globalMousePos))
            {
                cardPrefab.transform.position = globalMousePos;
                //rt.rotation = m_DraggingPlane.rotation;
            }
        }

        void BeginDragging()
        {
            if (!unitController.CurrentPlayer)
            {
                return;
            }
            handController.SelectedCard = cardId;
            isDragging = true;
            startPosition = new Vector2(arrowStart.position.x, arrowStart.position.y);
            handController.SetTagetingZoom(true);
            cardController.CardSelect(cardId);
            if (cardInfo.Targeting != CardTargeting.None)
            {
                handController.Targeting(cardId);
            }
        }

        void Dragging()
        {
            if (cardPrefab != null && isDragging)
            {
                if (cardInfo.Targeting != CardTargeting.None)
                {
                    DrawTargetLine();
                }
            }
        }

        void EndDrag(bool isPlayed)
        {
            if (!unitController.CurrentPlayer)
            {
                return;
            }
            handController.SelectedCard = null;
            handController.SelectCard(null);
            isDragging = false;
            HideArrow();
            cardController.CardSelected = null;
            if (isPlayed)
            {
                if (cardInfo.Targeting != CardTargeting.None)
                {
                    cardController.PlayCard(cardInfo.CardID);
                }
                else
                {
                    if (transform.position.y > Screen.height / 3)
                    {
                        cardController.PlayCard(cardInfo.CardID);
                    }
                }
            }
            handController.SetTagetingZoom(false);
        }
        #endregion

        #region Public Methods
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragging();
        }

        public void OnDrag(PointerEventData eventData)
        {
            
            if (cardInfo.Targeting != CardTargeting.None)
            {
                Dragging();
            }
            else
            {
                if (!isDragging)
                {
                    return;
                }
                SetDraggedPosition(eventData);
            }
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                EndDrag(true);
            }
            else
            {
                EndDrag(false);
            }
        }
        #endregion
    }
}