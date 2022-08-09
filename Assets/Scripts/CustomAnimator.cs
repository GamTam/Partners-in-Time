using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomAnimator : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;
    
    private AnimationClip _currentAnimation;
    private List<Frame> _frames;
    private int _currentFrame = 0;

    private float _time;
    

    private bool _loop;
    private bool _playing = false;

    public class Frame {
        public Sprite Sprite;
        public ObjectReferenceKeyframe Keyframe;
    }

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
            _frames = GetFrames(animation);
            _animator.enabled = false;
            _renderer.sprite = _frames[_currentFrame].Sprite;
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(_currentAnimation));
            StartCoroutine(AnimationTime(animation));
        }
    }

    private IEnumerator AnimationTime(AnimationClip animation) {
        _time = 0f;

        while(true) {
            _time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator PlayAnimation(AnimationClip animation) {
        float timer = 0f;
        float delay = 1f / (float) animation.frameRate;
        float fixedDelay = 1f / (float) animation.frameRate;
        
        while(_loop || _currentFrame < _frames.Count - 1) {

            if((_currentFrame + 1) < _frames.Count) {
                float nextFrameDelay = _frames[_currentFrame + 1].Keyframe.time - _frames[_currentFrame].Keyframe.time;
                delay = nextFrameDelay;
            } else {
                delay = fixedDelay;
            }

            while(timer < delay) {
                timer += Time.deltaTime;
                yield return null;
            }
            while(timer > delay) {
                timer -= delay;
                NextFrame(animation);
            }

            delay = 1f / (float) animation.frameRate;
            
            _renderer.sprite = _frames[_currentFrame].Sprite;
        }

        _playing = false;
    }

    private void NextFrame(AnimationClip animation) {
        _currentFrame++;
        
        if(_currentFrame >= _frames.Count - 1) {
            if(_loop) {
                _currentFrame = 0;
            } else {
                _currentFrame = _frames.Count - 1;
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

    public List<Frame> GetFrames(AnimationClip animation) {
        List<Frame> frames = new List<Frame>();

        foreach(EditorCurveBinding binding in AnimationUtility.GetObjectReferenceCurveBindings(animation)) {
            ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(animation, binding);
        
            foreach(ObjectReferenceKeyframe frame in keyframes) {
                Frame newFrame = new Frame();
                newFrame.Sprite = (Sprite) frame.value;
                newFrame.Keyframe = frame;

                frames.Add(newFrame);
            }
        }
        
        return frames;
    }
}