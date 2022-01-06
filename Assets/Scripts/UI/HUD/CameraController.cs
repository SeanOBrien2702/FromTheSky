#region Using Statements
using SP.Characters;
using SP.Grid;
using System;
using System.Collections;
using UnityEngine;
#endregion

public class CameraController : MonoBehaviour
{
    [SerializeField] HexGrid grid;
    Vector3 panLimit;

    private float moveSpeed;
    bool isEnabled = true;
    bool isFollowing = false;
    Transform currentCharacter;

    [Header("Movement Settings")]
    [SerializeField] float normalMoveSpeed;
    [SerializeField] float fastMoveSpeed;

    [Header("Rotation Settings")]
    [SerializeField] float rotateSpeed;

    [Header("Zoom Settings")]
    [SerializeField] float zoomSpeed;
    [SerializeField] float minZoomDist;
    [SerializeField] float maxZoomDist;

    private Camera cam;
    private Vector3 dir;
    private Vector3 newPos;
    private Vector3 startPos = new Vector3(100, 0 , 75);

    #region Properties
    public bool IsEnabled   // property
    {
        get { return isEnabled; }   // get method
        set { isEnabled = value; }  // set method
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        cam = Camera.main;
        newPos = grid.VehicleStart.transform.position;
        panLimit = grid.GetLastCellPosition();
    }

    void Update()
    {
        if (isEnabled)
        {
            Move();
            Zoom();
            Rotate();
            Pan();
        }
        else if (isFollowing)
        {
            newPos = currentCharacter.position;
            Move();
        }
        else
        {
            newPos = transform.position;
        }
    }
    #endregion

    #region Private Methods
    private void Rotate()
    {
        if (Input.GetKey("e"))
        {
            transform.Rotate(0, -rotateSpeed, 0);
        }

        if (Input.GetKey("q"))
        {
            transform.Rotate(0, rotateSpeed, 0);
        }
    }

    private void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        if (dist < minZoomDist && scrollInput > 0.0f)
        {
            return;
        }
        else if (dist > maxZoomDist && scrollInput < 0.0f)
        {
            return;
        }
        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }

    private void Move()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fastMoveSpeed;
        }
        else
        {
            moveSpeed = normalMoveSpeed;
        }
        dir = transform.forward * zInput + transform.right * xInput;
        newPos += dir * moveSpeed * Time.unscaledDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, 0, panLimit.x);
        newPos.z = Mathf.Clamp(newPos.z, 0, panLimit.z);
        transform.position = newPos;
    }
    private void Pan()
    {
    }
    #endregion

    #region Public Methods
    internal void StartCharacterFollow(Transform mover)
    {
        //Debug.Log("start following");
        currentCharacter = mover;
        isFollowing = true;
        isEnabled = false;
    }
    internal void StopCharacterFollow()
    {
        //Debug.Log("stop following");
        currentCharacter = null;
        isFollowing = false;
        isEnabled = true;
    }
    #endregion

    #region Coroutines
    public IEnumerator MoveToPosition(Vector3 moveTo)
    {
        float duration = 0.5f;
        float time = 0;
        Vector3 startPosition = transform.position;
        isEnabled = false;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, moveTo, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        isEnabled = true;
    }
    #endregion
}
