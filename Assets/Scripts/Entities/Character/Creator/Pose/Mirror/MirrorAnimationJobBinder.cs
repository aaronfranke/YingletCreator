using Reactivity;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

public class MirrorAnimationJobBinder : ReactiveBehaviour
{
    private IPoseYingDataRepository _dataRepo;
    private PlayableGraph _graph;
    private MirrorAnimationJob _job;
    private AnimationScriptPlayable _playable;

    void Start()
    {
        _dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();

        var animator = GetComponent<Animator>();
        _graph = PlayableGraph.Create("MirrorAnimationGraph");
        var output = AnimationPlayableOutput.Create(_graph, "Animation", animator);

        _job = new MirrorAnimationJob(animator);
        _playable = AnimationScriptPlayable.Create(_graph, _job);
        output.SetSourcePlayable(_playable);
        output.SetAnimationStreamSource(AnimationStreamSource.PreviousInputs);

        AddReflector(Reflect);
    }

    void Reflect()
    {
        bool mirror = _dataRepo.YingPoseData.Mirror;
        if (mirror)
        {
            _graph.Play();
        }
        else
        {
            _graph.Stop();
        }
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        _job.Dispose();
        _graph.Destroy();
    }
}
