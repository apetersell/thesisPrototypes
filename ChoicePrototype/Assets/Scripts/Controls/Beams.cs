using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Beams : MonoBehaviour {

	public int owner;
	public float damage;
	public float XSpeed; // How fast the projectile moves horizontally.
	public float YSpeed; // How fast the projectiel moves vertically.
	public Vector2 dir;  // What direction does it move in.
	public float maxLifeSpan; // The maximum amount of time the projectile will last on screen (in frames).
	public float currentLifeSpan; // How much time left the projectile has until it dies (in frames).
	public Vector3 modPos; // Where it appears in relation to the player who fired it.
	public Rigidbody2D rb;
	public SpriteRenderer sr;

	//Kills the projectile if it is on screen for too long.
	public void handleDuration ()
	{
		currentLifeSpan--;
		if (currentLifeSpan <= 0) 
		{
			killProjectile ();
		}
	}

	//Makes sure the projectile moves.
	public abstract void handleMovement ();

	//Resets the projectile's lifespan back to max.
	public void resetProjectile ()
	{
		currentLifeSpan = maxLifeSpan;
	}

	public virtual void OnCollisionEnter2D (Collision2D coll)
	{
		//Handles collisions with players.
		if (coll.gameObject.tag == "Player") 
		{
			SoloPlayers p = coll.gameObject.GetComponent<SoloPlayers> ();
			if (p.playerNum != owner) {
				hitPlayer (p);
			} 
			//Ignores collision if colliding with player who performed that attack (so you can't hit yourself).
			if (p.playerNum == owner) 
			{
				Physics2D.IgnoreCollision (coll.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
			}
		}

		if (coll.gameObject.tag == "Fusion") 
		{
			Physics2D.IgnoreCollision (coll.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		}

		if (coll.gameObject.tag == "Wall") 
		{
			Doors d = coll.gameObject.GetComponent<Doors> ();
			if (d != null) 
			{
				if (owner == d.playerNum) 
				{
					Destroy (coll.gameObject);
					killProjectile ();
				}
			}
		}
	}

	// Hanldes what happens when a projectile is killed.
	void killProjectile ()
	{
			Destroy (this.gameObject);
	}

	//Hanldes the colliions interaction between players and projectiles (same as in Attack Script).
	public virtual void hitPlayer (SoloPlayers p)
	{
		killProjectile ();
		p.takeDamage (damage);
	}
}
