using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereButtonFollow : MonoBehaviour {

    public TextMesh CamPosText;
    
    private float calculatedDegreeY;
    private float realDistanceZ;
    private float realDistanceX;
    private Vector3 pos;

    private float GetRad(float degree)
    {
        float rad = Mathf.PI * degree / 180.0f;
        return rad;
    }

    // Update is called once per frame
    void Update () {
        CamPosText.text = "Pos : " + gameObject.transform.position.ToString();
        calculatedDegreeY = Camera.main.transform.rotation.eulerAngles.y;
        realDistanceZ = (2.0f + Camera.main.transform.position.z) * Mathf.Cos(GetRad(calculatedDegreeY)) - (Camera.main.transform.position.x) * Mathf.Sin(GetRad(calculatedDegreeY));
        realDistanceX = (2.0f + Camera.main.transform.position.z) * Mathf.Sin(GetRad(calculatedDegreeY)) + (Camera.main.transform.position.x) * Mathf.Cos(GetRad(calculatedDegreeY));
      
        pos.z = realDistanceZ;
        pos.x = realDistanceX;

        gameObject.transform.position = pos;
    }
}
