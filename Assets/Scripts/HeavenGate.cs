using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeavenGate : MonoBehaviour
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
            Debug.Log("Victory!");
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("VictoryScreen");

        }
    }
}
