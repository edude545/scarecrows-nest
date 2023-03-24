using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaggleMeter : MonoBehaviour
{

    public GameObject LeftDistanceMeter;
    public GameObject RightDistanceMeter;
    public GameObject LeftWaggleMeter;
    public GameObject RightWaggleMeter;

    public TMP_Text TextDisplay;

    private void Update()
    {
        LeftDistanceMeter.transform.localScale = new Vector3(1, GameController.LeftArmExtension, 1);
        RightDistanceMeter.transform.localScale = new Vector3(1, GameController.RightArmExtension, 1);
        LeftWaggleMeter.transform.localScale = new Vector3(1, GameController.LeftHandPosDelta * GameController.LeftHandPosDelta * 1024, 1);
        RightWaggleMeter.transform.localScale = new Vector3(1, GameController.RightHandPosDelta * GameController.RightHandPosDelta * 1024, 1);
        TextDisplay.text = GameController.LeftHandPosDelta + "   " + GameController.WaggleScore + "   " + GameController.RightHandPosDelta;
    }

}
