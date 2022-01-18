#region Using Statements
using FTS.Cards;
using FTS.Characters;
using FTS.Turns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class GameUI : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] GameObject turnCounter;
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] GameObject playerMessagePanel;
    [SerializeField] TextMeshProUGUI playerMessageText;

    [Header("Buttons")]
    [SerializeField] GameObject startCombatButton;
    [SerializeField] GameObject endTurnButton;

    [Header("Energy")]
    [SerializeField] GameObject energy;
    [SerializeField] TextMeshProUGUI energyText;

    [Header("Deck")]
    [SerializeField] GameObject deck;
    [SerializeField] TextMeshProUGUI decksText;
    [SerializeField] GameObject discard;
    [SerializeField] TextMeshProUGUI discardText;
    [SerializeField] GameObject atomized;
    [SerializeField] TextMeshProUGUI atomizedText;

    UnitController unitController;
    TurnController turnController;
    CardController cardController;

    #region MonoBehaviour Callbacks
    void Awake()
    {
        CardController.OnCardPlayed += CardController_OnCardPlayed;
        CardController.OnCardDrawn += CardController_OnCardDrawn;
        CardController.OnCardCreated += CardController_OnCardCreated;
        CardController.OnEnergyChanged += CardController_OnEnergyChanged;
        TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
        TurnController.OnNewTurn += TurnController_OnNewTurn;
        TurnController.OnEndTurn += TurnController_OnEndTurn;
        turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
        unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
    }

    private void Start()
    {
        cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
        UpdateEnergy();
    }

    private void OnDestroy()
    {
        CardController.OnCardPlayed -= CardController_OnCardPlayed;
        CardController.OnCardDrawn -= CardController_OnCardDrawn;
        CardController.OnCardCreated -= CardController_OnCardCreated;
        CardController.OnEnergyChanged -= CardController_OnEnergyChanged;
        TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
        TurnController.OnNewTurn -= TurnController_OnNewTurn;
        TurnController.OnEndTurn -= TurnController_OnEndTurn;
    }
    #endregion

    #region Private Methods
    public void UpdateDeckList()
    {
        decksText.text = cardController.GetCardCountInDeck().ToString();
        discardText.text = cardController.GetCardCountInDiscard().ToString();
        atomizedText.text = cardController.GetCardCountAtomized().ToString();
    }

    void ToggleUI(bool enable)
    {
        energyText.gameObject.SetActive(enable);
        deck.SetActive(enable);
        discard.SetActive(enable);
        atomized.SetActive(enable);
        energy.SetActive(enable);
        endTurnButton.SetActive(enable);
        turnCounter.SetActive(enable);
        playerMessageText.text = "";
    }

    private void UpdateEnergy()
    {
        energyText.text = cardController.Energy + "/" + cardController.TotalEnergy;
    }
    #endregion

    #region Public Methods
    //Button
    public void StartCombat()
    {
        turnController.StartCombat();
        //turnCounter.SetActive(true);
        DisablePlayerInfo();
        Destroy(startCombatButton);
    }

    public void EnablePlayerInfo(string info)
    {
        playerMessagePanel.SetActive(true);
        playerMessageText.text = info;
    }
    internal void DisablePlayerInfo()
    {
        playerMessagePanel.SetActive(false);
        playerMessageText.text = "";
    }
    #endregion

    #region Events
    private void CardController_OnCardDrawn()
    {
        UpdateDeckList();
    }

    private void CardController_OnCardPlayed()
    {
        Debug.Log("Card played?");
        UpdateDeckList();
    }

    private void CardController_OnCardCreated()
    {
        UpdateDeckList();
    }

    private void CardController_OnEnergyChanged()
    {
        UpdateEnergy();
    }


    private void TurnController_OnNewTurn()
    {
        UpdateEnergy();
        UpdateDeckList();
        ToggleUI(true);
        //endTurnButton.SetActive(true);
    }
    private void TurnController_OnEndTurn()
    {
        ToggleUI(true);
        //endTurnButton.SetActive(false);
    }
    private void TurnController_OnEnemyTurn()
    {
        //ToggleUI(false);
        endTurnButton.SetActive(false);
    }
    #endregion
}

