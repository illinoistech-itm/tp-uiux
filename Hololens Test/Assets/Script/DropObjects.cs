using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjects : MonoBehaviour {

    public GameObject cuurentPosObj;
    public GameObject dropObj;

    private Vector3 initialPos;
    private Quaternion initialRot;

	// Use this for initialization
	void Start () {
        initialRot.eulerAngles = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        initialPos = cuurentPosObj.transform.position;
	}

    public void DropObj()
    {
        Instantiate(dropObj, initialPos, initialRot);
    }
}
