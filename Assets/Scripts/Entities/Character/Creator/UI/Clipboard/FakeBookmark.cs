using System.Collections;
using UnityEngine;

namespace Character.Creator.UI
{
    /// <summary>
    /// When the user clicks off the page, this will move with the page
    /// </summary>
    public interface IFakeBookmark
    {
        void Setup(GameObject realBookmark);
    }

    public class FakeBookmark : MonoBehaviour, IFakeBookmark
    {
        [SerializeField] RectTransform _animMotionRoot;
        private Animation _animation;
        private IBookmarkImageControl _imageControl;
        private IClipboardOrdering _clipboardOrdering;
        private IClipboardElementSelection _elementSelection;
        private Coroutine _freeFallCoroutine;
        private IPage _page;

        void Awake()
        {
            _animation = this.GetComponent<Animation>();
            _imageControl = this.GetComponent<IBookmarkImageControl>();
            _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
            _elementSelection = this.GetComponent<IClipboardElementSelection>();
        }

        void Start()
        {
            _elementSelection.Selected.OnChanged += Selected_OnChanged;
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _elementSelection.Selected.OnChanged -= Selected_OnChanged;
        }

        public void Setup(GameObject realBookmark)
        {
            _imageControl.CopyValuesFrom(realBookmark.GetComponent<IBookmarkImageControl>());
            this.transform.position = realBookmark.transform.position;

            var selectiontype = realBookmark.GetComponent<IClipboardElementSelection>().Type;
            _elementSelection.Type = selectiontype;

            _page = this.GetComponentInParent<IClipboardSelection>().GetPageWithType(selectiontype);

            _clipboardOrdering.SendToLayer(this.transform, ClipboardLayer.Back);
        }

        private void Selected_OnChanged(bool wasSelected, bool isSelected)
        {
            if (!wasSelected) return;

            _animation.Stop();
            _clipboardOrdering.SendToLayer(this.transform, ClipboardLayer.Freefall);
            this.gameObject.SetActive(true);
            this.StopAndStartCoroutine(ref _freeFallCoroutine, FreeFall());
        }

        IEnumerator FreeFall()
        {
            _animMotionRoot.localPosition = Vector3.zero;
            _animMotionRoot.localRotation = Quaternion.identity;
            _page?.SetParent(_animMotionRoot);
            _animation.Play();
            yield return new WaitForSeconds(_animation.clip.length);
            _animation.Stop();


            // Still our parent? Disable this
            _page?.DisableIfStillParented(_animMotionRoot);
        }
    }
}