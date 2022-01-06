//#region Using Statements
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEditor.UIElements;
//using SP.Characters;
//#endregion


//namespace SP.Cards
//{
//    #if UNITY_EDITOR
//    [CustomEditor(typeof(Card))]
//    public class CardEditor : Editor
//    {
//        private Card card;
//        private VisualElement rootElement;
//        private VisualElement effectList;
//        EnumField effectField;


//        #region MonoBehaviour Callbacks
//        public void OnEnable()
//        {
//            card = (Card)target;
//            rootElement = new VisualElement();

//            // Load in UXML template and USS styles, then apply them to the root element.
//            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Cards/CardEditor/CardEditor.uxml");
//            visualTree.CloneTree(rootElement);

//            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Cards/CardEditor/CardEditor.uss");
//            rootElement.styleSheets.Add(stylesheet);
//        }

//        public override VisualElement CreateInspectorGUI()
//        {          
//            #region Fields
//            // Find the visual element with the name "systemSprite" and make it display the star system sprite if it has one.
//            VisualElement systemSprite = rootElement.Query<VisualElement>("systemSprite").First();
//            systemSprite.style.backgroundImage = card.Image ? card.Image.texture : null;

//            // Find an object field with the name "systemSpriteField", set that it only accepts objects of type Sprite,
//            // set its initial value and register a callback that will occur if the value of the filed changes.
//            ObjectField spriteField = rootElement.Query<ObjectField>("systemSpriteField").First();
//            spriteField.objectType = typeof(Sprite);
//            spriteField.value = card.Image;
//            spriteField.RegisterCallback<ChangeEvent<Object>>(
//                e =>
//                {
//                    card.Image = (Sprite)e.newValue;
//                    systemSprite.style.backgroundImage = card.Image.texture;
//                // Set card as being dirty. This tells the editor that there have been changes made to the asset and that it requires a save. 
//                EditorUtility.SetDirty(card);
//                }
//            );

//            Label nameLbl = rootElement.Query<Label>("cardName").First();
//            nameLbl.text = card.name;
//            card.CardName = card.name;

//            IntegerField costField = rootElement.Query<IntegerField>("cardCost").First();
//            costField.value = card.Cost;
//            costField.RegisterCallback<ChangeEvent<int>>(
//                e => {
//                    card.Cost = e.newValue;
//                    EditorUtility.SetDirty(card);
//                }
//            );

//            IntegerField rangeField = rootElement.Query<IntegerField>("cardRange").First();
//            rangeField.value = card.Range;
//            rangeField.RegisterCallback<ChangeEvent<int>>(
//                e => {
//                    card.Range = e.newValue;
//                    EditorUtility.SetDirty(card);
//                }
//            );

//            Toggle atomizeFeild = rootElement.Query<Toggle>("cardAtomize").First();
//            atomizeFeild.value = card.IsAtomize;
//            atomizeFeild.RegisterCallback<ChangeEvent<bool>>(
//                e => {
//                    card.IsAtomize = e.newValue;
//                    EditorUtility.SetDirty(card);
//                }
//            );

//            EnumField targetFeild = rootElement.Query<EnumField>("cardTargeting").First();
//            targetFeild.bindingPath = "targeting"; //property name in card class
//            targetFeild.value = card.Targeting;
//            targetFeild.RegisterCallback<ChangeEvent<CardTargeting>>(
//                e => {
//                    card.Targeting = e.newValue;
//                    EditorUtility.SetDirty(card);
//                }
//            );

//            EnumField typeField = rootElement.Query<EnumField>("cardType").First();
//            typeField.bindingPath = "type";
//            typeField.value = card.Type;
//            typeField.RegisterCallback<ChangeEvent<CardType>>(
//                e =>
//                {
//                    card.Type = e.newValue;
//                    EditorUtility.SetDirty(card);
//                }
//            );

//            EnumField rarityField = rootElement.Query<EnumField>("cardRarity").First();
//            rarityField.bindingPath = "cardRarity";
//            rarityField.value = card.Rarity;
//            rarityField.RegisterCallback<ChangeEvent<CardRarity>>(
//                e =>
//                {
//                    card.Rarity = e.newValue;
//                    EditorUtility.SetDirty(card);
//                }
//            );

//            EnumField classField = rootElement.Query<EnumField>("cardClass").First();
//            classField.bindingPath = "characterClass";
//            classField.value = card.CharacterClass;
//            classField.RegisterCallback<ChangeEvent<CharacterClass>>(
//                e =>
//                {
//                    card.CharacterClass = e.newValue;
//                    //EditorUtility.SetDirty(card);
//                }
//            );

//            effectField = rootElement.Query<EnumField>("effectType").First();
//            effectField.bindingPath = "effectType";


//            #endregion
//            #region Display Card Effect Data 
//            // Store visual element that will contain the effect sub-inspectors.  
//            effectList = rootElement.Query<VisualElement>("effectList").First();
//            UpdateEffects();
//            #endregion

//            #region Buttons
//            // Assign methods to the click events of the two buttons.
//            Button btnAddEffect = rootElement.Query<Button>("btnAddNew").First();
//            btnAddEffect.clickable.clicked += AddEffect;

//            Button btnRemoveAlleffects = rootElement.Query<Button>("btnRemoveAll").First();
//            btnRemoveAlleffects.clickable.clicked += RemoveAll;

//            #endregion

//            return rootElement;
//        }
//        #endregion

//        #region Display effect Data Functions
//        public void UpdateEffects()
//        {
//            effectList.Clear();
//            // Create and add a effectSubEditor to our effectList container for each effect.
//            if (card.Effects != null)
//                foreach (Effect effect in card.Effects)
//                {
//                    EffectEditor effectEditor = new EffectEditor(this, effect);
//                    effectList.Add(effectEditor);
//                }
//        }
//        #endregion

//        #region Button Functions
//        // Create a new effect that is a child to the StarSystem asset. Save the assets to disk and update the effect sub-inspectors.
//        private void AddEffect()
//        {
//            Effect effect;

//            switch (effectField.value)
//            {
//                case EffectType.Attack:
//                    {
//                        Debug.Log("Happens once?");
//                        effect = ScriptableObject.CreateInstance<AttackEffect>();
//                        effect.name = "Attack effect";
//                        break;
//                    }
//                case EffectType.Draw:
//                    {
//                        effect = ScriptableObject.CreateInstance<DrawEffect>();
//                        effect.name = "Draw effect";
//                        break;
//                    }
//                case EffectType.Armour:
//                    {
//                        effect = ScriptableObject.CreateInstance<ArmourEffect>();
//                        effect.name = "Armour effect";
//                        break;
//                    }
//                case EffectType.Move:
//                    {
//                        effect = ScriptableObject.CreateInstance<MoveEffect>();
//                        effect.name = "Move effect";
//                        break;
//                    }
//                case EffectType.Heal:
//                    {
//                        effect = ScriptableObject.CreateInstance<HealEffect>();
//                        effect.name = "Heal effect";
//                        break;
//                    }
//                case EffectType.Enhancement:
//                    {
//                        effect = ScriptableObject.CreateInstance<EnhancementEffect>();
//                        effect.name = "Enhancement effect";
//                        break;
//                    }
//                default:
//                    effect = null;
//                    break;
//            }

//            if (!EffectAlreadyExists(effect))
//            {
//                if (effect != null)
//                {
//                    card.Effects.Add(effect);
//                    AssetDatabase.AddObjectToAsset(effect, card);
//                    AssetDatabase.SaveAssets();
//                    AssetDatabase.Refresh();
//                    UpdateEffects();
//                }
//            }
//        }

//        private bool EffectAlreadyExists(Effect newEffect)
//        {
//            bool effectExists = false;
//            foreach (Effect effect in card.Effects)
//            {
//                if (effect == newEffect)
//                {

//                    effectExists = true;
//                }
//            }
//            return effectExists;
//        }

//        // Remove all the effects from the StarSystem asset, save the changes and then remove all the effect sub-inspectors.
//        private void RemoveAll()
//        {
//            if (EditorUtility.DisplayDialog("Delete All effects", "Are you sure you want to delete all of the effects this star system has?", "Delete All", "Cancel"))
//            {
//                for (int i = card.Effects.Count - 1; i >= 0; i--)
//                {
//                    AssetDatabase.RemoveObjectFromAsset(card.Effects[i]);
//                    card.Effects.RemoveAt(i);
//                }
//                AssetDatabase.SaveAssets();
//                AssetDatabase.Refresh();
//                UpdateEffects();
//            }
//        }
//        // Remove a specified effect from the StarSystem asset, save the changes and update the effect sub-inspectors.
//        public void RemoveEffect(Effect effect)
//        {
//            card.Effects.Remove(effect);
//            AssetDatabase.RemoveObjectFromAsset(effect);
//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();
//            UpdateEffects();
//        }
//        #endregion

//    }
//    #endif
//}
