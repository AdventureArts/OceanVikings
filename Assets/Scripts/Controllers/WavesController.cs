using UnityEngine;
using System.Collections.Generic;

public class WavesController : MonoBehaviour
{
	List<GameObject> hiddenWaves = new List<GameObject>();
	List<GameObject> unHiddenWaves = new List<GameObject>();
	
	GameObject firstWave;
	GameObject lastWave;

	float viewWidth;
	float leftLimit;
	float oceanWidth;
	float visibleOcean;

	void Start()
	{
		viewWidth = Camera.main.orthographicSize * Screen.width / Screen.height * 2;
		leftLimit = Camera.main.transform.position.x - viewWidth / 2;

		oceanWidth = 0.0f;
		visibleOcean = 0.0f;

		loadWaves();

		if (hiddenWaves.Count == 0) return;

		firstWave = (GameObject)Instantiate(hiddenWaves[1]);
		lastWave = firstWave;

		Vector3 newPosition = new Vector3();
		newPosition.x = leftLimit;

		firstWave.transform.position = newPosition;

		unHiddenWaves.Add(firstWave);

		firstWave.SetActive(true);

		SpriteRenderer sr;

		sr = firstWave.GetComponent<SpriteRenderer>();

		if (sr == null) return;

		oceanWidth = sr.sprite.bounds.size.x;
		visibleOcean = oceanWidth;
	}

	void Update()
	{
		Debug.DrawRay(new Vector3(leftLimit, 0, 0), new Vector3(0, 1, 0));
		Debug.DrawRay(new Vector3(leftLimit + viewWidth, 0, 0), new Vector3(0, 1, 0));

		updateWaves();
	}

	void updateWaves()
	{
		if (unHiddenWaves.Count == 0) return;

		SpriteRenderer sr;
		float oceanLeftOffset;

		oceanWidth = 0.0f;

		foreach (GameObject wave in unHiddenWaves)
		{
			sr = wave.GetComponent<SpriteRenderer>();

			oceanWidth += sr.sprite.bounds.size.x;
		}

		sr = firstWave.GetComponent<SpriteRenderer>();

		oceanLeftOffset = sr.sprite.bounds.size.x / 2.0f;
		oceanLeftOffset = firstWave.transform.position.x - oceanLeftOffset;
		oceanLeftOffset = Mathf.Abs(oceanLeftOffset) - Mathf.Abs(leftLimit);

		visibleOcean = oceanWidth - oceanLeftOffset;

		while (visibleOcean < viewWidth)
		{
			int randWave;
			Vector3 newPosition;
			GameObject newWave;
			WaveAI newWaveAI;

			randWave = Random.Range(0, hiddenWaves.Count - 1);

			newWave = (GameObject)Instantiate(hiddenWaves[randWave]);
			newPosition = lastWave.transform.position;

			sr = lastWave.GetComponent<SpriteRenderer>();

			if (sr == null) return;

			newPosition.x += sr.sprite.bounds.size.x / 2.0f;

			sr = newWave.GetComponent<SpriteRenderer>();

			if (sr == null) return;

			newPosition.x += sr.sprite.bounds.size.x / 2.0f;
			newWave.transform.position = newPosition;
			newWave.SetActive(true);

			newWaveAI = newWave.GetComponent<WaveAI>();

			if (newWaveAI != null && unHiddenWaves.Count > 0)
			{
				newWaveAI.setNextWave(lastWave);
			}

			lastWave = newWave;

			unHiddenWaves.Add(lastWave);

			oceanWidth += sr.sprite.bounds.size.x;
			
			visibleOcean = oceanWidth - oceanLeftOffset;
		}

		sr = firstWave.GetComponent<SpriteRenderer>();

		oceanLeftOffset = sr.sprite.bounds.size.x / 2.0f;
		oceanLeftOffset = firstWave.transform.position.x - oceanLeftOffset;

		if (leftLimit - oceanLeftOffset > sr.sprite.bounds.size.x)
		{
			if (unHiddenWaves.Count < 2) return;

			GameObject oldFirstWave;
			WaveAI waveAI;

			oldFirstWave = firstWave;
			firstWave = unHiddenWaves[1];
			unHiddenWaves.RemoveAt(0);
			DestroyImmediate(oldFirstWave);

			waveAI = unHiddenWaves[0].GetComponent<WaveAI>();

			if (waveAI != null)
			{
				waveAI.setNextWave(null);
			}
		}
	}

	void loadWaves()
	{
		GameObject[] waves = Resources.LoadAll<GameObject>("Prefabs/Waves/");

		foreach (GameObject wave in waves)
		{
			hiddenWaves.Add(wave);
		}

		/*
		Sprite[] sprites;

		sprites = Resources.LoadAll<Sprite>("Sprites/Waves/");

		foreach (Sprite sprite in sprites)
		{
			GameObject wave = new GameObject(sprite.name);
			SpriteRenderer sr;

			wave.SetActive(false);

			sr = wave.AddComponent<SpriteRenderer>();
			sr.sprite = sprite;

			wave.AddComponent<PolygonCollider2D>();
			wave.AddComponent<Rigidbody2D>();

			wave.rigidbody2D.gravityScale = 0.0f;
			wave.rigidbody2D.isKinematic = true;

			wave.AddComponent<WaveAI>();

			hiddenWaves.Add(wave);
		}
		*/
	}
}
















