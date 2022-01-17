using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SP.Cards;

[CustomEditor(typeof(CardDatabase))]
public class CardDatabaseEditor : Editor
{
    CardDatabase cardDatabase;
    bool fold = true;
    Dictionary<Effect, Editor> effectEditors = new Dictionary<Effect, Editor>();
    //Editor colourEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();



        if (GUILayout.Button("Update Database"))
        {
            cardDatabase.UpdateDatabase();
        }
    }
    private void OnEnable()
    {
        cardDatabase = (CardDatabase)target;
    }
}
