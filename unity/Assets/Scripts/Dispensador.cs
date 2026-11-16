using System.Collections;
using UnityEngine;

public class Dispensador : MonoBehaviour
{
    Input_Gameobject spawnedInput;
    [SerializeField] GameObject prefab_InputToSpawn;
    [SerializeField] Transform spawningPosition;
    [SerializeField] bool startHidden = false;
    [SerializeField] AudioSource sfx_spawnInput, sfx_unhiden;
    Animator _animator;
    private void Awake()
    {
        GameController.instance.OnPlacedInput += OnPlacedInput;
        _animator = GetComponent<Animator>();
        if (startHidden) { _animator.SetBool("hidden", true); }
        else { spawnNewInput(false); }
        
    }

    void OnPlacedInput(Input_Gameobject eatenInput)
    {
        if(eatenInput == spawnedInput)
        {
            if (GameController.instance.CurrentLevel.instantRespawn)
            {
                spawnNewInput();
            }
        }
        else if(spawnedInput == null)
        {
            spawnNewInput();
        }
    }
    public void unhideDispenser()
    {
        _animator.SetBool("hidden", false);
        StartCoroutine(delayedSpawn());
        sfx_unhiden.Play();

        IEnumerator delayedSpawn()
        {
            yield return new WaitForSeconds(1.5f);
            spawnNewInput();
        }
    }
    public void hideDispenser()
    {
        //sfx_unhiden.Play();
        _animator.SetBool("hidden", true);
        Destroy(spawnedInput);
    }
    public void spawnNewInput(bool withSFX = true)
    {
        if (_animator.GetBool("hidden")) { return; }
        if (withSFX) { sfx_spawnInput.Play(); }
        GameObject newInput = Instantiate(prefab_InputToSpawn, spawningPosition.position, Quaternion.identity);
        spawnedInput = newInput.GetComponent<Input_Gameobject>();
        spawnedInput.dispensador = this;
        _animator.SetTrigger("spawn");
    }
}
