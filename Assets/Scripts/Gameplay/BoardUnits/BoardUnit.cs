using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardUnit : MasterMonoBehaviour
{
    public string unitName;

    public int maxHealth;
    private int _health;
    public int Health { get => _health; }
    [HideInInspector] public bool isDead = false;

    public SpriteRenderer mainSpriteRenderer;
    public SpriteRenderer outline;
    public Vector2 dimension;

    [HideInInspector] public Tile originTile;


    public Action OnHealthChange;

    public virtual void Awake()
    {
        _health = maxHealth;
    }

    #region Health
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
    }

    public void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    public void ResetUnit()
    {
        ChangeHealth(maxHealth);
        isDead = false;
        OnHealthChange = null;
        originTile = null;
        outline.enabled = false;
    }

    /// <summary>
    /// delta will be added to Health
    /// </summary>
    /// <param name="delta"></param>
    private void ChangeHealth(int delta)
    {
        _health += delta;
        OnHealthChange?.Invoke();
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
        else if (_health > maxHealth)
        {
            _health = maxHealth;
        }
    }
    #endregion

    public void Die()
    {
        isDead = true;
        BoardManager.Instance.RemoveUnit(this);

        if (this == BoardManager.Instance.selectedBoardUnit)
        {
            BoardUI.Instance.ResetSelectedBoardUnit();
        }
        Destroy();
    }

    public virtual void Destroy()
    {
        ResetUnit();
        gameObject.SetActive(false);
    }

    public virtual void Placed(Tile originTile) 
    {
        this.originTile = originTile;
    }

    public virtual void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        ProductionManager.Instance.ResetSelectedPlaceableUnit();
        BoardUI.Instance.BoardUnitSelected(this);
    }

    public override string ToString()
    {
        return "Max Health: " + maxHealth.ToString() + "\r\n" +
            "Health: " + Health;
    }
}
