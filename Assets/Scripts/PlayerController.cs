using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _verticalSpeed;

    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private Rigidbody2D _rb;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddForceX(_verticalSpeed * -0.1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rb.AddForceX(_verticalSpeed * 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForceY(_jumpSpeed * 0.1f);
        }
    }
}
