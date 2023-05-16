#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.UI;
using System;
using UnityEngine.EventSystems;
#endregion

namespace FTS.UI
{
    public class CharacterSelectCamera : MonoBehaviour
    {
        [Header("Zoom Settings")]
        [SerializeField] Transform startPosition;
        [SerializeField] Transform endPosition;
        [SerializeField] float duration = 1f;
        
        bool isLerping = false;

        #region Properties

        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            CharacterSelectUI.OnReturn += CharacterSelectUI_OnReturn;
        }

        private void OnDestroy()
        {
            CharacterSelectUI.OnReturn -= CharacterSelectUI_OnReturn;
        }
        #endregion

        #region Private Methods
        public void MoveToMenu()
        {
            if (!isLerping)
            {
                StartCoroutine(LerpCamera(startPosition.position,
                                          startPosition.rotation));
            }
        }

        public void MoveToCharacterSelect()
        {
            if (!isLerping)
            {
                StartCoroutine(LerpCamera(endPosition.position,
                                          endPosition.rotation));
            }
        }
        #endregion

        #region Public Methods

        #endregion

        #region Coroutines
        IEnumerator LerpCamera(Vector3 targetPosition, Quaternion targetRotation)
        {
            isLerping = true;
            float time = 0;
            Vector3 startPosition = gameObject.transform.localPosition;
            Quaternion startRotation = gameObject.transform.rotation;

            while (time < duration)
            {
                gameObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                gameObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            gameObject.transform.localPosition = targetPosition;
            gameObject.transform.rotation = targetRotation;

            isLerping = false;
        }   
        #endregion

        #region Events
        private void CharacterSelectUI_OnReturn()
        {
            MoveToMenu();
        }
        #endregion
    }
}