using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlowers : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50F;

    public GameObject flower;
    public Transform[] spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawningFlowers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawningFlowers()
    {
        for(int i = 0; i < spawnPosition.Length; i++)
        {
            Instantiate(flower, spawnPosition[i].position, spawnPosition[i].rotation);
        }
        yield return new WaitForSeconds(2);
    }
    private void CreateFlowers(float xPosition)
    {
        Transform flowers = Instantiate(GameAssets.GetInstance().flower);
        float flowerYposition;
        flowerYposition = +CAMERA_ORTHO_SIZE - Random.value * 0.5f;
        flowers.position = new Vector2(xPosition, flowerYposition);
    }
}
