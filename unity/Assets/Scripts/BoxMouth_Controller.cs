using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BoxMouth_Controller : MonoBehaviour
{
    BoxCollider2D _boxCollider;
    private void Awake()
    {
        GameController.instance.pointerPosAction.action.performed += CheckIfOverMouth;
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    void CheckIfOverMouth(InputAction.CallbackContext ctx)
    {
        Vector2 pointerPos = ctx.ReadValue<Vector2>();
        Vector3 screenPos = new Vector3(
               pointerPos.x,
               pointerPos.y,
               10f
           );
        Debug.DrawLine(Vector3.zero, screenPos);
        GameController.instance.isOverMouth = (_boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(screenPos)));
    }
}
