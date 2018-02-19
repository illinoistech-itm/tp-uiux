using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour {
    public GameObject hudUI;
    public TextMesh CamRotText;
    public TextMesh CamPosText;
    private float calculatedDegreeY;
    private float calculatedDegreeX;
    private float realDistanceZ;
    private float realDistanceY;
    private float realDistanceX;
    Vector3 pos;
    Quaternion rot;

    private float GetRad(float degree)
    {
        float rad = Mathf.PI * degree / 180.0f;
        return rad;
    }

    private void Start()
    {
        pos = Vector3.zero;
    }

    // Update is called once per frame
    void Update () {
        CamPosText.text = "Pos : "+Camera.main.transform.position.ToString();
        CamRotText.text = "Rot : " + Camera.main.transform.rotation.eulerAngles.ToString();
        calculatedDegreeY = Camera.main.transform.rotation.eulerAngles.y;
        realDistanceZ = (2.0f+ Camera.main.transform.position.z) * Mathf.Cos(GetRad(calculatedDegreeY)) - (Camera.main.transform.position.x) * Mathf.Sin(GetRad(calculatedDegreeY));
        realDistanceX = (2.0f + Camera.main.transform.position.z) * Mathf.Sin(GetRad(calculatedDegreeY)) + (Camera.main.transform.position.x) * Mathf.Cos(GetRad(calculatedDegreeY));
        calculatedDegreeX = Camera.main.transform.rotation.eulerAngles.x;
        realDistanceY = (2.0f + Camera.main.transform.position.z) * Mathf.Sin(GetRad(calculatedDegreeX)) + (Camera.main.transform.position.y) * Mathf.Cos(GetRad(calculatedDegreeX));
        pos.z = realDistanceZ;
        pos.y = (-realDistanceY) + 0.01f;
        pos.x = realDistanceX;

        

        //trackingObject[id].transform.position = pos;
        hudUI.transform.position = pos;
        //hudUI.transform.LookAt(gameObject.GetComponent<GameManager>().hololensCamera.transform.position);
        rot = hudUI.transform.rotation;
        rot.eulerAngles = Camera.main.transform.rotation.eulerAngles;
        hudUI.transform.rotation = rot;
    }
}
