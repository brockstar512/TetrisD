using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(PressedSound);
    }

    void PressedSound()
    {
        SoundManager.Instance.PlaySound(buttonSound);
    }
    private void OnDestroy()
    {
        button = this.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

}
