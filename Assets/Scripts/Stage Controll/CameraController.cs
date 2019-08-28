using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    //private GameObject Player1, Player2;

    //private float HalfHeight, HalfWidth;
    //private float Player1_PostionX, Player2_PostionX;

    //private float Cam_PositionX_Max, Cam_PositionX_Min;
    //private bool isResized;

    private Camera myCam;

    public List<Transform> targets;
    public Vector3 offset;

    public float maxZoom = 10f;
    public float minZoom = 40f;
    public float zoomLimiter = 50f;

    public float maxX = 4f, maxY;
    public float minX = -4f, minY;

    public float maxDistance = 18f;

    void Start()
    {
        myCam = GetComponent<Camera>();

        //HalfHeight = myCam.orthographicSize;
        //HalfWidth = myCam.aspect * HalfHeight;

        //Cam_PositionX_Max = myCam.transform.position.x + HalfWidth;
        //Cam_PositionX_Min = myCam.transform.position.x - HalfWidth;

        //isResized = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Resize();

        if(targets.Count == 0)
        {
            return;
        }

        if(GetGreatestDistance() < maxDistance)
        {
            Move();
        }
        //Zoom();
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance()/zoomLimiter);
        myCam.fieldOfView = newZoom;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    void Move()
    {
        Vector3 centerPont = GetCenterPoint();
        Vector3 newPosition = centerPont + offset;

        if(newPosition.x > maxX)
        {
            newPosition.x = maxX;
        }else if(newPosition.x < minX)
        {
            newPosition.x = minX;
        }

        transform.position = newPosition;

    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    //public void Resize()
    //{
    //    Player1_PostionX = Player1.transform.localPosition.x;
    //    Player2_PostionX = Player2.transform.localPosition.x;

    //    float distanceBtwPlayers = Mathf.Abs(Player1.transform.localPosition.x - Player2.transform.localPosition.x);

    //    if (distanceBtwPlayers >= (HalfWidth*2 - HalfWidth/2))
    //    {
    //        myCam.orthographicSize = 8;
    //        transform.localPosition = new Vector3(0, 3, -10);

    //        isResized = true;
    //    }
    //    else
    //    {
    //        myCam.orthographicSize = 5;

    //        if (Player1_PostionX >= Cam_PositionX_Max -1 || Player2_PostionX >= Cam_PositionX_Max - 1)
    //        {
    //            transform.localPosition = new Vector3(transform.localPosition.x + (10 * Time.deltaTime), 0, -10);
    //        }
    //        else if(Player1_PostionX < Cam_PositionX_Min + 1|| Player2_PostionX < Cam_PositionX_Min + 1)
    //        {
    //            transform.localPosition = new Vector3(transform.localPosition.x - (10 * Time.deltaTime), 0, -10);


    //        }
    //        else
    //        {
    //            if (isResized)
    //            {
    //                float newPos = (Player1_PostionX + Player2_PostionX)/2;
    //                if(newPos > 7.65)
    //                {
    //                    newPos = 7.65f;
    //                }else if(newPos < -7.65)
    //                {
    //                    newPos = -7.65f;
    //                }

    //                transform.localPosition = new Vector3(newPos, 0, -10);
    //            }
    //        }

    //        isResized = false;

    //    }
    //    Cam_PositionX_Max = myCam.transform.position.x + HalfWidth;
    //    Cam_PositionX_Min = myCam.transform.position.x - HalfWidth;
    //}
}
