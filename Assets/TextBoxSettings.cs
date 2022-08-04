using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxSettings : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private int _minWidth = 50;
    [SerializeField] private int _minHeight = 50;

    [Header("Tails")] 
    [SerializeField] private GameObject _talkTail;

    private RectTransform _tailRect;
    
    private Transform _parentPos;
    private SpriteRenderer _spriteRenderer;
    private Camera _cam;
    
    public Transform ParentPos { get { return _parentPos; } set { _parentPos = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } set { _spriteRenderer = value; } }

    private void Awake()
    {
        _backgroundRectTransform.sizeDelta = new Vector2(_minWidth, _minHeight);
        _tailRect = _talkTail.GetComponent<RectTransform>();
        
        _cam = Camera.main;
        _parentPos = transform;
    }
    
    void Update()
    {
        Vector3 min = _spriteRenderer.bounds.min;
        Vector3 max = _spriteRenderer.bounds.max;
 
        Vector3 screenMin = _cam.WorldToScreenPoint(min);
        Vector3 screenMax = _cam.WorldToScreenPoint(max);
        
        float boxHeight = screenMax.y - screenMin.y;

        _backgroundRectTransform.sizeDelta = new Vector2(_textMeshPro.textBounds.size.x + _minWidth, _textMeshPro.textBounds.size.y + _minHeight);
        
        Vector3 pos = _cam.WorldToScreenPoint(_spriteRenderer.bounds.center);

        if (_cam.WorldToScreenPoint(_parentPos.position).y < _cam.pixelHeight / 2)
        {
            transform.position = new Vector3(pos.x, pos.y + (_backgroundRectTransform.sizeDelta.y / 2) + (boxHeight / 2), pos.z);
            
            _tailRect.anchorMax = new Vector2(0.5f, 0);
            _tailRect.anchorMin = new Vector2(0.5f, 0);
            _tailRect.rotation = Quaternion.Euler(0f, 0f, 180);
            _tailRect.anchoredPosition = Vector3.zero;
        }
        else
        {
            transform.position = new Vector3(pos.x, pos.y - (_backgroundRectTransform.sizeDelta.y / 2) - (boxHeight / 2), pos.z);
            
            _tailRect.anchorMax = new Vector2(0.5f, 1);
            _tailRect.anchorMin = new Vector2(0.5f, 1);
            _tailRect.rotation = Quaternion.Euler(0f, 0f, 0);
            _tailRect.anchoredPosition = Vector3.zero;
        }
    }
}
