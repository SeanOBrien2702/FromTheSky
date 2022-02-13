using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FTS.Core
{
    public class MapObjective : MonoBehaviour
    {
        [SerializeField] Image image;
        ObjectiveDatabase objectiveController;
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] Sprite sprite;
        [SerializeField] Transform planet;
        bool overPlanet = false;

        public List<Objective> Objectives
        {
            get { return objectives; }   
            set { objectives = value; }  
        }

        void Start()
        {
            objectiveController = FindObjectOfType<ObjectiveDatabase>().GetComponent<ObjectiveDatabase>();
            transform.LookAt(planet);
            image.sprite = sprite;
        }

        // Update is called once per frame
        void Update()
        {
            if (overPlanet && Input.GetMouseButton(0))
            {
                objectiveController.SetObjective(objectives);
                SceneManager.LoadScene("GameScene");
            }
        }

        public void SetObjectives()
        {
            objectiveController.SetObjective(objectives);          
        }

        private void OnMouseEnter()
        {
            overPlanet = true;
        }

        private void OnMouseExit()
        {
            overPlanet = false;
        }
    }
}
