using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFacesPlayer : MonoBehaviour {
    [SerializeField] private BillboardType billboardType;

    public enum BillboardType { LookAtCamera, CameraForward };

    //Use Late update so everything should have finished moving.
    private void LateUpdate()
    {
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case BillboardType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
    }
}
