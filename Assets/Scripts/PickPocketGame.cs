using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PickpocketMinigame : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerMovementController playerController;
    [SerializeField] InputActionReference exitAction;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] Interactable interactable;
    [SerializeField] GameObject game;
    [SerializeField] Rigidbody2D walletRigidbody;
    [SerializeField] Collider2D walletCollider;
    [SerializeField] SpriteRenderer walletRenderer;
    [SerializeField] float moveForce = 10f;
    [SerializeField] float hitPenalty = 30f;
    [SerializeField] float colorFlash = 0.25f;

    Vector3 walletStart = Vector3.zero;
    [HideInInspector] public bool success = false;

    private void Awake()
    {
        walletStart = walletRigidbody.transform.localPosition;
        walletRigidbody.isKinematic = true;
        game.SetActive(false);

        interactable.interactEvent.AddListener(GameStart);
    }

    // Trigger for successful pickpocketing
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (game.activeSelf && other == walletCollider)
        {
            GameSuccess();
        }
    }

    // Hitting wall
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (game.activeSelf && collision.collider == walletCollider)
        {
            gameManager.WarnAll(hitPenalty);
            walletRenderer.color = Color.red;
            CancelInvoke(nameof(EndColorWarning));
            Invoke(nameof(EndColorWarning), colorFlash);
            //buzzSound.Play();
        }
    }

    private void FixedUpdate()
    {
        if (game.activeSelf)
        {
            // Move
            Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
            if (moveDirection.sqrMagnitude > Mathf.Epsilon)
            {
                if (walletRigidbody.isKinematic == true) walletRigidbody.isKinematic = false;
                walletRigidbody.AddForce(moveForce * Time.fixedDeltaTime * moveDirection);
            }
        }
    }

    private void Update()
    {
        if (game.activeSelf && exitAction.action.WasPressedThisFrame())
        {
            GameEnd();
        }
    }

    void EndColorWarning()
    {
        walletRenderer.color = Color.white;
    }

    public void GameSuccess()
    {
        game.SetActive(false);
        success = true;
        gameManager.CheckGameOver();
    }

    public void GameEnd()
    {
        walletRigidbody.isKinematic = true;
        walletRigidbody.transform.localPosition = walletStart;
        interactable.EnableInteraction(1f);
        game.SetActive(false);

        playerController.disableMovement = false;
    }

    public void GameStart(bool UNNECESSARY = false)
    {
        // should fix werid bug with wallet falling at start
        Invoke(nameof(GameStart), 0.25f);
        interactable.canInteract = false;

        playerController.disableMovement = true;
    }

    public void GameStart()
    {
        game.SetActive(true);
    }

    /*
    [SerializeField] InputActionReference exitGameButton;
    [SerializeField] GameObject game;
    public GameObject miniGame;
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

    Vector3 walletStart = Vector3.zero;

    void Start()
    {
        walletRb = wallet.GetComponent<Rigidbody2D>();
        walletRenderer = wallet.GetComponent<SpriteRenderer>();
        //feedbackText.text = "Steal the wallet without touching the sides!";
        walletRb.isKinematic = true;

        walletStart = walletRb.transform.localPosition;
    }

    public void onPickPocketTrigger()
    {
        game.SetActive(true);
    }    
    void FixedUpdate()
    {
        if (gameFailed || gameSuccess) return;

        if (!hasStarted && Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon || Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Epsilon)
        {
            hasStarted = true;
            Debug.Log("beginning");
            walletRb.isKinematic = false; // Enable physics on first input
            walletRb.transform.localPosition = walletStart;
        }

        else if (hasStarted)
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

    private void Update()
    {
        if (hasStarted)
        {
            if (exitGameButton.action.WasPressedThisFrame())
            {
                game.SetActive(false);
                ResetGame();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == wallet)
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
        if (collision.gameObject == wallet)
        {
            walletRenderer.color = safeColor;
            //buzzSound.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == wallet)
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
        wallet.transform.localPosition = walletStart;
        feedbackText.text = "Steal the wallet without touching the sides!";
        walletRenderer.color = safeColor;
    }
    */
}
