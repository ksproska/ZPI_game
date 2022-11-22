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
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            Debug.Log(linkInfo.GetLinkText() + " - " + linkInfo.GetLinkID());
            if (linkInfo.GetLinkID() == "")
            {
                return;
            }
            _showOrHide.Show();
            details.text = "Details for <b>" + linkInfo.GetLinkText() + "</b>:\n\n";
            var textAsset = Resources.Load<TextAsset>($"DescriptionTexts/{linkInfo.GetLinkID()}");
            details.text += textAsset.text;
        }
    }
}
