using System;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelManaging
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public PlayerController player;

        private void Awake()
        {
            Spawn(2);
        }

        private void Spawn(int num)
        {
            var camWidth = Camera.main!.aspect * Camera.main.orthographicSize;
            var enemyWidth = enemyPrefab.transform.localScale.x;

            for (var i = 0; i < num; i++)
            {
                var left = i < num / 2;
                
                var enemy = Instantiate(enemyPrefab);
                if (enemy.TryGetComponent<RunAtPlayerAI>(out var ai))
                {
                    ai.player = player;
                }
                enemy.transform.position = new Vector3((left ? -1 : 1) *
                                                       (camWidth+enemyWidth/2 + enemyWidth*3/2*(left ? i : i-num/2)), 0, 0);
            }
        }
    }
}