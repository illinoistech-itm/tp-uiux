// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using HoloToolkit.Examples.Prototyping;

namespace HoloLensHandTracking
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public class HandsTrackingController : MonoBehaviour
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        /// 
        private float GetRad(float degree)
        {
            float rad = Mathf.PI * degree / 180.0f;
            return rad;
        }


        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        public GameObject TrackingObject;
        public GameObject HololensCamera;
        public TextMesh StatusText;
        public TextMesh CorText;
        public TextMesh RotText;
        public Color DefaultColor = Color.green;
        public Color TapColor = Color.blue;
        public Color HoldColor = Color.red;

        private HashSet<uint> trackedHands = new HashSet<uint>();
        private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
        private GestureRecognizer gestureRecognizer;
        private uint activeId;

        void Awake()
        {
            InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
            gestureRecognizer.Tapped += GestureRecognizerTapped;
            gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;
            gestureRecognizer.HoldCompleted += GestureRecognizer_HoldCompleted;
            gestureRecognizer.HoldCanceled += GestureRecognizer_HoldCanceled;            
            gestureRecognizer.StartCapturingGestures();
            StatusText.text = "READY";
        }

        void ChangeObjectColor(GameObject obj, Color color)
        {            
            var rend = obj.GetComponentInChildren<Renderer>();
            if (rend)
            {
                rend.material.color = color;
                Debug.LogFormat("Color Change: {0}", color.ToString());
            }
        }


        private void GestureRecognizer_HoldStarted(HoldStartedEventArgs args)
        {
            uint id = args.source.id;            
            StatusText.text = "HoldStarted - Kind " + args.source.kind.ToString();
            if (trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], HoldColor);
                StatusText.text += "-TRACKED";
            }
        }

        private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
        {
            uint id = args.source.id;            
            StatusText.text = "HoldCompleted - Kind " + args.source.kind.ToString();
            if(trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], DefaultColor);
                StatusText.text += "-TRACKED";
            }
        }

        private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)
        {
            uint id = args.source.id;            
            StatusText.text = "HoldCanceled - Kind" + args.source.kind.ToString();
            if (trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], DefaultColor);
                StatusText.text += "-TRACKED";
            }
        }

        private void GestureRecognizerTapped(TappedEventArgs args)
        {            
            uint id = args.source.id;
            if (trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], TapColor);
                //StatusText.text += "-TRACKED";
            }
            
            if(!gameObject.GetComponent<GameManager>().mainUI.activeInHierarchy)
            {
                //gameObject.GetComponent<GameManager>().openUI.SetActive(true);
                gameObject.GetComponent<GameManager>().openUI.GetComponent<MoveWithObject>().StopRunning();
            }


            if (gameObject.GetComponent<GameManager>().selectedItem != null)
            {
                if(gameObject.GetComponent<GameManager>().selectedObject != null)
                {
                    StatusText.text = gameObject.GetComponent<GameManager>().selectedObject.transform.position + " " + gameObject.GetComponent<GameManager>().selectedItem.name;//"Tapped - Kind " + args.source.kind.ToString();
                }

                //Instantiate(gameObject.GetComponent<GameManager>().selectedItem, gameObject.GetComponent<GameManager>().cursor.transform.position, gameObject.GetComponent<GameManager>().cursor.transform.rotation);
            }
            else
            {
                StatusText.text = gameObject.GetComponent<GameManager>().cursor.transform.position.ToString() + " " + "NULL";//"Tapped - Kind " + args.source.kind.ToString();

            }
        }
        

        private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
        {
            uint id = args.state.source.id;
            
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                //StatusText.text = "Tracking failed..";
                return;
            }
            //StatusText.text = "Tracking..";
            trackedHands.Add(id);
            activeId = id;

            //GameObject obj = Instantiate(TrackingObject);
            TrackingObject.SetActive(true);
            Vector3 pos;

            if (args.state.sourcePose.TryGetPosition(out pos))
            {
                //obj.transform.position = pos;
                TrackingObject.transform.position = pos;
                //StatusText.text = "SetPos";
            }

            trackingObject.Add(id, TrackingObject);
        }

        private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
        {
            uint id = args.state.source.id;
            Vector3 pos;
            Quaternion rot;
            float calculatedDegree;
            float realDistanceZ;
            float realDistanceX;

            calculatedDegree = Camera.main.transform.rotation.eulerAngles.y;

            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackingObject.ContainsKey(id))
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        if(gameObject.GetComponent<GameManager>().selectedItem != null)
                        {
                            CorText.text = gameObject.GetComponent<GameManager>().selectedItem.name;
                        }else
                        {
                            CorText.text = "Nothing";
                        }
                        //pos.z += 0.6f;
                        realDistanceZ = 0.6f * Mathf.Cos(GetRad(calculatedDegree));
                        realDistanceX = 0.6f * Mathf.Sin(GetRad(calculatedDegree));
                        pos.z += realDistanceZ;
                        pos.x += realDistanceX;
                        //trackingObject[id].transform.position = pos;
                        TrackingObject.transform.position = pos;
                        TrackingObject.transform.LookAt(gameObject.GetComponent<GameManager>().hololensCamera.transform.position);
                        rot = TrackingObject.transform.rotation;
                        rot.eulerAngles += new Vector3(0.0f, 180.0f, 0.0f);
                        TrackingObject.transform.rotation = rot;
                    }

                    if (args.state.sourcePose.TryGetRotation(out rot))
                    {
                        RotText.text = "SetPos " + rot.x + " " + rot.y + " " + rot.z;
                        rot = TrackingObject.transform.rotation;
                        rot.eulerAngles += new Vector3(0.0f, 180.0f, 0.0f);
                        //trackingObject[id].transform.rotation = rot;
                        TrackingObject.transform.rotation = rot;
                    }
                    //StatusText.text = "Tracking Continue..";
                }
            }
        }

        private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
        {
            uint id = args.state.source.id;
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            if (trackedHands.Contains(id))
            {
                trackedHands.Remove(id);
            }

            if (trackingObject.ContainsKey(id))
            {
                var obj = trackingObject[id];
                trackingObject.Remove(id);
                TrackingObject.SetActive(false);
            }
            if (trackedHands.Count > 0)
            {
                activeId = trackedHands.First();
            }
        }

        void OnDestroy()
        {                        
            InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;

            gestureRecognizer.Tapped -= GestureRecognizerTapped;
            gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;
            gestureRecognizer.HoldCompleted -= GestureRecognizer_HoldCompleted;
            gestureRecognizer.HoldCanceled -= GestureRecognizer_HoldCanceled;
            gestureRecognizer.StopCapturingGestures();
        }
    }
}