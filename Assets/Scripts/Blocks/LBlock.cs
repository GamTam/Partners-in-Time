using UnityEngine;
using System.Collections;

public class LBlock : Billboard, Block
{
    // Animation States
    private const string DEFAULT = "l_block_default";
    private const string HIT = "l_block_hit";

    [SerializeField] private GameObject child;
    [SerializeField] private float _floatStrength = 0.1f;

    private float _floatY;
    private float _originalY;

    private bool _float = true;
    private bool _hit = false;
    private bool _hitByHitter = false;
    private bool _db = false;

    private float _hitDelay = 0.05f;
    private float _hitTime = 0.075f;
    private float _timer = 0;

    private void Start()
    {
        Init(child);
        _originalY = child.transform.position.y;
    }

    private void Update() {
        Float();

        if(_hit) {
            if(!_db) {
                _db = true;
                StartCoroutine(OnHit());
            }
        }
    }

    public void Float() {
        _floatY = child.transform.position.y;
        _floatY = _originalY + (Mathf.Sin(Time.time * 4) * _floatStrength);

        if(_float) {
            child.transform.position = new Vector3(child.transform.position.x, _floatY, child.transform.position.z);
        }
    }


    IEnumerator OnHit() {
        yield return new WaitForSeconds(_hitDelay);
        _timer += Time.deltaTime;

        if(_timer < _hitTime) {
            child.transform.position += Vector3.up * 7f * Time.deltaTime;
        } else {
            if(child.transform.position.y > (_hitByHitter ? _originalY : _floatY)) {
                child.transform.position += Vector3.down * 5f * Time.deltaTime;
            } else {
                child.transform.position = new Vector3(child.transform.position.x, (_hitByHitter ? _originalY : _floatY), child.transform.position.z);
                _hit = false;

                if(!_hitByHitter) {
                    _hitDelay = 0.05f;
                    _timer = 0;
                    _float = true;
                }
            }
        }

        if(_hit) {
            _hitDelay = 0f;
        }
        _db = false;
    }

    protected override void SetAnimation()
    {
        _animator.Play(!_hitByHitter ? DEFAULT : HIT);
    }

    public void OnBlockHit(string hitter) {
        if(hitter == "Luigi") {
            _animator.Play(HIT);
            _hitByHitter = true;
        }

        _hit = true;
        _float = false;
    }
}
