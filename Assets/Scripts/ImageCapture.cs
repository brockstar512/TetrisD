using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImageCapture : MonoBehaviour
{
    public KeyCode screenShotButton = KeyCode.P;
    int i = 0;

    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            StartCoroutine(TakeScreenShot());
        }
    }
    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot($"screenshot{i}.png");
        i++;
    }
}
