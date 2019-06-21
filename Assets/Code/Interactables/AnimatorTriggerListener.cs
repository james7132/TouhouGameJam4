using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[AddComponentMenu("Triggers/Listener/Animator Trigger")]
public class AnimatorTriggerListener : TriggerListener
{
    [SerializeField]
    List<Animator> _animators;
    public IReadOnlyCollection<Animator> Animators => _animators;

    [SerializeField]
    List<string> _animatorTriggerNames;
    List<int> _animatorTriggerIds;
    public IReadOnlyCollection<string> TriggerNames => _animatorTriggerNames;

    void Start() 
    {
        _animators = _animators.Where(anim => anim != null).ToList();
        _animatorTriggerIds = _animatorTriggerNames.Select(name => Animator.StringToHash(name))
                                                   .ToList();
        // Assure all of the animators match up with the triggers
        foreach (var animator in _animators) 
        {
            Assert.IsNotNull(animator);
            var animatorHashes = new HashSet<int>(animator.parameters.Select(param => param.nameHash));
            Assert.IsTrue(_animatorTriggerIds.All(id => animatorHashes.Contains(id)));
        }
    }

    void Reset() {
        _animators = GetComponentsInChildren<Animator>().ToList();
        if (_animators.Count <= 0) {
            _animators.Add(gameObject.AddComponent<Animator>());
        }
    }

    protected override void OnTriggerFired(TriggerBehaviour trigger, Character source) 
    {
        foreach (var animator in _animators) 
        {
            if (animator == null) continue;
            foreach (var id in _animatorTriggerIds) 
            {
                animator.SetTrigger(id);
            }
        }
    }
}