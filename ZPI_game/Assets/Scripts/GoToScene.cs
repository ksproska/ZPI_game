using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    [SerializeField] public string scene;
    public void GoTo()
    {
        SceneManager.LoadScene(scene);
    }
}
