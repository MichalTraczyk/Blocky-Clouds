using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSHandle : MonoBehaviour
{
    public int targetFps;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
    }
    private void Update()
    {
        if (Application.targetFrameRate != targetFps)
            Application.targetFrameRate = targetFps;
    }
}
