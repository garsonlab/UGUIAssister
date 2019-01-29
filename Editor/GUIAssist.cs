using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Introduction: GUIAssist
/// Author: 	刘家诚
/// Time: 
/// </summary>
public class GUIAssist : EditorWindow
{
    [MenuItem("Tools/UGUI辅助工具")]
    static void OpenAssist()
    {
        GetWindow<GUIAssist>("UGUI辅助").Show();
    }


    string[] tools = new string[] {"Raycast","Transform", "Rename", "Outline"};
    int selected = 0;

    private Transform selectTransform;
    private Vector2 scrollPos;
    private Graphic[] graphics;
    private Transform[] transforms;
    private Outline[] outlines;
    private string prefix = "";

    void OnGUI()
    {
        selected = GUILayout.Toolbar(selected, tools);

        #region Raycast
        if (selected == 0)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("修改UI射线检测：");
            GUILayout.BeginHorizontal();
            GUILayout.Label("父节点：");
            Transform newTrans = (Transform) EditorGUILayout.ObjectField(selectTransform, typeof (Transform), true);

            if (newTrans != selectTransform)
            {
                selectTransform = newTrans;
                if (selectTransform == null)
                    graphics = new Graphic[0];
                else
                    graphics = selectTransform.GetComponentsInChildren<Graphic>();
                scrollPos = Vector2.zero;
            }

            if (selectTransform != null)
            {
                if (GUILayout.Button("刷新"))
                {
                    graphics = selectTransform.GetComponentsInChildren<Graphic>();
                    scrollPos = Vector2.zero;
                }
            }

            if (graphics != null && graphics.Length > 0)
            {
               if (GUILayout.Button("全部关闭"))
               {
                    for (int i = 0; i < graphics.Length; i++)
                    {
                        if (graphics[i])
                        {
                            graphics[i].raycastTarget = false;
                        }
                    }
                }
                if (GUILayout.Button("全部打开"))
                {
                    for (int i = 0; i < graphics.Length; i++)
                    {
                        if (graphics[i])
                        {
                            graphics[i].raycastTarget = true;
                        }
                    }
                }
                
                GUILayout.EndHorizontal();


                if (graphics != null && graphics.Length > 0)
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    int len = graphics.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (graphics[i])
                        {
                            GUILayout.BeginHorizontal();
                            graphics[i].raycastTarget = GUILayout.Toggle(graphics[i].raycastTarget, "");
                            graphics[i] = (Graphic) EditorGUILayout.ObjectField(graphics[i], typeof (Graphic), false);
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndScrollView();

                }

                GUILayout.EndVertical();

            }
        }
        #endregion

        #region Transform
        if (selected == 1)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("对该节点的位置、缩放、旋转重置");
            GUILayout.BeginHorizontal();
            GUILayout.Label("父节点：");
            Transform newTrans = (Transform) EditorGUILayout.ObjectField(selectTransform, typeof (Transform), true);

            if (newTrans != selectTransform)
            {
                selectTransform = newTrans;
                if (selectTransform == null)
                    transforms = new Transform[0];
                else
                    transforms = selectTransform.GetComponentsInChildren<Transform>();
                scrollPos = Vector2.zero;
            }

            if (selectTransform != null)
            {
                if (GUILayout.Button("刷新"))
                {
                    transforms = selectTransform.GetComponentsInChildren<Transform>();
                    scrollPos = Vector2.zero;
                }
            }

            if (transforms != null && transforms.Length > 0)
            {
                if (GUILayout.Button("A"))
                {
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        if (transforms[i])
                        {
                            transforms[i].localPosition = Vector3.zero;
                            transforms[i].localScale = Vector3.one;
                            transforms[i].localRotation = Quaternion.Euler(Vector3.zero);
                        }
                    }
                }
                if (GUILayout.Button("P"))
                {
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        if (transforms[i])
                        {
                            transforms[i].localPosition = Vector3.zero;
                        }
                    }
                }
                if (GUILayout.Button("S"))
                {
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        if (transforms[i])
                        {
                            transforms[i].localScale = Vector3.one;
                        }
                    }
                }
                if (GUILayout.Button("R"))
                {
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        if (transforms[i])
                        {
                            transforms[i].localRotation = Quaternion.Euler(Vector3.zero);
                        }
                    }
                }
                GUILayout.EndHorizontal();


                if (transforms != null && transforms.Length > 0)
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    int len = transforms.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (transforms[i])
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("A"))
                            {
                                transforms[i].localPosition = Vector3.zero;
                                transforms[i].localScale = Vector3.one;
                                transforms[i].localRotation = Quaternion.Euler(Vector3.zero);
                            }
                            if (GUILayout.Button("P"))
                            {
                                transforms[i].localPosition = Vector3.zero;
                            }
                            if (GUILayout.Button("S"))
                            {
                                transforms[i].localScale = Vector3.one;
                            }
                            if (GUILayout.Button("R"))
                            {
                                transforms[i].localRotation = Quaternion.Euler(Vector3.zero);
                            }

                            transforms[i] = (Transform)EditorGUILayout.ObjectField(transforms[i], typeof(Transform), false);
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndScrollView();

                }

                GUILayout.EndVertical();
            }
        }
        #endregion

        #region Rename

        if (selected == 2)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("对该节点下的一级子节点按规则命名：");
            GUILayout.BeginHorizontal();
            GUILayout.Label("父节点：");
            selectTransform = (Transform) EditorGUILayout.ObjectField(selectTransform, typeof (Transform), true);
            if (selectTransform != null)
            {
                prefix = GUILayout.TextField(prefix, GUILayout.Width(100));
                if (GUILayout.Button("重命名"))
                {
                    for (int i = 0; i < selectTransform.childCount; i++)
                    {
                        Transform t = selectTransform.GetChild(i);
                        t.gameObject.name = prefix + (i + 1);
                    }
                }
            }
            GUILayout.EndHorizontal();
            if (selectTransform != null && selectTransform.childCount > 0)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                int length = selectTransform.childCount;
                for (int i = 0; i < length; i++)
                {
                    Transform t = selectTransform.GetChild(i);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(t.gameObject.name);
                    t = (Transform)EditorGUILayout.ObjectField(t, typeof(Transform), false);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
      
            GUILayout.EndVertical();
        }
        #endregion

        #region Outline

        if (selected == 3)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("修改Outline效果：");
            GUILayout.BeginHorizontal();
            GUILayout.Label("父节点：");
            Transform newTrans = (Transform)EditorGUILayout.ObjectField(selectTransform, typeof(Transform), true);
            if (newTrans != selectTransform)
            {
                selectTransform = newTrans;
                if (selectTransform == null)
                    outlines = new Outline[0];
                else
                    outlines = selectTransform.GetComponentsInChildren<Outline>();
                scrollPos = Vector2.zero;
            }

            if (selectTransform != null)
            {
                if (GUILayout.Button("刷新"))
                {
                    outlines = selectTransform.GetComponentsInChildren<Outline>();
                    scrollPos = Vector2.zero;
                }
                if (GUILayout.Button("重置所有"))
                {
                    int len = outlines.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (outlines[i])
                        {
                            outlines[i].effectDistance = new Vector2(1, -1);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            if (outlines != null && outlines.Length > 0)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                int len = outlines.Length;
                for (int i = 0; i < len; i++)
                {
                    if (outlines[i])
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(" R "))
                        {
                            outlines[i].effectDistance = new Vector2(1,-1);
                        }
                        outlines[i].effectDistance = EditorGUILayout.Vector2Field("", outlines[i].effectDistance);
                        outlines[i] = (Outline)EditorGUILayout.ObjectField(outlines[i], typeof(Outline), false);
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }
        #endregion

    }


    [MenuItem("Tools/WearPos/Arm")]
    static void ChangeArmWearPos()
    {
        var gameObject = Selection.activeGameObject;
        if (gameObject)
        {
            int[] pos = {14, 15, 2, 1, 5, 8, 4, 3, 6, 7, 12, 13, 10, 16};
            int[] img = {14, 15, 2, 1, 5, 8, 4, 3, 6, 7, 11, 11, 9, 9};

            var trans = gameObject.transform;
            var index = 0;
            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                if(!child.gameObject.activeSelf)
                    continue;

                if (index >= pos.Length)
                    return;

                var pimg = child.Find("pos").GetComponent<Image>();
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Atlas/ArmOperate/WearGrids/" + img[index]+".png");
                pimg.sprite = sprite;
                child.gameObject.name = pos[index].ToString();
                index++;
            }
        }
    }

    [MenuItem("Tools/WearPos/Dress")]
    static void ChangeDressWearPos()
    {
        var gameObject = Selection.activeGameObject;
        if (gameObject)
        {
            int[] pos = { 28, 29, 22, 21, 26, 25, 24, 27, 23 };
            int[] img = { 14, 15, 2, 1, 26, 25, 24, 27, 23 };

            var trans = gameObject.transform;
            var index = 0;
            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                if (!child.gameObject.activeSelf)
                    continue;
                 
                if(index >= pos.Length)
                    return;

                var pimg = child.Find("pos").GetComponent<Image>();
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Atlas/ArmOperate/WearGrids/" + img[index] + ".png");
                pimg.sprite = sprite;
                child.gameObject.name = pos[index].ToString();
                index++;
            }
        }
    }

}
