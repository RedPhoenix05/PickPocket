using UnityEngine;
using UnityEngine.UI;

public class PickpocketMinigame : MonoBehaviour
{
    public GameObject pants;
    public GameObject wallet;
    public Text feedbackText;
    public Color safeColor = Color.white;
    public Color dangerColor = Color.red;
    public int maxMistakes = 3;
    public float moveSpeed = 5f;

    private int mistakes = 0;
    private bool gameFailed = false;
    private bool gameSuccess = false;
    private bool hasStarted = false;
    private Rigidbody2D walletRb;
    private SpriteRenderer walletRenderer;

    void Start()
    {
        walletRb = wallet.GetComponent<Rigidbody2D>();
        walletRenderer = wallet.GetComponent<SpriteRenderer>();
        //feedbackText.text = "Steal the wallet without touching the sides!";
        walletRb.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (gameFailed || gameSuccess) return;

        if (!hasStarted && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            hasStarted = true;
            walletRb.isKinematic = false; // Enable physics on first input
        }

        if (hasStarted)
        {
            // WASD Movement
            float moveX = Input.GetAxis("Horizontal") * moveSpeed;
            float moveY = Input.GetAxis("Vertical") * moveSpeed;
            Vector2 v = new(moveX, moveY);
            if (v.sqrMagnitude > Mathf.Epsilon)
            {
                walletRb.AddForce(v, ForceMode2D.Force);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == pants)
        {
            mistakes++;
            walletRenderer.color = dangerColor;
            //buzzSound.Play();

            if (mistakes >= maxMistakes)
            {
                gameFailed = true;
                //feedbackText.text = "You got caught!";
                walletRb.velocity = Vector2.zero;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == pants)
        {
            walletRenderer.color = safeColor;
            //buzzSound.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == pants)
        {
            gameSuccess = true;
            //feedbackText.text = "Success! You got the wallet!";
            walletRb.velocity = Vector2.zero;
            walletRenderer.color = safeColor;
            Debug.Log("Pickpocket successful!");
        }
    }

    public void ResetGame()
    {
        gameFailed = false;
        gameSuccess = false;
        mistakes = 0;
        wallet.transform.localPosition = Vector3.zero;
        feedbackText.text = "Steal the wallet without touching the sides!";
        walletRenderer.color = safeColor;
    }
}
