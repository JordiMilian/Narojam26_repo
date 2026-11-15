using UnityEngine;
[CreateAssetMenu(menuName = "Level/Money", fileName = "Level_Money")]
public class Level_money : LevelSettings_SO
{

    public override GameController.OutputKind GetOutput(GameController.InputKind input)
    {
        if (GameController.instance.lostLastBet) { return GameController.OutputKind.Output_B; }
        else { return GameController.OutputKind.Output_A;}
    }
}
