using UnityEngine;

    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float lifeTime;

        private float timer;

        private void Update()
        {
            if (timer < lifeTime)
                timer += Time.deltaTime;
            else
                Destroy(gameObject);
        }
    }