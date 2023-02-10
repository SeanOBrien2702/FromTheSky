using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTS.Characters;
using UnityEngine.EventSystems;
using System;

namespace FTS.UI
{
    public class CharacterInfo : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Text health;
        [SerializeField] Text movement;

        Canvas canvas;

        // Start is called before the first frame update
        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            { 
                if (IsOverCharacter() || IsOverAcceptableUI())
                {
                    canvas.enabled = true;
                }
                else
                {
                    canvas.enabled = false;
                }
            }
        }

        private bool IsOverCharacter()
        {
            bool enableUI = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (var item in hits)
            {
                if (item.transform.tag == "Player")
                {
                    enableUI = true;
                    break;
                }
            }
            return enableUI;
        }
        private bool IsOverAcceptableUI()
        {
            bool enableUI = false;
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (var item in raycastResults)
            {
                if (item.gameObject.tag == "Card")
                {
                    enableUI = true;
                    break;
                }
            }
            return enableUI;
        }

        public void UpdateUI(Unit character)
        {
            Mover mover = character.GetComponent<Mover>();
            health.text = "Health: " + character.Health + "/" + character.MaxHealth;
            movement.text = "Movement: " + mover.MovementLeft + "/" + mover.Speed;
            //image.sprite = character.Picture;
        }
        public void UpdateUI(Player player)
        {
           
            Mover mover = player.GetComponent<Mover>();
            health.text = "Health: " + player.Health + "/" + player.MaxHealth;
            movement.text = "Movement: " + mover.MovementLeft + "/" + mover.Speed;
            //image.sprite = player.Picture;
        }

        public void ClearUI()
        {

        }

        public void EnableUI(Unit character)
        {
            canvas.enabled = true;
            UpdateUI(character);
        }

        public void DisableUI()
        {
            canvas.enabled = false;
        }

    }

}
