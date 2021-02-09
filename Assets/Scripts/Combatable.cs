using System;
using UnityEngine;

public class Combatable : MonoBehaviour, ICombatable
{
	public event Action<ICombatable> Died;

	[SerializeField]
	protected Combatable enemy;
	[SerializeField]
	private float size;
	[SerializeField]
	private float attackRange;
	[SerializeField]
	private float damage;
	[SerializeField]
	private float health;
	[SerializeField]
	private float attackAngle;
	[SerializeField]
	private float attackSpeed;

	public float Size => size;
	public float AttackRange => attackRange;
	public float Damage => damage;
	public float Health => health;
	public Vector3 Position => transform.position;
	public float AttackAngle => attackAngle;
	public float AttackSpeed => attackSpeed;

	protected float attackCooldown = 0f;

	private void Awake()
	{
		if(enemy != null)
		{
			enemy.Died += Enemy_Died;
		}
	}

	public void ReceiveDamage(ICombatable damageDealer, float damage)
	{
		health -= damage;
		if(health <= 0)
		{
			Died?.Invoke(this);
			Destroy(this.gameObject);
		}
	}

	public void AttackEnemy()
	{
		enemy.ReceiveDamage(this, Damage);
		attackCooldown = 1 / AttackSpeed;
	}

	public bool EnemyInRange()
	{
		return AttackRange + enemy.Size > Vector3.Distance(transform.position, enemy.Position);
	}

	public bool SeeEnemy()
	{
		//is in fov
		var vecForward = transform.forward;
		var vecToEnemy = enemy.Position - transform.position;

		if(Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized)) > AttackAngle)
		{
			return false;
		}

		//is in distance
		if(AttackRange < Vector3.Distance(enemy.Position, transform.position))
		{
			return false;
		}

		return true;
	}

	private void Enemy_Died(ICombatable obj)
	{
		enemy = null;
	}
}

public interface ICombatable
{
	event Action<ICombatable> Died;

	float AttackRange { get; }
	float AttackAngle { get; }
	float Damage { get; }
	float Health { get; }
	float Size { get; }
	Vector3 Position { get; }
	//attacks per second
	float AttackSpeed { get; }

	void ReceiveDamage(ICombatable damageDealer, float damage);
	void AttackEnemy();
	bool EnemyInRange();
	bool SeeEnemy();
}

//algebra liniowa, 
