using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour, IToneStuff
{
    public int toneNum;
    public AudioClip myToneClip;
    AudioSource m_MyAudioSource;
    public Light noteLight;
    public Color noteColor;
    public AudioClip note1, note2, note3, note4, note5, note6, note7, note8;
    public GameObject toneSphere;

    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //anim = gameObject.GetComponent<Animator>();
        //anim["ToneLight"].wrapMode = WrapMode.Once;
        m_MyAudioSource = GetComponent<AudioSource>();
        InvokeRepeating("PlayTone", 1.0f, 6.0f);
        m_MyAudioSource.clip = myToneClip;
        
        UpdateLightAndSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Beacon triggered collision.");

    }


    void PlayTone()
    {
        m_MyAudioSource.Play();
        //Debug.Log("Tone played.");
        AnimateLight();
    }

    void AnimateLight()
    {
       // anim.Play("ToneLight");
    }

    public void Destroy()
    {

    }

    public void UpdateLightAndSound()
    {
        Debug.Log("Updating light and sound");
        switch (toneNum)
        {
            case 1:
                m_MyAudioSource.clip = note1;
                noteColor = new Color(255, 2, 0, 1); // Red
                break;
            case 2:
                m_MyAudioSource.clip = note2;
                noteColor = new Color(255, 255, 0, 1);// Yellow
                break;
            case 3:
                m_MyAudioSource.clip = note3;
                noteColor = new Color(255, 130, 0, 1); //Orange
                break;
            case 4:
                m_MyAudioSource.clip = note4;
                noteColor = new Color(48, 255, 0, 1); //Green
                break;
            case 5:
                m_MyAudioSource.clip = note5;
                noteColor = new Color(0, 193, 255, 1); // Light blue
                break;
            case 6:
                m_MyAudioSource.clip = note6;
                noteColor = new Color(7, 0, 255, 1); //Deep blue
                break;
            case 7:
                m_MyAudioSource.clip = note7;
                noteColor = new Color(199, 0, 255, 1); //Purple
                break;
            case 8:
                m_MyAudioSource.clip = note8;
                noteColor = new Color(255, 72, 170, 1); //Pink
                break;
            default: break; //add error noise or something here

        }

        noteLight.color = noteColor;
        toneSphere.GetComponent<Renderer>().material.SetColor("_Albedo", noteColor);
        toneSphere.GetComponent<Renderer>().material.SetColor("_EmissionColor", noteColor);

    }

    public void SetTone(int toneSetter)
    {
        toneNum = toneSetter;
        UpdateLightAndSound();


        
    }

    
    
}
