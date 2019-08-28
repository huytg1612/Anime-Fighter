using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Being_Attack : MonoBehaviour
{
    public float forceX = 0, forceY = 0;

    public enum ComponentType
    {
        Head = 20 ,Body = 10
    }

    // Start is called before the first frame update
    public ComponentType Component;

    private Animator anim;
    private Rigidbody2D parentBody;

    public static float Health = 1000;
    private static float currentHealth;
    public Image HealthBar;

    private float positionX, postionY;
    private float scaleX, scaleY;

    void Start()
    {
        anim = GetComponentInParent<Animator>();

        currentHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("AttackPoint"))
        {
            float damage = collision.GetComponent<DamageCollider>().Damage;

            if(damage > 0)
            {
                damage += (int)Component;

                //Debug.Log(damage);

                currentHealth -= damage;

                HealthBar.fillAmount = currentHealth / Health;

                if(currentHealth > 0)
                {
                    if (damage >= 50)
                    {
                        anim.SetTrigger("GetDown");
                    }
                    else
                    {
                        anim.SetTrigger("BeingAttack");
                    }
                }
                else
                {
                    if (damage >= 50)
                    {
                        anim.SetTrigger("GetDown");
                    }
                    else
                    {
                        anim.SetTrigger("BeingAttack");
                    }

                    anim.SetTrigger("Dead");
                }

                EffectBeingAttack(damage);

                HealthBarColorUpdate();
            }
        }
        if (collision.gameObject.tag.Equals("AttackObject"))
        {
            currentHealth -= 5;

            anim.SetTrigger("BeingAttack");
        }
        
    }

    private void HealthBarColorUpdate()
    {
        if (HealthBar.fillAmount <= 0.3)
        {
            HealthBar.color = Color.red;
        }
        else if (HealthBar.fillAmount <= 0.6)
        {
            HealthBar.color = Color.yellow;
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
            if(scaleX > 0)
            {
                transform.parent.localPosition = new Vector2(positionX - (forceX * 5 * Time.deltaTime), postionY);
            }else if(scaleX < 0)
            {
                transform.parent.localPosition = new Vector2(positionX + (forceX * 5 * Time.deltaTime), postionY);
            }
        }
        else
        {
            float x = forceX;

            if(scaleX > 0)
            {
                x = -x;
            }

            parentBody = GetComponentInParent<Rigidbody2D>();

            parentBody.velocity = new Vector2(x, forceY);
        }

    }
}
