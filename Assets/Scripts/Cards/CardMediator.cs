using FTS.Cards;
using FTS.Grid;

public interface ICardMediator
{
    void HexSelected(HexCell hex);
    void CardPlayed(Card card);

}
