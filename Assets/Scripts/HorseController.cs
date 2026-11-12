using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class HorseController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public float frameRate = 10f;
    public Sprite[] runFrames;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool grounded;
    private float frameTimer;
    private int frameIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float h = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) h -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) h += 1f;

        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        bool jumpPressed = keyboard.spaceKey.wasPressedThisFrame
            || keyboard.wKey.wasPressedThisFrame
            || keyboard.upArrowKey.wasPressedThisFrame;
        if (jumpPressed && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            grounded = false;
        }

        if (h != 0f) sr.flipX = h < 0f;
        Animate(h != 0f && grounded);
    }

    private void Animate(bool running)
    {
        if (runFrames == null || runFrames.Length == 0) return;
        if (!running)
        {
            sr.sprite = runFrames[0];
            frameTimer = 0f;
            return;
        }
        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0f;
            frameIndex = (frameIndex + 1) % runFrames.Length;
            sr.sprite = runFrames[frameIndex];
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                grounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}
