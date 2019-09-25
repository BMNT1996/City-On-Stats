using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Text thePath;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Path");
        string path = obj.GetComponent<PathUtil>().getPath();
        var loadedObj = new OBJLoader().Load(path);
        loadedObj.AddComponent<MeshCollider>();
        thePath.text = path;
        //var loadedObj = new OBJLoader().Load(path);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
