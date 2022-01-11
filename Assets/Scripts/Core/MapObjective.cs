using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SP.Core
{
    public class MapObjective : MonoBehaviour
    {
        [SerializeField] Image image;
        ObjectiveController objectiveController;
        List<Objective> objectives = new List<Objective>();
        Vector3 position;
        [SerializeField] Sprite sprite;
        int DangerLevel;

        public List<Objective> Objectives
        {
            get { return objectives; }   
            set { objectives = value; }  
        }

        public Vector3 Position { get => position; set => position = value; }

        bool overPlanet = false;

        private void Awake()
        {
            objectiveController = FindObjectOfType<ObjectiveController>().GetComponent<ObjectiveController>();
        }

        void Start()
        {
            objectives.AddRange(objectiveController.GetRandomObjectives());
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
            //objectiveController.SetObjective(objectives);          
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
