using TMPro;
using UnityEngine;

public class NumberCounterUpdater : MonoBehaviour
{
    public NumberCounter NumberCounter;
    public TMP_InputField InputField;
    public int newValue;


    public void SetValue(int value)
    {
        newValue += value;

        //if (int.TryParse(InputField.text, out value))
        //{
            NumberCounter.Value = value;
        //}
    }
}
