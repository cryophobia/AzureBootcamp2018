using System;
using System.Threading.Tasks;

namespace Smile.Services {
    public interface IBiometricService {
        string BiometricCheck();
        Task<bool> AuthenticateMe();
    }
}
