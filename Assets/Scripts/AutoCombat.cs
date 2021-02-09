using UnityEngine;

public class AutoCombat : Combatable
{
	private void Update()
	{
		if(attackCooldown >= 0)
		{
			attackCooldown -= Time.deltaTime;

			return;
		}

		if(enemy != null)
		{
			if(EnemyInRange() && SeeEnemy())
			{
				AttackEnemy();
			}
		}
	}
}
