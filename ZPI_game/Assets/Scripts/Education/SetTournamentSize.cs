using UnityEngine;
using UnityEngine.UI;

public class SetTournamentSize : MonoBehaviour
{
    [SerializeField] public InputField GenerationSize;

    public void SetText(float val)
    {
        GenerationSize.text = $"{(int)val}";
    }
}
