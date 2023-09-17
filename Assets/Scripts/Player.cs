using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Credit to https://sharpcoderblog.com/blog/unity-3d-fps-controller for initial template. Weston Jones further modified for the Austin IDGA Game Jam in September 2023.
[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public int radioSelection = 0; //0-7 for the 8 tones and selections.
    public bool broadcasting = false; //whether or not the player is actively putting out a tone
    public GameObject beaconPrefab;
    public bool beaconOnCooldown = false; //If false, player can place a beacon.
    public Color noteColor;
    public GameObject toneSphere;
    AudioSource m_MyAudioSource;
    public AudioClip note1, note2, note3, note4, note5, note6, note7, note8;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        m_MyAudioSource = GetComponent<AudioSource>();
        InvokeRepeating("PlayTone", 1.0f, 6.0f);
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetMouseButtonDown(0)) // Left click, turn on radio.
        {
            if (broadcasting)
            { broadcasting = false; }
            else { broadcasting = true; }
        }

        if (Input.GetMouseButtonDown(1)) // Placing Beacon, right click.
        {
            if (!beaconOnCooldown)
            {
                beaconOnCooldown = true;
                Debug.Log("Beacononcool down set to: " + beaconOnCooldown);
                PlaceBeacon();
                
            }
            StartCoroutine(Cooldown());
            
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if(radioSelection<7)
            { radioSelection++; }
            else { radioSelection = 0; }//reset to beginning
            UpdateLightAndSound();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (radioSelection > 0)
            { radioSelection--; }
            else { radioSelection = 7; }
        }


    }

    void PlaceBeacon()
    {
        Beacon myBeacon = (Instantiate(beaconPrefab, transform.position + new Vector3(0, 0, 10), Quaternion.identity)).GetComponent<Beacon>();
        myBeacon.SetTone(radioSelection + 1);
    }

    void PlayTone()
    {
        if(broadcasting)
        {
            m_MyAudioSource.Play();
        }
    }

    void UpdateLightAndSound()
    {
        switch (radioSelection+1)
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

        
        toneSphere.GetComponent<Renderer>().material.SetColor("_Albedo", noteColor);
        toneSphere.GetComponent<Renderer>().material.SetColor("_EmissionColor", noteColor);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5);
        beaconOnCooldown = false;
        Debug.Log("Cooldown over");
    }

}