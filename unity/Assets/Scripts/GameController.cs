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
    [SerializeField] TextMesh tmp_currentMigajas;
    [SerializeField] GoalMeter_Controller goalMeter;
    [SerializeField] Dispensador thirdDispensador;
    [SerializeField] BoxMouth_Controller boxMouth;
    [SerializeField] SpriteRenderer boxOutputSriteRenderer;
    [SerializeField] Sprite sprite_outputA, sprite_outputB;
    [SerializeField] GameObject GO_boxOutputA, GO_boxOutputB;
    [SerializeField] AudioSource sfx_victoryFinal, sfx_derrotaFinal;

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

        if (lvl.thirdInput) { thirdDispensador.unhideDispenser(); }
        else { thirdDispensador.hideDispenser(); }

    }

    //esto sera un ienumerator?
    OutputKind lastOutput;
    public void PlaceInput(Input_Gameobject input)
    {
        CurrentMigajas--;
        lastOutput = CurrentLevel.GetOutput(input.inputKind);
        Debug.Log("OUTPUT: " + lastOutput);
        OnPlacedInput.Invoke(input);
        enabled = false;
        apostador_A.isDisabled = true;
        apostador_B.isDisabled = true;
        boxMouth.DisableTrigger();

        if (lastOutput == OutputKind.Output_A) 
        {
            //boxOutputSriteRenderer.sprite = sprite_outputA;
            GO_boxOutputB.SetActive(false);
            GO_boxOutputA.SetActive(true);
        }
        if (lastOutput == OutputKind.Output_B)
        {
            //boxOutputSriteRenderer.sprite = sprite_outputB;
            GO_boxOutputB.SetActive(true);
            GO_boxOutputA.SetActive(false);
        }

        boxMouth.TriggerEat();

        //aninimar UI contador migajar?

      
    }
    int moneyAfterLastBet = 0;
    public bool lostLastBet = true;
    public void OnOutputReveal()
    {
        if(lastOutput == OutputKind.Output_A)
        {
            apostador_A.OnWonBet();
            apostador_B.OnLostBet();
        }
        else
        {
            apostador_B.OnWonBet();
            apostador_A.OnLostBet();
        }
        if(CurrentMigajas >= moneyAfterLastBet) { lostLastBet = false; }
        else { lostLastBet = true; }
        moneyAfterLastBet = CurrentMigajas;

        if (CurrentMigajas >= CurrentLevel.migajas_goal)
        {
            StartCoroutine(C_WonBoxCorroutine());
        }
        else if (CurrentMigajas <= 0)
        {
            StartCoroutine(C_LostBoxCorroutine());
        }
        else
        {
            StartCoroutine(enableWithDelay());
        }
        IEnumerator enableWithDelay()
        {
            yield return new WaitForSeconds(2.75f);
            apostador_A.isDisabled = false;
            apostador_B.isDisabled = false;
            boxMouth.EnableTrigger();
        }
    }
    IEnumerator C_LostBoxCorroutine()
    {
        apostador_A.isDisabled = true;
        apostador_B.isDisabled = true;

        yield return new WaitForSeconds(1);
        sfx_derrotaFinal.Play();
        goalMeter.transform.DOShakePosition(2, 0.1f, 20);
        yield return new WaitForSeconds(3);
        boxMouth.TriggerRestartBox();
        yield return new WaitForSeconds(3);

        LoadLevel(CurrentLevel);

        apostador_A.isDisabled = false;
        apostador_B.isDisabled = false;
        boxMouth.EnableTrigger();
    }

    IEnumerator C_WonBoxCorroutine()
    {

        yield return new WaitForSeconds(1);
        sfx_victoryFinal.Play();
        DOTween.To(() => goalMeter.goalScale, x => goalMeter.goalScale = x, goalMeter.maxScale, vibrationDuration);
        goalMeter.transform.DOShakePosition(vibrationDuration, 0.1f, 20);
        yield return new WaitForSeconds(vibrationDuration + 2);
        if (currentLevelIndex == levelsList.Count - 1)
        {
            boxMouth.TriggerEnding();
            yield break;
        }
        boxMouth.TriggerNextBox();
        yield return new WaitForSeconds(2);

       
        LoadNewLevel();
        apostador_A.isDisabled = false;
        apostador_B.isDisabled = false;
        boxMouth.EnableTrigger();
    }
}
