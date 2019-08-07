using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being_Attack : MonoBehaviour
{
    public enum ComponentType
    {
        Head = 20 ,Front = 10 ,Back = 15
    }

    // Start is called before the first frame update
    public ComponentType Component;

    [SerializeField]
    private float forceX, GetDown;

    private Animator anim;
    private Rigidbody2D parentBody;

    private static int health = 100;
    private float positionX, postionY;
    private float scaleX, scaleY;

    private string typeComponent;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("AttackPoint"))
        {
            float damage = collision.gameObject.GetComponent<DamageCollider>().Damage;
            damage += (int)Component;

            EffectBeingAttack(damage);

            anim.SetTrigger("BeingAttack");
        } 
    }

    private void EffectBeingAttack(float damage)
    {
        scaleX = transform.parent.localScale.x;
        scaleY = transform.parent.localScale.y;

        positionX = transform.parent.localPosition.x;
        postionY = transform.parent.localPosition.y;

        if(damage < 50)
        {
            if (Component.ToString().Equals("Front"))
            {
                transform.parent.localPosition = new Vector2(positionX + (50 * Time.deltaTime), postionY);
            }
            else if (Component.ToString().Equals("Back"))
            {
                transform.parent.localPosition = new Vector2(positionX - (50 * Time.deltaTime), postionY);
            }
        }
        else
        {
            parentBody = GetComponentInParent<Rigidbody2D>();

            parentBody.velocity = new Vector2(7, 10);
        }

    }
}
