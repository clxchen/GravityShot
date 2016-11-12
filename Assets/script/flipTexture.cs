using UnityEngine;
using System.Linq;
using System.Collections;

public class flipTexture : MonoBehaviour {

    Mesh mesh;
    

    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
	
}
