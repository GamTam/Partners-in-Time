using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResizeBasedOnTextSize : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private int _minWidth = 50;
    [SerializeField] private int _minHeight = 50;

    // Update is called once per frame
    void Update()
    {
        _backgroundRectTransform.sizeDelta = new Vector2(_textMeshPro.textBounds.size.x + _minWidth, _textMeshPro.textBounds.size.y + _minHeight);
    }
}
