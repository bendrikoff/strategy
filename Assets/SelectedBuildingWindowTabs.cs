using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedBuildingWindowTabs : MonoBehaviour
{
    public GameObject tabPrefab; // Префаб для вкладки

    // Добавление новой вкладки
    public void AddTab()
    {
        if (tabPrefab != null && gameObject != null)
        {
            GameObject newTab = Instantiate(tabPrefab, container);
            UpdateTabSizes();
        }
    }

    // Удаление последней вкладки
    public void RemoveTab()
    {
        if (container.childCount > 0)
        {
            Destroy(container.GetChild(container.childCount - 1).gameObject);
            UpdateTabSizes();
        }
    }

    // Обновление размеров вкладок
    private void UpdateTabSizes()
    {
        int tabCount = container.childCount;

        if (tabCount == 0) return;

        foreach (Transform child in container)
        {
            RectTransform rect = child.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1.0f / tabCount, 1);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
        }
    }
}
