using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Destructible : Entity
    {
        [SerializeField] private bool isIndestructable;
        public bool IsIndestructable => isIndestructable;
        [SerializeField] private int hitPoints;
        public int MaxHitPoints => hitPoints;
        private int currentHitPoints;
        public int HitPoints => currentHitPoints;

        private bool isDeath;
        
        protected virtual void Start()
        {
            currentHitPoints = hitPoints;
        }

        public void ApplyDamage(int damage)
        {
            if (isIndestructable || isDeath) return;

            currentHitPoints -= damage;

            if (currentHitPoints <= 0)
            {
                OnDeath();
                isDeath = true;
            }
        }

        public void ApplyHeal(int heal)
        {
            currentHitPoints += heal;

            if (currentHitPoints > hitPoints)
                currentHitPoints = hitPoints;
        }

        public void HealFull(int heal)
        {
            currentHitPoints = hitPoints;
        }

        public void BlockDamage(float blockTime)
        {
            currentHitPoints = hitPoints;

            isIndestructable = true;
        }
        
        public void GetDamage()
        {
            isIndestructable = false;
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);

            EventOnDeath?.Invoke();
        }

        private static HashSet<Destructible> allDestructables;

        public static IReadOnlyCollection<Destructible> AllDestructable => allDestructables;

        protected virtual void OnEnable()
        {
            if (allDestructables == null)
            {
                allDestructables = new HashSet<Destructible>();
            }

            allDestructables.Add(this);
        }

        protected virtual void OnDestroy()
        {
            allDestructables.Remove(this);
        }

        public const int TeamIdNeuTral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamID => m_TeamId;

        public UnityEvent EventOnDeath;

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
    }