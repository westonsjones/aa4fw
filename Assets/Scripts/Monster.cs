using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created by Weston Jones
public class Monster : MonoBehaviour
{
    public float walkingSpeed = 10.0f;
    public float turnSpeed = 1f;
    public int[] songNotes; // Songs will be a sequence of ints, from 1-8, adhereing to the musical scale of a single octave.
    public int songSize; //Not super necessary but good for design work in the editor.
    public int songIndex = 0; //Used to iterate through the song.
    AudioSource m_MyAudioSource;
    public AudioClip note1, note2, note3, note4, note5, note6, note7, note8; // The different notes of the scale for this creature.
    public AudioClip angry, happy, fighting, death; // sounds for various states
    public AudioClip sfx_crunch; //for when they destroy a resource or town.
    public Light noteLight;

    public GameObject waypointTarget; //This can be a beacon, another monster, a random wander point, or a resource.

    // Start is called before the first frame update
    void Start()
    {
        noteLight = GetComponent<Light>();
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MeetMonster(Monster otherMonster)
    {
        if(otherMonster != null)
        {
            //VFX calls go here
            //SFX calls go here
            //Remember to delay long enough for them to finish OR make them spawn on their own before destroying gameobject
            Destroy(gameObject);


        }
    }

    void SingSong()
    {
        switch(songIndex)
        {
            case 1: m_MyAudioSource.clip = note1;
                break;
            case 2: m_MyAudioSource.clip = note2;
                break;
            case 3: m_MyAudioSource.clip = note3;
                break;
            case 4: m_MyAudioSource.clip = note4;
                break;
            case 5: m_MyAudioSource.clip = note5;
                break;
            case 6: m_MyAudioSource.clip = note6;
                break;
            case 7: m_MyAudioSource.clip = note7;
                break;
            case 8: m_MyAudioSource.clip = note8;
                break;
            default: break; //add error noise or something here

        }

        m_MyAudioSource.Play();
        Illuminate();

    }

    void Illuminate() //Note light will illuminate behind the monster for a visual as well as auditory cue. VFX stuff.
    {

    }
}
