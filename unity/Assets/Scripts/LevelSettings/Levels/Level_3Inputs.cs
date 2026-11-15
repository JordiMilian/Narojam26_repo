using UnityEngine;

[CreateAssetMenu(menuName = "Level/3Inputs", fileName = "Level_3Inputs")]
public class Level_3Inputs : LevelSettings_SO
{
    GameController.OutputKind lastOutput;
    public override GameController.OutputKind GetOutput(GameController.InputKind input)
    {
        if(input == GameController.InputKind.Input_C) { return lastOutput; }
        if(input == GameController.InputKind.Input_A) { return GameController.OutputKind.Output_A; }
        if(input == GameController.InputKind.Input_B) { return GameController.OutputKind.Output_B; }
        return GameController.OutputKind.Output_B;
    }
}
