using UnityEngine;
using System.Collections;

public class ShipAI : MonoBehaviour
{
	private Vector3 waterSidePosition;
	private Vector3 shipSidePosition;
	private GameObject waterSideGameObject;
	private CircleCollider2D[] colliders;
	private Vector2[] collidersCentre;
	private Rigidbody2D waterSideRigidbody2D;
	private Quaternion shipSideRotation;
	private Quaternion waterSideRotation;

	public float speedFactor = 8.0f;
	public float rotationFactor = 2.0f;

	void Start()
	{
		gameObject.layer = LayerMask.NameToLayer("Default");

		waterSidePosition = transform.position;
		waterSidePosition.z = 0.0f;

		shipSidePosition = transform.position;
		shipSidePosition.z = -1.0f;

		transform.position = shipSidePosition;
		transform.rotation = new Quaternion();

		waterSideGameObject = new GameObject("Water Colliders");
		waterSideGameObject.layer = LayerMask.NameToLayer("Water");
		waterSideGameObject.transform.position = waterSidePosition;
		waterSideGameObject.transform.rotation = new Quaternion();

		colliders = new CircleCollider2D[2];

		colliders[0] = waterSideGameObject.AddComponent<CircleCollider2D>();
		colliders[1] = waterSideGameObject.AddComponent<CircleCollider2D>();

		colliders[0].radius = 0.1f;
		colliders[1].radius = 0.1f;

		collidersCentre = new Vector2[2];

		collidersCentre[0] = new Vector2(-0.15f, 0.15f);
		collidersCentre[1] = new Vector2(0.15f, 0.15f);

		colliders[0].center = collidersCentre[0];
		colliders[1].center = collidersCentre[1];

		waterSideRigidbody2D = waterSideGameObject.AddComponent<Rigidbody2D>();
		waterSideRigidbody2D.gravityScale = 1.0f;
		waterSideRigidbody2D.drag = 10.0f;
	}

	void Update()
	{
		lockWaterSideObject();
		softMovement();
		softRotation();
	}

	void lockWaterSideObject()
	{
		waterSidePosition = waterSideGameObject.transform.position;
		waterSidePosition.x = -2.0f;
		waterSideGameObject.transform.position = waterSidePosition;
	}

	void softMovement()
	{
		shipSidePosition = transform.position;
		waterSidePosition = waterSideGameObject.transform.position;

		shipSidePosition = Vector3.Lerp(shipSidePosition, waterSidePosition, Time.deltaTime * speedFactor);

		shipSidePosition.z = -1.0f;

		transform.position = shipSidePosition;
	}

	void softRotation()
	{
		shipSideRotation = transform.rotation;
		waterSideRotation = waterSideGameObject.transform.rotation;

		shipSideRotation = Quaternion.Lerp(shipSideRotation, waterSideRotation, Time.deltaTime * rotationFactor);

		transform.rotation = shipSideRotation;
	}
}






















