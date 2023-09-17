using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    new Renderer rend;
    Material mat;


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
        rend = toneSphere.GetComponent<Renderer>();
        mat = toneSphere.GetComponent<Renderer>().material;

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
            if (!broadcasting)
            { broadcasting = true; }
            else { broadcasting = false; }
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
            UpdateLightAndSound();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
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
       // Debug.Log("Update Light and Sound called.");
        int sel = radioSelection + 1;
        switch (sel)
        {
            
            case 1:
                //Debug.Log("Case 1 called");
                m_MyAudioSource.clip = note1;
                noteColor = Color.red; // Red
                Debug.Log(noteColor.ToString());
                break;
            case 2:
                m_MyAudioSource.clip = note2;
                noteColor = new Color(1.0f, 0.45f, 0f, 1);//Orange
                break;
            case 3:
                m_MyAudioSource.clip = note3;
                noteColor = new Color(1f, 1f, 0f, 1); //Yellow
                break;
            case 4:
                m_MyAudioSource.clip = note4;
                noteColor = new Color(0, 1f, 0, 1); //Green
                break;
            case 5:
                m_MyAudioSource.clip = note5;
                noteColor = new Color(0, 1f, 1f, 1); // Light blue
                break;
            case 6:
                m_MyAudioSource.clip = note6;
                noteColor = new Color(0f, 0f, 1f, 1); //Deep blue
                break;
            case 7:
                m_MyAudioSource.clip = note7;
                noteColor = new Color(0.5f, 0f, 1f, 1); //Purple
                break;
            case 8:
                m_MyAudioSource.clip = note8;
                noteColor = new Color(1f, 0f, 1f, 1); //Pink
                break;
            default: break; //add error noise or something here

        }

        //mat.EnableKeyword("_EMISSION");
        //mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        mat.SetColor("_Color", noteColor);
       // mat.SetColor("_EmissionColor", noteColor * 10);
       // RendererExtensions.UpdateGIMaterials(GetComponent<Renderer>());

        // Inform Unity's GI system to recalculate GI based on the new emission map.
        //DynamicGI.SetEmissive(GetComponent<Renderer>(), noteColor * 10);
        //DynamicGI.UpdateEnvironment();
       // Debug.Log("Update Light and Sound finished.");
    }

    public void GameOver()
    {
        //Add game over text that tells you you lost and why. (City run over).
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver");

    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5);
        beaconOnCooldown = false;
        Debug.Log("Cooldown over");
    }

}