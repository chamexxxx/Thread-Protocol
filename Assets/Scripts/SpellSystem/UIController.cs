using DefaultNamespace;
using DefaultNamespace.Common;
using Player;
using SpellSystem.Data;
using SpellSystem.Views;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace SpellSystem
{
    public class UIController : MonoBehaviour
    {
        public GameObject studyPromptUI;
        
        public PlayerSpellController PlayerSpellController;

        private StudyableObject currentObject;

        public Image centerDot;
        [SerializeField] public Color highlightColor = Color.green;
        
        private Color originalDotColor;
        
        [SerializeField] private Transform studiedItemsParent;
        
        [SerializeField] private GameObject objectNamePrefab;
        [SerializeField] private GameObject propertyViewPrefab;
        [SerializeField] private GameObject dividerViewPrefab;
        [SerializeField] private GameObject simpleLineViewPrefab;
        
        [SerializeField] private Transform eStadyItemsParent;
        
        [SerializeField] private Transform studiedObjectsPanel;
        [SerializeField] private bool studiedObjectsPanelOpened = false;
        
        [SerializeField] private Transform spellPanel;
        [SerializeField] private bool spellPanelOpened = false;
        
        [SerializeField] private PropertyDatabase propertyDatabase;
        
        [SerializeField] private SpellCreator _spellCreator ;

        [SerializeField] private StudyableObjectRaycaster _studyableObjectRaycaster;

        private PlayerInput _playerInput;
        
        private bool uiIsActive => spellPanelOpened || studiedObjectsPanelOpened;
        
        private bool centerDotOnCanSpellingObject = false;
        
        private void Start()
        {
            originalDotColor = centerDot.color;
            
            studyPromptUI.SetActive(false);
            UpdateStudiedItemsUI();
            
            studiedObjectsPanel.gameObject.SetActive(false);
            spellPanel.gameObject.SetActive(false);

            _playerInput = GameManager.Instance.PlayerInput;

            _studyableObjectRaycaster.StudyableObjectFound += OnStudyableObjectFound;
            _studyableObjectRaycaster.StudyableObjectLost += OnStudyableObjectLost;
        }

        private void OnStudyableObjectFound(StudyableObject studyableObject)
        {
            if (uiIsActive)
            {
                return;
            }
            
            centerDot.color = highlightColor;
                    
            currentObject = studyableObject;

            foreach (Transform child in eStadyItemsParent)
            {
                Destroy(child.gameObject);
            }

            if (currentObject.Studed)
            {
                centerDotOnCanSpellingObject = true;
                _spellCreator.CurrentObject = currentObject;
                
                var propertyViewHeader = Instantiate(simpleLineViewPrefab, eStadyItemsParent).GetComponent<PropertyView>();
                propertyViewHeader.Name.text = $"[{studyableObject.itemData.ItemName}]";
                
                foreach (var property in currentObject.itemData.Properties)
                {
                    var propertyInfo = GetPropertyInfo(property);
                    var propertyView = Instantiate(simpleLineViewPrefab, eStadyItemsParent).GetComponent<PropertyView>();

                    if (currentObject.itemData.IsFeminine)
                    {
                        propertyView.Name.text = $"  * {propertyInfo.DisplayFeminineName}";
                    }
                    else
                    {
                        propertyView.Name.text = $"  * {propertyInfo.DisplayName}";
                    }
                }
                
                // TODO тут моя проверка
            }
            else
            {
                centerDotOnCanSpellingObject = false;
                _spellCreator.CurrentObject = null;
                    
                var propertyView = Instantiate(simpleLineViewPrefab, eStadyItemsParent).GetComponent<PropertyView>();
                propertyView.Name.text = $"[E] Изучить [{studyableObject.itemData.ItemName}]";
            }
            
            studyPromptUI.SetActive(true);
        }

        private void OnStudyableObjectLost()
        {
            if (uiIsActive)
            {
                return;
            }
            
            centerDotOnCanSpellingObject = false;
            _spellCreator.CurrentObject = null;
            centerDot.color = originalDotColor;
            
            if (studyPromptUI.activeSelf)
            {
                studyPromptUI.SetActive(false);
                currentObject = null;
            }
        }

        private void Update()
        {
            if (currentObject != null && Input.GetKeyDown(KeyCode.E))
            {
                StudyItem(currentObject);
                studyPromptUI.SetActive(false);
                currentObject = null;
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                // Переключаем состояние панели
                studiedObjectsPanelOpened = !studiedObjectsPanelOpened;
                studiedObjectsPanel.gameObject.SetActive(studiedObjectsPanelOpened);
                
                SwitchActions();
                
                // Если панель открылась, обновляем список предметов
                if (studiedObjectsPanelOpened)
                {
                    UpdateStudiedItemsUI();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && centerDotOnCanSpellingObject)
            {
                // Переключаем состояние панели
                if (!spellPanelOpened)
                {
                    spellPanel.gameObject.GetComponentInChildren<SpellCreator>().ClearFields();
                    
                    spellPanelOpened = true;
                    spellPanel.gameObject.SetActive(true);
                }

                SwitchActions();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSpellPanel();
            }
        }

        public void CloseSpellPanel()
        {
            spellPanelOpened = false;
            spellPanel.gameObject.SetActive(spellPanelOpened);

            SwitchActions();
            
            Debug.Log("CloseSpellPanel");
        }

        private void SwitchActions()
        {
            if (uiIsActive)
            {
                Debug.Log("Enable UI, lock camera and input");
                
                _playerInput.SwitchCurrentActionMap("UI");
                
                CursorController.Instance.SetLookEnabled(true);
            }
            else
            {
                Debug.Log("Disable UI, unlock camera and input");
                
                _playerInput.SwitchCurrentActionMap("Player");
                
                CursorController.Instance.SetLookEnabled(false);
            }
        }

        private void DisableActions()
        {
            
        }

        public void StudyItem(StudyableObject studyableObject)
        {
            PlayerProgress.Instance.AddStudiedItem(studyableObject.StartItemData);
            studyableObject.Studed = true;
            UpdateStudiedItemsUI();
        }

        private void UpdateStudiedItemsUI()
        {
            // Очищаем предыдущие элементы
            foreach (Transform child in studiedItemsParent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in PlayerProgress.Instance.studiedItems)
            {
                // Создаем префаб для предмета
                var itemView = Instantiate(objectNamePrefab, studiedItemsParent.transform).GetComponent<ObjectView>();
                itemView.Name.text = $"- {item.ItemName}";
        
                // Добавляем свойства предмета
                foreach (var property in item.Properties)
                {
                    var propertyInfo = GetPropertyInfo(property);
                    
                    var propertyView = Instantiate(propertyViewPrefab, studiedItemsParent.transform).GetComponent<PropertyView>();
                    
                    if (item.IsFeminine)
                    {
                        propertyView.Name.text = $"  * {propertyInfo.DisplayFeminineName}";
                    }
                    else
                    {
                        propertyView.Name.text = $"  * {propertyInfo.DisplayName}";
                    }
                }
                
                var itemDividerView = Instantiate(dividerViewPrefab, studiedItemsParent.transform);
            }
        }
        
        public PropertyDatabase.PropertyInfo GetPropertyInfo(PropertyType type)
        {
            return propertyDatabase.GetPropertyInfo(type);
        }
        
        
    }
}