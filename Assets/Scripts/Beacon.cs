using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    public int toneNum;
    public AudioClip myToneClip;
    AudioSource m_MyAudioSource;
    public Light noteLight;
    public Color noteColor;


    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //anim = gameObject.GetComponent<Animator>();
        //anim["ToneLight"].wrapMode = WrapMode.Once;
        InvokeRepeating("PlayTone", 0.1f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayTone()
    {
        //m_MyAudioSource.Play();
        AnimateLight();
    }

    void AnimateLight()
    {
       // anim.Play("ToneLight");
    }
    
}
