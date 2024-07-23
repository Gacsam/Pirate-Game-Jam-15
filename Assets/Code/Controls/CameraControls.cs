using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControls : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private enum CameraSide {none, left, right};
    [SerializeField]
    private CameraSide cameraSide;
    private bool startScroll;
    public float maxScroll;
    public float scrollSpeed = 10f;
    public void OnPointerEnter(PointerEventData eventData){startScroll = true;}

    public void OnPointerExit(PointerEventData eventData){startScroll = false;}

    void Update() {
        if(!startScroll){return;}
        if(cameraSide == CameraSide.none){return;}

        if(cameraSide == CameraSide.left){
            if(Camera.main.transform.position.x < maxScroll){return;}
            Vector3 newPosition = Camera.main.transform.position;
            newPosition.x -= scrollSpeed*Time.deltaTime;
            Camera.main.transform.position = newPosition;
            GameMan.MoveCloud(Vector2.left);
        }
        else{
            if(Camera.main.transform.position.x > maxScroll){return;}
            Vector3 newPosition = Camera.main.transform.position;
            newPosition.x += scrollSpeed*Time.deltaTime;
            Camera.main.transform.position = newPosition;
            GameMan.MoveCloud(Vector2.right);
        }

    }


}
