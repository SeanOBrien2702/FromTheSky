using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Keywords
{
    public static readonly Dictionary<string, string> KeywordTerms = new Dictionary<string, string>
    {
        {"ATOMIZE", "Can only be used once per battle"},
        {"ON DRAW", "This effect happens when the card is drawn"},
        {"ON DISCARD", "This effect happens when the card is discarded"},
        {"ENERGY", "Resource used to play cards"},
        {"ARMOUR", "Reduce damage taken for the turn"},
        {"BARRIER", "Absorb all damage from the next attack"},
        {"TEMPORARY", "Atomized when played or at end of turn"},
        {"FORETELL", "Select one of the top 3 cards the deck. Shuffle then put the select card on top"},
        {"INHERENT", "Starts in your opening hand"},
        {"HEAT", "Deal damage to all units around equal to heat than reduce damage by one"},
    };
}
