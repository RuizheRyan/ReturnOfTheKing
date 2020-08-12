using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Light2D : MonoBehaviour
{
    public Material mat;
    public Material result;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var rtSize = source.width;
        RenderTexture rt0 = RenderTexture.GetTemporary(rtSize, rtSize, 0);
        RenderTexture rt1 = RenderTexture.GetTemporary(rtSize, rtSize, 1);
        RenderTexture rt2 = RenderTexture.GetTemporary(rtSize, rtSize, 0);

        rt0.filterMode = FilterMode.Point;
        rt1.filterMode = FilterMode.Point;
        rt2.filterMode = FilterMode.Point;

        rt0.anisoLevel = 0;
        rt1.anisoLevel = 0;
        rt2.anisoLevel = 0;

        Graphics.Blit(source, rt0, mat, 0);
        Graphics.Blit(rt0, rt1, mat, 2);

        var iteratNum = Math.Log(rt1.width, 2);
        for (int i = 1; i < iteratNum; ++i)
        {
            if (i % 2 == 1)
            {
                RenderTexture.ReleaseTemporary(rt2);
                rt2 = RenderTexture.GetTemporary(rt1.width / 2, rt1.height, 0);
                rt1.filterMode = FilterMode.Point;
                rt2.filterMode = FilterMode.Point;
                Graphics.Blit(rt1, rt2, mat, 1);
            }
            else
            {
                RenderTexture.ReleaseTemporary(rt1);
                rt1 = RenderTexture.GetTemporary(rt2.width / 2, rt2.height, 0);
                rt1.filterMode = FilterMode.Point;
                rt2.filterMode = FilterMode.Point;
                Graphics.Blit(rt2, rt1, mat, 1);
            }
        }
        RenderTexture.ReleaseTemporary(rt0);
        rt0 = RenderTexture.GetTemporary(rtSize * 4, rtSize * 4, 1);
        rt0.filterMode = FilterMode.Trilinear;
        Graphics.Blit(iteratNum % 2 == 1 ? rt1 : rt2, rt0, mat, 3);
        RenderTexture.ReleaseTemporary(rt1);
        rt1 = RenderTexture.GetTemporary(rt0.width / 2, rt0.height / 2);
        Graphics.Blit(rt0, rt1, mat, 4);
        RenderTexture.ReleaseTemporary(rt0);
        Graphics.Blit(rt1, rt0, mat, 4);

        result.SetTexture("_MainTex", rt0);
        RenderTexture.ReleaseTemporary(rt0);
        RenderTexture.ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
    }
}
