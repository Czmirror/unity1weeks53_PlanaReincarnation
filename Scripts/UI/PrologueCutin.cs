using UnityEngine;
using DG.Tweening;


namespace Scripts.UI
{
    public class PrologueCutin : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private RectTransform cutOutpanelRectTransform;
        
        [SerializeField] private float cutInPositionInitX = 1000;
        [SerializeField] private float cutOutPositionTargetX = -1000;
        
        private void Start()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        }

        public void PushButton()
        {
            CutOutUI();
            CutInUI();
        }
        
        public void CutInUI()
        {
            if (!panelRectTransform)
            {
                return;
            }

            panelRectTransform.anchoredPosition = new Vector2(cutInPositionInitX,0);
            panelRectTransform.DOLocalMoveX(0f, 1f).SetEase(Ease.Linear);
        }

        public void CutOutUI()
        {
            if (!cutOutpanelRectTransform)
            {
                return;
            }
            cutOutpanelRectTransform.DOLocalMoveX(cutOutPositionTargetX, 1f).SetEase(Ease.Linear);
        }
        
    }
}
