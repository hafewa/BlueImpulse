﻿using UnityEngine;
using System;
using System.Collections;


public class DSPECrawlingLight : DSEffectBase
{
    public float resolution_scale = 1.0f;
    public float rayAdvance = 1.0f;
    public Material matSurfaceLight;
    public Material matCombine;
    public Material matFill;
    public RenderTexture[] rtTemp;
    Action m_render;


    void OnEnable()
    {
        ResetDSRenderer();
        if (m_render == null)
        {
            m_render = Render;
            GetDSRenderer().AddCallbackPostGBuffer(m_render, 1100);
            rtTemp = new RenderTexture[2];
        }
    }

    void UpdateRenderTargets()
    {
        Vector2 reso = GetDSRenderer().GetInternalResolution() * resolution_scale;
        if (rtTemp[0] != null && rtTemp[0].width != (int)reso.x)
        {
            for (int i = 0; i < rtTemp.Length; ++i)
            {
                rtTemp[i].Release();
                rtTemp[i] = null;
            }
        }
        if (rtTemp[0] == null || rtTemp[0].IsCreated())
        {
            for (int i = 0; i < rtTemp.Length; ++i)
            {
                rtTemp[i] = DSRenderer.CreateRenderTexture((int)reso.x, (int)reso.y, 0, RenderTextureFormat.ARGBHalf);
                rtTemp[i].filterMode = FilterMode.Bilinear;
            }
        }
    }

    void Render()
    {
        if (!enabled) { return; }

        UpdateRenderTargets();

        DSRenderer dsr = GetDSRenderer();
        Graphics.SetRenderTarget(rtTemp[1]);
        matFill.SetVector("_Color", new Vector4(0.0f, 0.0f, 0.0f, 0.02f));
        matFill.SetTexture("g_position_buffer1", dsr.rtPositionBuffer);
        matFill.SetTexture("g_position_buffer2", dsr.rtPrevPositionBuffer);
        matFill.SetPass(1);
        DSRenderer.DrawFullscreenQuad();

        Graphics.SetRenderTarget(rtTemp[0]);
        matSurfaceLight.SetFloat("_RayAdvance", rayAdvance);
        matSurfaceLight.SetTexture("g_normal_buffer", dsr.rtNormalBuffer);
        matSurfaceLight.SetTexture("g_position_buffer", dsr.rtPositionBuffer);
        matSurfaceLight.SetTexture("_ColorBuffer", dsr.rtAlbedoBuffer);
        matSurfaceLight.SetTexture("g_glow_buffer", dsr.rtEmissionBuffer);
        matSurfaceLight.SetTexture("g_glow_bufferB", dsr.rtPrevEmissionBuffer);
        matSurfaceLight.SetTexture("_PrevResult", rtTemp[1]);
        matSurfaceLight.SetPass(0);
        DSRenderer.DrawFullscreenQuad();

        rtTemp[0].filterMode = FilterMode.Trilinear;
        Graphics.SetRenderTarget(dsr.rtEmissionBuffer);
        matCombine.SetTexture("_MainTex", rtTemp[0]);
        matCombine.SetPass(3);
        DSRenderer.DrawFullscreenQuad();
        rtTemp[0].filterMode = FilterMode.Point;

        Swap(ref rtTemp[0], ref rtTemp[1]);
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
}
