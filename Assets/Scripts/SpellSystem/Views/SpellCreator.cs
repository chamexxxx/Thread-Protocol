using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using SpellSystem.Data;
using TMPro;
using UnityEngine.EventSystems;

namespace SpellSystem.Views
{
    public class SpellCreator : MonoBehaviour
    {
        [Header("References")]
        public Transform propertiesLayout;
        public TMP_InputField propertyInput;

        [SerializeField] private PlayerProgress _playerProgress;
        [SerializeField] private PropertyDatabase _propertyDatabase;

        public SearchDropdown propertySearchDropdown;

        public Button addPropertyButton;
        public Button bindButton;
        public GameObject propertyPrefab;

        public GameObject focusRedirectTarget; // 👈 перетаскивается в инспекторе

        private List<string> properties = new();
        private List<GameObject> propertyInstances = new();
        private List<string> availableProperties = new();

        [SerializeField] private PropertyApplier propertyApplier;
        [SerializeField] private UIController uiController;

        [HideInInspector] public StudyableObject CurrentObject;

        void Start()
        {
            addPropertyButton.onClick.AddListener(AddProperty);
            bindButton.onClick.AddListener(BindSpell);
        }

        private void SetupSearchDropdown()
        {
            if (CurrentObject == null || CurrentObject.StartItemData == null)
            {
                availableProperties = new List<string>();
                propertySearchDropdown.SetItems(availableProperties);
                propertyInput.text = string.Empty;
                return;
            }

            bool isFeminine = CurrentObject.StartItemData.IsFeminine;

            HashSet<PropertyType> uniqueTypes = new();

            foreach (var item in _playerProgress.studiedItems)
            {
                if (item?.Properties == null) continue;

                foreach (var prop in item.Properties)
                {
                    uniqueTypes.Add(prop);
                }
            }

            List<string> propertyNames = new();
            foreach (var type in uniqueTypes)
            {
                // Пропускаем, если у объекта уже есть это свойство
                if (CurrentObject.HasProperty(type))
                    continue;
                
                var info = _propertyDatabase.GetPropertyInfo(type);
                if (info != null)
                {
                    string name = isFeminine ? info.DisplayFeminineName : info.DisplayName;
                    if (!string.IsNullOrEmpty(name))
                    {
                        propertyNames.Add(name);
                    }
                }
            }

            propertyNames.Sort();
            availableProperties = propertyNames;
            propertySearchDropdown.SetItems(availableProperties);

            propertySearchDropdown.OnItemSelected.RemoveAllListeners();
            propertySearchDropdown.OnItemSelected.AddListener(value =>
            {
                propertyInput.text = value;
                AddProperty();
                propertySearchDropdown.HideList();
            });
        }

        public void SetCurrentObject(StudyableObject obj)
        {
            if (CurrentObject != obj)
            {
                CurrentObject = obj;
                SetupSearchDropdown();
            }
        }

        public void ClearFields()
        {
            propertyInput.text = string.Empty;
            properties.Clear();

            foreach (var property in propertyInstances)
            {
                Destroy(property);
            }

            propertyInstances.Clear();
        }

        void AddProperty()
        {
            string property = propertyInput.text.Trim();
            if (!string.IsNullOrEmpty(property) &&
                propertyPrefab != null &&
                !properties.Contains(property))
            {
                properties.Add(property);
                CreatePropertyUIElement(property);
                propertyInput.text = "";

                availableProperties.Remove(property);
                propertySearchDropdown.SetItems(availableProperties);
            }
        }

        void CreatePropertyUIElement(string property)
        {
            GameObject propertyElement = Instantiate(propertyPrefab, propertiesLayout);
            propertyElement.name = "Property_" + property;

            TMP_Text propertyText = propertyElement.GetComponentInChildren<TMP_Text>();
            Button removeButton = propertyElement.GetComponentInChildren<Button>();

            if (propertyText != null)
            {
                propertyText.text = property;
            }

            if (removeButton != null)
            {
                removeButton.onClick.AddListener(() => RemoveProperty(property, propertyElement));
            }

            propertyInstances.Add(propertyElement);
        }

        void RemoveProperty(string property, GameObject propertyElement)
        {
            if (EventSystem.current.currentSelectedGameObject == propertyElement)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            if (properties.Contains(property))
            {
                properties.Remove(property);
                availableProperties.Add(property);
                availableProperties.Sort();
                propertySearchDropdown.SetItems(availableProperties);
            }

            if (propertyInstances.Contains(propertyElement))
            {
                propertyInstances.Remove(propertyElement);
                // Задержка на 1 кадр перед Destroy
                DestroyDelayed(propertyElement).Forget();
            }
            propertySearchDropdown.HideList();
            EventSystem.current.SetSelectedGameObject(null);
            propertySearchDropdown.inputField.DeactivateInputField();
        }

        private async UniTaskVoid DestroyDelayed(GameObject go)
        {
            await UniTask.NextFrame();
            Destroy(go);
        }

        void BindSpell()
        {
            if (CurrentObject == null)
            {
                Debug.LogWarning("Предмет не может быть пустым!");
                uiController.CloseSpellPanel();
                return;
            }

            Debug.Log($"Создание заклинания: {CurrentObject.itemData.ItemName}");
            Debug.Log("Список свойств:");

            foreach (string property in properties)
            {
                Debug.Log($"- {property}");
            }

            uiController.CloseSpellPanel();
            uiController.vThirdPersonInput.SetRotateTarget(CurrentObject.gameObject);

            if (propertyApplier.TryApplyPropertiesToObject(CurrentObject, properties.ToArray()))
                uiController.vThirdPersonInput.cc.SpellSuccess();
            else
                uiController.vThirdPersonInput.cc.SpellFailure();
        }
    }
}
