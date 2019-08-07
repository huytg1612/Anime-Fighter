using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controll2 : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D myBody;
    private AnimatorStateInfo animState;
    private AudioSource audioPlayer;

    [SerializeField]
    private float forceX, forceY;

    private float positionX, positionY;
    private float scaleX, scaleY;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
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
            if (Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow))
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
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.Play("Galting_Gun");
                anim.Play("Galting_Gun");
                anim.Play("Galting_Gun");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                myBody.velocity = new Vector2(0, forceY);
                anim.SetTrigger("Jump");
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("Attack");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                anim.SetTrigger("Attack_U");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                anim.SetTrigger("Attack_I");
            }else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                anim.SetTrigger("Attack_O");
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = true;
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
}
