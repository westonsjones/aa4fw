using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        { 
            GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
            playerObj[0].GetComponent<Player>().GameOver();
        }
    }

}
