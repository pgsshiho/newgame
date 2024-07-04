using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textMeshPro;

    // 밝아질 색상
    public Color highlightColor = Color.yellow;
    // 원래 색상
    private Color originalColor;

    void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color;
        }
        else
        {
            Debug.LogError("TMP_Text 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = originalColor;
        }
    }
}