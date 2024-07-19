using System;
using System.Collections.Generic;
using DefaultNamespace.Player;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;

public class PlayerAttack_2 : MonoBehaviour
{
    public BulletController bulletObj;
    public uint atkTime;
    public uint spawnTime;
    public uint baseDmg;
    public float kb;
    public float bulletVel;
    public float bulletRange;
    public uint numBullets;
    public float hopVel;
    public float coneDegrees;
    public float spawnDist;
    public uint pierce;

    private PlayerController _player;
    private bool _isAttacking;
    private EventWindow _atkTimer;
    private EventWindow _spawnTimer;
    private uint _spawnedBullets;
    private Vector3 _spawnPos;
    private List<BulletController> _bullets;

    private void Awake()
    {
        _spawnTimer = new EventWindow(spawnTime, false);
        _atkTimer = new EventWindow(atkTime, false);
        _player = GetComponent<PlayerController>();
    }

    public void Attack()
    {
        if (!_isAttacking)
        {
            _spawnPos = transform.position;
            _atkTimer.Restart();
            _spawnTimer.Restart();
            _isAttacking = true;
            _spawnedBullets = 0;
            _bullets = new List<BulletController>();
        }
    }

    private void FixedUpdate()
    {
        _spawnTimer.Tick();
        _atkTimer.Tick();

        if (_isAttacking)
        {
            _player.SetYVel(hopVel);

            if (numBullets * (1 - _spawnTimer.Percent()) > _spawnedBullets)
            {
                var bullet = Instantiate(bulletObj.gameObject);

                bullet.GetComponent<SpriteRenderer>().enabled = true;
                
                var deg = 270 + coneDegrees/2 - (coneDegrees+coneDegrees/numBullets)*((float) _spawnedBullets/numBullets);
                bullet.transform.rotation = Quaternion.Euler(0, 0, deg);

                var rad = deg * Mathf.Deg2Rad;
                bullet.transform.position = _spawnPos + new Vector3(spawnDist*Mathf.Cos(rad),
                    spawnDist*Mathf.Sin(rad), 0); 
                
                _bullets.Add(bullet.GetComponent<BulletController>());
                
                _spawnedBullets += 1;
            }

            if (_spawnedBullets == numBullets)
            {
                _bullets.ForEach(b =>
                    b.Shoot(bulletVel, kb, baseDmg, bulletRange, pierce));
                _spawnedBullets++;
            }
            
            _isAttacking = _atkTimer.isActive;
        }
    }
}
