using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    // Use this for initialization
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {

        bool moved = false;
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        // Disable so our raycast doesnt hit the collider on the object thats trying to move
        boxCollider.enabled = false;

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;

        // Will only execute if the raycast didnt hit anything
        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            moved = true;
        }

        return moved;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPos);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if(hit.transform != null)
        {
            T hitComponent = hit.transform.GetComponent<T>();

            if(!canMove && hitComponent != null)
            {
                OnCantMove<T>(hitComponent);
            }
        }
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;
}
