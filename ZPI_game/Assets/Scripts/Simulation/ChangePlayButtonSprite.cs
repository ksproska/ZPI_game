using UnityEngine;
using UnityEngine.UI;


public class ChangePlayButtonSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite play, pause;

    [SerializeField] private Image image;

    public void changeImage()
    {
        var current = image.sprite;
        if (current == pause)
        {
            image.sprite = play;
        }
        else
        {
            image.sprite = pause;
        }
    }
}
