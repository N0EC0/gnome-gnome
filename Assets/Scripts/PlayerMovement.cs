using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 movement;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameManager.Instance == null ||
            !GameManager.Instance.GameStarted ||
            GameManager.Instance.GameEnded)
        {
            movement = Vector3.zero;
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(
            horizontal,
            0f,
            vertical
        ).normalized;
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}
