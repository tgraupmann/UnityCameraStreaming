#define USE_TEXTURE

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CameraStreaming
{
    public class StreamVideo : MonoBehaviour
    {
        public Renderer _mRenderer = null;

        public Material _mMaterialSource = null;

        public string _mUrl = string.Empty;

        private Material _mMaterialInstance = null;

        private DateTime _mTimer = DateTime.MinValue;

        private int _mFrameCount = 0;

        private int _mFrameRate = 0;

        public Text _mText = null;

        // Use this for initialization
        IEnumerator Start()
        {
            if (string.IsNullOrEmpty(_mUrl))
            {
                Debug.LogError("URL not set!");
                yield break;
            }

            if (_mMaterialSource)
            {
                _mMaterialInstance = Instantiate<Material>(_mMaterialSource);
            }
            else
            {
                Debug.LogError("Material not set!");
                yield break;
            }

            if (_mRenderer)
            {
                _mRenderer.material = _mMaterialInstance;
            }
            else
            {
                Debug.LogError("Renderer not set!");
                yield break;
            }

#if USE_TEXTURE
            Texture2D oldTexture = null;
#endif
            while (true)
            {
                WWW www = new WWW(_mUrl);
                yield return www;
#if USE_TEXTURE
                Texture2D newTexture = www.texture;
#endif
                bool hasError = !string.IsNullOrEmpty(www.error);
                if (!hasError)
                {
                    if (_mTimer < DateTime.Now)
                    {
                        _mTimer = DateTime.Now + TimeSpan.FromSeconds(1);
                        _mFrameRate = _mFrameCount;
                        _mFrameCount = 0;
                        if (_mText)
                        {
                            _mText.text = _mFrameRate.ToString();
                        }
                    }
                    else
                    {
                        ++_mFrameCount;
                    }
#if USE_TEXTURE
                    _mMaterialInstance.mainTexture = newTexture;
#endif
                }
                www.Dispose();
                if (hasError)
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }
#if USE_TEXTURE
                if (oldTexture)
                {
                    DestroyImmediate(oldTexture);
                }
                oldTexture = newTexture;
#endif
                //yield return new WaitForSeconds(0.1f);
                yield return null;
            }
        }
    }
}
