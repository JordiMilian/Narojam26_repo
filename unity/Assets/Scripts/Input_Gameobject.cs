using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Input_Gameobject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public  GameController.InputKind inputKind;
    Rigidbody2D _rb;
    bool isDragging = false;
    bool isOverhead = false;
    [HideInInspector] public Dispensador dispensador;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        GameController.instance.clickAction.action.started += OnClickDown;
        GameController.instance.clickAction.action.canceled += OnClickUp;
        GameController.instance.pointerPosAction.action.performed += OnPointMoved;
    }
    private void OnDisable()
    {
        GameController.instance.clickAction.action.started -= OnClickDown;
        GameController.instance.clickAction.action.canceled -= OnClickUp;
        GameController.instance.pointerPosAction.action.performed -= OnPointMoved;
        isDragging = false;
    }
    private void OnPointMoved(InputAction.CallbackContext ctx)
    {
        if (isDragging)
        {
            Vector2 pointerPos = ctx.ReadValue<Vector2>();

            Vector3 screenPos = new Vector3(
                pointerPos.x,
                pointerPos.y,
                10f
            );

            transform.position = Camera.main.ScreenToWorldPoint(screenPos);
        }
    }
      
    public void OnClickDown(InputAction.CallbackContext ctx)
    {
        if (isOverhead) 
        { 
            isDragging = true;
            _rb.angularVelocity = 0;
            _rb.linearVelocity = Vector2.zero; 
            _rb.gravityScale = 0; 
        }
    }
    public void OnClickUp(InputAction.CallbackContext ctx)
    {
        
        if (GameController.instance.isOverMouth && isDragging)
        {
            Debug.Log("yo soy" + inputKind);
            GameController.instance.PlaceInput(this);
            Destroy(gameObject);
        }

        isDragging = false;
        _rb.gravityScale = 1;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       isOverhead = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOverhead = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "DEATH")
        {
            OnReachedDeathrock();
        }
    }
    public void OnReachedDeathrock()
    {
        Destroy(gameObject);
        dispensador.spawnNewInput();
    }
}
