using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider volumeSlider; // The UI slider to adjust volume
    private AudioSource audioSource;

    // Singleton pattern to maintain a single instance of BackgroundMusic across scenes
    private static Music instance = null;
    public static Music Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        // Maintain a single instance of BackgroundMusic
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Make sure the slider updates the volume when its value changes
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        }
    }

    void UpdateVolume()
    {
        if (audioSource != null && volumeSlider != null)
        {
            audioSource.volume = volumeSlider.value;
        }
    }
}
