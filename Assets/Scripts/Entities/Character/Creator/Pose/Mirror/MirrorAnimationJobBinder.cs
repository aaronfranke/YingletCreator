using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

public class MirrorAnimationJobBinder : MonoBehaviour
{
    private PlayableGraph _graph;
    private MirrorAnimationJob _job;
    private AnimationScriptPlayable _playable;

    void Start()
    {
        var animator = GetComponent<Animator>();
        _graph = PlayableGraph.Create("MirrorAnimationGraph");
        var output = AnimationPlayableOutput.Create(_graph, "Animation", animator);

        _job = new MirrorAnimationJob(animator);
        _playable = AnimationScriptPlayable.Create(_graph, _job);
        output.SetSourcePlayable(_playable);
        output.SetAnimationStreamSource(AnimationStreamSource.PreviousInputs);

        _graph.Play();
    }

    void OnDestroy()
    {
        _job.Dispose();
        _graph.Destroy();
    }
}
