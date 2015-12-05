using System;
using System.Threading.Tasks;

namespace NichelyPrototype
{
    public interface ICameraService
    {
        Task<CameraResult> TakePictureAsync();
    }
}

