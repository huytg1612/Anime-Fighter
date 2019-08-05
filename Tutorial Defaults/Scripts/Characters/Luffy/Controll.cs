using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controll : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D myBody;

    [SerializeField]
    private float forceX, forceY;

    private float positionX,positionY;
    private float scaleX,scaleY;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();

        isGrounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;

        positionX = transform.localPosition.x;
        positionY = transform.localPosition.y;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                if(scaleX > 0)
                {
                    transform.localScale = new Vector2(scaleX * -1, scaleY);
                }
                transform.localPosition = new Vector2(positionX - (forceX*Time.deltaTime), positionY);

            }
            else
            {
                if(scaleX < 0)
                {
                    transform.localScale = new Vector2(scaleX * -1, scaleY);
                }
                transform.localPosition = new Vector2(positionX + (forceX * Time.deltaTime), positionY);

            }

            anim.SetBool("Run",true);

        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            anim.Play("Galting_Gun");
            anim.Play("Galting_Gun");
            anim.Play("Galting_Gun");
        }
        else if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            myBody.velocity = new Vector2(0,forceY);
            anim.SetTrigger("Jump");
            isGrounded = false;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = true;
        }
    }

    
}
