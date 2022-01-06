#region Using Statements
using System.Collections.Generic;
using UnityEngine;
using SP.Cards;
using System.Linq;
#endregion

namespace SP.Characters
{
    public class PlayerDatabase : MonoBehaviour
    {
        List<Player> players = new List<Player>();
        [SerializeField] List<Player> playerPrefabs = new List<Player>();

        #region Properties

        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            //players.Add(medicPrefab);
            //players.Add(scoutPrefab);
            //players.Add(tankPrefab);
        }
        #endregion

        #region Public Methods
        public void SetPlayers(List<CharacterClass> selectedClass)
        {
            foreach (var selected in selectedClass)
            {
                players.Add(playerPrefabs.Find(player => player.CharacterClass == selected));
            }
        }

        internal List<Player> GetPlayers()
        {
            return players;
        }

        public Player GetPlayer(CharacterClass characterClass)
        {
            return playerPrefabs.Find(item => item.CharacterClass == characterClass);
        }

        internal CharacterClass[] GetPlayerClasses()
        {
            CharacterClass[] classList = new CharacterClass[players.Count];
            int index = 0;
            foreach (var item in players)
            {
                if (item.GetComponent<Player>() != null)
                {
                    classList[index] = item.CharacterClass;
                    ++index;
                }
            }
            return classList;
        }

        internal Player GetUnplacedCharacter(List<Player> placedPlayers)
        {
            return players.Where(item => !placedPlayers
                          .Any(item2 => item2.CharacterClass == item.CharacterClass))
                          .FirstOrDefault();
        }
        #endregion
    }
}