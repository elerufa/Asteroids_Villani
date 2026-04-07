using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour 
{ 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb; 
   
    float forwardInput; 
    
    public float forwardSpeed; 
    
    float turnInput; 
    
    public float turnSpeed; 
   
    public GameObject bulletPrefab; 
   
    public TextMeshProUGUI vitaText; // UI della vita
    public TextMeshProUGUI scoreText; // UI del punteggio
    
    private int vita = 3; // VITA DEL PLAYER
    private int score = 0; // PUNTEGGIO

    public GameObject defeatPanel;

    public TextMeshProUGUI lastScoreText; 
    public TextMeshProUGUI highScoreText; 



    void Start() 
    { 
        rb = GetComponent<Rigidbody2D>(); 
        UpdateVitaText(); 
        UpdateScoreText(); 
    } 
    // Update is called once per frame
    void Update()
    { 
       forwardInput = Input.GetAxis("Vertical"); 
        turnInput = Input.GetAxis("Horizontal"); 
        if (Input.GetKeyDown(KeyCode.S)) 
        { 
            TeleportRandom(); 
        } 
        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            Instantiate(bulletPrefab, transform.position, transform.rotation); 
        } 
    } 
    
    private void FixedUpdate() 
    { 
        if (forwardInput > 0) 
        { 
            rb.AddForce(transform.up * forwardSpeed * forwardInput); 
        } 
        if (turnInput != 0) 
        { 
            float rotationAmount = -turnInput*turnSpeed*Time.fixedDeltaTime; 
            rb.MoveRotation(rb.rotation + rotationAmount); 
        } 
    } 
    
    private void OnTriggerEnter2D(Collider2D collision) 
    { 
        if (collision.gameObject.CompareTag("Asteroids")) 
        { 
            vita--; // il player perde vita
            UpdateVitaText();
            Debug.Log("vita -1"); 
            transform.position = Vector3.zero;
            
            if (vita <= 0) 
            { 
                ShowDefeatPanel(); 
            } 
        } 
    } 
    
    // Metodo per aggiornare la UI della vita
    private void UpdateVitaText() 
    {
        vitaText.text = "VITA: " + vita + "/3";
    } 
    
    public void AddScore(int value) 
    { 
        score += value; 
        UpdateScoreText(); 
    } 
    
    private void UpdateScoreText() 
    { 
        scoreText.text = "SCORE: " + score;
    }

    private void ShowDefeatPanel()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0f;

            // Gestione del Record (High Score)
            // Recupera il vecchio record salvato (se non esiste, è 0)
            int oldHighScore = PlayerPrefs.GetInt("BestScore", 0);

            // Se il punteggio di questa partita è più alto del record, aggiornalo
            if (score > oldHighScore)
            {
                PlayerPrefs.SetInt("BestScore", score);
                PlayerPrefs.Save();
                oldHighScore = score; // Aggiorna la variabile per mostrarla a video
            }

            // Aggiorna l'interfaccia UI
            if (lastScoreText != null)
                lastScoreText.text = "SCORE: " + score;

            if (highScoreText != null)
                highScoreText.text = "HIGHEST SCORE: " + oldHighScore;
        }
    }

   
    private void TeleportRandom() 
    { float distanceZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z); 
        Vector2 screenBL = Camera.main.ScreenToWorldPoint(new Vector3(0,0, distanceZ)); 
        Vector2 screenTR =Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanceZ)); 
        float randomX = Random.Range(screenBL.x, screenTR.x); 
        float randomY = Random.Range(screenBL.y, screenTR.y); 
        transform.position = new Vector3(randomX, randomY, transform.position.z); 
    } 
    
    public void RestartGame() 
    { 
        Time.timeScale = 1f; // riattiva il gioco se era fermato
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    } 
    
    public void QuitGame() 
    { 
        Debug.Log("Quit Game"); 
        Application.Quit(); // chiude il gioco buildato
    } 
}