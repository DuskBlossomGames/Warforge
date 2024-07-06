using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackControler : MonoBehaviour
{
    public uint prebuffersize;

    public PlayerAttack_0 attack_0_script;
    private InputAction _atk_0;
    private EventWindow _cd_0 = new(5);
    private EventWindow _atk0_prebuffer = new(0);
    private bool _didAtk_0;

    public PlayerAttack_1 attack_1_script;
    private InputAction _atk_1;
    private EventWindow _cd_1 = new(5);
    private EventWindow _atk1_prebuffer = new(0);
    private bool _didAtk_1;

    private PlayerController _controller;
    private EventWindow _cooldown = new(0);


    void Awake()
    {
        _atk_0 = InputSystem.actions.FindAction("Attack_0");
        _atk_1 = InputSystem.actions.FindAction("Attack_1");
        _controller = GetComponent<PlayerController>();
    }


    void FixedUpdate()
    {
        _cd_0.Tick();
        _cd_1.Tick();
        _cooldown.Tick();
        _atk0_prebuffer.Tick();
        _atk1_prebuffer.Tick();

        if (DidAtk_0())
        {
            _atk0_prebuffer.RestartAt(prebuffersize);
        }

        if (_atk0_prebuffer.isActive && _cd_0.hasEnded && _cooldown.hasEnded)
        {
            attack_0_script.Attack();
            _cooldown.RestartAt(attack_0_script.atkTime);
            _cd_0.Restart();
            return;
        }

        if (DidAtk_1())
        {
            _atk1_prebuffer.RestartAt(prebuffersize);
        }
        if (_atk1_prebuffer.isActive && _controller.floorCheck.isGrounded && _cd_1.hasEnded && _cooldown.hasEnded)
        {
            attack_1_script.Attack();
            _cooldown.RestartAt(attack_0_script.atkTime);
            _cd_1.Restart();
            return;
        }
    }
    private void Update()
    {
        if (_atk_0.WasPressedThisFrame())
        {
            _didAtk_0 = true;
        }
        if (_atk_1.WasPressedThisFrame())
        {
            _didAtk_1 = true;
        }
    }
    bool DidAtk_0()
    {
        if (_didAtk_0)
        {
            _didAtk_0 = false;
            return true;
        }
        return false;
    }
    bool DidAtk_1()
    {
        if (_didAtk_1)
        {
            _didAtk_1 = false;
            return true;
        }
        return false;
    }
}
