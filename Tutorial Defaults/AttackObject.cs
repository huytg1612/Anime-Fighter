using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    private Animator anim;
    private bool isHitted = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHitted)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if(transform.localScale.x > 0)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + 10*Time.deltaTime,transform.localPosition.y);
        }
        else
        {
            transform.localPosition = new Vector2(transform.localPosition.x - 10 * Time.deltaTime, transform.localPosition.y);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Wall_Left") || collision.gameObject.tag.Equals("Wall_Right"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag.Equals("Component"))
        {
            anim.SetTrigger("Hit");

            isHitted = true;

            StartCoroutine(DestroyObject(0.5f));
        }
    }

    IEnumerator DestroyObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
