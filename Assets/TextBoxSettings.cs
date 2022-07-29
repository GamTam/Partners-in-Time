using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxSettings : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _minWidth = 50;
    [SerializeField] private int _minHeight = 50;
    
    private Transform _parentPos;
    private Camera _cam;
    
    public Transform ParentPos {get { return _parentPos; } set { _parentPos = value; } }

    private void Awake()
    {
        _backgroundRectTransform.sizeDelta = new Vector2(_minWidth, _minHeight);

        _cam = Camera.main;
        _parentPos = transform;
    }
    
    void Update()
    {
        Vector3 pos;
        
       pos = _cam.WorldToScreenPoint(_parentPos.position).y > _cam.pixelHeight / 2 ?  _cam.WorldToScreenPoint(_parentPos.position - _offset) : _cam.WorldToScreenPoint(_parentPos.position + _offset);
       
       Debug.Log(pos);
       transform.position = pos;
        
        _backgroundRectTransform.sizeDelta = new Vector2(_textMeshPro.textBounds.size.x + _minWidth, _textMeshPro.textBounds.size.y + _minHeight);
    }
}
