using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator animator;
    // Stores the players location
    private Transform target;

    // Used to make the enemy move every other turn
    private bool skipMove;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }
    
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if(skipMove)
        {
            skipMove = false;
        }
        else
        {
            base.AttemptMove<T>(xDir, yDir);
            skipMove = true;
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger("enemyAttack");

        hitPlayer.LoseFood(playerDamage);
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        // Check if the player is in the same column as enemy
        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            if(target.position.y > transform.position.y)
            {
                yDir = 1;
            }
            else
            {
                yDir = -1;
            }
        }
        else
        {
            if(target.position.x > transform.position.x)
            {
                xDir = 1;
            }
            else
            {
                xDir = -1;
            }
        }

        AttemptMove<Player>(xDir, yDir);
    }
}