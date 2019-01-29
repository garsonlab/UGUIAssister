using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorCreator : EditorWindow 
{
    [MenuItem("Assets/Single Animator")]
    static void Create()
    {
        GetWindow<AnimatorCreator>("空白动画创建").Show();
    }

    private Vector2 pos = Vector2.zero;
    private string path = "";
    private string name = "";
    private string[] clips = new[] {"", "", "", "", "", "", "", ""};

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(20);

        if (string.IsNullOrEmpty(path))
        {
            var obj = Selection.activeObject;
            if (obj != null)
                path = AssetDatabase.GetAssetPath(obj);
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("保存路径:", GUILayout.Width(60));
        GUILayout.Label(path);
        if (GUILayout.Button("...", GUILayout.Width(30)))
        {
            path = EditorUtility.SaveFolderPanel("保存目录", "Assets/", "");
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("动画名称:", GUILayout.Width(60));
        name = GUILayout.TextField(name);
        if (GUILayout.Button("创建", GUILayout.Width(80)))
        {
            CreateAnimator();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("模版：", GUILayout.Width(30));
        if (GUILayout.Button("Enter", GUILayout.Width(120)))
        {
            clips = new[] { "Enter", "", "", "", "", "", "", "" };
        }
        if (GUILayout.Button("Normal/Selected", GUILayout.Width(120)))
        {
            clips = new[] { "Normal", "Selected", "", "", "", "", "", "" };
        }
        if (GUILayout.Button("N/H/P/D", GUILayout.Width(80)))
        {
            clips = new[] { "Normal", "Highlight", "Pressed", "Disabled", "", "", "", "" };
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Index");
        GUILayout.Label("Name");
        if (GUILayout.Button("删除所有", GUILayout.Width(50)))
        {
            clips = new[] { "", "", "", "", "", "", "", "" };
        }
        GUILayout.EndHorizontal();

        pos = GUILayout.BeginScrollView(pos);
        for (int i = 0; i < clips.Length; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(i.ToString(), GUILayout.Width(50));

            clips[i] = GUILayout.TextField(clips[i]);
            if (GUILayout.Button("X", GUILayout.Width(50)))
            {
                clips[i] = "";
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUILayout.EndVertical();
    }

    void CreateAnimator()
    {
        if (string.IsNullOrEmpty(path))
        {
            EditorUtility.DisplayDialog("Error", "先选择保存地址", "OK");
            return;
        }

        if (string.IsNullOrEmpty(name))
        {
            EditorUtility.DisplayDialog("Error", "先设置保存名字", "OK");
            return;
        }

        List<string> cps = new List<string>();
        foreach (var clip in clips)
        {
            if (!string.IsNullOrEmpty(clip.Trim()))
            {
                cps.Add(clip.Trim());
            }
        }

        if (cps.Count == 0)
        {
            if (!EditorUtility.DisplayDialog("Attention", "未设置有效的Clip名字，是否继续", "是", "否"))
            {
                return;
            }
        }

        string url = path + "/" + name + ".controller";

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(url);
        AnimatorControllerLayer layer = controller.layers[0];
        AnimatorStateMachine stateMachine = layer.stateMachine;
        stateMachine.anyStatePosition = new Vector3(0, 80, 0);
        stateMachine.entryPosition = Vector3.zero;
        stateMachine.exitPosition = new Vector3(0, -80, 0);

        int index = 0;
        foreach (var cp in cps)
        {
            AnimationClip animationClip = new AnimationClip();
            animationClip.name = cp;
            AssetDatabase.AddObjectToAsset(animationClip, controller);

            controller.AddParameter(cp, AnimatorControllerParameterType.Trigger);
            AnimatorState state = stateMachine.AddState(cp,
                new Vector3(250 + index / 4 * 220, index % 4 * 50));
            AnimatorStateTransition transition = stateMachine.AddAnyStateTransition(state);
            transition.AddCondition(AnimatorConditionMode.If, 0, cp);
            state.motion = animationClip;
            index++;
        }
        AssetDatabase.ImportAsset(url);
        EditorUtility.DisplayDialog("Success", "创建成功", "Ok");
    }
}
