using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    private Vector2 currentDirection = Vector2.zero;
    private PlayerControl playerControl;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControl = new PlayerControl();

    }

    void Start()
    {
        playerControl.Enable();
    }

    // Update is called once per frame

    private void Update()
    {
        currentDirection = playerControl.Player.Movement.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(currentDirection.x, 0, currentDirection.y);
        Quaternion forwardsRotation = Quaternion.LookRotation(transform.forward);
        Quaternion directionRotation = Quaternion.LookRotation(direction);


        rb.MovePosition(transform.position + (direction * speed * Time.fixedDeltaTime));

        Quaternion rotation = Quaternion.RotateTowards(forwardsRotation, directionRotation, angularSpeed * Time.fixedDeltaTime);

        if (currentDirection != Vector2.zero)
            rb.MoveRotation(rotation);
    }
}
