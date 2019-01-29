#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UGUIAssister
{
    internal class ContextMenus
    {
        public static void AddCreateMenus(GenericMenu menu, List<RectTransform> targets)
        {
            menu.AddItem(new GUIContent("UI/Empty"), false, AddEmpty, targets);
            menu.AddItem(new GUIContent("UI/Text"), false, CreateUI, "Text");
            menu.AddItem(new GUIContent("UI/Image"), false, CreateUI, "Image");
            menu.AddItem(new GUIContent("UI/Raw Image"), false, CreateUI, "RawImage");
            menu.AddItem(new GUIContent("UI/Button"), false, CreateUI, "Button");
            menu.AddItem(new GUIContent("UI/Toggle"), false, CreateUI, "Toggle");
            menu.AddItem(new GUIContent("UI/Slider"), false, CreateUI, "Slider");
            menu.AddItem(new GUIContent("UI/Scrollbar"), false, CreateUI, "Scrollbar");
            menu.AddItem(new GUIContent("UI/Dropdown"), false, CreateUI, "Dropdown");
            menu.AddItem(new GUIContent("UI/Input Field"), false, CreateUI, "InputField");
            menu.AddItem(new GUIContent("UI/Canvas"), false, CreateUI, "Canvas");
            menu.AddItem(new GUIContent("UI/Panel"), false, CreateUI, "Panel");
            menu.AddItem(new GUIContent("UI/Scroll View"), false, CreateUI, "ScrollView");
        }

        public static void AddAlignMenus(GenericMenu menu, List<RectTransform> targets)
        {
            if (targets.Count <= 1)
            {
                menu.AddDisabledItem(new GUIContent("Align(Operate Multi)"));
                return;
            }

            //if (targets.Count > 1)
            //{
                menu.AddItem(new GUIContent("Align/Left\t╞"), false, Alignment.AlignLeft, targets);
                menu.AddItem(new GUIContent("Align/Vertical\t‖"), false, Alignment.AlignVertical, targets);
                menu.AddItem(new GUIContent("Align/Right\t╡"), false, Alignment.AlignRight, targets);
                menu.AddItem(new GUIContent("Align/Top\tㅠ"), false, Alignment.AlignTop, targets);
                menu.AddItem(new GUIContent("Align/Horizontal\t="), false, Alignment.AlignHorizontal, targets);
                menu.AddItem(new GUIContent("Align/Bottom\tㅛ"), false, Alignment.AlignBottom, targets);
            //}
            //else
            //{
            //    menu.AddDisabledItem(new GUIContent("Align/Left\t╞"));
            //    menu.AddDisabledItem(new GUIContent("Align/Vertical\t‖"));
            //    menu.AddDisabledItem(new GUIContent("Align/Right\t╡"));
            //    menu.AddDisabledItem(new GUIContent("Align/Top\tㅠ"));
            //    menu.AddDisabledItem(new GUIContent("Align/Horizontal\t="));
            //    menu.AddDisabledItem(new GUIContent("Align/Bottom\tㅛ"));
            //}
        }

        public static void AddRelativeMenus(GenericMenu menu, List<RectTransform> targets)
        {
            if (targets.Count > 1)
            {
                menu.AddDisabledItem(new GUIContent("Relative(Operate Single)"));
                return;
            }

            menu.AddItem(new GUIContent("Relative/TopLeft\t┏"), false, RelativeParent, 1);
            menu.AddItem(new GUIContent("Relative/TopMiddle\t┳"), false, RelativeParent, 2);
            menu.AddItem(new GUIContent("Relative/TopRight\t┓"), false, RelativeParent, 3);
            menu.AddItem(new GUIContent("Relative/MiddleLeft\t┣"), false, RelativeParent, 4);
            menu.AddItem(new GUIContent("Relative/Middle\t╋"), false, RelativeParent, 5);
            menu.AddItem(new GUIContent("Relative/MiddleRight\t┫"), false, RelativeParent, 6);
            menu.AddItem(new GUIContent("Relative/BottomLeft\t┗"), false, RelativeParent, 7);
            menu.AddItem(new GUIContent("Relative/BottomMiddle\t┻"), false, RelativeParent, 8);
            menu.AddItem(new GUIContent("Relative/BottomRight\t┛"), false, RelativeParent, 9);
        }

        public static void AddSameMenus(GenericMenu menu, List<RectTransform> targets)
        {
            if (targets.Count <= 1)
            {
                menu.AddDisabledItem(new GUIContent("SameTo(Operate Multi)"));
                menu.AddItem(new GUIContent("Same Size With Parent"), false, () =>
                {
                    var target = targets[0];
                    RectTransform parent = target.parent as RectTransform;
                    if (parent)
                    {
                        target.sizeDelta = parent.sizeDelta;
                    }
                });
                return;
            }
            //if (targets.Count > 1)
            //{
                menu.AddItem(new GUIContent("SameTo/Max Size\t□"), false, Alignment.SameSizeMax, targets);
                menu.AddItem(new GUIContent("SameTo/Min Size\t○"), false, Alignment.SameSizeMin, targets);
                menu.AddItem(new GUIContent("SameTo/Pos X\tX"), false, Alignment.SamePosX, targets);
                menu.AddItem(new GUIContent("SameTo/Pos Y\tY"), false, Alignment.SamePosY, targets);
            //}
            //else
            //{
            //    menu.AddDisabledItem(new GUIContent("SameTo/Max Size\t□"));
            //    menu.AddDisabledItem(new GUIContent("SameTo/Min Size\t○"));
            //    menu.AddDisabledItem(new GUIContent("SameTo/Pos X\tX"));
            //    menu.AddDisabledItem(new GUIContent("SameTo/Pos Y\tY"));
            //}
        }

        public static void AddLayoutMenus(GenericMenu menu, List<RectTransform> targets)
        {
            if (targets.Count == 1)
            {
                menu.AddDisabledItem(new GUIContent("Layout/Group(Operate Multi)"));
                var trans = targets[0];
                if (trans.childCount > 0)
                {
                    menu.AddItem(new GUIContent("Layout/Split"), false, SplitChildren, targets);
                    menu.AddItem(new GUIContent("Layout/Rename Children 1,2...N"), false, RenameNum, targets);
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Layout/Split(No Children)"));
                    menu.AddDisabledItem(new GUIContent("Layout/Rename Children 1,2...N"));
                }
            }
            else
            {
                if (SceneAssister.m_InSameLayer)
                {
                    menu.AddItem(new GUIContent("Layout/Group"), false, MakeGroup, targets);
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Layout/Group(Need Same Layer)"));
                }
                menu.AddDisabledItem(new GUIContent("Layout/Split(Operate Single)"));

                menu.AddDisabledItem(new GUIContent("Layout/Rename Children 1,2...N"));
            }
            
        }

        public static void AddComponentMenus(GenericMenu menu, List<RectTransform> mTargets)
        {
            menu.AddItem(new GUIContent("Component/Horizontal Layout Group"), false, AddComponent, typeof(HorizontalLayoutGroup));
            menu.AddItem(new GUIContent("Component/Vertical Layout Group"), false, AddComponent, typeof(VerticalLayoutGroup));
            menu.AddItem(new GUIContent("Component/Grid Layout Group"), false, AddComponent, typeof(GridLayoutGroup));
            menu.AddItem(new GUIContent("Component/Content Size Filter"), false, AddComponent, typeof(ContentSizeFitter));
            menu.AddItem(new GUIContent("Component/Image"), false, AddComponent, typeof(Image));
            menu.AddItem(new GUIContent("Component/Text"), false, AddComponent, typeof(Text));
            menu.AddItem(new GUIContent("Component/Button"), false, AddComponent, typeof(Button));
        }

        public static void AddActiveMenus(GenericMenu menu, List<RectTransform> mTargets)
        {
            menu.AddItem(new GUIContent("Active"), !SceneAssister.m_HasUnactive, () =>
            {
                foreach (var target in mTargets)
                {
                    target.gameObject.SetActive(SceneAssister.m_HasUnactive);
                }
            });
        }

        public static void AddPRSMenus(GenericMenu menu, List<RectTransform> mTargets)
        {
            menu.AddItem(new GUIContent("Transform/Reset Position"), false, Alignment.ResetPosition, mTargets);
            menu.AddItem(new GUIContent("Transform/Reset Rotation"), false, Alignment.ResetRotation, mTargets);
            menu.AddItem(new GUIContent("Transform/Reset Scale"), false, Alignment.ResetScale, mTargets);
            menu.AddItem(new GUIContent("Transform/Reset Povit"), false, Alignment.ResetPovit, mTargets);
            menu.AddItem(new GUIContent("Transform/Reset Anchor"), false, Alignment.ResetAnchor, mTargets);
        }

        private static void CreateUI(object userdata)
        {
            string com = userdata.ToString();
            var list = SceneAssister.m_Targets;
            foreach (var transform in list)
            {
                if (com.Equals("Text"))
                    UICreater.AddText(new MenuCommand(transform));
                else if (com.Equals("Image"))
                    UICreater.AddImage(new MenuCommand(transform));
                else if (com.Equals("RawImage"))
                    UICreater.AddRawImage(new MenuCommand(transform));
                else if (com.Equals("Button"))
                    UICreater.AddButton(new MenuCommand(transform));
                else if (com.Equals("Toggle"))
                    UICreater.AddToggle(new MenuCommand(transform));
                else if (com.Equals("Slider"))
                    UICreater.AddSlider(new MenuCommand(transform));
                else if (com.Equals("Scrollbar"))
                    UICreater.AddScrollbar(new MenuCommand(transform));
                else if (com.Equals("Dropdown"))
                    UICreater.AddDropdown(new MenuCommand(transform));
                else if (com.Equals("InputField"))
                    UICreater.AddInputField(new MenuCommand(transform));
                else if (com.Equals("Canvas"))
                    UICreater.AddCanvas(new MenuCommand(transform));
                else if (com.Equals("Panel"))
                    UICreater.AddPanel(new MenuCommand(transform));
                else if (com.Equals("ScrollView"))
                    UICreater.AddScrollView(new MenuCommand(transform));
            }

        }

        private static void RelativeParent(object userdata)
        {
            int type = (int) userdata;
            var targets = SceneAssister.m_Targets;
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                var self = targets[i];
                var parent = self.parent as RectTransform;
                if(parent == null)
                    continue;

                Vector2 pos = self.anchoredPosition;

                Vector2 p_middle = parent.sizeDelta * 0.5f;
                p_middle.x = p_middle.x * parent.localScale.x; p_middle.y = p_middle.y * parent.localScale.y;//Modify Scale
                Vector2 s_middle = self.sizeDelta * 0.5f;
                s_middle.x = s_middle.x * self.localScale.x; s_middle.y = s_middle.y * self.localScale.y;

                self.anchorMax = Vector2.one * 0.5f;//Reset pivot center
                self.anchorMin = Vector2.one * 0.5f;
                self.anchoredPosition = Vector2.zero;//Reset position center

                switch (type)
                {
                    case 1:
                        pos.x = -(p_middle.x - s_middle.x);
                        pos.y = (p_middle.y - s_middle.y);
                        break;
                    case 2:
                        pos.x = 0;
                        pos.y = (p_middle.y - s_middle.y);
                        break;
                    case 3:
                        pos.x = (p_middle.x - s_middle.x);
                        pos.y = (p_middle.y - s_middle.y);
                        break;
                    case 4:
                        pos.x = -(p_middle.x - s_middle.x);
                        pos.y = 0;
                        break;
                    case 5:
                        pos.x = 0;
                        pos.y = 0;
                        break;
                    case 6:
                        pos.x = (p_middle.x - s_middle.x);
                        pos.y = 0;
                        break;
                    case 7:
                        pos.x = -(p_middle.x - s_middle.x);
                        pos.y = -(p_middle.y - s_middle.y);
                        break;
                    case 8:
                        pos.x = 0;
                        pos.y = -(p_middle.y - s_middle.y);
                        break;
                    case 9:
                        pos.x = (p_middle.x - s_middle.x);
                        pos.y = -(p_middle.y - s_middle.y);
                        break;
                }
                self.anchoredPosition = pos;
            }
        }
        
        private static void SplitChildren(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            RectTransform target = targets[0];
            Transform parent = target.parent;

            int count = target.childCount;
            for (int i = count-1; i >= 0; i--)
            {
                var child = target.GetChild(i);
                if(child)
                    child.SetParent(parent);
            }
            Undo.DestroyObjectImmediate(target.gameObject);
        }

        private static void MakeGroup(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>) userdata;
            GameObject group = new GameObject("Container", typeof(RectTransform));

            Undo.IncrementCurrentGroup();
            int group_index = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Make Group");
            Undo.RegisterCreatedObjectUndo(group, "create group object");
            RectTransform rectTrans = group.GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                Transform parent = targets[0].parent;
                Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(parent, targets[0]);
                for (int i = 1; i < targets.Count; i++)
                {
                    Bounds a = RectTransformUtility.CalculateRelativeRectTransformBounds(parent, targets[i]);
                    bounds.Encapsulate(a);
                }

                rectTrans.SetParent(targets[0].parent);
                rectTrans.localScale = Vector3.one;
                rectTrans.sizeDelta = bounds.size;
                rectTrans.localPosition = bounds.center;

                foreach (var target in targets)
                {
                    Undo.SetTransformParent(target, rectTrans, "move item to group");
                }
            }
            Selection.activeGameObject = group;
            Undo.CollapseUndoOperations(group_index);
        }
        
        private static void AddEmpty(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            foreach (var target in targets)
            {
                GameObject obj = new GameObject("GameObject", typeof(RectTransform));
                obj.transform.SetParent(target);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                Selection.activeObject = obj;
            }
        }

        private static void AddComponent(object userdata)
        {
            Type type = (Type)userdata;
            var targets = SceneAssister.m_Targets;
            foreach (var target in targets)
            {
                var com = target.GetComponent(type);
                if (com == null)
                    target.gameObject.AddComponent(type);
                Selection.activeObject = target;
            }
        }

        private static void RenameNum(object userdata)
        {
            List<RectTransform> targets = (List<RectTransform>)userdata;
            var target = targets[0];
            int count = target.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = target.GetChild(i);
                child.gameObject.name = (i + 1).ToString();
            }
        }

    }
}
#endif