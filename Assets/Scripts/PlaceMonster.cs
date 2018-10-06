using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMonster : MonoBehaviour
{
    [SerializeField] private MonsterData monsterPrefab;
    private MonsterData monster;
    private GameManagerBehavior gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManagerBehavior>();
    }

    private bool CanPlaceMonster()
    {
        int cost = monsterPrefab.levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }

    private bool CanUpgradeMonster()
    {
        if (monster != null)
        {
            MonsterLevel nextLevel = monster.GetNextLevel();
            if (nextLevel != null && nextLevel.cost <= gameManager.Gold)
            {
                return true;
            }
        }
        return false;
    }

    private void OnMouseUp()
    {
        if (CanPlaceMonster())
        {
            monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);

            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
            gameManager.Gold -= monster.CurrentLevel.cost;
        }
        else if (CanUpgradeMonster())
        {
            monster.IncreaseLevel();
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            gameManager.Gold -= monster.CurrentLevel.cost;
        }
    }
}