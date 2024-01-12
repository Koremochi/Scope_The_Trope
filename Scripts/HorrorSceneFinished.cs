using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorSceneFinished : MonoBehaviour
{
    [SerializeField] GameObject activeFreddie; 
    [SerializeField] GameObject happyFreddie;

    private void Start()
    {
        if(ObjectInteraction.Interaction.horrorSceneFinished)
        {
            activeFreddie.SetActive(false);
            happyFreddie.SetActive(true);
        }
    }
}
