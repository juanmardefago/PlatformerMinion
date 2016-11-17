using UnityEngine;
using System.Collections;

public class EnemyPatrolMovement : MonoBehaviour {

    private Rigidbody2D rBody;
    public Transform[] points;
    private int currentPoint;
    public float speed;
    private bool canMove;
    public float platformStopDelay;
    private int lastCurrentPointDirChange;
    private bool isDead;

    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        canMove = true;
        lastCurrentPointDirChange = DirectionTowardsCurrentPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rBody.position.x != points[currentPoint].position.x && canMove && !isDead)
        {
            rBody.MovePosition(Vector2.MoveTowards(rBody.position, points[currentPoint].position, speed * Time.deltaTime));
        }
        else if(!isDead)
        {
            canMove = false;
            if (rBody.position.x == points[currentPoint].position.x) StartCoroutine(ChangeCurrentPoint());
        }
    }

    private void CorrectLocalScale()
    {
        if(lastCurrentPointDirChange != DirectionTowardsCurrentPoint())
        {
            FlipScale();
            lastCurrentPointDirChange = DirectionTowardsCurrentPoint();
        }
    }

    private void FlipScale()
    {
        Vector3 scale = transform.localScale;
        scale.x = scale.x * -1;
        transform.localScale = scale;
    }

    private int DirectionTowardsCurrentPoint()
    {
        int res = 0;
        if (points[currentPoint].position.x > rBody.position.x)
        {
            res = 1;
        }
        else if (points[currentPoint].position.x < rBody.position.x)
        {
            res = -1;
        }
        return res;
    }

    private IEnumerator ChangeCurrentPoint()
    {
        currentPoint = (currentPoint + 1) % points.Length;
        yield return new WaitForSeconds(platformStopDelay);
        canMove = true;
        CorrectLocalScale();
    }

    public void Die()
    {
        rBody.isKinematic = true;
        isDead = true;
    }
}
