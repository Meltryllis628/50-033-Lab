using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    private bool isFacingRight = true;
    private Rigidbody2D marioBody;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalText;
    public GameObject enemies;
    private Vector3 startPosition;
    public Canvas gameOverUI;
    public Canvas inGameUI;

    public JumpOverGoomba jumpOverGoomba;
    // Start is called before the first frame update
    void Start() {
        // Set to be 30 FPS
        HideGameOverUI();
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        Debug.Log(gameObject.name + startPosition);
    }

    private void HideGameOverUI() {
        gameOverUI.enabled = false;
        inGameUI.enabled = true;
    }
    private void ShowGameOverUI() {
        gameOverUI.enabled = true;
        inGameUI.enabled = false;
    }

    // Update is called once per frame
    void Update() {

    }

    void flip() {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with goomba!");
            finalText.text = "Score: " + jumpOverGoomba.score.ToString();
            ShowGameOverUI();
            Time.timeScale = 0.0f;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }

    public void RestartButtonCallback(int input) {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate() {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // flip
        if (moveHorizontal > 0 && !isFacingRight) flip();
        else if (moveHorizontal < 0 && isFacingRight) flip();

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
            // stop
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState) {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

    private void ResetGame() {
        HideGameOverUI();
        // reset position
        marioBody.transform.position = startPosition;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform) {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().StartPosition;
        }
        jumpOverGoomba.score = 0;

    }
}