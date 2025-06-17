using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{

    [SerializeField] private PartProperties properties;

    [SerializeField] private MaterialList availableMaterials ;
    [SerializeField] private Cancelator cancelLastAction ;

    public override void InstallBindings()
    {
        Container.Bind<Cancelator>().FromInstance(cancelLastAction);
        Container.Bind<PartProperties>().FromInstance(properties);
        Container.BindInstance(availableMaterials)
                 .AsSingle()
                 .NonLazy();
    }
}