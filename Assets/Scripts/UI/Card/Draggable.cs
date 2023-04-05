#region Using Statements
using FTS.Cards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;
#endregion

namespace FTS.UI
{
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [Header("Drag Properties")]
        [SerializeField] bool dragOnSurfaces = true;
        CardController cardController;
        HandController handController;
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

        [Header("MM Feedbacks")]
        [SerializeField] MMFeedbacks dragFeedback;
        MMFeedbackPosition position;

        List<RectTransform> arrowNodes = new List<RectTransform>();
        List<Vector2> controlPoints = new List<Vector2>();       
        readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.15f, 0.4f), new Vector2(0.05f, 0.7f) };

        Vector2 startPosition = new Vector2(Screen.height /2, Screen.width /2);
        PointerEventData pointerData;
        string cardId;
        bool isDragging;
        bool select = false;

        #region Properties
        public bool IsDragging
        {
            get { return isDragging; }
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            cardId = GetComponent<CardUI>().CardID;
            cardPrefab = GetComponent<RectTransform>();
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            handController = FindObjectOfType<HandController>().GetComponent<HandController>();
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
            //if (isDragging)
            //{
            //    if (cardPrefab != null)
            //    {
            //        if (cardInfo.Targeting != CardTargeting.None)
            //        {
            //            DrawTargetLine(pointerData);
            //            cardController.CardSelecte(cardId);
            //            //handController.Targeting();

            //        }
            //        else
            //        {
            //            //DrawTargetLine(pointerData);

            //            //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //            //mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            //            //transform.position = mousePosition;
            //            //SetDraggedPosition(pointerData);
            //        }
            //    }
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        if (select)
            //        {
            //            Debug.Log("select");
            //            DeSelect();
            //        }
            //        else
            //        {
            //            select = true;
            //        }

            //    }
            //}
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

        private void DrawTargetLine(PointerEventData data)
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
        #endregion

        #region Public Methods

        //public void Selected()
        //{
        //    Debug.Log("selected " + cardId);
        //    Cursor.visible = false;
        //    isDragging = true;
        //    startPosition = new Vector2(arrowStart.position.x, arrowStart.position.y);
        //    if (cardInfo.Targeting != CardTargeting.None)
        //    {
        //        handController.Targeting(cardId);
        //    }

        //}

        //public void DeSelect()
        //{
        //    Cursor.visible = true;
        //    isDragging = false;
        //    HideArrow();
        //    cardController.CardSelected = null;
        //    if (cardInfo.Targeting != CardTargeting.None)
        //    {
        //        cardController.PlayCard(cardInfo.CardID);
        //    }
        //    else
        //    {
        //        if (transform.position.y > Screen.height / 3)
        //        {
        //            cardController.PlayCard(cardInfo.CardID);
        //        }
        //        else
        //        {
        //            //handController.ReadjustHand();
        //        }
        //    }
        //    handController.ReadjustHand();
        //}


        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("drag");
            Cursor.visible = false;
            isDragging = true;
            startPosition = new Vector2(arrowStart.position.x, arrowStart.position.y);
            handController.SetTagetingZoom(true);
            if (cardInfo.Targeting != CardTargeting.None)
            {
                handController.Targeting(cardId);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (cardPrefab != null)
            {
                if (cardInfo.Targeting != CardTargeting.None)
                {
                    DrawTargetLine(eventData);
                    cardController.CardSelecte(cardId);


                }
                else
                {
                    SetDraggedPosition(eventData);
                }
            }
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            Cursor.visible = true;
            isDragging = false;
            HideArrow();
            cardController.CardSelected = null;
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
                else
                {
                    //handController.ReadjustHand();
                }
            }
            handController.ReadjustHand();
            handController.SetTagetingZoom(false);
        }
        #endregion
    }
}