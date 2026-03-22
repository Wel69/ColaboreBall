using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue =  0;
    Text score;

    // Start é chamado antes da primeira atualização de frame
    void Start()
    {
        score = GetComponent<Text>();
    }

    // Update é chamado a cada frame
    void Update()
    {
        score.text = "" + scoreValue;
    }
}
