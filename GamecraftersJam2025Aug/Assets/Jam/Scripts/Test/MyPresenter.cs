using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Test
{
    public class MyFutureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MyPresenter>().AsSingle();
            Container.Bind<MyModel>().AsSingle();
            Container.Bind<MyView>().AsSingle();
        }
    }
    
    public class MyPresenter : IInitializable, IDisposable
    {
        [Inject] private MyModel model;
        [Inject] private MyView view;

        public void Initialize()
        {
            model.OnHealthChanged += view.UpdateHealth;
            view.MyButton.onClick.AddListener(Take10Damage);
        }

        public void Dispose()
        {
            model.OnHealthChanged -= view.UpdateHealth;
            view.MyButton.onClick.RemoveListener(Take10Damage);
        }

        private void Take10Damage()
        {
            TakeDamage(10);
        }

        public void TakeDamage(int damage)
        {
            model.DecreaseHealth(damage);
            //other buisness logic
        }

    }

    public class MyModel
    {
        public int health;

        public Action<int> OnHealthChanged = delegate { };

        public void DecreaseHealth(int damage)
        {
            health -= damage;
            if (health < 0)
                health = 0;

            OnHealthChanged.Invoke(health);
        }

        public int GetHealth()
        {
            return health;
        }
    }

    public class MyView : MonoBehaviour
    {
        [Required]
        [SerializeField]
        public TextMeshPro _healthText;

        [field: Required]
        [field: SerializeField]
        public Button MyButton { get; private set; }

        public void UpdateHealth(int health)
        {
            _healthText.text = health.ToString();
        }
    }
}