using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{

    public int DAMAGE = 1;
    public int POINTS_PER_FOOD = 10;
    public int POINTS_PER_SODA = 20;
    public float RESTART_LEVEL_DELAY = 0.1f;
    public Text foodText;
    public int food;

    private Animator animator;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;        

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    private void Update()
    {
        if (GameManager.instance.playersTurn)
        {
            int horizontal = 0;
            int vertical = 0;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
            {
                vertical = 0;
            }

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Wall>(horizontal, vertical);
            }
        }
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            enabled = false;
            Invoke("Restart", RESTART_LEVEL_DELAY);
        }
        else if(other.tag == "Food")
        {
            food += POINTS_PER_FOOD;
            foodText.text = "+" + POINTS_PER_FOOD + " Food: " + food;

            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Soda")
        {
            food += POINTS_PER_SODA;
            foodText.text = "+" + POINTS_PER_SODA + " Food: " + food;

            other.gameObject.SetActive(false);
        }
    }

    private void CheckIfGameOver()
    {
        if(food <= 0)
        {
            food = GameManager.instance.basePlayerFood;
            GameManager.instance.GameOver();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoseFood(int loss)
    {
        foodText.text = "-" + loss + " Food: " + food;
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;
        CheckIfGameOver();

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(DAMAGE);
        animator.SetTrigger("playerChop");
    }

}
