using UnityEngine;

namespace Enemies.Behaviors
{
    public class MovementController : MonoBehaviour
    {
        public float maxSpeed;
        public float gravity;
        public uint knockbackFreezeTime;
        public float jumpHeight;
        public float accel;

        public MoveMode mode = MoveMode.PLAYER;
        
        private float _curSpeed;
        private float _jumpVel;
        private Rigidbody2D _rb;
        private FloorCheck _fc;
        private EventWindow _knockback;
        private float _knockbackDecel;
        
        public enum MoveMode { NONE, PLAYER, PLAYER_AWAY }
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _fc = GetComponentInChildren<FloorCheck>();
            _jumpVel = Mathf.Sqrt(2 * jumpHeight * gravity);

            _knockback = new EventWindow(knockbackFreezeTime, startActive: false);
        }
        
        private void FixedUpdate()
        {
            _knockback.Tick();
            
            if (!_knockback.isActive)
            {
                if (mode is MoveMode.PLAYER or MoveMode.PLAYER_AWAY)
                {
                    var dist = PlayerController.Player.transform.position.x - transform.position.x;
                    if (mode == MoveMode.PLAYER_AWAY) dist *= -1;
                    
                    _curSpeed = Mathf.Lerp(_curSpeed, Mathf.Sign(dist) * maxSpeed, accel);

                    transform.localScale = Util.SetX(transform.localScale, Mathf.Abs(transform.localScale.x)*Mathf.Sign(dist));
                }
                else
                {
                    _curSpeed = Mathf.Lerp(_curSpeed, 0, accel);
                }
            }
            else
            {
                _curSpeed -= _knockbackDecel * Time.fixedDeltaTime;
            }

            _rb.velocity = new Vector2(_curSpeed, _rb.velocity.y);
            _rb.velocity -= new Vector2(0, gravity) * Time.fixedDeltaTime;
        }

        public void Jump()
        {
            if (_fc.isGrounded) _rb.velocity += _jumpVel * Vector2.up;
        }
        
        public void Knockback(float xDist, float yDist)
        {
            xDist += Mathf.Sign(xDist)*2*Random.value;
            yDist += Mathf.Sign(yDist)*2*Random.value;
            
            _knockbackDecel = 2 * xDist / Mathf.Pow(knockbackFreezeTime * Time.fixedDeltaTime, 2);
        
            _curSpeed = Mathf.Sign(xDist)*Mathf.Sqrt(2 * Mathf.Abs(_knockbackDecel * xDist));
            _rb.velocity = _curSpeed * Vector2.right + Mathf.Sqrt(2 * yDist * gravity) * Vector2.up;
            _knockback.Restart();
        }
    }
}