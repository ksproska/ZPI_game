using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropHandler : MonoBehaviour
{
    public bool AreAllCorrect()
    {
        return FindObjectsOfType<DropSlot>().All(ds => ds.IsCorrect());
    }

    public void HandleAreAllCorrect()
    {
        var wasCorrect = AreAllCorrect();
        Debug.Log("is input correct: " + wasCorrect);
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }
}
