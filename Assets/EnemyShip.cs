using UnityEngine;
public class EnemyShip : MonoBehaviour 
{ 
    Rigidbody2D rb; 
    public float speed = 2f; 
   
    public float changeDirectionInterval = 3f; 
    private float changeDirectionTimer = 1f; 
    
    private Vector2 currentDirection; 
    public float shootInterval = 2f; 
    
    private float shootTimer = 1f; 
    
    public GameObject explosion; 
    public GameObject bulletPrefab; 
    
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>(); 
        ChooseNewDirection(); 
    } 
void FixedUpdate() 
    { 
        // Movimento rettilineo nella direzione corrente
        rb.linearVelocity = currentDirection * speed; 
        // Cambio direzione periodico
        changeDirectionTimer -= Time.fixedDeltaTime; 
        if (changeDirectionTimer <= 0f) 
        { 
            ChooseNewDirection(); 
        } 
    } 
    
    void Update() 
    { // Gestione del timer di sparo shootTimer += Time.deltaTime;
      if (shootTimer >= shootInterval) 
        { 
            // Spara un proiettile
            Instantiate(bulletPrefab, transform.position, Random.rotation);
            shootTimer = 0f; 
        } 
    } 
    
    void ChooseNewDirection()
    { 
        // Scegli un nuovo angolo casuale
        float randomAngle = Random.Range(0f, 360f); 
        currentDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)); 
        changeDirectionTimer = changeDirectionInterval; 
    } 
    void OnTriggerEnter2D(Collider2D collider) 
    { Debug.Log("Collisione con: " + collider.gameObject.name); 
        Debug.Log("Tag dell'oggetto: " + collider.gameObject.tag); 
        // Controlla se l'asteroide è stato colpito da un proiettile
        
        if (collider.gameObject.CompareTag("Bullet")) 
        { // Distruggi l'asteroide
          Destroy(gameObject); 
            // Distruggi il proiettile
            Destroy(collider.gameObject); 
            Instantiate(explosion, transform.position, transform.rotation); 
        } 
    } 
}