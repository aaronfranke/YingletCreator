using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
    public class BookmarkIndenting : ReactiveBehaviour
    {
        [SerializeField] float _notSelectedIndent;
        [SerializeField] EaseSettings _easeSettings;
        private Vector3 _originalPos;
        private IClipboardElementSelection _selection;
        private IHoverableDetector _hoverable;
        private Coroutine _transitionCoroutine;

        private void Awake()
        {
            _originalPos = transform.localPosition;
            _selection = this.GetComponent<IClipboardElementSelection>();
            _hoverable = this.GetComponent<IHoverableDetector>();
        }

        void Start()
        {
            AddReflector(Reflect);
        }

        private void Reflect()
        {
            Vector3 fromPos = transform.localPosition;
            Vector3 toPos = _originalPos;
            bool keepLeft = _selection.IsSelected.Val || _hoverable.Hovered.Val;
            if (!keepLeft)
            {
                toPos += Vector3.right * _notSelectedIndent;
            }
            this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => transform.localPosition = Vector3.LerpUnclamped(fromPos, toPos, p));
        }
    }
}