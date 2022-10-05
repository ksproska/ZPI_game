using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    [SerializeField] public SceneAsset scene;
    public void GoTo()
    {
        SceneManager.LoadScene(scene.name);
    }
}
