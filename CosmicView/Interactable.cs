using UnityEngine;

// Originally done by a co-worker and then reworked by me
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private NavigationManager.ViewLevel levelToEnter;
    
    private CameraListHolder cameraListHolder;
    protected NavigationManager navigationManager;

    public string ObjectName { get; protected set; }

    protected virtual void Start()
    {
        navigationManager = NavigationManager.instance;
        cameraListHolder = transform.root.GetComponent<CameraListHolder>();
    }

    private void OnMouseDown()
    {
        navigationManager.EnterLevel(levelToEnter, gameObject);

        foreach (Camera cam in cameraListHolder.cameraList)
        {
            if (cam == null) continue;

            // Right now only needed to go from Planet to Node View -> Can maybe be deleted later
            if (HandleSpecialCamera(cam))
                break;

            if (cam.gameObject.layer != gameObject.layer)
                continue;
            
            navigationManager.activeCamera.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            navigationManager.activeCamera = cam;
            navigationManager.cosmicCamera.gameObject.SetActive(false);
            Debug.LogWarning("Name: " + cam.gameObject.name + " Layer: " + cam.gameObject.layer);
        }

        OnClick();
    }

    protected virtual bool HandleSpecialCamera(Camera cam)
    {
        return false;
    }
    
    protected virtual void OnClick() { }

    public void SetName(string objectName) => ObjectName = objectName;
}
