using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace SpellSystem.Views
{
    public class SpellCreator : MonoBehaviour
    {
        [Header("References")]
        //public TMP_InputField titleInput; // Поле для названия предмета
        public Transform propertiesLayout; // Layout группа для свойств
        public TMP_InputField propertyInput; // Поле для ввода нового свойства
        public Button addPropertyButton; // Кнопка добавления свойства
        public Button bindButton; // Кнопка "Связать"
        public GameObject propertyPrefab; // Префаб элемента свойства

        private List<string> properties = new List<string>(); // Список свойств
        private List<GameObject> propertyInstances = new List<GameObject>(); // Список экземпляров свойств

        [SerializeField] private PropertyApplier propertyApplier;
        [SerializeField] private UIController uiController;
        
        [HideInInspector] public StudyableObject CurrentObject;
        void Start()
        {
            // Назначаем обработчики кнопок
            addPropertyButton.onClick.AddListener(AddProperty);
            bindButton.onClick.AddListener(BindSpell);
        }

        // Отчищение всего
        public void ClearFields()
        {
            propertyInput.text = String.Empty;
            
            properties.Clear();
            
            foreach (var property in propertyInstances)
            {
                Destroy(property);
            }
        }

        // Добавление нового свойства
        void AddProperty()
        {
            Debug.Log("AddProperty");
            string property = propertyInput.text.Trim();
            Debug.Log("property: " + property);
            if (!string.IsNullOrEmpty(property) && propertyPrefab != null)
            {
                Debug.Log("yes");
                properties.Add(property);
                CreatePropertyUIElement(property);
                propertyInput.text = ""; // Очищаем поле ввода
            }
        }

        // Создание UI элемента для свойства из префаба
        void CreatePropertyUIElement(string property)
        {
            // Создаем экземпляр префаба
            GameObject propertyElement = Instantiate(propertyPrefab, propertiesLayout);
            propertyElement.name = "Property_" + property;
            
            // Находим компоненты в префабе
            TMP_Text propertyText = propertyElement.GetComponentInChildren<TMP_Text>();
            Button removeButton = propertyElement.GetComponentInChildren<Button>();
            
            if (propertyText != null)
            {
                propertyText.text = property;
            }
            
            if (removeButton != null)
            {
                // Добавляем обработчик удаления
                removeButton.onClick.AddListener(() => RemoveProperty(property, propertyElement));
            }
            
            propertyInstances.Add(propertyElement);
        }

        // Удаление свойства
        void RemoveProperty(string property, GameObject propertyElement)
        {
            // Удаляем из списка свойств
            properties.Remove(property);
            
            // Удаляем из списка экземпляров и уничтожаем GameObject
            if (propertyInstances.Contains(propertyElement))
            {
                propertyInstances.Remove(propertyElement);
                Destroy(propertyElement);
            }
        }

        // Обработка кнопки "Связать"
        void BindSpell()
        {
            if (CurrentObject ==null)
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
            {
                uiController.vThirdPersonInput.cc.SpellFailure();
            }
            
        }
    }
}