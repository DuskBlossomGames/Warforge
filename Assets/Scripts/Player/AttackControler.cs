using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackControler : MonoBehaviour
{
    public PlayerAttack_0 attack_0_script;
    public PlayerAttack_1 attack_1_script;

    private InputAction _atk_0;
    private InputAction _atk_1;

    private bool _didAtk_0;
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
        _cooldown.Tick();
        if (_cooldown.hasEnded) {

            if (DidAtk_0())
            {
                attack_0_script.Attack();
                _cooldown.RestartAt(attack_0_script.atkTime);
                return;
            }

            if (DidAtk_1() && _controller.floorCheck.isGrounded)
            {
                attack_1_script.Attack();
                _cooldown.RestartAt(attack_1_script.atkTime);
                return;
            }
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
