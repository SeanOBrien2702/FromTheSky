//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEditor.UIElements;

//namespace SP.Cards
//{
//    #if UNITY_EDITOR
//    public class EffectEditor : VisualElement
//    {
//        Effect effect;
//        CardEditor cardEditor;

//        public EffectEditor(CardEditor cardEditor, Effect effect)
//        {
//            this.cardEditor = cardEditor;
//            this.effect = effect;

//            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Cards/CardEditor/EffectEditor.uxml");
//            visualTree.CloneTree(this);

//            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Cards/CardEditor/EffectEditor.uss");
//            this.styleSheets.Add(stylesheet);

//            this.AddToClassList("effectEditor");
//            #region Fields
//            Label nameLbl = this.Query<Label>("effectName").First();
//            nameLbl.text = effect.name;
//            #endregion  

//            #region Buttons
//            Button btnAddEffect = this.Query<Button>("btnRemove").First();
//            btnAddEffect.clickable.clicked += RemoveEffect;
//            #endregion
//        }

//        #region Button Functions
//        private void RemoveEffect()
//        {
//            if (EditorUtility.DisplayDialog("Delete Effect", "Are you sure you want to delete this effect?", "Delete", "Cancel"))
//                cardEditor.RemoveEffect(effect);
//        }
//        #endregion
//    }
//    #endif
//}