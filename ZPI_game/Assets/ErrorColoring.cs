using CurrentState;
using LevelUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ErrorColoring : MonoBehaviour
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
                slot.GetComponentInChildren<Image>().color = _colorOk;
            }
            else if (slot._placedContent == "")
            {
                slot.GetComponentInChildren<Image>().color = _colorEmpty;
            }
            else
            {
                slot.GetComponentInChildren<Image>().color = _colorNotOk;
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
                Debug.Log(CurrentGameState.Instance.CurrentLevelName);
                Debug.Log(CurrentGameState.Instance.CurrentSlot);
                LevelMap.Instance.CompleteALevel(CurrentGameState.Instance.CurrentLevelName, CurrentGameState.Instance.CurrentSlot);
            }
            CurrentGameState.Instance.CurrentLevelName = null;
            GoToScene transporter = new GoToScene();
            transporter.scene = "WorldMap";
            transporter.GoTo();
        }
        // SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }
}
