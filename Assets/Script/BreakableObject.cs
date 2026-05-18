using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreakableObject : MonoBehaviour, IAttackable, IInteractable
{
	[SerializeField]
	private float m_copperQuantityToDrop;

	[SerializeField]
	private float m_life;

	[SerializeField]
	private Sprite m_brokenSprite;
	
	private bool m_isBroken;
	private SpriteRenderer m_spriteRenderer;
	
	private void Awake()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void IsAttacked(float damage)
	{
		m_life -= damage;
		if (m_life <= 0)
		{
			m_isBroken = true;
			
			//Change the sprite (color for now)
			// m_spriteRenderer.sprite = m_brokenSprite;
			m_spriteRenderer.color = Color.red;
		}
	}
	
	public float Interact()
	{
		if (!m_isBroken)
			return 0;
		
		Destroy(gameObject);
		return m_copperQuantityToDrop;
	}
}
