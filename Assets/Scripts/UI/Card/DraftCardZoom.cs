#region Using Statements
using UnityEngine;
#endregion

namespace SP.UI
{
    public class DraftCardZoom : MonoBehaviour
    {
        [SerializeField] GameObject cardPrefab;
        Transform draftHUD;

        #region MonoBehaviour Callbacks
        void Start()
        {
            draftHUD = FindObjectOfType<DraftUI>().GetComponent<Transform>();
        }
        #endregion

        #region Public Methods
        public void ZoomIn()
        {
            cardPrefab.SetActive(true);
            cardPrefab.transform.SetParent(draftHUD);
            cardPrefab.transform.SetAsLastSibling();
        }

        public void ZoomOut()
        {
            cardPrefab.SetActive(false);
        }
        #endregion
    }
}