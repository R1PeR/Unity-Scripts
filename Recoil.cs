using System;
using UnityEngine;
namespace Custom
{
    [Serializable]
    public class Recoil
    {
        public float reachingMultiplier = 2f;
        public float comingBackMultiplier = 2f;
        private Vector3 endPos, startPos;
        private bool reachedEnd = false;
        private bool didLoop = false;
        private float time = 0.0f;
        public GameObject gameObj;
        public Recoil(Vector3 start, Vector3 end, GameObject gameObject)
        {
            endPos = end;
            startPos = start;
            gameObj = gameObject;
        }
        public void NewTarget(Vector3 start, Vector3 end) {
            reachedEnd = false;
            didLoop = false;
            endPos = end;
            startPos = start;
        }
        public void RunRoutine()
        {
            if (!didLoop)
            {
                if (!reachedEnd)
                {
                    gameObj.transform.localPosition = Vector3.Lerp(gameObj.transform.localPosition, endPos, reachingMultiplier * Time.deltaTime);
                }
                if (reachedEnd)
                {
                    gameObj.transform.localPosition = Vector3.Lerp(gameObj.transform.localPosition, startPos, comingBackMultiplier * Time.deltaTime);
                }
                if (gameObj.transform.localPosition == endPos)
                {
                    reachedEnd = true;
                }
                if (reachedEnd && gameObj.transform.localPosition == startPos)
                {
                    didLoop = true;
                }
            }
        }
    }
    public class Recoil2 {
        public float forceX;
        public float forceY ;
        private Vector3 endPosCam, startPosCam,endPosPlayer,startPosPlayer;
        private float time;
        private bool reachedEnd = false;
        private bool didLoop = false;
        public GameObject player;
        public Camera cam;
        public Recoil2(Camera _cam, GameObject _player)
        {
            cam = _cam;
            player = _player;
        }
        public void NewTarget(float fX, float fY)
        {
            time = 0;
            reachedEnd = false;
            didLoop = false;
            forceX = fX;
            forceY = fY;

        }
        public void RunRoutine()
        {
            if (!didLoop)
            {
                if (!reachedEnd)
                {
                    cam.transform.Rotate(Vector3.left, Time.deltaTime * forceX * 20);
                    player.transform.Rotate(Vector3.up, Time.deltaTime * forceY * 20);
                }
                else
                {
                    cam.transform.Rotate(Vector3.left, -Time.deltaTime * forceX * 10);
                    player.transform.Rotate(Vector3.up, -Time.deltaTime * forceY * 10);
                }
                if (time >= 0.1f && !reachedEnd)
                {
                    reachedEnd = true;
                    time = 0;
                }
                else if (reachedEnd && (time >= 0.2f))
                {
                    didLoop = true;
                }
                time += Time.deltaTime;
            }
        }
    }

}
