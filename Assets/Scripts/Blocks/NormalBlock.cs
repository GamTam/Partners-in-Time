using UnityEngine;
using System.Collections;

public class NormalBlock : Billboard, Block
{
    // Animation States
    private const string DEFAULT = "n_block_default";
    private const string HIT = "n_block_hit";

    [SerializeField] private GameObject child;
    [SerializeField] private float _floatStrength = 0.1f;

    private float _floatY;
    private float _originalY;

    private bool _float = true;
    private bool _hit = false;

    private float _hitDelay = 0.05f;
    private float _hitTime = 0.1f;
    private float _timer = 0;

    private void Start()
    {
        Init(child);
        _originalY = child.transform.position.y;
    }

    private void Update() {
        Float();
    }

    public void Float() {
        if(_float) {
            _floatY = child.transform.position.y;
            _floatY = _originalY + (Mathf.Sin(Time.time * 4) * _floatStrength);
            child.transform.position = new Vector3(child.transform.position.x, _floatY, child.transform.position.z);
        }

        if(_hit) {
            StartCoroutine(OnHit());
        }
    }


    IEnumerator OnHit() {
        yield return new WaitForSeconds(_hitDelay);

        _timer += Time.deltaTime;

        if(_timer < _hitTime) {
            child.transform.position += Vector3.up * 7f * Time.deltaTime;
        } else {
            if(child.transform.position.y > _originalY) {
                child.transform.position += Vector3.down * 2f * Time.deltaTime;
            }
        }
        _hitDelay = 0f;
    }

    protected override void SetAnimation()
    {
        _animator.Play(!_hit ? DEFAULT : HIT);
    }

    public void OnBlockHit(string hitter) {
        _animator.Play(HIT);
        _float = false;
        _hit = true;
    }
}
