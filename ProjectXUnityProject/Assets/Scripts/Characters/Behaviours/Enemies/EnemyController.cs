using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State {
    Idle,
    Wait,
    Move,
    Action
    }

    private State actionState;
    private bool myTurn; //used to update the controller. Only updates when true

    // Start is called before the first frame update
    void Start()
    {
        actionState = State.Idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myTurn)
        {
            //
            //perform turn logic here
            //
        }
    }

    public void CombatStart() 
    {
        actionState = State.Wait;
        myTurn = false;
        CombatHandler.StartCombat(gameObject);
    }

    public void MyTurn()
    {
        myTurn = true;
    }

    public void EndTurn()
    {
        myTurn = false;
    }
}
