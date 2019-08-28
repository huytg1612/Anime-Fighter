using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCombo : MonoBehaviour
{

    //private static string[] combo1 = { "D", "H" };
    //private static string[] combo2 = { "D", "K" };
    //private static string[] combo3 = { "S", "H" };
    //private static string[] combo4 = { "S", "K" };
    //private static string[] combo6 = { "D", "D","S","S","H","K" };

    private static string combo_1 = "DH";
    private static string combo_2 = "DK";
    private static string combo_3 = "SH";
    private static string combo_4 = "SK";
    //private static string combo_5 = "DD";
    private static string combo_6 = "DDSSWHK";

    //public static void ComboAnimator(Animator anim, List<string> listKey,bool isGround) {
    //    //Debug.Log("------------");
    //    //foreach (string key in listKey){
    //    //    Debug.Log(key);
    //    //}

    //    if (Compare(combo6, listKey))
    //    {
    //        SceneUltimate.SetActive(true);
    //        anim.SetTrigger("Attack_Ultimate");
    //    }

    //    if (Compare(combo1, listKey))
    //    {
    //        anim.SetTrigger("Attack_D_H");
    //    }
    //    else if (Compare(combo2, listKey))
    //    {
    //        anim.SetTrigger("Attack_D_K");
    //    }else if (Compare(combo3, listKey))
    //    {
    //        anim.SetTrigger("Attack_S_H");
    //    }else if (Compare(combo4, listKey))
    //    {
    //        if (!isGround)
    //        {
    //            anim.SetTrigger("Attack_J_S_K");
    //        }
    //    }
    //}

    private static bool Compare(string[] combo,List<string> listKey)
    {
        if(combo.Length != listKey.Count)
        {
            return false;
        }

        int i = 0;

        foreach(string key in listKey)
        {
            if (!combo[i].Equals(key))
            {
                return false;
            }
            i++;
        }

        return true;
    }

    public static bool CheckCombo(Animator anim,string listKey,bool isGround)
    {
        bool isPlayed = false;

        //Debug.Log(listKey.Length);

        if (listKey.Contains(combo_6))
        {
            anim.SetTrigger("Attack_Ultimate");

            isPlayed = true;
        }

        if (listKey.Contains(combo_1))
        {
            anim.SetTrigger("Attack_D_H");

            isPlayed = true;
        }
        else if (listKey.Contains(combo_2))
        {
            anim.SetTrigger("Attack_D_K");

            isPlayed = true;
        }
        else if (listKey.Contains(combo_3))
        {
            anim.SetTrigger("Attack_S_H");

            isPlayed = true;
        }
        //}else if (listKey.Contains(combo_5))
        //{
        //    anim.SetTrigger("Dash");
        //}

        return isPlayed;
    }
}