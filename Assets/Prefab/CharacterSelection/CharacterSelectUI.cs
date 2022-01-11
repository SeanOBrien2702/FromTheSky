using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SP.Characters;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.EventSystems;

namespace SP.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController characterSelectAnimator; 
        int numPlayerChar = 3;
        PlayerDatabase playerDatabase;
        [SerializeField] Transform[] playerPositions;
        [SerializeField] CharacterStatsUI[] playerUI;
        List<PlayerModel> playerModels = new List<PlayerModel>();
        [SerializeField] Button continueButton;

        // Start is called before the first frame update
        void Start()
        {
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            
        }

        void AddPlayer(Player player)
        {
            //Debug.Log("Add player " + player.CharacterClass);
            //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
            if (playerModels.Count < numPlayerChar)
            {
                PlayerModel playerModel;
                playerModel.position = FindFirstFreePosition();
                playerModel.characterClass = player.CharacterClass;

                playerModel.model = Instantiate(player.transform.GetChild(0).gameObject);
                playerModel.model.transform.SetParent(playerPositions[playerModel.position], false);
                playerModels.Add(playerModel);

                playerUI[playerModel.position].gameObject.SetActive(true);
                playerUI[playerModel.position].UpdateUI(player);
                Animator animator = playerModel.model.GetComponent<Animator>();
                animator.runtimeAnimatorController = characterSelectAnimator;
            }

            if (playerModels.Count >= numPlayerChar)
            {
                continueButton.interactable = true;
            }
        }

        private int FindFirstFreePosition()
        {
            int freePosition = 0;
            foreach (var position in playerPositions)
            {
                if (position.childCount < 1)
                {
                    break;
                }
                freePosition++;
            }
            return freePosition;
        }

        void RemovePlayer(Player player)
        {
            Debug.Log("remove player " + player.CharacterClass);
            PlayerModel playerModel = playerModels.Find(item => item.characterClass == player.CharacterClass);
            playerUI[playerModel.position].gameObject.SetActive(false);
            Destroy(playerModel.model);
            playerModels.Remove(playerModel);
            continueButton.interactable = false;
        }

        public void SelectCharacter(int classIndex)
        {
            Player player = playerDatabase.GetPlayer((CharacterClass)classIndex);

            if(playerModels.Any(item => item.characterClass == player.CharacterClass))
            {
                RemovePlayer(player);
            }
            else
            {
                AddPlayer(player);
            }
            
        }

        public void Continue()
        {
            List<CharacterClass> players = new List<CharacterClass>();
            foreach (var item in playerModels)
            {
                players.Add(item.characterClass);
            }
            playerDatabase.SetPlayers(players);
            SceneManager.LoadScene("HubScene");
        }

    }

    struct PlayerModel
    {
        public GameObject model;
        public int position;
        public CharacterClass characterClass;
    }
}
