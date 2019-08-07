using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStateBehavior : StateMachineBehaviour
{
    protected Controll Fighter;
    protected Controll2 Fighter2;

    private Rigidbody2D FighterBody;

    private GomuPistol PistolScene;

    [SerializeField]
    private AudioClip soundEff;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Fighter == null)
        {
            Fighter = animator.gameObject.GetComponent<Controll>();
        }

        if(Fighter2 == null)
        {
            Fighter2 = animator.gameObject.GetComponent<Controll2>();
        }

        if(soundEff != null)
        {
            if(Fighter != null)
            {
                Fighter.PlaySound(soundEff);
            }

            if(Fighter2 != null)
            {
                Fighter2.PlaySound(soundEff);
            }
        }
        if (Fighter != null)
        {
            FighterAnimatorBegin(Fighter.gameObject,"Player2", animator, stateInfo, layerIndex);
        }
        if (Fighter2 != null)
        {
            FighterAnimatorBegin(Fighter2.gameObject,"Player1", animator, stateInfo, layerIndex);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Fighter != null)
        {
            Fighter.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        if (Fighter2 != null)
        {
            Fighter2.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void FighterAnimatorBegin(GameObject fighter,string ano_player,Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject ano_fighter = GameObject.Find(ano_player);

        FighterBody = fighter.GetComponent<Rigidbody2D>();

        float positionX, postionY,scaleX, scaleY;
        float ano_positionX = 0, ano_postionY = 0, ano_scaleX = 0, ano_scaleY = 0;

        positionX = fighter.transform.localPosition.x;
        postionY = fighter.transform.localPosition.y;
        scaleX = fighter.transform.localScale.x;
        scaleY = fighter.transform.localScale.y;

        if(ano_fighter != null)
        {
            ano_positionX = ano_fighter.transform.localPosition.x;
            ano_postionY = ano_fighter.transform.localPosition.y;
            ano_scaleX = ano_fighter.transform.localScale.x;
            ano_scaleY = ano_fighter.transform.localScale.y;
        }

        //At the begin animator
        if (stateInfo.IsName("Bazooka"))
        {
            if (scaleX > 0)
            {
                fighter.transform.localPosition = new Vector2(positionX + (250 * Time.deltaTime), postionY);
            }else if(scaleX < 0)
            {
                fighter.transform.localPosition = new Vector2(positionX - (250 * Time.deltaTime), postionY);
            }
        }
        else if (stateInfo.IsName("Air_Foot"))
        {
            fighter.GetComponent<Rigidbody2D>().isKinematic = true;

            if(ano_fighter != null)
            {
                fighter.transform.localPosition = new Vector2(ano_positionX, ano_postionY + 4);
            }
        }
    }
}