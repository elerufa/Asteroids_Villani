using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public float speed;
    public float maxAngularVelocity;

    public enum Type{Big, Medium, Small };

    public Type type;

    public GameObject explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Calcola la direzione verso il centro dello schermo (0,0)
        Vector2 directionToCenter = (Vector2.zero - (Vector2)transform.position).normalized;

        //variazione casuale alla direzione così non puntano tutti esattamente allo stesso punto
        directionToCenter += new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

        // Applica la forza nella direzione corretta
        rb.AddForce(directionToCenter.normalized * speed);
        rb.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tag dell'oggetto:" + collision.tag);
        if (collision.CompareTag("Bullet"))
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance NON inizializzato!");
                return;
            }

            if (type == Type.Big)
            {
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Medium);
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Medium);
            }

            else if (type == Type.Medium)
            {
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Small);
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Small);
            }

            GameManager.Instance.OnAsteroidDestroyed();
            Debug.Log("Asteroide colpito");
           //distrugge il proiettile
            Destroy(collision.gameObject);
            //distruggere l'asteroide
            Destroy(gameObject);

            Destroy(gameObject);

            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
