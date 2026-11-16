using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        GameController.instance.pointerPosAction.action.performed += onPointerMove;
    }
    void onPointerMove(InputAction.CallbackContext ctx)
    {
        Vector2 pointerPos = ctx.ReadValue<Vector2>();

        Vector3 screenPos = new Vector3(
            pointerPos.x,
            pointerPos.y,
            10f
        );
        screenPos += new Vector3(0, 0, -1);

        transform.position = Camera.main.ScreenToWorldPoint(screenPos);
    }
}
