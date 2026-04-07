using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenposition = Camera.main.WorldToScreenPoint(transform.position);
        if(screenposition.x < 0 || screenposition.x > Screen.width|| screenposition.y < 0 || screenposition.y > Screen.height)
            {
            Destroy(gameObject);
        }
    }

 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroids"))
        {
            // Aggiorna punteggio del player
            Object.FindFirstObjectByType<PlayerController>().AddScore(1);

            Destroy(collision.gameObject); // distrugge asteroide
            Destroy(gameObject);           // distrugge bullet
        }
    }
}
