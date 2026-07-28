using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Image orbPrefab;
    [SerializeField] Sprite fullOrb;
    [SerializeField] Sprite emptyOrb;

    List<Image> orbs = new();
    void Start()
    {
        
    }

    void Update()
    {
        UpdateOrbs();
    }

    void UpdateOrbs()
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            if (i < player.Health)
            {
                orbs[i].sprite = fullOrb;
            }
            else
            {
                orbs[i].sprite = emptyOrb;
            }
        }
    }
    public void GenerateOrbs(int maxHealth)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        orbs.Clear();
        for (int i = 0; i < maxHealth; i++)
        {
            Image orb = Instantiate(orbPrefab, transform);
            orbs.Add(orb);
        }
    }


}
