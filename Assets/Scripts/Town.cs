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
        //Debug.Log("House triggered!");
        if (other.gameObject.tag == "Monster")
        {
            Debug.Log("Monster hit the house, game over!");
            GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
            playerObj[0].GetComponent<Player>().GameOver();
        }
    }

}
