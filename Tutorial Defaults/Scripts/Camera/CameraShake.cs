using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float shakeAmount = 0;
    private float defaultX, defaultY, defaultZ;

    private void Awake()
    {
        defaultX = transform.position.x;
        defaultY = transform.position.y;
        defaultZ = transform.position.z;
    }

    private void Update()
    {

    }

    public void Shake(float amt,float length)
    {
        shakeAmount = amt;

        transform.position = new Vector3(transform.position.x, defaultY, defaultZ);

        defaultX = transform.position.x;
        defaultY = transform.position.y;
        defaultZ = transform.position.z;

        //Debug.Log("Camera: "+ defaultX+","+defaultY+","+defaultY);

        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = transform.position;

            //float shakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
            float shakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;

            //camPos.x += shakeAmtX;
            camPos.y += shakeAmtY;

            transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        transform.position = new Vector3(transform.position.x,defaultY,defaultZ);
    }
}
