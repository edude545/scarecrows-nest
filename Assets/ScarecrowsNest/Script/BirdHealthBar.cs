using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHealthBar : MonoBehaviour
{

    GameObject bar;
    Crow crow;

    MaterialPropertyBlock propBlock;

    Mesh barMesh;
    Material barMaterial;

    private void Awake()
    {
        bar = new GameObject();
        bar.transform.parent = transform;
        bar.transform.localPosition = new Vector3(0, 5, 0);

        if (barMesh == null) { generateBarMesh(); }
        bar.AddComponent<MeshFilter>();
        bar.GetComponent<MeshFilter>().sharedMesh = barMesh;

        if (barMaterial == null) { generateBarMaterial(); }
        bar.AddComponent<MeshRenderer>();
        bar.GetComponent<MeshRenderer>().material = barMaterial;

        propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        crow = gameObject.GetComponent<Crow>();
    }

    private void Update()
    {
        //bar.transform.rotation = crow.Target.transform.rotation;
        float f = crow.Fear / crow.Braveness;
        propBlock.SetColor("_Color", crow.Fleeing ? Color.yellow : new Color(1*f, 1*(1-f), 0));
        bar.transform.localScale = new Vector3(crow.Braveness, f, 1);
        bar.GetComponent<MeshRenderer>().SetPropertyBlock(propBlock);
    }

    private void generateBarMesh()
    {
        barMesh = new Mesh();
        barMesh.vertices = new Vector3[] { new Vector3(-1,-1,0), new Vector3(1,-1,0), new Vector3(1,1,0), new Vector3(-1,1,0) };
        barMesh.triangles = new int[] { 0,1,2, 2,3,0 };
        barMesh.RecalculateNormals();
    }

    private void generateBarMaterial()
    {
        barMaterial = new Material(Shader.Find("Diffuse"));
    }

}
