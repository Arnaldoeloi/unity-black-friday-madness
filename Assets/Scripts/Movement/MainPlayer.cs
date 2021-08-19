using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class MainPlayer: MonoBehaviour
{
    [SerializeField] private MoveSettings _settings = null;

    [HideInInspector]
    public Vector3 _moveDirection;
    private CharacterController _controller;

    public Gun gun = null;

    private int score = 0;
    public TextMeshProUGUI text;
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

    public void UpdateScore(int n)
    {
        score += n; 
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = score.ToString();
    }

    public int GetPoints()
    {
        return score;
    }
}