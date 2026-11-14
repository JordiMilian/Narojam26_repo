using UnityEngine;

public class Dispensador : MonoBehaviour
{
    Input_Gameobject spawnedInput;
    [SerializeField] GameObject prefab_InputToSpawn;
    [SerializeField] Transform spawningPosition;
    private void Awake()
    {
        GameController.instance.OnPlacedInput += OnPlacedInput;
        spawnNewInput();
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
    public void spawnNewInput()
    {
        GameObject newInput = Instantiate(prefab_InputToSpawn, spawningPosition.position, Quaternion.identity);
        spawnedInput = newInput.GetComponent<Input_Gameobject>();
        spawnedInput.dispensador = this;
    }
}
