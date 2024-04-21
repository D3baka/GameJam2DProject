using UnityEngine;

public class SpriteBouncer : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;

    [SerializeField]
    private Transform pointB;
    private Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomDirection();

       
           rb.angularVelocity = Random.Range(-200, 200);
      
    }

    void Update()
    {
        MoveSprite();
        CheckBounds();

        // rotate monke randomly
       
    }

    // Set a random direction
    void SetRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // Random angle in radians
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // Convert angle to unit vector
    }

    // Move the sprite in the random direction
    void MoveSprite()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Check if the sprite is out of bounds and reset if necessary
    void CheckBounds()
    {
        if (transform.position.x < Mathf.Min(pointA.position.x, pointB.position.x) || transform.position.x > Mathf.Max(pointA.position.x, pointB.position.x) ||
            transform.position.y < Mathf.Min(pointA.position.y, pointB.position.y) || transform.position.y > Mathf.Max(pointA.position.y, pointB.position.y))
        {
            // change direction back to the center
            direction = Vector3.Normalize(new Vector3(0, 0, 0) - gameObject.transform.position);
            rb.angularVelocity = Random.Range(-200, 200);

        }
    }
}
