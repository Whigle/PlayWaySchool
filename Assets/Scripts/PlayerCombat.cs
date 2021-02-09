using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combatable
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
			if(Input.GetKeyDown(KeyCode.Space))
			{
				if(EnemyInRange() && SeeEnemy() && Input.GetKeyDown(KeyCode.Space))
				{
					AttackEnemy();
				}
			}
		}
	}
}
