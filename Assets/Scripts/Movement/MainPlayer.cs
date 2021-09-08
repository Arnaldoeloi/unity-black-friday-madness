using UnityEngine;
using System.Collections;
//using UnityEngine.UI;


public class MainPlayer : MonoBehaviour
{
    [SerializeField] private MoveSettings _settings = null;

    [HideInInspector]
    public Vector3 _moveDirection;
    private CharacterController _controller;

    private Rigidbody rb;
    private bool canDash = true;

    public Gun gun = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        DefaultMovement();
    }

    private void FixedUpdate()
    {
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    private void DefaultMovement()
    {
        if (_controller.isGrounded)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (input.x != 0 && input.y != 0)
            {
                input *= 0.777f;
            }

            _moveDirection.x = input.x * _settings.speed;
            _moveDirection.z = input.y * _settings.speed;
            _moveDirection.y = -_settings.antiBump;

            _moveDirection = transform.TransformDirection(_moveDirection);

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                if (canDash) {
                    StartCoroutine(Dash());
                }
            }
        }
        else
        {
            _moveDirection.y -= _settings.gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        _moveDirection.y += _settings.jumpForce;
    }

    private IEnumerator Dash()
    {
        //_moveDirection += Camera.main.transform.forward * _settings.dashForce;

        rb.AddForce(transform.forward * _settings.dashForce, ForceMode.Impulse);
        //rb.velocity = transform.forward * _settings.dashForce;

        yield return new WaitForSeconds(0.2f);

        //Debug.Log("DashEnd");
        //rb.velocity = Vector3.zero;
        StartCoroutine(PauseDash());
    }

    private IEnumerator PauseDash()
    {
        rb.velocity = Vector3.zero;
        canDash = false;

        yield return new WaitForSeconds(5);

        canDash = true;
    }
}