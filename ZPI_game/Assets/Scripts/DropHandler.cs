using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DropHandler : MonoBehaviour
{
    [SerializeField] private Color _colorOk, _colorNotOk;
    public bool AreAllCorrect()
    {
        return FindObjectsOfType<DropSlot>().All(ds => ds.IsCorrect());
    }

    public void SetCheckColors()
    {
        var allSlots = FindObjectsOfType<DropSlot>();
        foreach (var slot in allSlots)
        {
            if (slot.IsCorrect())
            {
                slot.GetComponent<Image>().color = _colorOk;
            }
            else
            {
                slot.GetComponent<Image>().color = _colorNotOk;
            }
        }
    }

    public void HandleAreAllCorrect()
    {
        var wasCorrect = AreAllCorrect();
        Debug.Log("is input correct: " + wasCorrect);
        SetCheckColors();
        // SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }
}
