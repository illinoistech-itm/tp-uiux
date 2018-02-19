using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject UIManager;
    public GameObject mainUI;
    public GameObject openUI;
    public GameObject hololensCamera;
    public GameObject selectedItem;
    public GameObject selectedObject;
    public GameObject cursor;

    private HandDragging handdraggingComp;
    private Quaternion initialPanelRot;
    private Vector3 initialPanelPos;
    private float calculatedDegree;
    private float realDistanceZ;
    private float realDistanceX;

    public void InstallItem()
    {
        GameObject createdObject;
        Quaternion rot;
        if(UIManager.GetComponent<UIManager>().itemEditPanel.activeInHierarchy)
        {
            UIManager.GetComponent<UIManager>().ClickEditPanelCloseBtn();
        }
        if(selectedItem != null && !UIManager.GetComponent<UIManager>().itemEditPanel.activeInHierarchy)
        {
            createdObject = Instantiate(selectedItem, cursor.transform.position, cursor.transform.rotation);
            createdObject.transform.LookAt(hololensCamera.transform);
            rot = createdObject.transform.rotation;
            rot.eulerAngles -= new Vector3(0.0f, 90.0f, 0.0f);
            createdObject.transform.rotation = rot;
        }
    }
  

}


