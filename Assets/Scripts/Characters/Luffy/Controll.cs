using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controll : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D myBody;
    private AnimatorStateInfo animState;
    private AudioSource audioPlayer;

    private GameObject Player2;

    [SerializeField]
    private float forceX, forceY;

    private CameraShake cameraShake;
    private CameraController cameraCon;

    private int jumpTimes = 2,jump = 0;
    private float positionX,positionY;
    private float scaleX,scaleY;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(),GameObject.Find("Player2").GetComponent<Collider2D>());

        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        cameraCon = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        Player2 = GameObject.Find("Player2");

        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        animState = new AnimatorStateInfo();
        audioPlayer = GetComponent<AudioSource>();

        isGrounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;

        positionX = transform.localPosition.x;
        positionY = transform.localPosition.y;

        if (!PlayingAttack())
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.W) && (jump < jumpTimes))
                {
                    myBody.velocity = new Vector2(0, forceY);
                    if (jump == 0)
                    {
                        anim.SetTrigger("Jump");
                    }

                    jump++;
                }
                else
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        if (scaleX > 0)
                        {
                            transform.localScale = new Vector2(scaleX * -1, scaleY);
                        }
                        transform.localPosition = new Vector2(positionX - (forceX * Time.deltaTime), positionY);

                    }
                    else
                    {
                        if (scaleX < 0)
                        {
                            transform.localScale = new Vector2(scaleX * -1, scaleY);
                        }
                        transform.localPosition = new Vector2(positionX + (forceX * Time.deltaTime), positionY);

                    }
                    anim.SetBool("Run", true);
                }

            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetTrigger("Attack_J");
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                anim.SetTrigger("Attack_H");
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                anim.SetTrigger("Attack_U");
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                anim.SetTrigger("Attack_I");
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                anim.SetTrigger("Attack_O");
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Static"))
        {
            Direct();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetDown"))
            {
                cameraShake.Shake(0.1f, 0.5f);
                cameraCon.Resize();

                StartCoroutine(HandleGetUpAnim(1f));
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                anim.SetTrigger("Reset");
            }

            jump = 0;
        }
    }

    private bool PlayingAttack()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }

    public void PlaySound(AudioClip clip)
    {
        GameUtils.PlaySound(clip, audioPlayer);
    }

    private void Direct()
    {
        if (Player2.transform.localPosition.x > positionX)
        {
            if (scaleX < 0)
            {
                transform.localScale = new Vector2(-scaleX, scaleY);
            }
        }
        else if (Player2.transform.localPosition.x < positionX)
        {
            if (scaleX > 0)
            {
                transform.localScale = new Vector2(-scaleX, scaleY);
            }
        }
    }

    IEnumerator HandleGetUpAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetTrigger("GetUp");
    }
}
