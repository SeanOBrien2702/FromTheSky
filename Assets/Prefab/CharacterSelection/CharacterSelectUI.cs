using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTS.Characters;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using FTS.Core;

namespace FTS.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController characterSelectAnimator;
        [SerializeField] GameObject orbitSupportDraft;
        PlayerDatabase playerDatabase;
        [SerializeField] Transform playerPosition;
        CharacterStatsUI playerUI;
        PlayerModel playerModel = new PlayerModel();

        void Start()
        {
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            playerUI = GetComponent<CharacterStatsUI>();
            AddPlayer(playerDatabase.GetPlayer((CharacterClass)1));
            Time.timeScale = 1.0f;
        }

        void AddPlayer(Player player)
        {
            playerModel.characterClass = player.CharacterClass;

            playerModel.model = Instantiate(player.transform.GetChild(0).gameObject);
            playerModel.model.transform.SetParent(playerPosition, false);

            playerUI.gameObject.SetActive(true);
            playerUI.UpdateUI(player);
            Animator animator = playerModel.model.GetComponent<Animator>();
            animator.runtimeAnimatorController = characterSelectAnimator;
        }

        void ChangePlayer(Player player)
        {
            PlayerModel newPlayerModel = new PlayerModel();
            Destroy(playerModel.model);
            playerUI.UpdateUI(player);

            newPlayerModel.characterClass = player.CharacterClass;
            newPlayerModel.model = Instantiate(player.transform.GetChild(0).gameObject);
            newPlayerModel.model.transform.SetParent(playerPosition, false);

            playerModel = newPlayerModel;
        }

        public void SelectCharacter(int index)
        {
            Player player = playerDatabase.GetPlayer((CharacterClass)index);
            ChangePlayer(player);
        }

        public void Continue()
        {
            playerDatabase.SetPlayer(playerModel.characterClass);
            SceneController.Instance.LoadScene(Scenes.HubScene);          
        }
    }

    struct PlayerModel
    {
        public GameObject model;
        public CharacterClass characterClass;
    }
}
