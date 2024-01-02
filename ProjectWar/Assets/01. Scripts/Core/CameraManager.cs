using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<CameraManager>();
            return instance;
        }
    }

    private CinemachineVirtualCamera mainVCam = null;
    public CinemachineVirtualCamera MainVCam {
        get {
            if(mainVCam == null)
                mainVCam = GameObject.Find("MainVCam").GetComponent<CinemachineVirtualCamera>();
            return mainVCam;
        }
    }

    private Camera mainCam = null;
    public Camera MainCam {
        get {
            if(mainCam == null)
                mainCam = Camera.main;
            return mainCam;
        }
    }
}
