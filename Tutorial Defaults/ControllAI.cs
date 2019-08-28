using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllAI : MonoBehaviour
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

    public static bool CanControll;
    private float timeInit;

    private bool CanMoving = true;
    private float timeDelay;

    // Start is called before the first frame update
    void Start()
    {
        CanControll = false;

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

        if (character != null)
        {
            characterSpriteRender.sprite = character.characterSprite;
            anim.runtimeAnimatorController = character.controller;
        }

        timeInit = Time.time;
        timeDelay = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Floor(Time.time - timeInit) == 3)
        {
            CanControll = true;
        }

        if (CanControll)
        {
            Direct();

            float distance = Vector2.Distance(transform.localPosition, Player1.transform.localPosition);
            //Debug.Log("DS:  "+distance);

            if (distance <= 2f)
            {
                anim.SetBool("Run", false);

                int attackNum = Random.Range(1, 4);

                if(Time.time - timeDelay >= 1)
                {
                    Attack(attackNum);
                    timeDelay = Time.time;
                }

            }
            else
            {
                //Debug.Log("Time result: " + (Time.time - timeDelay));
                //if (Time.time - timeDelay >= 3f)
                //{
                //    anim.SetBool("Run", false);
                //}
                //else
                //{
                //    Move();
                //    timeDelay = Time.time;
                //}
            }
        }
    }

    private void Attack(int attackNum)
    {
        Debug.Log("Time:" + Time.time);
        switch (attackNum)
        {
            case 1: anim.SetTrigger("Attack_H");
                break;
            case 2: anim.SetTrigger("Attack_K");
                break;
            case 3: anim.SetTrigger("Attack_D_H");
                break;
            case 4: anim.SetTrigger("Attack_D_K");
                break;
        }
    }

    private void Move()
    {
        float x = forceX;

        positionX = transform.localPosition.x;
        positionY = transform.localPosition.y;

        if (!isGrounded)
        {
            x = x / 2;
        }

        float player1_X = Player1.transform.localPosition.x;

        if(transform.localPosition.x > player1_X)
        {
            transform.localPosition = new Vector2(positionX - (x * Time.deltaTime), positionY);
        }else if(transform.localPosition.x < player1_X)
        {
            transform.localPosition = new Vector2(positionX + (x * Time.deltaTime), positionY);
        }

        anim.SetBool("Run", true);
    }

    private IEnumerator MoveDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CanMoving = !CanMoving;
    }

    private void Direct()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;

        if (Player1.transform.localPosition.x > positionX)
        {
            if (scaleX < 0)
            {
                transform.localScale = new Vector2(-scaleX, scaleY);
            }
        }
        else if (Player1.transform.localPosition.x < positionX)
        {
            if (scaleX > 0)
            {
                transform.localScale = new Vector2(-scaleX, scaleY);
            }
        }
    }

    //private void EnergyBarUpdate()
    //{
    //    if (currentEnergy < Energy)
    //    {
    //        currentEnergy += EnergyPerSecond * Time.deltaTime;

    //        if (currentEnergy > Energy)
    //        {
    //            currentEnergy = Energy;
    //        }
    //    }
    //    EnergyBar.fillAmount = currentEnergy / Energy;
    //}

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
}
