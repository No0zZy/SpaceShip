using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_AudioSource.volume = AuidoSettings.Instance.AudioValue;
    }

    public void PlayAudioClip(AudioClip clip)
    {
        m_AudioSource.PlayOneShot(clip);
    }

    public void UpdatePitch(float pitch)
    {
        m_AudioSource.pitch = pitch;
    }
}
