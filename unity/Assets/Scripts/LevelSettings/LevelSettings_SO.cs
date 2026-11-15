using UnityEngine;
using static GameController;

public abstract class LevelSettings_SO : ScriptableObject
{
    public abstract GameController.OutputKind GetOutput(GameController.InputKind input);
    public int migajas_goal;
    public int migajas_iniciales;
    public int migajas_maxBet;
    public bool instantRespawn;
    public bool thirdInput = false;
}
