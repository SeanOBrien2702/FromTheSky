#region Using Statements
using UnityEngine;
using SP.Cards;
#endregion

namespace SP.UI
{
    //[RequireComponent(typeof(Draggable))]
    public class CardZoom : MonoBehaviour
    {
        HandController handController;
        Draggable draggable;
        Vector3 offset = new Vector3(0, 450, 0);
        Quaternion tilt;
        int siblingIndex;
        bool zoomed = false;
        bool reset = false;
        CardTargeting targeting;

        #region MonoBehaviour Callbacks
        void Start()
        {
            handController = FindObjectOfType<HandController>().GetComponent<HandController>();
            draggable = GetComponent<Draggable>();
            targeting = GetComponent<CardUI>().Targeting;
        }

        private void Update()
        {
            if (handController.LERPing && zoomed)
            {
                reset = true;
            }
            if (zoomed && draggable.IsDragging && !(targeting == CardTargeting.None))
            {
                ZoomOut();
            }
        }
        #endregion

        #region Public Methods
        public void ZoomIn()
        {
            if (!handController.LERPing)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1);
                tilt = transform.localRotation;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                siblingIndex = transform.GetSiblingIndex();
                transform.SetAsLastSibling();
                transform.localPosition += offset;
                reset = false;
                zoomed = true;
            }
        }

        public void ZoomOut()
        {
            if (!reset && zoomed)
            {
                transform.localScale = new Vector3(1f, 1f, 1);
                transform.localPosition -= offset;
                transform.localRotation = tilt;
                transform.SetSiblingIndex(siblingIndex);
                zoomed = false;
            }
        }
        #endregion
    }
}