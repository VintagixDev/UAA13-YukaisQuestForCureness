using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    //[Header("Scripts")]
    //[SerializeField, Tooltip("Superviseur")]
    //private GameSupervisor MASTER;

    //[Header("Player")]
    //[SerializeField, Tooltip("Joueur")]
    //private GameObject _player;

    [Header("Coeurs")]
    const int maxDisplayedHearts = 10; // Nombre de coeurs affichés
    public PlayerStats stats; // Statistiques du joueur
    public GameObject heart; // Prefab de l'UI Coeur
    public GameObject HPDisplay; // GameObject contenant tous les coeurs
    public TMP_Text heartText; // Texte +X

    [Header("Golds")]
    public TMP_Text GoldsText; // Texte du nombre de Coins

    [Header("Clés")]
    public TMP_Text KeysText;
    // Start is called before the first frame update

    /// <summary>
    /// Ajout de ma part signé -Andras
    /// </summary>
    private void Start()
    {
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        updateDisplayHearts();
        updateCollectableUI();
    }

    private void Update()
    {
        if (stats == null)
        {
            stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
            updateDisplayHearts();
            updateCollectableUI();
        }
    }

    /// <summary>
    /// Afficher le nombre de cœurs que le joueur possède.
    /// </summary>
    public void updateDisplayHearts()
    {
        // Vider les anciens coeurs
        foreach (Transform child in HPDisplay.transform)
        {
            Destroy(child.gameObject);
        }

        int maxDisplayedHearts = 10;
        float x = -510f;
        float y = 215f;

        // Affiche jusqu'à 10 coeurs
        int heartsToDisplay = Mathf.Min(stats.playerHP, maxDisplayedHearts);

        for (int i = 0; i < heartsToDisplay; i++)
        {
            GameObject hChild = Instantiate(heart);
            hChild.transform.SetParent(HPDisplay.transform, false);
            hChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            x += 60f;
        }

        // Si le joueur a plus de 10 coeurs, affiche +X
        if (stats.playerHP > maxDisplayedHearts)
        {
            heartText.text = "+" + (stats.playerHP - maxDisplayedHearts);
            heartText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            heartText.transform.parent.gameObject.SetActive(false);
        }
    }


    public void updateCollectableUI()
    {
        GoldsText.text = ""+stats.playerGolds;
        KeysText.text = ""+stats.playerKeys;
    }
}
