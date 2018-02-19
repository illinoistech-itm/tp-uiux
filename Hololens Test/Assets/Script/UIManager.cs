using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject inventoryManager;
    public GameObject itemEditPanel;
    public GameObject gameManger;
    public GameObject measurePrefabs;
    public GameObject hudUI;
    public GameObject spherePlayBtnGroup;
    public GameObject sphereStopBtnGroup;
    public GameManager gameManagerComp;
    public bool isMeasureMode;
    public bool isDragging;

    // Use this for initialization
    void Start()
    {
        gameManagerComp = gameManger.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectItem(int num)
    {
        gameManagerComp.selectedItem = inventoryManager.GetComponent<InventoryManager>().items[num];
    }

    public void DeselectItem()
    {
        gameManagerComp.selectedItem = null;
        isMeasureMode = false;
    }

    public void ClickDragBtn()
    {
        gameManagerComp.selectedObject.GetComponent<ObjectController>().ClickDragBtn();
    }

    public void ClickRotateBtn()
    {
        gameManagerComp.selectedObject.GetComponent<ObjectController>().ClickRotateBtn();
    }

    public void ClickResizeBtn()
    {
        gameManagerComp.selectedObject.GetComponent<ObjectController>().ClickResizeBtn();
    }

    public void ClickEditPanelCloseBtn()
    {
        gameManagerComp.selectedObject.GetComponent<Rigidbody>().useGravity = true;
        gameManagerComp.selectedObject.GetComponent<Rigidbody>().freezeRotation = false;
        gameManagerComp.selectedObject = null;
        itemEditPanel.SetActive(false);
    }

    public void ClickMeasureWorldBtn()
    {
        isMeasureMode = true;
        gameManagerComp.selectedItem = null;
    }

    public void ClickHudSwitch()
    {
        if(spherePlayBtnGroup.activeInHierarchy)
        {
            sphereStopBtnGroup.SetActive(true);
            spherePlayBtnGroup.SetActive(false);
            //hudUI.SetActive(false);
        }else
        {
            sphereStopBtnGroup.SetActive(false);
            spherePlayBtnGroup.SetActive(true);
            OpenEditPanel();
            //hudUI.SetActive(true);
        }
    }

    public void HoldDragBtn()
    {
        isDragging = true;
    }

    public void HoldDownDragBtn()
    {
        isDragging = false;
    }

    public void OpenEditPanel()
    {
        Vector3 initialPos;
        Quaternion initialRot;

        //initialPos = Vector3.zero;
        if(gameManagerComp.selectedObject != null)
        {
            initialPos = gameManagerComp.selectedObject.transform.position;
            initialPos.y += 0.35f;
            itemEditPanel.SetActive(true);
            itemEditPanel.transform.position = initialPos;
            itemEditPanel.transform.LookAt(gameManagerComp.hololensCamera.transform.position);
            initialRot = itemEditPanel.transform.rotation;
            initialRot.eulerAngles += new Vector3(0.0f, 180.0f, 0.0f);
            itemEditPanel.transform.rotation = initialRot;
        }
    }
}
