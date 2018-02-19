﻿using HoloToolkit;

using UnityEngine;

using HoloToolkit.Unity;





public class GestureManager : Singleton<GestureManager>

{

    // Tap and Navigation gesture recognizer.

    public UnityEngine.XR.WSA.Input.GestureRecognizer NavigationRecognizer { get; private set; }



    // Manipulation gesture recognizer.

    public UnityEngine.XR.WSA.Input.GestureRecognizer ManipulationRecognizer { get; private set; }



    // Currently active gesture recognizer.

    public UnityEngine.XR.WSA.Input.GestureRecognizer ActiveRecognizer { get; private set; }



    public bool IsNavigating { get; private set; }



    public Vector3 NavigationPosition { get; private set; }



    public bool IsManipulating { get; private set; }



    public Vector3 ManipulationPosition { get; private set; }



    void Awake()

    {

        /* TODO: DEVELOPER CODING EXERCISE 2.b */



        // 2.b: Instantiate the NavigationRecognizer.





        // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.







        // 2.b: Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.



        // 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.



        // 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.



        // 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 



        // 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 





        // Instantiate the ManipulationRecognizer.

        ManipulationRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();



        // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.

        ManipulationRecognizer.SetRecognizableGestures(

            UnityEngine.XR.WSA.Input.GestureSettings.ManipulationTranslate);



        // Register for the Manipulation events on the ManipulationRecognizer.

        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;

        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;

        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;

        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;



        ResetGestureRecognizers();

    }



    void OnDestroy()

    {

        // 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.





        // Unregister the Manipulation events on the ManipulationRecognizer.

        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;

        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;

        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;

        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;

    }



    /// <summary>

    /// Revert back to the default GestureRecognizer.

    /// </summary>

    public void ResetGestureRecognizers()

    {

        // Default to the navigation gestures.

        Transition(NavigationRecognizer);

    }



    /// <summary>

    /// Transition to a new GestureRecognizer.

    /// </summary>

    /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>

    public void Transition(UnityEngine.XR.WSA.Input.GestureRecognizer newRecognizer)

    {

        if (newRecognizer == null)

        {

            return;

        }



        if (ActiveRecognizer != null)

        {

            if (ActiveRecognizer == newRecognizer)

            {

                return;

            }



            ActiveRecognizer.CancelGestures();

            ActiveRecognizer.StopCapturingGestures();

        }



        newRecognizer.StartCapturingGestures();

        ActiveRecognizer = newRecognizer;

    }



    private void NavigationRecognizer_NavigationStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)

    {

        // 2.b: Set IsNavigating to be true.





        // 2.b: Set NavigationPosition to be relativePosition.



    }



    private void NavigationRecognizer_NavigationUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)

    {

        // 2.b: Set IsNavigating to be true.





        // 2.b: Set NavigationPosition to be relativePosition.



    }



    private void NavigationRecognizer_NavigationCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)

    {

        // 2.b: Set IsNavigating to be false.



    }



    private void NavigationRecognizer_NavigationCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)

    {

        // 2.b: Set IsNavigating to be false.



    }



    private void ManipulationRecognizer_ManipulationStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)

    {

        if (HandsManager.Instance.FocusedGameObject != null)

        {

            IsManipulating = true;



            ManipulationPosition = position;



            HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position);

        }

    }



    private void ManipulationRecognizer_ManipulationUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)

    {

        if (HandsManager.Instance.FocusedGameObject != null)

        {

            IsManipulating = true;



            ManipulationPosition = position;



            HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position);

        }

    }



    private void ManipulationRecognizer_ManipulationCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)

    {

        IsManipulating = false;

    }



    private void ManipulationRecognizer_ManipulationCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)

    {

        IsManipulating = false;

    }



    private void NavigationRecognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray ray)

    {

        GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;



        if (focusedObject != null)

        {

            focusedObject.SendMessageUpwards("OnSelect");

        }

    }

}