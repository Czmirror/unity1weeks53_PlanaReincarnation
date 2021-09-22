using UnityEngine;

namespace Scripts.UI
{
    public class UIEffect: MonoBehaviour
    {
        
        private bool isMainColor = false;
        [SerializeField] Color color1 = Color.white, color2 = Color.white;
        [SerializeField] UnityEngine.UI.Image image = null;

        [SerializeField]
        CanvasGroup group = null;

        [SerializeField]
        Fade fade = null;

        void Start()
        {
//            fade.FadeOut(1);
            fade.FadeIn(1);
//            fade.FadeIn(1, () => {fade.FadeOut(1); });
//            Fadeout();
//            group.blocksRaycasts = false;
        }
        
        
        
        public void Fadeout()
        {
            group.blocksRaycasts = false;
            fade.FadeIn (1, () =>
            {
                image.color = (isMainColor) ? color1 : color2;
                isMainColor = !isMainColor;
                fade.FadeOut(1, ()=>{
                    group.blocksRaycasts = true;
                });
            });
        }
        
    }
}
