#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UGUIAssister
{
    internal class SceneAssister : Editor
    {
        const int MOUSE_RIGHT_DOWN = 1;
        internal static readonly List<RectTransform> m_Targets = new List<RectTransform>();
        internal static bool m_InSameLayer;
        internal static bool m_HasActive;
        internal static bool m_HasUnactive;

        [InitializeOnLoadMethod]
        static void Init()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }
        
        private static void OnSceneGUI(SceneView sceneview)
        {
            if (Event.current.control && Event.current.type == EventType.MouseDown && Event.current.button == MOUSE_RIGHT_DOWN)//EventType.ContextClick No Dispatch
            {
                GameObject[] objs = Selection.gameObjects;
                m_Targets.Clear();
                m_InSameLayer = true;
                m_HasActive = false;
                m_HasUnactive = false;
                Transform parent = null;
                foreach (var obj in objs)
                {
                    var rect = obj.GetComponent<RectTransform>();
                    if (rect)
                    {
                        m_Targets.Add(rect);

                        if (parent == null)
                            parent = rect.parent;
                        if (parent != rect.parent)
                            m_InSameLayer = false;

                        if (obj.activeSelf)
                            m_HasActive = true;
                        else
                            m_HasUnactive = true;
                    }
                }

                if (objs.Length <= 0)
                    return;

                GenericMenu menu = new GenericMenu();
                ContextMenus.AddActiveMenus(menu, m_Targets);
                ContextMenus.AddCreateMenus(menu, m_Targets);
                ContextMenus.AddPRSMenus(menu, m_Targets);
                ContextMenus.AddAlignMenus(menu, m_Targets);
                ContextMenus.AddRelativeMenus(menu, m_Targets);
                ContextMenus.AddSameMenus(menu, m_Targets);
                ContextMenus.AddLayoutMenus(menu, m_Targets);
                ContextMenus.AddComponentMenus(menu, m_Targets);
                menu.ShowAsContext();
            }

        }
        
    }
}
#endif