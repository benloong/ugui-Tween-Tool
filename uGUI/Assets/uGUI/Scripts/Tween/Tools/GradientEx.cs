using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[AddComponentMenu("UI/Plus/GradientEx")]
public class GradientEx : BaseMeshEffect
{
    public bool horizontal = false;
    public Color colorFrom = Color.white;
    public Color colorTo = Color.black;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        // 先计算出最小坐标和最大坐标
        float minDis = float.MaxValue;
        float maxDis = float.MinValue;
        UIVertex v = UIVertex.simpleVert;
        for (int i = 0, len = vh.currentVertCount; i < len; i++)
        {
            vh.PopulateUIVertex(ref v, i);
            float dis = horizontal ? -v.position.x : v.position.y;
            if (minDis > dis)
                minDis = dis;
            if (maxDis < dis)
                maxDis = dis;
        }

        // 计算渐变
        float size = maxDis - minDis;
        for (int i = 0, len = vh.currentVertCount; i < len; i++)
        {
            vh.PopulateUIVertex(ref v, i);
            //v.color *= Color.Lerp(colorTo, colorFrom, ((horizontal ? -v.position.x : v.position.y) - minDis) / size);
            v.color *= Color.Lerp(colorTo, colorFrom, curve.Evaluate(((horizontal ? -v.position.x : v.position.y) - minDis) / size));
            vh.SetUIVertex(v, i);
        }
    }
}