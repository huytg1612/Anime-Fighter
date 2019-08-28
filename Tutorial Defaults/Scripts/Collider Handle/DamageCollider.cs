using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public float Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Component"))
        {
            StartCoroutine(ResetDamage());
        }
    }

    private IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f);
        Damage = 0;
    }
}
