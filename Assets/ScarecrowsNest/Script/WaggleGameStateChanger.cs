using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaggleGameStateChanger : WaggleTrigger
{

    public GameController.GameStates TargetState;
    public Image Image;

    void Update()
    {
        updateWaggleScore();
        float f = Waggle / MaxWaggle;
        Image.color = new Color(1 * f, 1 * (1 - f), 0);
        if (Waggle > MaxWaggle)
        {
            GameController.Instance.ChangeGameState(TargetState);
            Waggle = 0f;
        }
    }

}
