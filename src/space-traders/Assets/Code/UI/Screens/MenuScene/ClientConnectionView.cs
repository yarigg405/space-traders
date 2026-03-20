using Assets.Code.UI.Infrastructure;
using TMPro;
using UnityEngine;


namespace Assets.Code.UI.Screens.MainMenu
{
    internal sealed class ClientConnectionView: UIScreenView
    {
        [field: SerializeField] public TMP_InputField HostAddressIF { get; private set; }
        [field: SerializeField] public TMP_InputField HostPortIF { get; private set; }
        [field: SerializeField] public TMP_InputField PlayerNameIF { get; private set; }
        [field: SerializeField] public TMP_InputField HostPasswordIF { get; private set; }
    }
}
