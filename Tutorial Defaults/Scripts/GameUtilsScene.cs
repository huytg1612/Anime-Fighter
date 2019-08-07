using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtilsScene : MonoBehaviour
{
    private AudioSource audioPlayer;
    [SerializeField]
    private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(clip != null)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        audioPlayer.Stop();
        audioPlayer.clip = clip;
        audioPlayer.loop = true;
        audioPlayer.time = 0;
        audioPlayer.Play();
    }
}
