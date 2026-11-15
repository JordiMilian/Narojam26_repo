using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ending : MonoBehaviour
{
    [SerializeField] InputActionReference escapeInput;
    [SerializeField] Canvas canvasss;
    public void triggerEnding()
    {
        canvasss.gameObject.SetActive(true);
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("end");
        escapeInput.action.performed += escaped;
    }
    void escaped(InputAction.CallbackContext ctx)
    {
        escapeInput.action.performed -= escaped;
        SceneManager.LoadScene(0);
    }
}
