using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class TextLinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI details;
    private ShowOrHide _showOrHide;

    public void Start()
    {
        _showOrHide = GameObject.FindWithTag("CodeDetails").GetComponent<ShowOrHide>();
        details = GameObject.FindWithTag("CodeDetails").GetComponentInChildren<TextMeshProUGUI>();
        _showOrHide.Hide();
    }

    public void OnPointerClick(PointerEventData eventData) {
        var pTextMeshPro = GetComponent<TextMeshProUGUI>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);
        
        if (linkIndex != -1) { // was a link clicked?
            _showOrHide.Show();
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            details.text = "Details for <b>" + linkInfo.GetLinkText() + "</b>:\n";
            string path = Directory.GetCurrentDirectory() + "\\Assets\\DescriptionTexts\\" + linkInfo.GetLinkID() + ".txt";
            string readText = File.ReadAllText(path);
            details.text += readText;
        }
    }
}
