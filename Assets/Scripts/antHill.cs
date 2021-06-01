using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antHill : MonoBehaviour
{
    public GameObject antPrefab;
    //public GameObject parent;
    public int spawnCt;
    public static int maxAnts = 50;
    public int numAnts = 0;
    public float spawnRate;
    public static GameObject[] allAnts = new GameObject[maxAnts];

    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnRate;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (numAnts + spawnCt < maxAnts)
        {
            
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                Debug.Log(numAnts + spawnCt);

                for (int i = 0; i < spawnCt; i++)
                {
                    Debug.Log("Num Ants" + numAnts);
                    numAnts += 1;
                    allAnts[numAnts - 1] = (GameObject)Instantiate(antPrefab, this.transform.position, Quaternion.identity);
                    
                    //Instantiate(antPrefab, new Vector3(this.transform.position.x + GetModifier(), this.transform.position.y + GetModifier())
                      //  , Quaternion.identity, this.transform);                 
                }
                spawnTimer = spawnRate;
            }
        }

        float GetModifier()
        {
            float modifier = Random.Range(0f, 1f);
            if (Random.Range(0, 2) > 0)
                return -modifier;
            else
                return modifier;
        }
    }
}
