using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Player1, Player2;

    private float HalfHeight, HalfWidth;
    private float Player1_PostionX, Player2_PostionX;

    private float Cam_PositionX_Max, Cam_PositionX_Min;
    private bool isResized;

    private Camera myCam;

    void Start()
    {
        myCam = GetComponent<Camera>();

        HalfHeight = myCam.orthographicSize;
        HalfWidth = myCam.aspect * HalfHeight;

        Cam_PositionX_Max = myCam.transform.position.x + HalfWidth;
        Cam_PositionX_Min = myCam.transform.position.x - HalfWidth;

        isResized = false;
    }

    // Update is called once per frame
    void Update()
    {
        Resize();

    }

    private void Resize()
    {
        Player1_PostionX = Player1.transform.localPosition.x;
        Player2_PostionX = Player2.transform.localPosition.x;

        float distanceBtwPlayers = Mathf.Abs(Player1.transform.localPosition.x - Player2.transform.localPosition.x);

        if (distanceBtwPlayers >= (HalfWidth*2 - HalfWidth/2))
        {
            myCam.orthographicSize = 8;
            transform.localPosition = new Vector3(0, 3, -10);

            isResized = true;
        }
        else
        {
            myCam.orthographicSize = 5;

            if (Player1_PostionX >= Cam_PositionX_Max -1 || Player2_PostionX >= Cam_PositionX_Max - 1)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + (10 * Time.deltaTime), 0, -10);

                Cam_PositionX_Max = myCam.transform.position.x + HalfWidth;
                Cam_PositionX_Min = myCam.transform.position.x - HalfWidth;
            }
            else if(Player1_PostionX < Cam_PositionX_Min + 1|| Player2_PostionX < Cam_PositionX_Min + 1)
            {
                transform.localPosition = new Vector3(transform.localPosition.x - (10 * Time.deltaTime), 0, -10);

                Cam_PositionX_Max = myCam.transform.position.x + HalfWidth;
                Cam_PositionX_Min = myCam.transform.position.x - HalfWidth;
            }
            else
            {
                if (isResized)
                {
                    float newPos = (Player1_PostionX + Player2_PostionX)/2;
                    if(newPos > 7.65)
                    {
                        newPos = 7.65f;
                    }else if(newPos < -7.65)
                    {
                        newPos = -7.65f;
                    }

                    transform.localPosition = new Vector3(newPos, 0, -10);

                    Cam_PositionX_Max = myCam.transform.position.x + HalfWidth;
                    Cam_PositionX_Min = myCam.transform.position.x - HalfWidth;
                }
            }

            isResized = false;

        }
    }
}
