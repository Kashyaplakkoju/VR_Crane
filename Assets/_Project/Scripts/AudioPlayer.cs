using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource[] audioSources;
    public int delay;
    // Start is called before the first frame update
    void Start()
    {
        audioSources = this.GetComponents<AudioSource>();

        for (int i = 1; i <= audioSources.Length; i++)
        {
            audioSources[i-1].PlayDelayed(delay*i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
