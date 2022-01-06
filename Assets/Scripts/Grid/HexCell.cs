﻿#region Using Statements
using SP.Characters;
using SP.Core;
using SP.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#endregion

namespace SP.Grid
{
    public enum HighlightIndex
    {
        DefaultColour,
        CanReachColour,
        CantReachColour,
        HighlightColour
    }


    public class HexCell : MonoBehaviour
    {
        private HexCoordinates location;
        private HexStatus status = HexStatus.Unavailable;
        [SerializeField] bool isObstacle = false;
        [SerializeField] HexCell[] neighbors;
        [SerializeField] Text label;
        [SerializeField] Image highlight;

        [Header("Highlight Colours")]
        [SerializeField] List<Color> highlightColours;
        [SerializeField] GameObject dangerHighlight;
        //[SerializeField] Color canReachColour;
        //[SerializeField] Color cantReachColour;
        //[SerializeField] Color highlightColour;
        Renderer renderer;

        DestinationController destination;
        GameObject hexModel;
        GameObject modelOnHex;
        RandomCell RNGController;
        int dangerIndicator = 0;
        bool isDestination = false;
        bool isSpawn = false;

        int distance = 0;
        int movementCost;
        int searchHeuristic;


        //TODO: fix the publicly facing properties
        [HideInInspector] public HexCell pathFrom;
        public HexCell NextWithSamePriority { get; set; }
        public int SearchPhase { get; set; }
        private Character unit;
        HexUI hexUI;
        Canvas canvas;
        HexMesh hexMesh;
        MeshRenderer meshRenderer;


        #region Properties
        public int SearchPriority
        {
            get { return distance + searchHeuristic; }
        }
        public int MovementCost  // property
        {
            get { return movementCost; }   // get method
            set { movementCost = value; }  // set method
        }
        public int SearchHeuristic  // property
        {
            get { return searchHeuristic; }   // get method
            set { searchHeuristic = value; }  // set method
        }
        public HexCell PathFrom  // property
        {
            get { return pathFrom; }   // get method
            set { pathFrom = value; }  // set method
        }
        public Character Unit  // property
        {
            get { return unit; }   // get method
            set { unit = value;
                CheckDestination();
            }  // set method
        }

        public int Distance  // property
        {
            get { return distance; }   // get method
            set { distance = value; }  // set method
        }

        public HexStatus Status  // property
        {
            get { return status; }   // get method
            set
            {
                status = value;
                if (status != HexStatus.Unavailable)
                {
                    isObstacle = true;
                }
            }  // set method
        }

        public HexCoordinates Location  // property
        {
            get { return location; }   // get method
            set { location = value; }  // set method
        }

        public bool IsObstacle // property
        {
            get { return isObstacle; }   // get method
            set { isObstacle = value; }  // set method
        }

        public bool IsDestination // property
        {
            get { return isDestination; }   // get method
            set { isDestination = value; }  // set method
        }

        public bool IsSpawn // property
        {
            get { return isSpawn; }   // get method
            set { isSpawn = value; }  // set method
        }

        public int DangerIndicator // property
        {
            get { return dangerIndicator; }   // get method
            set { dangerIndicator = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            hexMesh = GetComponent<HexMesh>();
            meshRenderer = GetComponent<MeshRenderer>();
            RNGController = GetComponent<RandomCell>();
            destination= FindObjectOfType<DestinationController>().GetComponent<DestinationController>();
        }
        #endregion

        #region Private Methods
        private void DestroyModelsOnHex()
        {
            Transform child = gameObject.transform.GetChild(0);
            foreach (Transform grandchild in child)
            {
                //if (child.GetComponent<Canvas>() != null)
                //Destroy(grandchild.gameObject);
            }
        }

        private void CheckDestination()
        {
            if (unit &&
                unit.CharacterClass == CharacterClass.Vehicle &&
                IsDestination)
            {
                destination.ReachedDestination(location);
            }
        }
        #endregion

        #region Public Methods
        public void UpdateDistanceLabel()
        {
            //Text label = uiRect.GetComponent<Text>();
            //Debug.Log("Update distance");
            label.text = distance == int.MaxValue ? "" : distance.ToString();
        }

        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighbors[(int)direction];
        }

        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        }

        public void SetMeshMaterial(Material material)
        {

            hexMesh.Triangulate(this);
            meshRenderer.material = material;

            //DestroyModel();
            //hexModel = Instantiate(gameObject);
            //hexModel.transform.SetParent(transform, false);
            //renderer = hexModel.GetComponent<Renderer>();
        }

        public void SetModel(GameObject gameObject)
        {
            DestroyModel();
            hexModel = Instantiate(gameObject);
            hexModel.transform.SetParent(transform, false);
            renderer = hexModel.GetComponent<Renderer>();
        }

        public void SetModel()
        {
            RNGController.DropLoot();
        }

        public void SetModelOnHex(GameObject gameObject)
        {
            DestroyModelsOnHex();
            hexModel = Instantiate(gameObject);
            hexModel.transform.SetParent(transform.GetChild(0), false);
        }

        public void DestroyModel()
        {
            foreach (Transform child in this.transform)
            {
                //if(child.GetComponent<Canvas>() != null)
                //Destroy(child.gameObject);
            }
        }

        public void SetDangerIndicator(bool danger)
        {
            if (danger)
            {
                ++dangerIndicator;
            }
            else
            {
                if (dangerIndicator > 0)
                {
                    --dangerIndicator;
                }
            }
            if (dangerIndicator > 0)
            {
                //renderer.material.color = highlightColours[(int)HighlightIndex.CantReachColour];//Color.red;
                dangerHighlight.SetActive(true);
            }
            else
            {
                //renderer.material.color = highlightColours[(int)HighlightIndex.CanReachColour];
                dangerHighlight.SetActive(false);
            }
        }

        public void SetColour(Color color)
        {
            renderer.material.color = color;
        }

        public void SetHighlight(HighlightIndex highlightIndex, bool color)
        {
            if (highlightIndex != null)
            {
                highlight.color = highlightColours[(int)highlightIndex];
                highlight.enabled = true;
            }
            else
            {
                highlight.color = highlightColours[(int)HighlightIndex.CantReachColour];
            }
        }

        public void SetHighlight(HighlightIndex highlightIndex)//Color color)
        {
            if (highlightIndex != null)
            {
                highlight.color = highlightColours[(int)highlightIndex];
                highlight.enabled = true;
            }
            else
            {
                highlight.color = highlightColours[(int)HighlightIndex.CantReachColour];
            }
        }

        internal void Unavailable()
        {
            Debug.Log("unavailable");
        }

        public void SearchConplete()
        {
            RNGController.DropLoot();
            Debug.Log("Search complete");
        }

        public void ParseName()
        {
            string[] buffer;
            Transform allChildren = GetComponentInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                buffer = child.name.Split('(');
                this.name = buffer[0];
            }
        }

        public string GetName()
        {
            return transform.GetChild(0).name;
        }

        internal void SetLabel(int turns)
        {
            label.text = turns == 0 ? "" : turns.ToString();
        }
        #endregion
    }
}