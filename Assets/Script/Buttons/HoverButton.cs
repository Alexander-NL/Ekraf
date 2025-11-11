using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject hoverImage;
    private TMP_Text buttonText;
    private Color textColor;

    private void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        textColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.GetComponent<CanvasGroup>().alpha = 1f;
        buttonText.color = Color.yellow;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.GetComponent<CanvasGroup>().alpha = 0f;
        buttonText.color = textColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.GetComponent<CanvasGroup>().alpha = 0f;
        buttonText.color = textColor;
    }
}
