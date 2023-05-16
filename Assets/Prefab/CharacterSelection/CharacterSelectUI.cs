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
using System.Reflection;

namespace FTS.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        public static event System.Action OnReturn = delegate { };

        [SerializeField] AnimatorOverrideController characterSelectAnimator;
        [SerializeField] GameObject orbitSupportDraft;
        PlayerDatabase playerDatabase;
        [SerializeField] Transform playerPosition;
        CharacterStatsUI playerUI;
        int selectedIndex = -1;
        List<PlayerModel> playerModels = new List<PlayerModel>();

        CanvasFader canvasFader;

        void Start()
        {
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            playerUI = GetComponent<CharacterStatsUI>();
            canvasFader = GetComponent<CanvasFader>();
            for (int i = 1; i < 3; i++)
            {
                Debug.Log("player " + (CharacterClass)i);
                InitializeModels(playerDatabase.GetPlayer((CharacterClass)i));
            }
            canvasFader.FadeCanvas(1);
            SelectCharacter(0);
            Time.timeScale = 1.0f;
            MainMenuUIController.OnCharacterSelect += MainMenuUIController_OnCharacterSelect;
        }

        private void OnDestroy()
        {
            MainMenuUIController.OnCharacterSelect -= MainMenuUIController_OnCharacterSelect;
        }

        void InitializeModels(Player player)
        {
            PlayerModel newPlayerModel = new PlayerModel();
            newPlayerModel.model = Instantiate(player.transform.GetChild(player.transform.childCount - 1).gameObject);
            newPlayerModel.model.transform.SetParent(playerPosition, false);
            newPlayerModel.animator = newPlayerModel.model.GetComponent<Animator>();
            newPlayerModel.animator.runtimeAnimatorController = characterSelectAnimator;
            newPlayerModel.characterClass = player.CharacterClass;
            newPlayerModel.player = player;
            playerModels.Add(newPlayerModel);
        }

        void SetModel(int index)
        {
            if(index == selectedIndex)
            {
                return;
            }
            foreach (var player in playerModels)
            {
                player.model.SetActive(false);
            }
            playerModels[index].model.SetActive(true);
            playerModels[index].animator.SetTrigger("Select");
            selectedIndex = index;
            playerUI.UpdateUI(playerModels[selectedIndex].player);
        }

        public void SelectCharacter(int index)
        {
            SetModel(index);
            
        }

        public void Return()
        {
            canvasFader.FadeCanvas(0);
            OnReturn?.Invoke();
        }

        public void Continue()
        {
            playerDatabase.SetPlayer(playerModels[selectedIndex].characterClass);
            RunController.Instance.StartValues();
            SceneController.Instance.LoadScene(Scenes.GameScene);          
        }

        private void MainMenuUIController_OnCharacterSelect()
        {
            canvasFader.FadeCanvas(1);
        }
    }

    struct PlayerModel
    {
        public GameObject model;
        public CharacterClass characterClass;
        public Animator animator;
        public Player player;
    }
}
