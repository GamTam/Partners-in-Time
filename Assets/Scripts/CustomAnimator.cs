using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomAnimator : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;
    
    private AnimationClip _currentAnimation;
    private List<Sprite> _frames;
    private int _currentFrame = 0;

    private float _time;
    

    private bool _loop;
    private bool _playing = false;

    public bool Playing { get { return _playing; } }
    public float NormalizedTime { get { return _currentAnimation ? (_time / _currentAnimation.length) : 0f; } }
    public float TimeStamp { get { return _time; } }

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void OnDisable() { 
        _playing = false;
        _currentAnimation = null;
    }

    public void Play(string name) {
        AnimationClip animation = GetAnimation(name);

        if(animation != null) {
            if(animation != _currentAnimation) {
                ForcePlay(name);
            }
        } else {
            Debug.LogWarning("Could not find animation: " + name);
        }
    }

    public void ForcePlay(string name) {
        AnimationClip animation = GetAnimation(name);

        if(animation != null) {

            if(_currentAnimation && _currentAnimation.name.Split("_")[1] != animation.name.Split("_")[1]) {
                _currentFrame = 0;
            }

            _loop = AnimationUtility.GetAnimationClipSettings(animation).loopTime;
            _currentAnimation = animation;
            _playing = true;
            _frames = GetSprites(animation);
            _animator.enabled = false;
            _renderer.sprite = _frames[_currentFrame];
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(_currentAnimation));
        }
    }

    private IEnumerator PlayAnimation(AnimationClip animation) {
        float timer = 0f;
        float delay = 1f / (float) animation.frameRate;

        _time = 0f;

        while(_loop || _currentFrame < _frames.Count - 1) {
            _time += Time.deltaTime;

            while(timer < delay) {
                timer += Time.deltaTime;
                _time += Time.deltaTime;
                yield return 0f;
            }
            while(timer > delay) {
                timer -= delay;
                NextFrame(animation);
            }
            _renderer.sprite = _frames[_currentFrame];
        }

        _time = animation.length;
    }

    private void NextFrame(AnimationClip animation) {
        _currentFrame++;
        
        if(_currentFrame >= _frames.Count) {
            if(_loop) {
                _currentFrame = 0;
            } else {
                _currentFrame = _frames.Count - 1;
                _playing = false;
            }
        }
    }

    public AnimationClip GetAnimation(string name) { 
        AnimationClip anim = null;

        foreach (AnimationClip animation in _animator.runtimeAnimatorController.animationClips) {
            if (animation.name == name) {
                anim = animation;
            }
        }
        
        return anim;
    }

    public List<Sprite> GetSprites(AnimationClip clip) {
        List<Sprite> sprites = new List<Sprite>();

        foreach(EditorCurveBinding binding in AnimationUtility.GetObjectReferenceCurveBindings(clip)) {
            ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);

            foreach(ObjectReferenceKeyframe frame in keyframes) {
                sprites.Add((Sprite) frame.value);
            }
        }
        
        return sprites;
    }
}
