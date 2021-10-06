using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SaltyTank.UISystem
{
    public class AchievementPopUI : MonoBehaviour
    {
        public Animator animator;
        public TextMeshProUGUI title;

        public float timeToDestroy = 5f;

        private void Start()
        {
            animator.SetBool("isOpened", true);
            Invoke(nameof(ClosePop), timeToDestroy);
        }

        void ClosePop()
        {
            animator.SetBool("isOpened", false);
            Invoke(nameof(DestroyPop), 0.9f);
        }

        void DestroyPop()
        {
            Destroy(gameObject);
        }
    }
}