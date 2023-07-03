using TMPro;
using UnityEngine;

public class TextBoxSettings : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private int _minWidth = 50;
    [SerializeField] private int _minHeight = 50;

    [Header("Tails")] 
    [SerializeField] private GameObject _talkTail;

    private RectTransform _tailRect;
    private RectTransform _rectTransform;
    private Vector2 _screenSize;
    private float _screenFactor;

    private string _text;
    private float _time;
    
    private Transform _parentPos;
    private SpriteRenderer _spriteRenderer;
    private Camera _cam;
    
    public Transform ParentPos { get { return _parentPos; } set { _parentPos = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } set { _spriteRenderer = value; } }

    public void Open()
    {
        transform.position = Vector3.zero;
        _backgroundRectTransform.sizeDelta = new Vector2(_minWidth, _minHeight);
        _tailRect = _talkTail.GetComponent<RectTransform>();
        _rectTransform = GetComponent<RectTransform>();
        
        RectTransform screen = GameObject.FindWithTag("UI").GetComponent<RectTransform>();
        _screenSize = new Vector2(screen.sizeDelta.x, screen.sizeDelta.y);
        _screenFactor = _screenSize.x / Screen.width;
        
        _cam = Camera.main;
        _parentPos = transform;

        LateUpdate();
    }
    
    void LateUpdate()
    {
        Vector3 min = _spriteRenderer.bounds.min;
        Vector3 max = _spriteRenderer.bounds.max;
 
        Vector3 screenMin = _cam.WorldToScreenPoint(min);
        Vector3 screenMax = _cam.WorldToScreenPoint(max);
        
        float parentHeight = screenMax.y - screenMin.y;

        if (_text != _textMeshPro.text)
        {
            _text = _textMeshPro.text;
            _time = 0;
        }

        _time += Time.deltaTime;
        
        _backgroundRectTransform.sizeDelta = Vector2.Lerp(_backgroundRectTransform.sizeDelta, new Vector2(_textMeshPro.textBounds.size.x + _minWidth, _textMeshPro.textBounds.size.y + _minHeight), _time);

        Vector3 pos = _cam.WorldToScreenPoint(_spriteRenderer.bounds.center) * _screenFactor;
        
        // Bubble Above Head
        _rectTransform.anchoredPosition = new Vector3(pos.x, pos.y + (_backgroundRectTransform.sizeDelta.y / 2 + _tailRect.sizeDelta.y + parentHeight / 1.5f * _screenFactor) , pos.z);
            
        _tailRect.anchorMax = new Vector2(0.5f, 0);
        _tailRect.anchorMin = new Vector2(0.5f, 0);
        _tailRect.rotation = Quaternion.Euler(0f, 0f, 180);

        if (_rectTransform.anchoredPosition.y + _backgroundRectTransform.sizeDelta.y >= _screenSize.y) {
            // Bubble Below Head
            _rectTransform.anchoredPosition = new Vector3(pos.x, pos.y - (_backgroundRectTransform.sizeDelta.y / 2 + _tailRect.sizeDelta.y + parentHeight / 1.5f * _screenFactor), pos.z);
            
            _tailRect.anchorMax = new Vector2(0.5f, 1);
            _tailRect.anchorMin = new Vector2(0.5f, 1);
            _tailRect.rotation = Quaternion.Euler(0f, 0f, 0);
        }
        
        float xpos = _rectTransform.anchoredPosition.x;
        xpos = Mathf.Clamp(xpos, _backgroundRectTransform.sizeDelta.x + 15, _screenSize.x - _backgroundRectTransform.sizeDelta.x - 15);
        
        _tailRect.anchoredPosition = new Vector2((pos.x - xpos) / 2, 0);
        _rectTransform.anchoredPosition = new Vector2(xpos, _rectTransform.anchoredPosition.y);
    }
}
