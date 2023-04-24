using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public List<GameObject> images;
    public Button button;
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(Next);
        images[0].SetActive(true);
    }
    void Next()
    {
        images[currentIndex].SetActive(false);
        currentIndex++;
        
        if(currentIndex >= images.Count)
        {
            currentIndex = 0;
        }
        images[currentIndex].SetActive(true);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
