#region Using Statements
using FTS.Grid;
using FTS.Turns;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    [RequireComponent(typeof(Character))]
    public class Mover : MonoBehaviour
    {

        HexGridController gridController;
        CameraController cameraController;
        Character character;
        StateController stateController;

        [SerializeField] float travelSpeed = 2f;
        [SerializeField] float pushSpeed = 0.1f;
        float rotationSpeed = 0.5f;
        [SerializeField] Animator animator;

        [SerializeField] SFXGroup movementSounds;

        HexCell location;
        List<HexCell> pathToTravel;

        bool canMove = true;
        int speed;
        int movementLeft;

        #region Properties
        public HexCell Location
        {
            get { return location; }
            set
            {
                if (location)
                {
                    location.Unit = null;
                }
                //gridController.UpdateIndicators(location, value);
                location = value;
                character.Location = value;
                value.Unit = character;
                transform.localPosition = value.transform.localPosition;
                    
            }
        }

        public bool CanMove { get => canMove; set => canMove = value; }

        public int Speed { get => speed; set => speed = value; }

        public int MovementLeft { get => movementLeft; set => movementLeft = value; }
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            character = GetComponent<Character>();
        }

        void Start()
        {
            cameraController = FindObjectOfType<CameraController>().GetComponent<CameraController>();
            stateController = FindObjectOfType<StateController>().GetComponent<StateController>();   
            gridController = FindObjectOfType<HexGridController>().GetComponent<HexGridController>();
            speed = character.Stats.GetStat(Stat.Movement, character.CharacterClass);
            movementLeft = speed;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
        }

        private void OnDestroy()
        {
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
        }
        #endregion

        #region Private Methods
        private int DistanceTraveled(List<HexCell> path)
        {
            int distance = 0;
            if (path != null)
            {
                foreach (HexCell cell in path.Skip(1))
                {
                    distance += cell.MovementCost;
                }
            }
            return distance;
        }
        #endregion

        #region Public Methods
        internal HexCell Travel(List<HexCell> path)//, Vector3 lookTowards)
        {
            movementLeft -= DistanceTraveled(path);
           
            if (path != null)
            {
                Location = path[path.Count - 1];
                pathToTravel = path;
                StopAllCoroutines();
                StartCoroutine(TravelPath(Vector3.zero));
            }
            else
            {
                Debug.LogWarning("path not found");
                stateController.ActionDone = true;
            }
            return Location;
        }

        internal bool IsValidDestination(HexCell cell)
        {
            return !cell.Unit;
        }

        internal void Push(HexDirection direction)
        {
            HexCell newLocation = Location.GetNeighbor(direction);
            StartCoroutine(TravelPush(newLocation.transform.localPosition));
            Location = newLocation;
        }

        public void LookAt(int angle)
        {
            StartCoroutine(RotateToTarget(angle));
        }

        public void LookAt(HexDirection direction)
        {
            int angle = HexDirectionExtensions.Angle(direction);
            StartCoroutine(RotateToTarget(angle));
        }
        #endregion

        #region Coroutines
        IEnumerator TravelPush(Vector3 targetPosition)
        {           
            float time = 0;
            Vector3 startPosition = transform.position;
            Debug.Log("Push?" + startPosition + "  " + targetPosition);
            while (time < pushSpeed)
            {
                float t = time / pushSpeed;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            Debug.Log("Push?" + startPosition + "  " + targetPosition);
        }

        IEnumerator TravelPath(Vector3 lookTowards)
        {
            SFXManager.Main.Play(movementSounds);
            canMove = false;
            cameraController.StartCharacterFollow(this.transform);
            Vector3 a, b, c = pathToTravel[0].transform.localPosition;
            transform.localPosition = c;
            if(animator != null)
            {
                animator.SetTrigger("Walk");
            }
            float time = Time.deltaTime * travelSpeed;
            for (int i = 1; i < pathToTravel.Count; i++)
            {
                a = c;
                b = pathToTravel[i - 1].transform.localPosition;
                c = (b + pathToTravel[i].transform.localPosition) * 0.5f;
                for (; time < 1f; time += Time.deltaTime * travelSpeed)
                {
                    transform.localPosition = Bezier.GetPoint(a, b, c, time);
                    Vector3 d = Bezier.GetDerivative(a, b, c, time);
                    d.y = 0f;
                    transform.localRotation = Quaternion.LookRotation(d);
                    yield return null;
                }
                time -= 1f;
            }
            if (animator != null)
            {
                animator.SetTrigger("Stop");
            }
            a = c;
            b = pathToTravel[pathToTravel.Count - 1].transform.localPosition;
            c = b;
            for (; time < 1f; time += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, time);
                Vector3 d = Bezier.GetDerivative(a, b, c, time);
                d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            transform.localPosition = location.transform.localPosition;
            pathToTravel = null;

            cameraController.StopCharacterFollow();
            stateController.ActionDone = true;
  
            if (movementLeft >= 1)
                canMove = true;
        }

        IEnumerator RotateToTarget(float targetAngle)
        {
            float time = 0;
            Vector3 targetRotation = new Vector3(0, targetAngle, 0);

            while (time < rotationSpeed)
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetRotation, time / rotationSpeed);
                time += Time.deltaTime;
                yield return null;
            }
            transform.eulerAngles = targetRotation;
            stateController.ActionDone = true;
        }
       
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            canMove = true;
            movementLeft = speed;
        }

        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            canMove = true;
            movementLeft = speed;
        }
        #endregion
    }
}
