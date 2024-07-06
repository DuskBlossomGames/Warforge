using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelManaging
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public GameObject elevator;
        public float elevatorVel;
        public uint elevatorRaiseTime;
        public PlayerController player;

        private readonly List<GameObject> _enemies = new();
        private EventWindow _elevatorRaise;
        private Camera _cam;
        
        private float _displacementPerFrame;
        
        private void Awake()
        {
            Spawn();
            _cam = Camera.main;
            _displacementPerFrame = elevatorVel * Time.fixedDeltaTime;
            _elevatorRaise = new EventWindow(elevatorRaiseTime, false);
        }

        private void FixedUpdate()
        {
            _elevatorRaise.Tick();
            if (_elevatorRaise.isActive)
            {
                elevator.transform.position += _displacementPerFrame * Vector3.up;
                player.transform.position += _displacementPerFrame * Vector3.up;
                _cam.transform.position += _displacementPerFrame * Vector3.up;

                if (_elevatorRaise.time == elevatorRaiseTime / 2)
                {
                    elevator.transform.position -= elevatorRaiseTime * _displacementPerFrame * Vector3.up;
                    player.transform.position -= elevatorRaiseTime * _displacementPerFrame * Vector3.up;
                    _cam.transform.position -= elevatorRaiseTime * _displacementPerFrame * Vector3.up;
                }

                if (_elevatorRaise.time == 0)
                {
                    Spawn();
                }
            }
            
            if (_enemies.Count != 0)
            {
                _enemies.RemoveAll(o => o == null);
                if (_enemies.Count == 0)
                {
                    _elevatorRaise.Restart();
                }
            }
        }
        
        private void Spawn()
        {
            var num = Random.Range(1, 5);
            
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

                enemy.GetComponent<EnemyInfo>().player = player;
                enemy.transform.position = new Vector3((left ? -1 : 1) *
                                                       (camWidth+1+enemyWidth/2 + enemyWidth*3/2*(left ? i : i-num/2)), 0, 0);
                
                _enemies.Add(enemy);
            }
        }
    }
}