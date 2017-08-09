using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputManager : MonoBehaviour {
    public static VRInputManager instance = null;

    [Tooltip("Reference to GvrReticlePointer")]
    public GameObject reticlePointer;

#if UNITY_EDITOR
  public enum EmulatedPlatformType {
    Daydream,
    Cardboard
  }
  // Cardboard by default if there is no native integration.
  [Tooltip("Emulated GVR Platform")]
  public EmulatedPlatformType gvrEmulatedPlatformType = EmulatedPlatformType.Daydream;

#else
    private GvrSettings.ViewerPlatformType viewerPlatform; // Use unity GVRSettings instead

#endif  // UNITY_EDITOR

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
    // check playersettings for VR type
        if (UnityEngine.VR.VRSettings.isDeviceActive)
    gvrEmulatedPlatformType = EmulatedPlatformType.Cardboard;
#else
        //if (UnityEngine.VR.VRSettings.supportedDevices.GetLength(1) > 0)
        //{
            viewerPlatform = GvrSettings.ViewerPlatform; // Set it to Unity tracked VR platform
        //}

#endif  // UNITY_EDITOR

        SetVRInputMechanism(); // Check whether gazeinput should be turned on for Cardboard and controller input for Daydream
    }

	// Update is called once per frame
	void Update () {
		
	}

    private void SetVRInputMechanism()
    {
        if (reticlePointer == null) // Check from Unity inspector
        {
            return;
        }
        SetGazeInputActive();
    }

    private void SetGazeInputActive()
    {
        reticlePointer.SetActive(true); // Since it is inactive at start
        GvrReticlePointer pointer = reticlePointer.GetComponent<GvrReticlePointer>();
        if (pointer != null)
        {
            pointer.SetAsMainPointer(); // For Event system to use
        }
    }

    void OnDisable()
    {
        GvrPointerManager.Pointer = null;
    }

    // Checks if the player is gazing at the floor
    //public bool GetGazeAtFloor()
    //{
    //    RaycastResult result = new RaycastResult();
    //    Ray ray = new Ray();
    //    GvrPointerManager.Pointer.OnPointerEnter(result, ray, false);
    //    if (result.gameObject.CompareTag("Floor"))
    //        return true;
    //    else
    //        return false;
    //}
}
