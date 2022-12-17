using System.Collections.Generic;

namespace KioskLife.Interfaces
{
    public interface IErrorsCheck<D, P>
    {
        public List<P> DeviceErrors { get; set; }
        public List<string> Errors { get; set; }
        public void CheckForErrors(D device);
        public void ClearErrors()
        {
            DeviceErrors.Clear();
            Errors.Clear();
        }
    }
}
