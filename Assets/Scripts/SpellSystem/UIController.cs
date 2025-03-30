using DefaultNamespace;
using Invector.vCharacterController;
using SpellSystem.Data;
using SpellSystem.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SpellSystem
{
    public class UIController : MonoBehaviour
    {
        public GameObject studyPromptUI;
        public Camera playerCamera;
        public float maxStudyDistance = 5f;
        public LayerMask studyableLayer;

        private StudyableObject currentObject;
        private bool wasPromptVisible = false;

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
        [SerializeField] private vThirdPersonCamera vThirdPersonCamera;
        [SerializeField] private vThirdPersonInput vThirdPersonInput;
        
        private bool centerDotOnCanSpellingObject = false;
        
        [SerializeField] private SpellCreator _spellCreator ;
        
        private void Start()
        {
            originalDotColor = centerDot.color;
            
            studyPromptUI.SetActive(false);
            UpdateStudiedItemsUI();
            
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
            
            studiedObjectsPanel.gameObject.SetActive(false);
            spellPanel.gameObject.SetActive(false);
            
        }

        private void Update()
        {
            CheckForStudyableObjects();
            
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
                spellPanelOpened = !spellPanelOpened;
                spellPanel.gameObject.SetActive(spellPanelOpened);

                SwitchActions();
                    
                // Если панель открылась, Отчищаем все поля
                if (spellPanelOpened)
                {
                    spellPanel.gameObject.GetComponentInChildren<SpellCreator>().ClearFields();
                }
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
            var uiIsActive = spellPanelOpened || studiedObjectsPanelOpened;
            
            if (uiIsActive)
            {
                Debug.Log("Enable UI, lock camera and input");
                
                CursorController.Instance.SetLookEnabled(true);
                vThirdPersonCamera.lockCamera = true;
                vThirdPersonInput.block = true;
            }
            else
            {
                Debug.Log("Disable UI, unlock camera and input");
                
                CursorController.Instance.SetLookEnabled(false);
                vThirdPersonCamera.lockCamera = false;
                vThirdPersonInput.block = false;
            }
        }

        private void DisableActions()
        {
            
        }

        private void CheckForStudyableObjects()
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxStudyDistance, studyableLayer))
            {
                StudyableObject studyable = hit.collider.GetComponent<StudyableObject>();
                if (studyable != null)
                {
                    centerDot.color = highlightColor;
                    
                    currentObject = studyable;

                    foreach (Transform child in eStadyItemsParent)
                    {
                        Destroy(child.gameObject);
                    }

                    if (currentObject.Studed)
                    {
                        centerDotOnCanSpellingObject = true;
                        _spellCreator.CurrentObject = currentObject;
                        
                        var propertyViewHeader = Instantiate(simpleLineViewPrefab, eStadyItemsParent).GetComponent<PropertyView>();
                        propertyViewHeader.Name.text = $"Изучено [{studyable.itemData.ItemName}]";
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
                    }
                    else
                    {
                        centerDotOnCanSpellingObject = false;
                        _spellCreator.CurrentObject = null;
                            
                        var propertyView = Instantiate(simpleLineViewPrefab, eStadyItemsParent).GetComponent<PropertyView>();
                        propertyView.Name.text = $"[E] Изучить [{studyable.itemData.ItemName}]";
                    }
                    
                    studyPromptUI.SetActive(true);
                    wasPromptVisible = true;
                    return;
                }
                else
                {
                    centerDotOnCanSpellingObject = false;
                    _spellCreator.CurrentObject = null;
                    centerDot.color = originalDotColor;
                }
            }
            else
            {
                centerDotOnCanSpellingObject = false;
                _spellCreator.CurrentObject = null;
                centerDot.color = originalDotColor;
            }

            if (wasPromptVisible)
            {
                studyPromptUI.SetActive(false);
                currentObject = null;
                wasPromptVisible = false;
            }
        }

        public void StudyItem(StudyableObject studyableObject)
        {
            PlayerProgress.Instance.AddStudiedItem(studyableObject.itemData);
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