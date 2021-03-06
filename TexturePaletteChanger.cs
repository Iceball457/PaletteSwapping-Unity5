﻿using UnityEngine;
using System.Collections;

public class TexturePaletteChanger : MonoBehaviour
{
    public ColorPalette[] Palettes;

    private MeshRenderer _spriteRenderer;
    private Texture2D _texture;
    private MaterialPropertyBlock _matBlock;

    private void Awake()
    {
        _spriteRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if(Palettes.Length > 0)
            ChangeColors(Palettes[Random.Range(0, Palettes.Length)]);
    }

    private void LateUpdate()
    {
        _spriteRenderer.SetPropertyBlock(_matBlock);
    }

    private void ChangeColors(ColorPalette palette)
    {
        if (palette.CachedTexture == null)
        {
            _texture = (Texture2D)_spriteRenderer.materials [0].mainTexture;
            var width = _texture.width;
            var height = _texture.height;

            var cloneTexture = new Texture2D(width, height)
            {

                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            var colors = _texture.GetPixels();
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = palette.GetColor(colors[i]);
            }

            cloneTexture.SetPixels(colors);
            cloneTexture.Apply();

            palette.CachedTexture = cloneTexture;
        }

        _matBlock = new MaterialPropertyBlock();
        _matBlock.SetTexture("_MainTex", palette.CachedTexture);
    }
}
