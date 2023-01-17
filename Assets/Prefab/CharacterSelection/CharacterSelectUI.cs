using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTS.Characters;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using System.Security.Cryptography;

namespace FTS.UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController characterSelectAnimator; 
        int numPlayerChar = 2;
        PlayerDatabase playerDatabase;
        [SerializeField] Transform[] playerPositions;
        [SerializeField] CharacterStatsUI[] playerUI;
        List<PlayerModel> playerModels = new List<PlayerModel>();
        int numClasses = Enum.GetValues(typeof(CharacterClass)).Length - 1;

        void Start()
        {
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            Player startingPlayer;
            for (int i = 1; i <= numPlayerChar; i++)
            {
                startingPlayer = playerDatabase.GetPlayer((CharacterClass)i);
                AddPlayer(startingPlayer, i - 1);
            }
        }

        void AddPlayer(Player player, int position)
        {
            PlayerModel playerModel;
            playerModel.position = position;
            playerModel.characterClass = player.CharacterClass;

            playerModel.model = Instantiate(player.transform.GetChild(0).gameObject);
            playerModel.model.transform.SetParent(playerPositions[playerModel.position], false);
            playerModels.Add(playerModel);

            playerUI[playerModel.position].gameObject.SetActive(true);
            playerUI[playerModel.position].UpdateUI(player);
            Animator animator = playerModel.model.GetComponent<Animator>();
            animator.runtimeAnimatorController = characterSelectAnimator;
        }

        void ChangePlayer(Player player, int position)
        {
            PlayerModel playerModel = playerModels.Find(item => item.position == position);
            Destroy(playerModel.model);
            playerUI[playerModel.position].UpdateUI(player);

            playerModel.characterClass = player.CharacterClass;
            playerModel.model = Instantiate(player.transform.GetChild(0).gameObject);
            playerModel.model.transform.SetParent(playerPositions[playerModel.position], false);

            playerModels[position] = playerModel;
        }

        private void SwitchCharacter(int position, int direction)
        {
            PlayerModel playerModel = playerModels.Find(item => item.position == position);
            ChangePlayer(GetPlayerClass(playerModel, direction), position);
        }

        private Player GetPlayerClass(PlayerModel playerModel, int direction)
        {
            int charClass = (int)playerModel.characterClass;
            
            charClass += direction;
            if(IsAlreadySelected(playerModel, direction))
            {
                charClass += direction;
            }

            if(charClass < 1)
            {
                charClass = numClasses;
            }
            else if(charClass >= numClasses)
            {
                charClass = 1;
            }
            Debug.Log(charClass);
            return playerDatabase.GetPlayer((CharacterClass)charClass);
        }

        private bool IsAlreadySelected(PlayerModel playerModel, int direction)
        {
            bool isSelected = false;
            int position = playerModel.position;
            PlayerModel otherModel = playerModels.First(item => item.position != playerModel.position);

            if (otherModel.characterClass == playerModel.characterClass + direction)
            {
                isSelected = true;
            }

            return isSelected;
        }

        public void PreviousCharacter(int position)
        {
            SwitchCharacter(position, -1);
        }

        public void NextCharacter(int position)
        {
            SwitchCharacter(position, 1);
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
