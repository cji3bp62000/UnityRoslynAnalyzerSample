using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDestroyAnalyzerTestScript : MonoBehaviour
{
    private Material matOK;
    private Material matOK2;
    private Material matNG;

    void Start()
    {
        matOK = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        // matNG は Destroy していないため、RA0001 警告あり
        matNG = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
    }

    private void OnDestroy()
    {
        Destroy(matOK);
        // matOK2 は Destroy していないけど、new はしていないので、警告なし
    }
}
