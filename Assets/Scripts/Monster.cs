using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float walkingSpeed = 10.0f;
    public float turnSpeed = 1f;
    public int[] songNotes;

    public GameObject waypointTarget; //This can be a beacon, another monster, a random wander point, or a resource.

    // Start is called before the first frame update
    void Start()
    {
        
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
}
