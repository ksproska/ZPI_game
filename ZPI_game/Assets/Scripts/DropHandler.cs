using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LevelUtils;
using CurrentState;

public class DropHandler : MonoBehaviour
{
    [SerializeField] private Color _colorOk, _colorNotOk, _colorEmpty;
    [SerializeField] GoToScene transporter;
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
            else if (slot._placedContent == "")
            {
                slot.GetComponent<Image>().color = _colorEmpty;
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
        if (wasCorrect)
        {

            if (!LevelMap.Instance.IsLevelDone(CurrentGameState.Instance.CurrentLevelName, CurrentGameState.Instance.CurrentSlot))
            {
                LevelMap.Instance.CompleteALevel(CurrentGameState.Instance.CurrentLevelName, CurrentGameState.Instance.CurrentSlot);
            }
            CurrentGameState.Instance.CurrentLevelName = null;
            GoToScene transporter = Instantiate(this.transporter);
            transporter.scene = "WorldMap";
            transporter.GoTo();
        }
        // SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }
}
