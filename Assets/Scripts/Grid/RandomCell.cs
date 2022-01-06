using UnityEngine;

namespace SP.Grid
{
    public class RandomCell : MonoBehaviour
    {
        [SerializeField] Mesh[] meshList;
        [Header("Chance to find item")]
        [SerializeField] GameObject[] hexList;
        public int[] table = { 100, 80, 70, 20 };
        int totalWeight;
        HexCell hexCell;

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            hexCell = GetComponent<HexCell>();
        }
        #endregion

        #region Public Methods
        public void DropLoot()
        {
            ////randomIndex = Random.Range(0, lootList.Length);
            //foreach (int item in table)
            //{
            //    totalWeight += item;
            //}

            //int itemPicked = Random.Range(0, totalWeight);
            //for (int i = 0; i < table.Length; i++)
            //{
            //    if (itemPicked <= table[i])
            //    {
            //        if (hexList[i] != null)
            //        {
            //            hexCell.DestroyModel();
            //            hexCell.SetModel(hexList[i]);
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        itemPicked -= table[i];
            //    }
            //}
        }
        #endregion
    }
}
