using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LoadStartLevel();
    }

    public InputActionReference clickAction, pointerPosAction;
    public enum InputKind { Input_A, Input_B, Input_C }
    public enum OutputKind { Output_A, Output_B}

    [SerializeField] GameObject Prefab_OutputA, Prefab_OutputB;

    public List<LevelSettings_SO> levelsList = new();

    public LevelSettings_SO debugLevel;
    public LevelSettings_SO CurrentLevel;
    int currentLevelIndex;
    public UnityAction<Input_Gameobject> OnPlacedInput;

    int _currentMigajas;
    [SerializeField] TextMeshProUGUI tmp_currentMigajas;
    [SerializeField] GoalMeter_Controller goalMeter;

    [SerializeField] float vibrationDuration = 8;
    public int CurrentMigajas
    {
        get {  return _currentMigajas; }
        set
        {
            _currentMigajas = value;
            string newText = _currentMigajas + "/" + CurrentLevel.migajas_goal;
            tmp_currentMigajas.text = newText;
            goalMeter.OnUpdatedMigajas();
        }
    }
    [SerializeField] Apostador_Controller apostador_A, apostador_B;

    public bool isOverMouth;

    void LoadStartLevel()
    {
        CurrentLevel = Instantiate(levelsList[currentLevelIndex]);
        LoadLevel(CurrentLevel);
    }
    void LoadNewLevel()
    {
        currentLevelIndex++;
        CurrentLevel = Instantiate(levelsList[currentLevelIndex]);
        LoadLevel(CurrentLevel);
    }
    void LoadLevel(LevelSettings_SO lvl)
    {
        CurrentMigajas = lvl.migajas_iniciales;
        apostador_A.maxBet = lvl.migajas_maxBet;
        apostador_B.maxBet = lvl.migajas_maxBet;
        apostador_A.currentBet = 0;
        apostador_B.currentBet = 0;
        apostador_A.UpdateUI();
        apostador_B.UpdateUI();

    }

    //esto sera un ienumerator?
    public void PlaceInput(Input_Gameobject input)
    {
        OutputKind output = CurrentLevel.GetOutput(input.inputKind);
        Debug.Log("OUTPUT: " + output);
        OnPlacedInput.Invoke(input);
        //instanciar output
        //animaciones UI victoria

        GameObject PrefabToSpawn = null;
        if (output == OutputKind.Output_A) { apostador_A.OnWonBet(); apostador_B.OnLostBet(); PrefabToSpawn = Prefab_OutputA; }
        if (output == OutputKind.Output_B) { apostador_B.OnWonBet(); apostador_A.OnLostBet(); PrefabToSpawn = Prefab_OutputB; }

        GameObject newPrefab = Instantiate(PrefabToSpawn, transform.position, Quaternion.identity);
        Destroy(newPrefab, 2);


        //aninimar UI contador migajar?

        if (CurrentMigajas >= CurrentLevel.migajas_goal)
        {
            StartCoroutine(C_WonBoxCorroutine());
        }
        IEnumerator C_WonBoxCorroutine()
        {
            yield return new WaitForSeconds(1);
            DOTween.To(() => goalMeter.goalScale, x => goalMeter.goalScale = x, goalMeter.maxScale, vibrationDuration);
            goalMeter.transform.DOShakePosition(vibrationDuration, 0.1f,20);
            yield return new WaitForSeconds(vibrationDuration + 3);
            LoadNewLevel();
        }
    }
}
