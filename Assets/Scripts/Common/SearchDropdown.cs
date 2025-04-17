using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    public class SearchDropdown : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_InputField inputField;
        public RectTransform listRoot;
        public ScrollRect scrollRect;
        public GameObject listItemPrefab;

        [Header("Config")]
        public float listHeight = 150f;

        [Serializable]
        public class StringEvent : UnityEvent<string> { }
        public StringEvent OnItemSelected;

        private List<string> allItems = new();
        private List<GameObject> itemObjects = new();

        private CancellationTokenSource focusCts;

        private void Start()
        {
            inputField.onValueChanged.AddListener(FilterList);

            inputField.onSelect.AddListener(_ =>
            {
                FilterList(inputField.text);
                ShowList();

                focusCts?.Cancel();
                focusCts = new CancellationTokenSource();
                WatchFocusLostAsync(focusCts.Token).Forget();
            });

            HideList();
        }

        public void SetItems(List<string> items)
        {
            allItems = items.OrderBy(x => x).ToList();
            FilterList(inputField.text);
        }

        public void SelectItem(string value)
        {
            inputField.text = value;
            OnItemSelected?.Invoke(value);
            HideList();
        }

        private void FilterList(string filter)
        {
            ClearList();

            var filtered = string.IsNullOrEmpty(filter)
                ? allItems
                : allItems.Where(x => x.StartsWith(filter, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x).ToList();

            foreach (var item in filtered)
            {
                var obj = Instantiate(listItemPrefab, scrollRect.content);
                obj.GetComponentInChildren<TMP_Text>().text = item;

                var btn = obj.GetComponent<Button>();
                string captured = item;
                btn.onClick.AddListener(() =>
                {
                    SelectItem(captured);
                });

                itemObjects.Add(obj);
            }

            listRoot.gameObject.SetActive(true);
        }

        private void ClearList()
        {
            foreach (var obj in itemObjects)
            {
                Destroy(obj);
            }
            itemObjects.Clear();
        }

        private void ShowList()
        {
            listRoot.sizeDelta = new Vector2(listRoot.sizeDelta.x, listHeight);
            listRoot.gameObject.SetActive(true);
        }

        public void HideList()
        {
            listRoot.gameObject.SetActive(false);
        }

        private async UniTaskVoid WatchFocusLostAsync(CancellationToken token)
        {
            Debug.Log("[Dropdown] Start watching focus");

            await UniTask.Yield(PlayerLoopTiming.Update);

            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);

                Debug.Log($"[Dropdown] isFocused: {inputField.isFocused}");

                if (!inputField.isFocused)
                {
                    await UniTask.NextFrame();

                    var selected = EventSystem.current.currentSelectedGameObject;

                    //Debug.Log($"[Dropdown] focus left. Selected: {selected?.name}");

                    if (selected == null ||
                        (!selected.transform.IsChildOf(listRoot) && selected != inputField.gameObject))
                    {
                        Debug.Log("[Dropdown] Hiding list due to lost focus.");
                        HideList();
                        break;
                    }
                }
            }
        }

    }
}
