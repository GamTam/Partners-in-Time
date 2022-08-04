using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CustomAnimator : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;

    private AnimationClip _previousClip;

    private int _currentFrame = 0;
    private bool _debounce = false;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void Play(string clipName) {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

        if(_previousClip && _previousClip.name.Split("_")[1] != clipName.Split("_")[1]) {
            _currentFrame = 0;
        }

        int i = 0;
        bool found = false;

        while(i < clips.Length && !found) {
            if(clips[i].name == clipName) {
                found = true;
                _previousClip = clips[i];

                if(!_debounce) {
                    StartCoroutine(PlayClip(clips[i]));
                } else {
                    List<Sprite> sprites = GetSpritesFromClip(clips[i]);

                    _renderer.sprite = sprites[_currentFrame];
                }
            }
            i++;
        }
    }

    private IEnumerator PlayClip(AnimationClip clip) {
        _debounce = true;

        List<Sprite> sprites = GetSpritesFromClip(clip);

        try {
            _renderer.sprite = sprites[_currentFrame];
        }
        catch (Exception e) {
            
        }
        
        _currentFrame++;

        if(_currentFrame >= sprites.Count) {
            _currentFrame = 0;
        }
        
        yield return new WaitForSeconds(1f / (clip.frameRate * 1.25f));
        _debounce = false;
    }

    private List<Sprite> GetSpritesFromClip(AnimationClip clip) {
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
