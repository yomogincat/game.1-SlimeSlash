using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("References")]
	protected Rigidbody2D rb;

	[SerializeField]
	protected Animator graphicsAnim;

	[SerializeField]
	protected Animator[] eyeAnim;

	protected AudioSource audioSource;

	protected Player player;

	protected Room room;

	[Header("Transforms")]
	[SerializeField]
	protected Transform graphics;

	[SerializeField]
	protected Transform wallRayR;

	[SerializeField]
	protected Transform wallRayL;

	[SerializeField]
	protected Transform playerRay;

	[Header("Layers")]
	[SerializeField]
	protected LayerMask groundLayer;

	[SerializeField]
	protected LayerMask playerLayer;

	[Header("Sounds")]
	[SerializeField]
	protected AudioClip hitSE;

	[Header("Stats")]
	[SerializeField]
	protected int maxHealth = 3;

	protected int health;

	[Header("Attack")]
	[SerializeField]
	private int damage = 1;

	[SerializeField]
	private Vector2 knockbackPower = new Vector2(5f, 5f);

	[SerializeField]
	private float knockbackTime = 0.1f;

	[Header("Timers")]
	[SerializeField]
	protected float invincibleTime = 0.2f;

	protected float invincibleTimer;

	protected float knockbackTimer;

	[Header("Checks")]
	[SerializeField]
	protected float wallRayLength = 0.2f;

	[SerializeField]
	protected float playerRayLength = 2f;

	protected RaycastHit2D wallHitR;

	protected RaycastHit2D wallHitL;

	protected bool isTouchingWallR;

	protected bool isTouchingWallL;

	protected bool isTouchingFrontWall;

	protected bool isTouchingBackWall;

	protected bool isPlayerAhead;

	protected bool wasPlayerAhead;

	protected bool isPlayerBehind;

	protected bool wasPlayerBehind;

	[Header("State")]
	[SerializeField]
	protected float direction = -1f;

	protected bool isGrounded;

	protected bool wasGrounded;

	protected bool isTouchingWall;

	protected bool isFocusing;

	public float Direction => direction;

	public event Action OnDie;

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
	}

	protected virtual void Start()
	{
		health = maxHealth;
		player = UnityEngine.Object.FindFirstObjectByType<Player>();
	}

	protected virtual void Update()
	{
		HandleDebug();
		HandleWallCheck();
		HandlePlayerCheck();
		HandleAI();
		HandleTimer();
		HandleSpriteDirection();
		HandleAnimation();
	}

	protected virtual void FixedUpdate()
	{
		HandleMovement();
	}

	protected virtual void HandleDebug()
	{
	}

	protected virtual void HandleTimer()
	{
		knockbackTimer = TickTimer(knockbackTimer);
		invincibleTimer = TickTimer(invincibleTimer);
	}

	protected void HandleWallCheck()
	{
		wallHitR = Physics2D.Raycast(wallRayR.position, Vector2.right, wallRayLength, groundLayer);
		wallHitL = Physics2D.Raycast(wallRayL.position, Vector2.left, wallRayLength, groundLayer);
		isTouchingWallR = wallHitR;
		isTouchingWallL = wallHitL;
		isTouchingFrontWall = ((direction > 0f) ? isTouchingWallR : isTouchingWallL);
		isTouchingBackWall = ((direction > 0f) ? isTouchingWallL : isTouchingWallR);
		isTouchingWall = isTouchingFrontWall || isTouchingBackWall;
	}

	protected virtual void HandlePlayerCheck()
	{
		PlayerFrontCheck();
	}

	protected virtual void HandleAnimation()
	{
		SetAnim("YVelocity", rb.linearVelocity.y);
		SetAnim("IsGrounded", isGrounded);
		SetAnim("IsFocusing", isFocusing);
	}

	protected void HandleSpriteDirection()
	{
		graphics.localScale = new Vector3(direction, 1f, 1f);
	}

	protected virtual void HandleAI()
	{
	}

	protected virtual void Die()
	{
		OnDie?.Invoke();
		UnityEngine.Object.Destroy(base.gameObject, 0.1f);
	}

	protected float TickTimer(float timer)
	{
		timer = Mathf.Max(timer - Time.deltaTime, 0f);
		return timer;
	}

	protected virtual void HandleMovement()
	{
		if (ShouldTurn())
		{
			direction = 0f - direction;
		}
		if (!(knockbackTimer > 0f))
		{
			Move();
		}
	}

	protected virtual void Move()
	{
	}

	protected void SetAnimValue<T>(Animator anim, string parameter, T value)
	{
		if (HasParameter(anim, parameter))
		{
			if (value is int value2)
			{
				anim.SetInteger(parameter, value2);
			}
			else if (value is float value3)
			{
				anim.SetFloat(parameter, value3);
			}
			else if (value is bool value4)
			{
				anim.SetBool(parameter, value4);
			}
		}
	}

	protected void SetAnim<T>(string parameter, T value)
	{
		SetAnimValue(graphicsAnim, parameter, value);
		Animator[] array = eyeAnim;
		foreach (Animator anim in array)
		{
			SetAnimValue(anim, parameter, value);
		}
	}

	private bool HasParameter(Animator anim, string paramName)
	{
		if (anim == null)
		{
			return false;
		}
		AnimatorControllerParameter[] parameters = anim.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].name == paramName)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void OnLanded()
	{
	}

	protected virtual bool ShouldTurn()
	{
		if (!ShouldTurnByObstacle() && !ShouldTurnByPlayer())
		{
			return ShouldTurnByRoom();
		}
		return true;
	}

	protected virtual bool ShouldTurnByObstacle()
	{
		return false;
	}

	protected virtual bool ShouldTurnByPlayer()
	{
		return false;
	}

	protected virtual bool ShouldTurnByRoom()
	{
		return false;
	}

	protected virtual bool CanAttack()
	{
		return true;
	}

	public virtual void Attack(Player player)
	{
		if (CanAttack())
		{
			OnAttack(player);
		}
		else
		{
			OnTouch(player);
		}
	}

	protected virtual void OnAttack(Player player)
	{
		player.HitDamage(damage, knockbackPower, direction, knockbackTime);
	}

	protected virtual void OnTouch(Player player)
	{
		player.TouchHitDamage(damage);
	}

	protected virtual void PlayerFrontCheck()
	{
		Vector2 dir = ((direction > 0f) ? Vector2.right : Vector2.left);
		isPlayerAhead = CheckPlayer(dir);
	}

	protected virtual void PlayerBackCheck()
	{
		Vector2 dir = ((direction > 0f) ? Vector2.left : Vector2.right);
		isPlayerBehind = CheckPlayer(dir);
	}

	protected bool CheckPlayer(Vector2 dir)
	{
		return Physics2D.Raycast(playerRay.position, dir, playerRayLength, playerLayer);
	}

	public virtual void HitDamage(int damage, Vector2 knockback, float direction, float knockbackTime)
	{
		if (!(invincibleTimer > 0f))
		{
			health -= damage;
			rb.linearVelocity = new Vector2(knockback.x * direction, knockback.y);
			knockbackTimer = knockbackTime;
			invincibleTimer = invincibleTime;
			audioSource.PlayOneShot(hitSE);
			if (health <= 0)
			{
				Die();
			}
		}
	}

	public void Initialize(float direction, Room room)
	{
		this.direction = direction;
		this.room = room;
	}
}
