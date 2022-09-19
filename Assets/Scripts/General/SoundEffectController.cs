using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController instance;
    public AudioClip[] audioClips;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudioClip(int index)
    {
        if (audioClips[index] != null)
        {
            AudioSource.PlayClipAtPoint(audioClips[index], Camera.main.transform.position, .25f);
        }
    }
}
