using System.Collections;
using UnityEngine;

namespace CameraStreaming
{
    public class FixEyeMask : MonoBehaviour
    {
        // Use this for initialization
        IEnumerator Start()
        {
            Camera camera = GetComponent<Camera>();
            if (camera)
            {
                int leftEyeMask = LayerMask.GetMask("LeftEye");
                int rightEyeMask = LayerMask.GetMask("RightEye");

                if (camera.transform.parent)
                {
                    if (camera.transform.parent.name.Equals("VREye1"))
                    {
                        camera.cullingMask = (camera.cullingMask | rightEyeMask) & ~leftEyeMask;
                    }
                }

                yield return new WaitForSeconds(1);

                StreamVideo[] videos = GameObject.FindObjectsOfType<StreamVideo>();
                foreach (StreamVideo video in videos)
                {
                    if (!video.gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    int mask = 1 << video.gameObject.layer;
                    //Debug.Log("&: "+ (camera.cullingMask & mask) + " Mask: " + mask);
                    if ((camera.cullingMask & mask) == mask)
                    {
                        video.transform.SetParent(camera.transform);
                    }
                }
            }
        }
    }
}
