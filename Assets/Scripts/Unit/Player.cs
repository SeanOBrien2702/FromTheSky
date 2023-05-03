#region Using Statements
using UnityEngine;
using FTS.Cards;
using FTS.Saving;
using System.Collections.Generic;
using System;
using FTS.Turns;
using System.Collections;
#endregion

namespace FTS.Characters
{
    public class Player : Character, ISaveable
    {
        [SerializeField] Color colour;
        bool placed = false;

        int energy = 4;
        [SerializeField] int maxEnergy = 4;
        IAbility abilty;

        [Header("Attack animation")]
        [SerializeField] GameObject projectileStart;
        [SerializeField] Projectile projectileAnimation;
        [SerializeField] GameObject piercingAnimation;
        GameObject piercingInstance;

        #region Properties
        public Color Colour   // property
        {
            get { return colour; }   // get method
            set { colour = value; }  // set method
        }

        public bool Placed   // property
        {
            get { return placed; }   // get method
            set { placed = value; }  // set method
        }

        public int Energy   // property
        {
            get { return energy; }   // get method
            set { energy = value;
                unitController.EnergyChanged(this, energy);
            }  // set method
        }

        public int MaxEnergy   // property
        {
            get { return maxEnergy; }   // get method
            set { maxEnergy = value; }  // set method
        }

        public IAbility Abilty { get => abilty; set => abilty = value; }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();
            abilty = GetComponent<IAbility>();
        }

        protected override void Start()
        {
            base.Start();         
            energy = maxEnergy;
            CardController.OnCardPlayed += CardController_OnCardPlayed;
            TurnController.OnEnemySpawn += TurnController_OnEnemySpawn;
        }

        private void OnDestroy()
        {
            CardController.OnCardPlayed += CardController_OnCardPlayed;
            TurnController.OnEnemySpawn -= TurnController_OnEnemySpawn;
        }
        #endregion

        #region Public Methods
        internal int GetCardRange(CardType type)
        {
            int range = 0;
            switch (type)
            {
                case CardType.Attack:
                    range = Stats.GetStat(Stat.AttackRange, CharacterClass);
                    break;
                case CardType.Support:
                    range = Stats.GetStat(Stat.SupportRange, CharacterClass);
                    break;
                case CardType.Summon:
                    range = Stats.GetStat(Stat.SupportRange, CharacterClass);
                    break;
                default:
                    Debug.LogError("Card type missing");
                    break;
            }
            return range;
        }

        public void Attack(Card card)
        {
            if(card.Type != CardType.Attack)
            {
                return;
            }
            StartCoroutine(AttackAnimation(card));          
        }

        #endregion

        IEnumerator AttackAnimation(Card card)
        {
            yield return new WaitUntil(() => Mover.IsRotating == false || Input.GetKeyDown(KeyCode.M));
            animator.SetTrigger("Shoot");
            if (card.Targeting == CardTargeting.Projectile)
            {
                Projectile projectile = Instantiate(projectileAnimation, projectileStart.transform.position, projectileStart.transform.rotation);
                projectile.Card = card;
            }
            else if (card.Targeting == CardTargeting.Piercing)
            {
                Destroy(piercingInstance);
                piercingInstance = Instantiate(piercingAnimation, projectileStart.transform.position, projectileStart.transform.rotation);
                piercingInstance.transform.parent = transform;
                Destroy(piercingInstance, 0.5f);
            }
        }

        #region Saving Methods
        public object CaptureState()
        {
            Debug.Log("save health " + Health + " " + CharacterClass);
            return Health;
        }

        public void RestoreState(object state)
        {
            Health = Convert.ToInt32(state);
            Debug.Log("load health " + Health + " " + CharacterClass);
        }
        #endregion

        #region Events
        private void TurnController_OnEnemySpawn()
        {
            Energy = maxEnergy;
        }

        private void CardController_OnCardPlayed(Card card, Player player)
        {
            if(player == this)
                Attack(card);
        }
        #endregion
    }
}
