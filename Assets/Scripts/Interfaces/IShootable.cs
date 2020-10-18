using System;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using Task = System.Threading.Tasks.Task;

namespace Interfaces
{
    public interface IShootable
    {
        Task Reset(object sender);
        Task Shoot(object sender, object shootableArgs); 
        Task Reload(object sender);

        bool isAuto();

        void InjectAnimator(PlayerArmAnimationController armAnimationController);

        WeaponStats getStats();

    }
}