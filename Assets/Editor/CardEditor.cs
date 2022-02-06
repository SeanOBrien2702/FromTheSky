using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FTS.Cards;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    Card card;
    bool fold = true;
    Dictionary<Effect, Editor> effectEditors = new Dictionary<Effect, Editor>();
    //Editor colourEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            foreach (var effect in card.OnDrawEffects)
            {
                if (!effectEditors.ContainsKey(effect))
                {
                    Editor editor = new Editor();
                    effectEditors.Add(effect, editor);
                }

                DrawSettingsEditor(effect, ref effect.effectFoldout, effectEditors[effect]);
            }
            if (card.Effects.Count >= 2)
            {
                fold = EditorGUILayout.InspectorTitlebar(fold, card);
            }
            if (fold)
            {

                foreach (var effect in card.Effects)
                {
                    if (!effectEditors.ContainsKey(effect))
                    {
                        Editor editor = new Editor();
                        effectEditors.Add(effect, editor);
                    }

                    DrawSettingsEditor(effect, ref effect.effectFoldout, effectEditors[effect]);
                }
            }

            foreach (var effect in card.OnDiscardEffects)
            {
                if (!effectEditors.ContainsKey(effect))
                {
                    Editor editor = new Editor();
                    effectEditors.Add(effect, editor);
                }

                DrawSettingsEditor(effect, ref effect.effectFoldout, effectEditors[effect]);
            }

        }
    }
    private void OnEnable()
    {
        card = (Card)target;
    }

    void DrawSettingsEditor(Object settings, ref bool foldout, Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                }
            }
        }
    }
}
