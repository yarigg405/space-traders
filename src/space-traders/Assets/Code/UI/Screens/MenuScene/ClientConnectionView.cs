using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.MainMenu
{
    public sealed class ClientConnectionView : UIScreenView
    {
        [field: SerializeField] public TMP_InputField HostAddressIF { get; private set; }
        [field: SerializeField] public TMP_InputField HostPortIF { get; private set; }
        [field: SerializeField] public TMP_InputField PlayerNameIF { get; private set; }
        [field: SerializeField] public TMP_InputField HostPasswordIF { get; private set; }

        [field: SerializeField] public Button ConnectButton { get; private set; }
    }
}
