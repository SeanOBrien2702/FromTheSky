using UnityEngine;
using SP.Cards;
using System.Collections.Generic;

namespace SP.Characters
{
    public class PlayerController : Character//, Character
    {

        [SerializeField] List<Card> deck = new List<Card>();
        [SerializeField] Sprite cardBoarder;
        // Start is called before the first frame update
        void Start()
        {

        }


        internal override void StartRound()
        {
            //Debug.Log("player turn");
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectUnit();
            }
        }

        public override Sprite GetBorder()
        {
            return cardBoarder;
        }


        private void SelectUnit()
        {

        }
    }
}
