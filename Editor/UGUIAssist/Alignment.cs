#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace UGUIAssister
{
    internal class Alignment
    {
        public static void AlignLeft(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>) userdata;
            if(targets.Count < 1)
                return;

            Vector4 vector = GetModifySize(targets[0]);
            for (int i = 1; i < targets.Count; i++)
            {
                RectTransform trans = targets[i];
                float tw = trans.sizeDelta.x * trans.lossyScale.x;
                Vector3 pos = trans.position;
                pos.x = vector.x + tw * trans.pivot.x;
                trans.position = pos;
            }
        }

        public static void AlignVertical(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector4 vector = GetModifySize(targets[0]);
            for (int i = 1; i < targets.Count; i++)
            {
                RectTransform trans = targets[i];
                float tw = trans.sizeDelta.x * trans.lossyScale.x;
                Vector3 pos = trans.position;
                pos.x = vector.x + 0.5f * vector.z - 0.5f * tw + tw * trans.pivot.x;
                trans.position = pos;
            }
        }

        public static void AlignRight(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector4 vector = GetModifySize(targets[0]);
            for (int i = 1; i < targets.Count; i++)
            {
                RectTransform trans = targets[i];
                float tw = trans.sizeDelta.x * trans.lossyScale.x;
                Vector3 pos = trans.position;
                pos.x = vector.x + vector.z - tw + tw * trans.pivot.x;
                trans.position = pos;
            }
        }

        public static void AlignTop(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector4 vector = GetModifySize(targets[0]);

            for (int i = 1; i < targets.Count; i++)
            {
                RectTransform trans = targets[i];
                float th = trans.sizeDelta.y * trans.localScale.y;
                Vector3 pos = trans.position;
                pos.y = vector.y + vector.w - th + trans.pivot.y * th;
                trans.position = pos;
            }
        }

        public static void AlignHorizontal(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector4 vector = GetModifySize(targets[0]);
            for (int i = 1; i < targets.Count; i++)
            {
                RectTransform trans = targets[i];
                float th = trans.sizeDelta.y * trans.localScale.y;
                Vector3 pos = trans.position;
                pos.y = vector.y + 0.5f * vector.w - 0.5f * th + th * trans.pivot.y;
                trans.position = pos;
            }
        }

        public static void AlignBottom(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector4 vector = GetModifySize(targets[0]);
            for (int i = 1; i < targets.Count; i++)
            {
                RectTransform trans = targets[i];
                float th = trans.sizeDelta.y * trans.localScale.y;
                Vector3 pos = trans.position;
                pos.y = vector.y + th * trans.pivot.y;
                trans.position = pos;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenplate"></param>
        /// <returns>Vector4: x, y , width, height</returns>
        private static Vector4 GetModifySize(RectTransform tenplate)
        {
            Vector4 vec = new Vector4(0, 0, 0, 0);
            vec.z = tenplate.sizeDelta.x * tenplate.lossyScale.x;//Modify Scale effect
            vec.w = tenplate.sizeDelta.y * tenplate.localScale.y;

            vec.x = tenplate.position.x - tenplate.pivot.x * vec.z; //Modify Pivot not (0.5，0.5) effect
            vec.y = tenplate.position.y - tenplate.pivot.y * vec.w;
            
            return vec;
        }

        public static void SameSizeMax(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector2 max = new Vector2(-1, -1);
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                if (targets[i].sizeDelta.x > max.x)
                    max.x = targets[i].sizeDelta.x;
                if (targets[i].sizeDelta.y > max.y)
                    max.y = targets[i].sizeDelta.y;
            }

            for (int i = 0; i < count; i++)
            {
                targets[i].sizeDelta = max;
            }
        }

        public static void SameSizeMin(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                if (targets[i].sizeDelta.x < min.x)
                    min.x = targets[i].sizeDelta.x;
                if (targets[i].sizeDelta.y < min.y)
                    min.y = targets[i].sizeDelta.y;
            }

            for (int i = 0; i < count; i++)
            {
                targets[i].sizeDelta = min;
            }
        }

        public static void SamePosX(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            float x = targets[0].anchoredPosition.x;
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                var pos = targets[i].anchoredPosition;
                pos.x = x;
                targets[i].anchoredPosition = pos;
            }
        }

        public static void SamePosY(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            if (targets.Count < 1)
                return;

            float y = targets[0].anchoredPosition.y;
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                var pos = targets[i].anchoredPosition;
                pos.y = y;
                targets[i].anchoredPosition = pos;
            }
        }

        public static void ResetPosition(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            foreach (var target in targets)
            {
                target.anchoredPosition = Vector3.zero;
            }
        }

        public static void ResetRotation(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            foreach (var target in targets)
            {
                target.localEulerAngles = Vector3.zero;
            }
        }

        public static void ResetScale(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            foreach (var target in targets)
            {
                target.localScale = Vector3.one;
            }
        }

        public static void ResetPovit(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            foreach (var target in targets)
            {
                target.pivot = Vector2.one*0.5f;
            }
        }

        public static void ResetAnchor(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            foreach (var target in targets)
            {
                target.anchorMin = Vector2.one*0.5f;
                target.anchorMax = Vector2.one * 0.5f;
            }
        }
    }
}
#endif