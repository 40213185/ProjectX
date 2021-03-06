using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //stats
    Stats stats;

    public enum State {
    Idle,
    Wait,
    Move,
    Action
    }

    private State actionState;
    private bool myTurn; //used to update the controller. Only updates when true

    public EnemyLibrary.EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
    {
        actionState = State.Idle;
        stats = EnemyLibrary.GetEnemyStats(enemyType);
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
