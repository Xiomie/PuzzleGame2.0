using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundVFX : MonoBehaviour
{
    public AudioClip gunShotClip; // The AudioClip of the gunshot sound
    public Slider volumeSlider; // The UI Slider for adjusting the volume
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
            volumeSlider.value = audioSource.volume;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            PlayGunShotSound();
        }
    }

    void PlayGunShotSound()
    {
        if (audioSource != null && gunShotClip != null)
        {
            audioSource.PlayOneShot(gunShotClip);
        }
    }

    void UpdateVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }
}