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
        public static event System.Action<HexCell, HexCell> OnMoved = delegate { };
        public static event System.Action<int> OnMovementChanged = delegate { };
        CameraController cameraController;
        Character character;
        StateController stateController;
        UnitController unitController;

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
                HexCell oldLocation = location;
                location = value;
                character.Location = value;
                value.Unit = character;
                transform.localPosition = value.transform.localPosition;
                OnMoved?.Invoke(oldLocation, location);   
            }
        }

        public bool CanMove { get => canMove; set => canMove = value; }

        public int Speed { get => speed; set => speed = value; }

        public int MovementLeft { get => movementLeft; set {
                movementLeft = value;
                unitController.MovementChanged(movementLeft);
                OnMovementChanged?.Invoke(movementLeft); 
            } 
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            character = GetComponent<Character>();
            speed = character.Stats.GetStat(Stat.Movement, character.CharacterClass);
            movementLeft = speed;
        }

        void Start()
        {
            cameraController = FindObjectOfType<CameraController>().GetComponent<CameraController>();
            stateController = FindObjectOfType<StateController>().GetComponent<StateController>();          
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
        internal HexCell Travel(List<HexCell> path)
        {
            MovementLeft -= DistanceTraveled(path);
            if (path != null)
            {
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

        internal void Push(HexCell newLocation)
        {
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
            yield return StartCoroutine(cameraController.MoveToPosition(transform.position, false));
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
                    animator.transform.localRotation = Quaternion.LookRotation(d);
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
                animator.transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            transform.localPosition = location.transform.localPosition;
            Location = pathToTravel[pathToTravel.Count - 1];
            pathToTravel = null;
            cameraController.StopCharacterFollow();
            stateController.ActionDone = true;
            Debug.Log("position " + transform.position);
            if (movementLeft >= 1)
                canMove = true;
        }

        IEnumerator RotateToTarget(float targetAngle)
        {
            float time = 0;
            Vector3 targetRotation = new Vector3(0, targetAngle, 0);

            while (time < rotationSpeed)
            {
                animator.transform.eulerAngles = Vector3.Lerp(animator.transform.rotation.eulerAngles, targetRotation, time / rotationSpeed);
                time += Time.deltaTime;
                yield return null;
            }
            animator.transform.eulerAngles = targetRotation;
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
