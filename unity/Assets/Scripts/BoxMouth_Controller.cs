using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BoxMouth_Controller : MonoBehaviour
{
    BoxCollider2D _boxCollider;
    Animator _animator;
    bool enabledT = true;
    [SerializeField] ending endScreen;
    [SerializeField] AudioSource sfx_eat, sfx_nextLevel;
    private void Awake()
    {
        GameController.instance.pointerPosAction.action.performed += CheckIfOverMouth;
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }
    void CheckIfOverMouth(InputAction.CallbackContext ctx)
    {
        if(enabledT == false) { GameController.instance.isOverMouth = false; return; }

        Vector2 pointerPos = ctx.ReadValue<Vector2>();
        Vector3 screenPos = new Vector3(
               pointerPos.x,
               pointerPos.y,
               10f
           );
        GameController.instance.isOverMouth = (_boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(screenPos)));
    }
    public void TriggerNextBox()
    {
        _animator.SetTrigger("next");
        sfx_nextLevel.Play();
    }
    public void TriggerRestartBox()
    {
        _animator.SetTrigger("restart");
    }
    public void TriggerEat()
    {
        _animator.SetTrigger("eat");
        sfx_eat.Play();
    }
    public void TriggerEnding()
    {
        _animator.SetTrigger("end");
    }
    public void EnableTrigger()
    {
        enabledT = true;
    }
    public void DisableTrigger()
    {
        enabledT = false;
    }
    [SerializeField] List<Material> mat_dientes = new();
    [SerializeField] List<Material> mat_bases = new();
    [SerializeField] List<Material> mat_lados = new();
    [SerializeField] List<Material> mat_tubos = new();
    int boxIndex = 0;
    [SerializeField] MeshRenderer mesh_base, mesh_dientes, mesh_lado1, mesh_lado2, mesh_tubo;

    public void EV_nextBoxSkin()
    {
        boxIndex++;
        mesh_base.material = mat_bases[boxIndex];
        mesh_dientes.material = mat_dientes[boxIndex];
        mesh_lado1.material = mat_lados[boxIndex];
        mesh_lado2.material = mat_lados[boxIndex];
        mesh_tubo.material = mat_tubos[boxIndex];
    }
    public void EV_OutputReveal()
    {
        GameController.instance.OnOutputReveal();
    }
    public void EV_EndingScreen()
    {
        endScreen.triggerEnding();
    }
}
