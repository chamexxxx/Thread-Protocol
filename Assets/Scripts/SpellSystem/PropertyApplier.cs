using System.Collections.Generic;
using System.Linq;
using SpellSystem.Data;
using UnityEngine;

namespace SpellSystem
{
    public class PropertyApplier : MonoBehaviour
    {
        [SerializeField] private float _searchRadius = 5f;
        [SerializeField] private PropertyDatabase _propertyDatabase;
        [SerializeField] private PlayerProgress _playerProgress;
        
        private void Start()
        {
            ApplyPropertiesToAllObjects();
        }

        public void ApplyPropertiesToAllObjects()
        {
            StudyableObject[] allObjects = FindObjectsOfType<StudyableObject>();
            foreach (StudyableObject obj in allObjects)
            {
                ApplyObjectProperties(obj);
            }
        }

        private void ApplyObjectProperties(StudyableObject targetObject)
        {
            if (targetObject.itemData == null || targetObject.itemData.Properties == null)
                return;

            foreach (PropertyType propertyType in targetObject.itemData.Properties)
            {
                targetObject.CheckPropertyController(propertyType);
            }
        }

        public void ApplyPropertiesToObject(StudyableObject currentObject, string[] propertyNames)
        {
            StudyableObject targetObject = currentObject;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                var prop1 = GetPropertyInfoByName(propertyNames[i]);
                for (int j = i + 1; j < propertyNames.Length; j++)
                {
                    var prop2 = GetPropertyInfoByName(propertyNames[j]);
                    if (_propertyDatabase.AreAntonyms(prop1.Type, prop2.Type))
                    {
                        Debug.LogError($"Конфликт свойств: '{prop1.DisplayName}' и '{prop2.DisplayName}' являются антонимами!");
                        return;
                    }
                }
            }
            // Вызов TryAddPropertyToObject для каждого свойства
            foreach (string propertyName in propertyNames)
            {
                TryAddPropertyToObject(targetObject, propertyName);
            }
        }
        
        private PropertyDatabase.PropertyInfo GetPropertyInfoByName(string name)
        {
            return _propertyDatabase.AllProperties
                .FirstOrDefault(p => p.DisplayName == name || p.DisplayFeminineName == name);
        }

        private StudyableObject FindStudyableObject(string objectName)
        {
            // Получаем все StudyableObject в радиусе
            StudyableObject[] nearbyObjects = FindObjectsOfType<StudyableObject>()
                .Where(obj => Vector3.Distance(transform.position, obj.transform.position) <= _searchRadius)
                .ToArray();

            // Ищем по точному совпадению имени
            return nearbyObjects.FirstOrDefault(obj => obj.itemData.ItemName == objectName);
        }

        private void TryAddPropertyToObject(StudyableObject targetObject, string propertyName)
        {
            // 1. Проверяем, что свойство существует в базе
            PropertyDatabase.PropertyInfo propertyInfo = _propertyDatabase.AllProperties
                .FirstOrDefault(p => p.DisplayName == propertyName || p.DisplayFeminineName == propertyName);

            if (propertyInfo == null)
            {
                Debug.LogError($"Свойство '{propertyName}' не найдено в базе данных!");
                return;
            }

            // 2. Проверяем род объекта и соответствие свойства
            bool isFeminine = targetObject.itemData.IsFeminine;
            string correctName = isFeminine ? propertyInfo.DisplayFeminineName : propertyInfo.DisplayName;

            if (propertyName != correctName)
            {
                Debug.LogError($"Для {(isFeminine ? "женского" : "мужского")} рода должно использоваться свойство '{correctName}'!");
                return;
            }

            // 3. Проверяем, изучено ли свойство игроком (хотя бы одним изученным предметом)
            bool isPropertyStudied = _playerProgress.studiedItems
                .Any(item => item.Properties.Contains(propertyInfo.Type));

            if (!isPropertyStudied)
            {
                Debug.LogError($"Свойство '{propertyName}' ещё не изучено игроком (не найдено ни в одном изученном предмете)!");
                return;
            }

            // 4. Удаляем антонимы, если они есть
            RemoveAntonyms(targetObject, propertyInfo.Type);

            // 5. Добавляем свойство (если его ещё нет)
            if (!targetObject.itemData.Properties.Contains(propertyInfo.Type))
            {
                targetObject.AddProperty(propertyInfo.Type);
                
                Debug.Log($"Добавлено свойство '{propertyName}' к объекту '{targetObject.itemData.ItemName}'");
            }
        }

        private void RemoveAntonyms(StudyableObject targetObject, PropertyType newPropertyType)
        {
            if (targetObject?.itemData?.Properties == null) 
                return;

            // Создаем копию списка для итерации
            var propertiesToCheck = new List<PropertyType>(targetObject.itemData.Properties);

            // Собираем антонимы для удаления
            var antonymsToRemove = propertiesToCheck
                .Where(p => _propertyDatabase.AreAntonyms(p, newPropertyType))
                .ToList();

            // Удаляем антонимы из оригинального списка
            foreach (var antonym in antonymsToRemove)
            {
                
                
                targetObject.RemoveProperty(antonym);
                var propInfo = _propertyDatabase.GetPropertyInfo(antonym);
                Debug.Log($"Удален антоним '{propInfo?.DisplayName}' при добавлении нового свойства");
            }
        }
    }
}
