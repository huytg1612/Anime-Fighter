using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being_Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float forceX;

    private Animator anim;

    private static int health = 100;
    private float positionX, postionY;
    private float scaleX, scaleY;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("AttackPoint"))
        {
            EffectBeingAttack();

            anim.SetTrigger("BeingAttack");

            health -= 1;
            Debug.Log(health);
        } 
    }

    private void EffectBeingAttack()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;

        positionX = transform.position.x;
        postionY = transform.position.y;

        if(scaleX < 0)
        {
            transform.position = new Vector2(positionX + (forceX * Time.deltaTime), postionY);
        }
        else
        {
            transform.position = new Vector2(positionX - (forceX * Time.deltaTime), postionY);
        }
    }
}
