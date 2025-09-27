using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;

public struct MirrorAnimationJob : IAnimationJob
{
    private struct BonePair
    {
        public TransformStreamHandle Left;
        public TransformStreamHandle Right;
    }

    private NativeArray<BonePair> _bonePairs;
    private NativeArray<TransformStreamHandle> _centerBones;

    public MirrorAnimationJob(Animator animator)
    {
        // Temporary lists for gathering data
        var bonePairList = new List<BonePair>();
        var centerBoneList = new List<TransformStreamHandle>();

        // Gather all transforms
        foreach (var t in animator.GetComponentsInChildren<Transform>())
        {
            if (t.name.EndsWith("L"))
            {
                string partnerName = t.name.Substring(0, t.name.Length - 1) + "R";
                var partner = FindChild(animator.transform, partnerName);
                if (partner != null)
                {
                    bonePairList.Add(new BonePair
                    {
                        Left = animator.BindStreamTransform(t),
                        Right = animator.BindStreamTransform(partner)
                    });
                }
            }
            else if (t.name.EndsWith("R"))
            {
                // Skip, already handled by the L case
            }
            else
            {
                centerBoneList.Add(animator.BindStreamTransform(t));
            }
        }

        // Convert lists to NativeArrays
        _bonePairs = new NativeArray<BonePair>(bonePairList.Count, Allocator.Persistent);
        for (int i = 0; i < bonePairList.Count; i++)
            _bonePairs[i] = bonePairList[i];

        _centerBones = new NativeArray<TransformStreamHandle>(centerBoneList.Count, Allocator.Persistent);
        for (int i = 0; i < centerBoneList.Count; i++)
            _centerBones[i] = centerBoneList[i];
    }

    public void ProcessAnimation(AnimationStream stream)
    {
        // Swap mirrored bone transforms
        for (int i = 0; i < _bonePairs.Length; i++)
        {
            var leftHandle = _bonePairs[i].Left;
            var rightHandle = _bonePairs[i].Right;

            // Get local position and rotation for both bones
            var leftPos = leftHandle.GetLocalPosition(stream);
            var leftRot = leftHandle.GetLocalRotation(stream);
            var rightPos = rightHandle.GetLocalPosition(stream);
            var rightRot = rightHandle.GetLocalRotation(stream);

            // Mirror positions and rotations over X axis
            leftPos.x = -leftPos.x;
            rightPos.x = -rightPos.x;
            leftRot = MirrorRotation(leftRot);
            rightRot = MirrorRotation(rightRot);

            // Swap the mirrored transforms
            leftHandle.SetLocalPosition(stream, rightPos);
            leftHandle.SetLocalRotation(stream, rightRot);
            rightHandle.SetLocalPosition(stream, leftPos);
            rightHandle.SetLocalRotation(stream, leftRot);
        }

        // Mirror center bones
        for (int i = 0; i < _centerBones.Length; i++)
        {
            var handle = _centerBones[i];
            var pos = handle.GetLocalPosition(stream);
            var rot = handle.GetLocalRotation(stream);

            pos.x = -pos.x;
            rot = MirrorRotation(rot);

            handle.SetLocalPosition(stream, pos);
            handle.SetLocalRotation(stream, rot);
        }
    }

    public void ProcessRootMotion(AnimationStream stream) { }

    public void Dispose()
    {
        if (_bonePairs.IsCreated) _bonePairs.Dispose();
        if (_centerBones.IsCreated) _centerBones.Dispose();
    }

    private static Transform FindChild(Transform root, string name)
    {
        foreach (var t in root.GetComponentsInChildren<Transform>())
            if (t.name == name) return t;
        return null;
    }

    private static Quaternion MirrorRotation(Quaternion q)
    {
        // Negate Y and Z to mirror over X axis
        return new Quaternion(-q.x, q.y, q.z, -q.w);
    }
}
