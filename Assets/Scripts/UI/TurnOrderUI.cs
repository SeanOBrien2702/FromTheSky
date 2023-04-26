using FTS.Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTS.UI
{
    public class TurnOrderUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static event System.Action<bool> OnHover = delegate { };
        [SerializeField] Sprite showSprite;
        [SerializeField] Sprite hideSprite;
        Image image;
        
        void Start()
        {
            image = GetComponent<Image>();
            image.sprite = hideSprite;
        }

        void ShowTurnOrder()
        {
            image.sprite = showSprite;
            OnHover?.Invoke(true);
        }    

        void HideTurnOrder()
        {
            image.sprite = hideSprite;
            OnHover?.Invoke(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTurnOrder();        
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTurnOrder();
        }
    }
}
