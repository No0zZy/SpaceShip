using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawn : MonoBehaviour
{
    [SerializeField] private float m_Radius;

    [SerializeField] private SpaceShip m_SpaceShipPrefab;

    [SerializeField] private int m_NumBots;

    [SerializeField] private AIPointPatrol[] m_PatrolArea;

    [SerializeField] private int m_TeamId;

    [SerializeField] private bool m_AutoSpawnOnStart;

    private void Start()
    {
        m_BotsAlive = 0;
        if (m_AutoSpawnOnStart)
            SpawnBots(m_NumBots);
    }

    public void SpawnBots(int NumBots)
    {
        for (int i = 0; i < NumBots; i++)
        {
            var bot = Instantiate(m_SpaceShipPrefab.gameObject);

            m_BotsAlive++;

            bot.transform.position = transform.position + Random.insideUnitSphere * m_Radius;
            bot.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);

            bot.GetComponent<SpaceShip>().SetTeamId(m_TeamId);
            bot.GetComponent<SpaceShip>().Death += MinusAliveBot;


            var botAI = bot.GetComponent<SpaceShipAIController>();
            botAI.SetPatrolBehavior(m_PatrolArea);
            botAI.SetAccuracy(Random.Range(0.0f, 1.0f));
        }
    }

    [SerializeField] private int m_BotsAlive;

    private void MinusAliveBot()
    {
        m_BotsAlive--;
    }
}
