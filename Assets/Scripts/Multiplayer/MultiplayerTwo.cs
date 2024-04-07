using System;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkDebugStart))]
public class MultiplayerTwo : Fusion.Behaviour
{

    public bool EnableHotkeys;
    public GUISkin BaseSkin;

    private NetworkDebugStart _networkDebugStart;
    private string _clientCount;
    private bool _isMultiplePeerMode;

    private Dictionary<NetworkDebugStart.Stage, string> _nicifiedStageNames;
    public Text statusText;

    protected virtual void Reset()
    {
        _networkDebugStart = EnsureNetworkDebugStartExists();
        _clientCount = _networkDebugStart.AutoClients.ToString();
    }

    protected virtual void OnValidate()
    {
        ValidateClientCount();
    }

    protected void ValidateClientCount()
    {
        if (_clientCount == null)
        {
            _clientCount = "1";
        }
        else
        {
            _clientCount = System.Text.RegularExpressions.Regex.Replace(_clientCount, "[^0-9]", "");
        }
    }

    protected int GetClientCount()
    {
        try
        {
            return Convert.ToInt32(_clientCount);
        }
        catch
        {
            return 0;
        }
    }

    protected virtual void Awake()
    {
        _nicifiedStageNames = ConvertEnumToNicifiedNameLookup<NetworkDebugStart.Stage>("Fusion Status: ");
        _networkDebugStart = EnsureNetworkDebugStartExists();
        _clientCount = _networkDebugStart.AutoClients.ToString();
        ValidateClientCount();
    }

    protected virtual void Start()
    {
        _networkDebugStart = FindObjectOfType<NetworkDebugStart>();
        if (_networkDebugStart == null)
        {
            Debug.LogError("NetworkDebugStart component not found in the scene.");
            return;
        }

        // Update the Fusion status text initially
        UpdateFusionStatusText();


        _isMultiplePeerMode = NetworkProjectConfig.Global.PeerMode == NetworkProjectConfig.PeerModes.Multiple;
    }

    protected NetworkDebugStart EnsureNetworkDebugStartExists()
    {
        if (_networkDebugStart)
        {
            if (_networkDebugStart.gameObject == gameObject)
                return _networkDebugStart;
        }

        if (TryGetBehaviour<NetworkDebugStart>(out var found))
        {
            _networkDebugStart = found;
            return found;
        }

        _networkDebugStart = AddBehaviour<NetworkDebugStart>();
        return _networkDebugStart;
    }

    private void Update()
    {
        UpdateFusionStatusText();
        var nds = EnsureNetworkDebugStartExists();
        if (nds.StartMode != NetworkDebugStart.StartModes.UserInterface)
        {
            return;
        }

        var currentstage = nds.CurrentStage;
        if (currentstage != NetworkDebugStart.Stage.Disconnected)
        {
            return;
        }

        if (EnableHotkeys)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _networkDebugStart.StartSinglePlayer();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                if (_isMultiplePeerMode)
                {
                    StartHostWithClients(_networkDebugStart);
                }
                else
                {
                    _networkDebugStart.StartHost();
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (_isMultiplePeerMode)
                {
                    StartServerWithClients(_networkDebugStart);
                }
                else
                {
                    _networkDebugStart.StartServer();
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (_isMultiplePeerMode)
                {
                    StartMultipleClients(nds);
                }
                else
                {
                    nds.StartClient();
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (_isMultiplePeerMode)
                {
                    StartMultipleSharedClients(nds);
                }
                else
                {
                    nds.StartSharedClient();
                }
            }
        }
    }

   /* protected virtual void OnGUI()
    {

        var nds = EnsureNetworkDebugStartExists();
        if (nds.StartMode != NetworkDebugStart.StartModes.UserInterface)
        {
            return;
        }

        var currentstage = nds.CurrentStage;
        if (nds.AutoHideGUI && currentstage == NetworkDebugStart.Stage.AllConnected)
        {
            return;
        }

        GUI.skin = FusionScalableIMGUI.GetScaledSkin(BaseSkin, out var height, out var width, out var padding, out var margin, out var leftBoxMargin);

        GUILayout.BeginArea(new Rect(leftBoxMargin, margin, width, Screen.height));
        {
            GUILayout.BeginVertical(GUI.skin.window);
            {
                GUILayout.BeginHorizontal(GUILayout.Height(height));
                {
                    var stagename = _nicifiedStageNames.TryGetValue(nds.CurrentStage, out var stage) ? stage : "Unrecognized Stage";
                    GUILayout.Label(stagename, new GUIStyle(GUI.skin.label) { fontSize = (int)(GUI.skin.label.fontSize * .8f), alignment = TextAnchor.UpperLeft });

                    // Add button to hide Shutdown option after all connect, which just enables AutoHide - so that interface will reappear after a disconnect.
                    if (nds.AutoHideGUI == false && nds.CurrentStage == NetworkDebugStart.Stage.AllConnected)
                    {
                        if (GUILayout.Button("X", GUILayout.ExpandHeight(true), GUILayout.Width(height)))
                        {
                            nds.AutoHideGUI = true;
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUI.skin.window);
            {

                if (currentstage == NetworkDebugStart.Stage.Disconnected)
                {

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Room:", GUILayout.Height(height), GUILayout.Width(width * .33f));
                        nds.DefaultRoomName = GUILayout.TextField(nds.DefaultRoomName, 25, GUILayout.Height(height));
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button(EnableHotkeys ? "Start Single Player (I)" : "Start Single Player", GUILayout.Height(height)))
                    {
                        nds.StartSinglePlayer();
                    }

                    if (GUILayout.Button(EnableHotkeys ? "Start Shared Client (P)" : "Start Shared Client", GUILayout.Height(height)))
                    {
                        if (_isMultiplePeerMode)
                        {
                            StartMultipleSharedClients(nds);
                        }
                        else
                        {
                            nds.StartSharedClient();
                        }
                    }

                    if (GUILayout.Button(EnableHotkeys ? "Start Server (S)" : "Start Server", GUILayout.Height(height)))
                    {
                        if (_isMultiplePeerMode)
                        {
                            StartServerWithClients(nds);

                        }
                        else
                        {
                            nds.StartServer();
                        }
                    }

                    if (GUILayout.Button(EnableHotkeys ? "Start Host (H)" : "Start Host", GUILayout.Height(height)))
                    {
                        if (_isMultiplePeerMode)
                        {
                            StartHostWithClients(nds);
                        }
                        else
                        {
                            nds.StartHost();
                        }
                    }

                    if (GUILayout.Button(EnableHotkeys ? "Start Client (C)" : "Start Client", GUILayout.Height(height)))
                    {
                        if (_isMultiplePeerMode)
                        {
                            StartMultipleClients(nds);
                        }
                        else
                        {
                            nds.StartClient();
                        }
                    }

                    if (_isMultiplePeerMode)
                    {

                        GUILayout.BeginHorizontal(GUI.skin.button);
                        {
                            GUILayout.Label("Client Count:", GUILayout.Height(height));
                            GUILayout.Label("", GUILayout.Width(4));
                            string newcount = GUILayout.TextField(_clientCount, 10, GUILayout.Width(width * .25f), GUILayout.Height(height));
                            if (_clientCount != newcount)
                            {
                                // Remove everything but numbers from our client count string.
                                _clientCount = newcount;
                                ValidateClientCount();
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                else
                {

                    if (GUILayout.Button("Shutdown", GUILayout.Height(height)))
                    {
                        _networkDebugStart.ShutdownAll();
                    }
                }

                GUILayout.EndVertical();
            }
        }
        GUILayout.EndArea();
    }
    */


    // Public methods to start different types of network sessions

    public void StartSinglePlayer()
    {
        _networkDebugStart.StartSinglePlayer();
    }

    public void StartSharedClient()
    {
        _networkDebugStart.StartSharedClient();
    }

    public void StartHost()
    {
        _networkDebugStart.StartHost();
    }

    public void StartServer()
    {
        _networkDebugStart.StartServer();
    }

    public void StartClient()
    {
        _networkDebugStart.StartClient();
    }

    public void ShutdownAll()
    {
        _networkDebugStart.ShutdownAll();
    }

    // Additional public methods for starting sessions with multiple clients

    public void StartHostWithClients(NetworkDebugStart nds)
    {
        int count;
        try
        {
            count = Convert.ToInt32(_clientCount);
        }
        catch
        {
            count = 0;
        }
        nds.StartHostPlusClients(count);
    }

    public void StartServerWithClients(NetworkDebugStart nds)
    {
        int count;
        try
        {
            count = Convert.ToInt32(_clientCount);
        }
        catch
        {
            count = 0;
        }
        nds.StartServerPlusClients(count);
    }

    public void StartMultipleClients(NetworkDebugStart nds)
    {
        int count;
        try
        {
            count = Convert.ToInt32(_clientCount);
        }
        catch
        {
            count = 0;
        }
        nds.StartMultipleClients(count);
    }

    public void StartMultipleSharedClients(NetworkDebugStart nds)
    {
        int count;
        try
        {
            count = Convert.ToInt32(_clientCount);
        }
        catch
        {
            count = 0;
        }
        nds.StartMultipleSharedClients(count);
    }



    private void UpdateFusionStatusText()
    {
        if (statusText == null || SceneManager.GetActiveScene().name != "MainMenu")
            return;

        // Check if the NetworkDebugStart component is available
        if (_networkDebugStart == null)
            return;

        // Get the current Fusion status stage
        NetworkDebugStart.Stage currentStage = _networkDebugStart.CurrentStage;

        // Update the Text component with Fusion status stage
        statusText.text = "Fusion Status: " + currentStage.ToString();
    }




    // TODO Move to a utility
    public static Dictionary<T, string> ConvertEnumToNicifiedNameLookup<T>(string prefix = null, Dictionary<T, string> nonalloc = null) where T : System.Enum
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (nonalloc == null)
        {
            nonalloc = new Dictionary<T, string>();
        }
        else
        {
            nonalloc.Clear();
        }

        var names = Enum.GetNames(typeof(T));
        var values = Enum.GetValues(typeof(T));
        for (int i = 0, cnt = names.Length; i < cnt; ++i)
        {
            sb.Clear();
            if (prefix != null)
            {
                sb.Append(prefix);
            }
            var name = names[i];
            for (int n = 0; n < name.Length; n++)
            {
                // If this character is a capital and it is not the first character add a space.
                // This is because we don't want a space before the word has even begun.
                if (char.IsUpper(name[n]) == true && n != 0)
                {
                    sb.Append(" ");
                }

                // Add the character to our new string
                sb.Append(name[n]);
            }
            nonalloc.Add((T)values.GetValue(i), sb.ToString());
        }
        return nonalloc;
    }
#if UNITY_EDITOR

    public static T GetAsset<T>(string Guid) where T : UnityEngine.Object
    {
        var path = UnityEditor.AssetDatabase.GUIDToAssetPath(Guid);
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        else
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
#endif
}



// Other utility methods (if any)

