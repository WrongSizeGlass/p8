using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlowers : MonoBehaviour
{
    public GameObject Flower;
    //public GameObject Flower1;
    //public GameObject Flower2;
    //public GameObject Flower3;
    //public GameObject Flower4;
    public int numItemsToSpawn = 100;


    public float itemXSpread = 10;
    public float itemYSpread = 0;
    public float itemZSpread = 20;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numItemsToSpawn; i++)
        {
            SpreadItems();
        }
        
    }

    void SpreadItems()
    {
        Vector3 randPosition = new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread)) + transform.position;
        GameObject clone = Instantiate(Flower, randPosition, Quaternion.identity);
        //GameObject clone1 = Instantiate(Flower1, randPosition, Quaternion.identity);
        //GameObject clone2 = Instantiate(Flower2, randPosition, Quaternion.identity);
        //GameObject clone3 = Instantiate(Flower3, randPosition, Quaternion.identity);
        //GameObject clone4 = Instantiate(Flower4, randPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
