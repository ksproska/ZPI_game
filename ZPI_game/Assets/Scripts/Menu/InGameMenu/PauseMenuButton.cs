using DeveloperUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PauseMenuButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color baseColor, secondaryColor, textColor;
    [SerializeField] private Image baseImage;
    [SerializeField] private Image secondaryImage;
    [SerializeField] private Text buttonText;

    private void Start()
    {
        baseImage.color = baseColor;
        secondaryImage.color = secondaryColor;
        buttonText.color = textColor;
    }

    private void OnEnable()
    {
        baseImage.color = baseColor;
        secondaryImage.color = secondaryColor;
        buttonText.color = textColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        baseImage.color = textColor;
        secondaryImage.color = baseColor;
        buttonText.color = baseColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        baseImage.color = baseColor;
        secondaryImage.color = secondaryColor;
        buttonText.color = textColor;
    }
}