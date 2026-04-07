using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour { 
   
    public GameObject[] bigAsteroids; 
    public GameObject[] mediumAsteroids; 
    public GameObject[] smallAsteroids; 
    public Dictionary<Asteroid.Type, GameObject[]> asteroids; 
    
    public int numInitialAsteroids = 4; 
    public float spawnRadius; 
    public int numCurrentAsteroids; 
   
    public static GameManager Instance { get; private set;}
  
    
    private void Awake() 
    { if (Instance != null && Instance != this) 
        { Destroy(gameObject); 
            return; 
        } 
        Instance = this;
        
        asteroids = new Dictionary<Asteroid.Type, GameObject[]>
        {
          { Asteroid.Type.Big, bigAsteroids },
          { Asteroid.Type.Medium, mediumAsteroids},
          { Asteroid.Type.Small, smallAsteroids}
        };
    } 
    
    private void Start() 
    { 
        numCurrentAsteroids = 0; 
        SpawnInitialAsteroids();
    } 
    
    private void SpawnInitialAsteroids() 
    { 
        for (int i = 0; i < numInitialAsteroids; i++) 
        {
            SpawnNewWaveAsteroid();
        } 
    }

    private void SpawnNewWaveAsteroid()
    {
        // Calcola i confini della visuale della telecamera
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Distanza fuori dallo schermo 
        float margin = 2.0f;

        Vector3 spawnPos = Vector3.zero;

        // Scelto un lato a caso (0: Sopra, 1: Sotto, 2: Sinistra, 3: Destra)
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Sopra
                spawnPos = new Vector3(Random.Range(-screenWidth / 2, screenWidth / 2), (screenHeight / 2) + margin, 0);
                break;
            case 1: // Sotto
                spawnPos = new Vector3(Random.Range(-screenWidth / 2, screenWidth / 2), (-screenHeight / 2) - margin, 0);
                break;
            case 2: // Sinistra
                spawnPos = new Vector3((-screenWidth / 2) - margin, Random.Range(-screenHeight / 2, screenHeight / 2), 0);
                break;
            case 3: // Destra
                spawnPos = new Vector3((screenWidth / 2) + margin, Random.Range(-screenHeight / 2, screenHeight / 2), 0);
                break;
        }

        SpawnAsteroid(spawnPos, Asteroid.Type.Big);
    }

    public void SpawnAsteroid(Vector2 position, Asteroid.Type type) 
    { 
        GameObject asteroidPrefab = asteroids[type][Random.Range(0, asteroids[type].Length)];
        Instantiate(asteroidPrefab, position, Quaternion.identity); 
        numCurrentAsteroids++; 
    }


    public void OnAsteroidDestroyed()
    {
        numCurrentAsteroids--;

        if (numCurrentAsteroids <= 0)
        {
            numInitialAsteroids++; // Aumenta la difficoltà
            StartCoroutine(NewWaveRoutine()); // Aspetta prima di spawnare
        }
    }

    private System.Collections.IEnumerator NewWaveRoutine()
    {
        yield return new WaitForSeconds(0.5f); // Pausa tra le ondate
        SpawnInitialAsteroids();
    }
} 

