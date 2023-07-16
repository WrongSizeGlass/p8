using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;



public class ReadJson : MonoBehaviour
{

    private string jsonString;
    private string path = "/Json/PuzzleStates.json";
    private JsonData itemData;
    private List<Vector3> positions;
    int insertNumber = 0;
    public List<Material> mat;   
    MeshRenderer meshrender ;
    SphereCollider col ;
    public GameObject prefab;
    public TextAsset test;
    bool firsthalf = false;
    bool firstquater = false;
    bool thridquearter = false;
    bool rest = false;
    int counter = 0;
    // File.AppendAllText(Application.dataPath + path, data);
    // Start is called before the first frame update
    void Start()
    {       
   
        positions = new List<Vector3>();
        jsonString = File.ReadAllText(Application.dataPath + path);
        itemData = JsonMapper.ToObject(jsonString);
        Debug.LogError("itemData[0].Count: " + itemData[0][0][0].Count);
        for (int k =0;  k< itemData[0].Count; k++) {
            for (int i = 0; i < itemData[0][0][0].Count; i++) {
                GetPlayerPositionsFromPuzzle(k, i);
            }
        }
            
        
       
    }
    private void FixedUpdate()
    {
        counter++;

        if (thridquearter && !rest)
        {
            if (counter % Mathf.Round(1f / Time.fixedDeltaTime) == 0)
            {
                for (int l = (positions.Count / 4 * 3); l < positions.Count; l++)
                {
                    CreateHeatMap(l);
                }
                rest = true;
                counter = 0;
            }
        }
        Debug.Log("is done "+rest);

        if (firsthalf && !thridquearter)
        {
            if (counter % Mathf.Round(1f / Time.fixedDeltaTime) == 0)
            {
                for (int l = positions.Count / 2; l < (positions.Count / 4*3); l++)
                {
                    CreateHeatMap(l);
                }
                thridquearter = true;
                counter = 0;
            }
        }


        if (firstquater && !firsthalf)
        {
            if (counter % Mathf.Round(1f / Time.fixedDeltaTime) == 0)
            {
                for (int l = positions.Count / 4; l < positions.Count / 2; l++)
                {
                    CreateHeatMap(l);
                }
                firsthalf = true;
                counter = 0;
            }
        }

        if (!firstquater){

            if (counter % Mathf.Round(1f / Time.fixedDeltaTime) == 0)
            {
                for (int j = 0; j < positions.Count / 4; j++)
                {
                    CreateHeatMap(j);
                }
                firstquater = true;
                counter = 0;
            }
        }
        
    }
    void CreateHeatMap(int posIndex){

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        meshrender = sphere.GetComponent<MeshRenderer>();
        col = sphere.GetComponent<SphereCollider>();
        sphere.AddComponent<ChangesHeatMapColor>();
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().isKinematic = true;
        sphere.tag = "Positions";
        sphere.name = "pos" + posIndex;
        col.isTrigger = true;
        //Instantiate(prefab, positions[posIndex], Quaternion.identity);
        sphere.transform.position = positions[posIndex];
        sphere.transform.localScale = new Vector3(1, 0.5f, 1);
        meshrender.material = mat[0];
    }

    void GetPlayerPositionsFromPuzzle(int participatn,int PuzzleNumberInOrder){
        float x = 0;
        float y = 0;
        float z = 0;
        for (int i = 0; i < itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"].Count; i++){
           // Debug.Log(itemData[0][0][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString());
            
                int startZ = itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().IndexOf(itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Split(',')[2]);
                int endZ = itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().IndexOf(')');

                string xString = itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Split(',')[0].Trim('(');
                string yString = itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Split(',')[1];
                string zString = itemData[0][participatn][0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Substring(startZ, endZ - startZ);

                x = float.Parse(xString);
                y = float.Parse(yString);
                z = float.Parse(zString);

                positions.Insert(insertNumber, new Vector3(x, y, z));
                insertNumber++;
        }
    }

}
