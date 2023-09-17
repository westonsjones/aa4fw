using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created by Weston Jones
public class Monster : MonoBehaviour
{
    public float walkingSpeed = 4.0f;
    public float turnSpeed = 1f;
    public int[] songNotes; // Songs will be a sequence of ints, from 1-8, adhereing to the musical scale of a single octave.
    public int songSize; //Not super necessary but good for design work in the editor.
    public int songIndex = 0; //Used to iterate through the song.
    public int lightIndex = 0; //Used to iterate through the light flashes during notes.
    AudioSource m_MyAudioSource;
    public AudioClip note1, note2, note3, note4, note5, note6, note7, note8; // The different notes of the scale for this creature.
    public AudioClip angry, happy, fighting, death, song; // sounds for various states
    public AudioClip sfx_crunch; //for when they destroy a resource or town.
    public Light noteLight;
    public Color noteColor;
    public bool isMoving = true; //Set to false when the monster is eating resources or other behavior. Otherwise every update it will move.
    public GameObject waypointTarget; //This can be a beacon, another monster, a random wander point, or a resource.
    public bool playerBroadcasting; //True if the player is playing a song on their radio. Used to override beacon behavior.
    public bool isAnimating = false;
    

    //public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
        noteLight = GetComponent<Light>();
        m_MyAudioSource = GetComponent<AudioSource>();
        //anim = GetComponentInChildren<Animator>();
        //anim["ToneLight"].wrapMode = WrapMode.Once;
        InvokeRepeating("FindBeacon", 0.1f, 3.0f);
        InvokeRepeating("SingSong", 0.1f, 10.0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && waypointTarget != null)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, waypointTarget.transform.position, Time.deltaTime * walkingSpeed);
        }
       

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger Enter called)");
        if(other.gameObject.tag == "Monster")
        { MeetMonster(other.gameObject); }
        else if (other.gameObject.tag == "Beacon")
        { 
            BeaconCollide(other.gameObject); }
        
    }

    void MeetMonster(GameObject otherMonster)
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
        
        m_MyAudioSource.clip = song;
        m_MyAudioSource.Play();
        InvokeRepeating("Illuminate", 0.1f, 1.50f);

    }

    void BeaconCollide(GameObject myBeacon)
    {
        Debug.Log("Ran into beacon!");
        if(songIndex < songNotes.Length)
        { songIndex++; }
        else { songIndex = 0; }
        Destroy(myBeacon);

    }

    void Illuminate() //Note light will illuminate behind the monster for a visual as well as auditory cue. VFX stuff.
    {
        switch (songNotes[lightIndex])
        {
            case 1:
                //m_MyAudioSource.clip = note1;
                noteColor = new Color(255, 2, 0, 1); // Red
                break;
            case 2:
                //m_MyAudioSource.clip = note2;
                noteColor = new Color(255, 255, 0, 1);// Yellow
                break;
            case 3:
                //m_MyAudioSource.clip = note3;
                noteColor = new Color(255, 130, 0, 1); //Orange
                break;
            case 4:
                //m_MyAudioSource.clip = note4;
                noteColor = new Color(48, 255, 0, 1); //Green
                break;
            case 5:
                //m_MyAudioSource.clip = note5;
                noteColor = new Color(0, 193, 255, 1); // Light blue
                break;
            case 6:
                //m_MyAudioSource.clip = note6;
                noteColor = new Color(7, 0, 255, 1); //Deep blue
                break;
            case 7:
                //m_MyAudioSource.clip = note7;
                noteColor = new Color(199, 0, 255, 1); //Purple
                break;
            case 8:
                //m_MyAudioSource.clip = note8;
                noteColor = new Color(255, 72, 170, 1); //Pink
                break;
            default: break; //add error noise or something here

        }
        noteLight.color = noteColor;
        if(lightIndex < songNotes.Length -1)
        { lightIndex++; }
        else{
            lightIndex = 0;
            CancelInvoke("Illuminate");
        }
        //InvokeRepeating("PlayTone", 0.1f, 2.0f);
    }

    void PlayTone()
    {
        
    }

    void PulseLight()
    {
        
        
    }
    void FindBeacon()
    {
        //Debug.Log("Find Beacon called");
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Beacon");
        //Debug.Log("Found this many objects with Beacon tag:" + allObjects.Length);
        float nearestDistance = 10000;
        for (int i = 0; i < allObjects.Length; i++)
        {
           
            float distance = Vector3.Distance(this.transform.position, allObjects[i].transform.position);

            if(distance < nearestDistance)
            {
                Beacon myBeacon = allObjects[i].GetComponent<Beacon>();
                //Debug.Log("Tone from beacon is: " + myBeacon.toneNum);
                if (myBeacon.toneNum == songNotes[songIndex])
                {
                    waypointTarget = allObjects[i];
                    nearestDistance = distance;
                    //Debug.Log("Found waypoint with matching tone.");
                }
            }
            else
            {
                //Debug.Log("Not found, iteration:" + i);
            }
        }

    }
    

}
