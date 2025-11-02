using UnityEngine;

public class MovingEnemyPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition = pointB.position;
        FaceDirection(nextPosition);
    }

    void Update()
    {
        // Move the platform
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        // If reached the target, switch direction
        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;

            // Flip the object
            FaceDirection(nextPosition);
        }
    }

    void FaceDirection(Vector3 target)
    {
        Vector3 scale = transform.localScale;

        if (target == pointA.position)
            scale.x = -Mathf.Abs(scale.x); // face left
        else
            scale.x = Mathf.Abs(scale.x);  // face right

        transform.localScale = scale;
    }

}
