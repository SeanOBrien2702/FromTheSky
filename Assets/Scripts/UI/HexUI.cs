#region Using Statements
using FTS.Grid;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace FTS.UI
{
    public class HexUI : MonoBehaviour
    {
        [SerializeField] Text hexName;
        [SerializeField] Text hexCoord;

        HexCell curCell;
        Canvas canvas;

        #region MonoBehaviour Callbacks
        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1))
                canvas.enabled = false;
        }
        #endregion

        #region Public Methods
        public void ToggleUI(HexCell hexCell)
        {

            //transform.gameObject.SetActive(true);
            if (!canvas.enabled)
            {
                canvas.enabled = true;
                hexName.text = hexCell.GetName();
                hexCoord.text = hexCell.Location.ToString();
                curCell = hexCell;
            }
            else
            {
                canvas.enabled = false;
            }
        }
        public void Close()
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

        public void Build(GameObject gameObject)
        {

            if (curCell != null)
            {
                //curCell.BuildOnHex(gameObject);
                Debug.Log(curCell.name);
            }
            else
            {
                Debug.LogError("Hex cell not set");
            }
        }
        #endregion
    }
}
