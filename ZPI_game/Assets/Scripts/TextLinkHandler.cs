using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class TextLinkHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI details;

    public void OnPointerClick(PointerEventData eventData) {
        var pTextMeshPro = GetComponent<TextMeshProUGUI>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);
        if (linkIndex != -1) { // was a link clicked?
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            details.text = "Details for <b>" + linkInfo.GetLinkText() + "</b>:\n";
            string path = Directory.GetCurrentDirectory() + "\\Assets\\DescriptionTexts\\" + linkInfo.GetLinkText() + ".txt";
            string readText = File.ReadAllText(path);
            details.text += "> " +readText;
        }
    }
}
