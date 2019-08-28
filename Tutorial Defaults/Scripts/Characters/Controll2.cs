using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controll2 : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer characterSpriteRender;
    private Rigidbody2D myBody;
    private AnimatorStateInfo animState;
    private AudioSource audioPlayer;

    private GameObject Player1;

    [SerializeField]
    private float forceX, forceY;
    private int jumpTimes = 2, jump = 0;

    private CameraShake cameraShake;
    private CameraController cameraCon;

    private float currentTime = 0;
    private float positionX, positionY;
    private float scaleX, scaleY;
    private bool isGrounded;

    private Character character;

    [Header("Energy Bar")]
    public static float Energy = 100;
    private static float currentEnergy;
    public float EnergyPerSecond = 10f;
    public Image EnergyBar;

    public static bool CanMove;
    private float timeInit;

    // Start is called before the first frame update
    void Start()
    {
        CanMove = false;

        currentEnergy = Energy;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Player1").GetComponent<Collider2D>());

        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        cameraCon = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        characterSpriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        myBody = GetComponent<Rigidbody2D>();
        animState = new AnimatorStateInfo();
        audioPlayer = GetComponent<AudioSource>();

        Player1 = GameObject.Find("Player1");

        isGrounded = true;

        character = CharacterSelect.Player2Selected;

        if(character != null)
        {
            characterSpriteRender.sprite = character.characterSprite;
            anim.runtimeAnimatorController = character.controller;
        }

        timeInit = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Floor(Time.time - timeInit) == 3)
        {
            CanMove = true;
        }

        if(anim.runtimeAnimatorController != null && CanMove)
        {
            scaleX = transform.localScale.x;
            scaleY = transform.localScale.y;

            positionX = transform.localPosition.x;
            positionY = transform.localPosition.y;

            if (!StateStatic())
            {
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (Input.GetKeyDown(KeyCode.Alpha2) && (jump < jumpTimes))
                    {
                        myBody.velocity = new Vector2(0, forceY);

                        if (jump == 0)
                        {
                            anim.SetTrigger("Jump");

                        }
                        isGrounded = false;

                        jump++;
                    }
                    else
                    {
                        float x = forceX;

                        if (!isGrounded)
                        {
                            x = x / 2;
                        }
                        else
                        {
                            x = forceX;
                        }

                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            if (scaleX > 0)
                            {
                                transform.localScale = new Vector2(scaleX * -1, scaleY);
                            }
                            transform.localPosition = new Vector2(positionX - (x * Time.deltaTime), positionY);

                        }
                        else if (Input.GetKey(KeyCode.RightArrow))
                        {
                            if (scaleX < 0)
                            {
                                transform.localScale = new Vector2(scaleX * -1, scaleY);
                            }
                            transform.localPosition = new Vector2(positionX + (x * Time.deltaTime), positionY);

                        }

                        anim.SetBool("Run", true);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    anim.SetTrigger("Attack_J");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    anim.SetTrigger("Attack_H");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    if (currentEnergy >= 20)
                    {
                        anim.SetTrigger("Attack_U");
                        currentEnergy -= 20;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    if (currentEnergy >= 40)
                    {
                        anim.SetTrigger("Attack_I");
                        currentEnergy -= 40;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    if (currentEnergy >= 40)
                    {
                        anim.SetTrigger("Attack_O");
                        currentEnergy -= 40;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (Time.time - currentTime <= 1 && currentTime > 0)
                    {
                        anim.SetTrigger("Attack_W_K");
                    }
                    else
                    {
                        anim.SetTrigger("Attack_K");
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentTime = Time.time;
                }
                else
                {
                    anim.SetBool("Run", false);
                }
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Direct();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("High_Kick2"))
            {
                cameraShake.Shake(0.1f, 0.5f);
            }

            EnergyBarUpdate();
        }
    }

    private void EnergyBarUpdate()
    {
        if (currentEnergy < Energy)
        {
            currentEnergy += EnergyPerSecond * Time.deltaTime;

            if (currentEnergy > Energy)
            {
                currentEnergy = Energy;
            }
        }
        EnergyBar.fillAmount = currentEnergy / Energy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = true;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetDown"))
            {

                cameraShake.Shake(0.1f, 0.5f);

                anim.SetTrigger("Lie");
                StartCoroutine(HandleGetUpAnim(1f));
                
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Air"))
            {
                anim.SetTrigger("Reset");
            }

            jump = 0;
        }
    }

    private bool StateStatic()
    {

        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Static")
            || anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead");
    }

    public void PlaySound(AudioClip clip)
    {
        GameUtils.PlaySound(clip, audioPlayer);
    }

    private void Direct()
    {
        if(Player1.transform.localPosition.x > positionX)
        {
            if(scaleX < 0)
            {
                transform.localScale = new Vector2(-scaleX, scaleY);
            }
        }else if(Player1.transform.localPosition.x < positionX)
        {
            if(scaleX > 0)
            {
                transform.localScale = new Vector2(-scaleX, scaleY);
            }
        }
    }

    IEnumerator HandleGetUpAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetAllTriggers();
        anim.SetTrigger("GetUp");
    }

    private void ResetAllTriggers()
    {
        anim.ResetTrigger("Lie");
        anim.ResetTrigger("GetUp");
    }

    //private void OnBecameInvisible()
    //{
    //    Debug.Log("Player1 is out of camera");
    //}
}
