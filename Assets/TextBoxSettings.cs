using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxSettings : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private int _minWidth = 50;
    [SerializeField] private int _minHeight = 50;
    
    private Transform _parentPos;
    private float _offset;
    private Camera _cam;
    
    public Transform ParentPos { get { return _parentPos; } set { _parentPos = value; } }
    public float Offset { get { return _offset; } set { _offset = value; } }

    private void Awake()
    {
        _backgroundRectTransform.sizeDelta = new Vector2(_minWidth, _minHeight);

        _cam = Camera.main;
        _parentPos = transform;
    }
    
    void Update()
    {
        Debug.Log(_offset);
        _backgroundRectTransform.sizeDelta = new Vector2(_textMeshPro.textBounds.size.x + _minWidth, _textMeshPro.textBounds.size.y + _minHeight);
        
        Vector3 pos = _cam.WorldToScreenPoint(_parentPos.position);

        if (_cam.WorldToScreenPoint(_parentPos.position).y < _cam.pixelHeight / 2)
        {
            transform.position = new Vector3(pos.x, pos.y + (_backgroundRectTransform.sizeDelta.y / 2) + _offset, pos.z);
        }
        else
        {
            transform.position = new Vector3(pos.x, pos.y - (_backgroundRectTransform.sizeDelta.y / 2), pos.z);
        }
    }
}
