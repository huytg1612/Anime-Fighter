using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controll : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer characterSpriteRender;
    private Rigidbody2D myBody;
    private AnimatorStateInfo animState;
    private AudioSource audioPlayer;

    private GameObject Player2;

    [SerializeField]
    private float forceX, forceY;
    private int jumpTimes = 2, jump = 0;

    private CameraShake cameraShake;
    private CameraController cameraCon;
    
    private float currentTime = 0;
    private float positionX,positionY;
    private float scaleX,scaleY;
    private bool isGrounded;

    private Character character;

    [Header("Energy Bar")]
    public static float Energy = 100;
    private static float currentEnergy;
    public float EnergyPerSecond = 10f;
    public Image EnergyBar;

    public static bool CanControll;
    private float timeInit;

    private List<string> listKey;
    private float timePress;
    private string listKeys;

    private int time_Attack_H = 1;

    // Start is called before the first frame update
    void Start()
    {
        CanControll = false;

        currentEnergy = Energy;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(),GameObject.Find("Player2").GetComponent<Collider2D>());

        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        cameraCon = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        Player2 = GameObject.Find("Player2");

        characterSpriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        myBody = GetComponent<Rigidbody2D>();
        animState = new AnimatorStateInfo();
        audioPlayer = GetComponent<AudioSource>();

        isGrounded = true;

        character = CharacterSelect.Player1Selected;

        if(character!= null)
        {
            characterSpriteRender.sprite = character.characterSprite;
            anim.runtimeAnimatorController = character.controller;
        }

        timeInit = Time.time;
        listKey = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Floor(Time.time - timeInit) == 3)
        {
            CanControll = true;
        }

        if (anim.runtimeAnimatorController != null && CanControll)
        {
            scaleX = transform.localScale.x;
            scaleY = transform.localScale.y;

            positionX = transform.localPosition.x;
            positionY = transform.localPosition.y;

            if (true)
            {
                if (Input.anyKeyDown)
                {
                    if (Time.time - timePress > 0.5f)
                    {
                        //listKey.Clear();
                        listKeys = "";
                    }

                    //listKey.Add(Input.inputString.ToUpper());

                    listKeys += Input.inputString.ToUpper();
                    timePress = Time.time;

                    if(FightCombo.CheckCombo(anim, listKeys, isGrounded)){
                        listKeys = "";
                    }

                    //Debug.Log("List Key: " + (listKey.Count));
                    //FightCombo.ComboAnimator(anim, listKey,isGrounded);
                }

                ControllKeyCode();

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    Direct();
                }

                EnergyBarUpdate();
            }
        }
    }


    private void ControllKeyCode()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.J))
        {
            if (Input.GetKeyDown(KeyCode.J) && (jump < jumpTimes))
            {
                myBody.velocity = new Vector2(0, forceY);
                if (jump == 0)
                {
                    anim.SetTrigger("Jump");
                }

                jump++;

                isGrounded = false;
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

                if (Input.GetKey(KeyCode.A))
                {
                    if (scaleX > 0)
                    {
                        transform.localScale = new Vector2(scaleX * -1, scaleY);
                    }

                    transform.localPosition = new Vector2(positionX - (x * Time.deltaTime), positionY);

                }
                else if (Input.GetKey(KeyCode.D))
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
        else if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("Attack_J");
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            if(time_Attack_H == 1)
            {
                anim.SetTrigger("Attack_H");
            }else if (time_Attack_H == 2)
            {
                anim.SetTrigger("Attack_H_2");
            }else if(time_Attack_H == 3)
            {
                anim.SetTrigger("Attack_H_3");
            }

            time_Attack_H++;

            if(time_Attack_H >= 4)
            {
                time_Attack_H = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            if (currentEnergy >= 20)
            {
                anim.SetTrigger("Attack_U");
                currentEnergy -= 20;
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (currentEnergy >= 40)
            {
                anim.SetTrigger("Attack_I");
                currentEnergy -= 40;
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentEnergy >= 40)
            {
                anim.SetTrigger("Attack_O");
                currentEnergy -= 40;
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Attack_K");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentTime = Time.time;
        }
        else
        {
            anim.SetBool("Run", false);
        }
    
    }

    private void EnergyBarUpdate()
    {
        if(currentEnergy < Energy)
        {
            currentEnergy += EnergyPerSecond * Time.deltaTime;

            if(currentEnergy > Energy)
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
                Debug.Log("123");
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
