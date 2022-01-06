using SP.Cards;
using SP.Grid;

public interface ICardMediator
{
    void HexSelected(HexCell hex);
    void CardPlayed(Card card);

}
