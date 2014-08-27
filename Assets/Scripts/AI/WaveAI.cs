using UnityEngine;
using System.Collections;

public class WaveAI : MonoBehaviour
{
	GameObject nextWave = null;
	Vector3 instantPosition;

	Vector3 startRay;
	Vector3 rayDir = new Vector3(0.0f, 1.0f, 0.0f);

	SpriteRenderer sr;

	public float velocity = 16.0f;

	void Start()
	{

	}

	void Update()
	{
		if (rigidbody2D != null)
		{
			if (rigidbody2D.velocity.magnitude != velocity)
			{
				rigidbody2D.velocity = new Vector2(-1.0f, 0.0f);
				rigidbody2D.velocity *= velocity * Time.deltaTime;
			}
		}

		if (nextWave != null)
		{
			sr = nextWave.GetComponent<SpriteRenderer>();

			if (sr != null)
			{
				float tempDistance;
				float tempPosition;

				tempDistance = sr.sprite.bounds.size.x / 2.0f;
				tempPosition = nextWave.transform.position.x + tempDistance;

				sr = GetComponent<SpriteRenderer>();

				if (sr != null)
				{
					tempPosition += sr.sprite.bounds.size.x / 2.0f;

					if (transform.position.x != tempPosition)
				    {
						instantPosition = transform.position;
						instantPosition.x = tempPosition;
						transform.position = instantPosition;
					}
				}
			}
		}

		startRay = transform.position;
		sr = GetComponent<SpriteRenderer>();

		if (sr != null)
		{
			startRay.x -= sr.sprite.bounds.size.x / 2;
			startRay.y = -1.0f;

			Debug.DrawRay(startRay, rayDir);
		}
	}

	public void setNextWave(GameObject nextWave)
	{
		this.nextWave = nextWave;
	}
}
