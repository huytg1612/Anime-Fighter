using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils
{
    private static GameObject BackgroundFade;

    public static void PlaySound(AudioClip clip, AudioSource audioPlayer)
    {
        audioPlayer.Stop();
        audioPlayer.clip = clip;
        audioPlayer.loop = false;
        audioPlayer.time = 0;
        audioPlayer.Play();
    }
    
    public static IEnumerator LoadScene(string scene,float seconds)
    {
        BackgroundFade = GameObject.Find("Background Fade");

        Animator anim = null;
        if(BackgroundFade != null)
        {
            anim = BackgroundFade.GetComponent<Animator>();
        }

        yield return new WaitForSeconds(seconds);
        
        if(anim != null)
        {
            anim.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene(scene);
    }
}
