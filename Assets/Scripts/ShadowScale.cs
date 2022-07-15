using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScale : MonoBehaviour
{
    [SerializeField] private Transform _object;
    [SerializeField] private float _deadZone = 0.1f;

    private float _maxScale;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxDistance = 4;

    private float _scale;

    private void Awake()
    {
        _maxScale = transform.localScale.x;
    }

    void LateUpdate()
    {
        _scale = (_maxDistance / (_object.transform.position.y - transform.position.y + _maxDistance)) * _maxScale;
        
        if (_scale >= _minScale && _scale <= _minScale)
        {
            _scale = _minScale;
        }

        if (_object.transform.position.y < _deadZone) _scale = _maxScale;

        transform.localScale = new Vector3(_scale, _scale, _scale);
    }
}
