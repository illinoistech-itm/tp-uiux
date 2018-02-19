using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    private HandDragging handDraggingComp;
    private HandRotate handRotateComp;
    private HandResize handResizeComp;
    private GameManager gameManagerComp;


    public enum EEditMode
    {
        EModeDrag = 0,
        EModeRotate = 1,
        EModeResize = 2,
        
    };

    EEditMode _eEditMode;

    public EEditMode eEditMode
    {
        get { return _eEditMode; }
        private set { _eEditMode = value; }
    }

    // Use this for initialization
    void Awake () {
        handDraggingComp = gameObject.GetComponent<HandDragging>();
        handRotateComp = gameObject.GetComponent<HandRotate>();
        handResizeComp = gameObject.GetComponent<HandResize>();
        gameManagerComp = GameObject.Find("Managers").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void changeEditMode(EEditMode eEditMode)
    {
        this.eEditMode = eEditMode;
    }

    public void ClickDragBtn()
    {
        changeEditMode(EEditMode.EModeDrag);
        handDraggingComp.SetDragging(true);
        handRotateComp.SetRotating(false);
        handResizeComp.SetResizing(false);
    }

    public void ClickRotateBtn()
    {
        changeEditMode(EEditMode.EModeRotate);
        handDraggingComp.SetDragging(false);
        handRotateComp.SetRotating(true);
        handResizeComp.SetResizing(false);
    }

    public void ClickResizeBtn()
    {
        changeEditMode(EEditMode.EModeResize);
        handDraggingComp.SetDragging(false);
        handRotateComp.SetRotating(false);
        handResizeComp.SetResizing(true);
    }

    public void SetSelectedObejct()
    {
        if(gameManagerComp.selectedObject != null)
        {
            gameManagerComp.selectedObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        }
        gameManagerComp.selectedObject = gameObject;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
      gameManagerComp.UIManager.GetComponent<UIManager>().OpenEditPanel();
    }
}
