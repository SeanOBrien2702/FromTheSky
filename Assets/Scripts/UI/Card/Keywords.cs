using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Keywords
{
    public static readonly Dictionary<string, string> KeywordTerms = new Dictionary<string, string>
    {
        {"atomize", "Can only be used once per battle"},
        {"on draw", "This effect happens when the card is drawn"},
        {"on discard", "This effect happens when the card is discarded"},
        {"energy", "Resource used to play cards"},
        {"armour", "Reduce damage taken for the turn"},
        {"barrier", "Absorb all damage from the next attack"},
        {"temporary", "Atomized when played or at end of turn"},
        {"foretell", "Select one of the top 3 cards the deck. Shuffle then put the select card on top"},
        {"inherent", "Starts off in your opening hand"},
        {"heat", "Deal damage to all units around equal to heat than reduce damage by one"},
    };
}
