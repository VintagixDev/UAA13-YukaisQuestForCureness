using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("Coeurs")]
    const int maxDisplayedHearts = 10; // Nombre de coeurs affichés
    public PlayerStats stats; // Statistiques du joueur
    public Image heart; // Prefab de l'UI Coeur
    public GameObject HPDisplay; // GameObject contenant tous les coeurs
    public TMP_Text heartText; // Texte +X

    [Header("Golds")]
    public TMP_Text GoldsText; // Texte du nombre de Coins

    [Header("Clés")]
    public TMP_Text KeysText;
    // Start is called before the first frame update
    void Start()
    {
        updateDisplayHearts();
        updateCollectableUI();
    }

    /// <summary>
    /// Afficher le nombre de coeurs que le joueur possède.
    /// </summary>
    public void updateDisplayHearts()
    {
       
        
        foreach (Transform ts in HPDisplay.transform)
        {
            Destroy(ts.gameObject);
        }
        

        float x = -510f;
        float y = 215f;
        for (int i = 0; i < stats.playerHP; i++)
        {
            if (i < maxDisplayedHearts)
            {
                // Passer a la ligne de coeur suivante
                if (i == 5)
                {
                    y = 165f;
                    x = -510f;
                }
                Image hChild = Instantiate(heart);
                hChild.transform.SetParent(HPDisplay.transform, false);
                hChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                x += 60f;
            }
        }

        // Afficher le "+X" si le nombre de coeur du joueur est > a maxDisplayedHearts
        if (stats.playerHP > maxDisplayedHearts)
        {
            heartText.text = "+" + (stats.playerHP - maxDisplayedHearts);
            GameObject parent = heartText.transform.parent.gameObject;
            parent.SetActive(true);
        }
    }

    public void updateCollectableUI()
    {
        GoldsText.text = ""+stats.playerGolds;
        KeysText.text = ""+stats.playerKeys;
    }

   
}
