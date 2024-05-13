using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace Udp
{
    public class UdpHostMulti : MonoBehaviour
    {
        public static event Action<int, string> OnReceiveMsg;
        public static event Action<string> OnClientError;

        [SerializeField, Tooltip("Set to true if you want the debug logs to show in the console")]
        protected bool _consoleLogsEnabled = false;
        [Header("Host settings")]
        [SerializeField] protected Int32 _hostPort = 5013;
        [SerializeField] protected string _hostIp = "127.0.0.1";
        [Header("Client settings")]
        [SerializeField] protected Int32 _MasterClientPort = 5011;
        [SerializeField] protected string _MasterClientIp = "127.0.0.1";
        [SerializeField] protected Int32 _SecondaryClientPort = 5011;
        [SerializeField] protected string _SecondaryClientIp = "127.0.0.1";
        [SerializeField, Tooltip("Set true if host should auto start and connect to the client")]
        protected bool _autoConnect = false;
        [Header("Stream")]
        [SerializeField] protected string _message;
        protected Thread _socketThread = null;
        protected bool _MasterConnected;
        protected bool _SecondaryConnected;
        protected EndPoint _MasterClient;
        protected EndPoint _SecondaryClient;
        protected Socket _socket;

        public virtual void Start()
        {
            if (_autoConnect) Connect();
        }

        /// <summary>
        /// Opens a connection to the set client
        /// </summary>
        public virtual void Connect()
        {
            //Only try to connect if the host and client are valid
            if (IsSecondaryClientValid() && IsMasterClientValid() && IsHostValid())
            {
                _socketThread = new Thread(ExecuteHost);
                _socketThread.IsBackground = true;
                _socketThread.Start();
            }
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public virtual void Close()
        {
            _MasterConnected = false; //Set to false, the thread will do the rest
            _SecondaryConnected = false; //Set to false, the thread will do the rest
        }

        #region Settings
        public virtual void SetClient(string master_clientIp, Int32 master_clientPort, string secondary_clientIp, Int32 secondary_clientPort)
        {
            _MasterClientIp = master_clientIp;
            _MasterClientPort = master_clientPort;
            _SecondaryClientIp = secondary_clientIp;
            _SecondaryClientPort = secondary_clientPort;
        }

        public virtual void SetHost(string hostIp, Int32 hostPort)
        {
            _hostIp = hostIp;
            _hostPort = hostPort;
        }

        #endregion

        #region Comms
        /// <summary>
        /// Send a message to the client
        /// </summary>
        /// <param name="msg"></param>
        public virtual void SendMasterMsg(string msg)
        {
            if (_MasterConnected)
            {
                byte[] data = Encoding.ASCII.GetBytes(msg);
                _socket.SendTo(data, data.Length, SocketFlags.None, _MasterClient);
            }
            else
            {
                Log("Master client not connected, can't send a message");
            }
        }

        public virtual void SendSecondaryMsg(string msg)
        {
            if (_SecondaryConnected)
            {
                byte[] data = Encoding.ASCII.GetBytes(msg);
                _socket.SendTo(data, data.Length, SocketFlags.None, _SecondaryClient);
            }
            else
            {
                Log("Secondary client not connected, can't send a message");
            }
        }

        /// <summary>
        /// Called when a message is received
        /// </summary>
        /// <param name="message">string msg</param>
        public virtual void MessageReceived(int clientId, string message)
        {
            _message = message;

            OnReceiveMsg?.Invoke(clientId, message);
        }

        /// <summary>
        /// Starts the host and runs a loop waiting for messages.
        /// </summary>
        protected virtual void ExecuteHost()
        {
            try
            {
                byte[] data = new byte[1024];
                IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Parse(_hostIp), _hostPort);

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socket.Bind(hostEndPoint);
                Log("Host is up and waiting for clients...");

                // Create endpoints for each client
                IPEndPoint masterEndPoint = new IPEndPoint(IPAddress.Parse(_MasterClientIp), _MasterClientPort);
                _MasterClient = (EndPoint)masterEndPoint;
                IPEndPoint secondaryEndPoint = new IPEndPoint(IPAddress.Parse(_SecondaryClientIp), _SecondaryClientPort);
                _SecondaryClient = (EndPoint)secondaryEndPoint;

                _MasterConnected = true;
                _SecondaryConnected = true;

                // Send initial message to both clients
                byte[] welcomeData = Encoding.ASCII.GetBytes("0");
                _socket.SendTo(welcomeData, welcomeData.Length, SocketFlags.None, _MasterClient);
                _socket.SendTo(welcomeData, welcomeData.Length, SocketFlags.None, _SecondaryClient);

                while (_MasterConnected || _SecondaryConnected)
                {
                    data = new byte[1024];
                    EndPoint sender = new IPEndPoint(IPAddress.Any, 0);

                    int recv = _socket.ReceiveFrom(data, ref sender);
                    string message = Encoding.ASCII.GetString(data, 0, recv);

                    // Determine the sender client
                    if (sender.Equals(_MasterClient))
                    {
                        MessageReceived(1, message);
                    }
                    else if (sender.Equals(_SecondaryClient))
                    {
                        MessageReceived(0, message);
                    }
                }
            }
            catch (SocketException e)
            {
                HandleSocketException(e);
            }
            finally
            {
                // Cleanup
                CloseSocket();
            }
        }

        private void HandleSocketException(SocketException e)
        {
            // Handle specific socket exceptions based on ErrorCode
            Log($"Socket Exception: {e.Message} (Code: {e.ErrorCode})");
            switch (e.ErrorCode)
            {
                case 10051:
                case 10054:
                    OnClientError?.Invoke($"Network error: {e.Message}");
                    break;
                case 10048:
                    Log("Address already in use.");
                    break;
                default:
                    OnClientError?.Invoke("Unexpected socket error occurred.");
                    break;
            }
        }

        private void CloseSocket()
        {
            Log("Closing socket connection...");
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
                _socket.Close();
            }
        }
        #endregion

        #region Utils
        protected bool IsMasterClientValid()
        {
            if (!IsIpValid(_MasterClientIp))
            {
                Log("Client ip is not a valid ip!");
                return false;
            }
            return true;
        }

        protected bool IsSecondaryClientValid()
        {
            if (!IsIpValid(_SecondaryClientIp))
            {
                Log("Client ip is not a valid ip!");
                return false;
            }
            return true;
        }

        protected bool IsHostValid()
        {
            if (!IsIpValid(_hostIp))
            {
                Log("Host ip is not a valid ip!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Used to verify that the given IP string is valid
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        protected bool IsIpValid(string ip)
        {
            string ipPattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            Regex regex = new Regex(ipPattern);

            return !string.IsNullOrEmpty(ip) && regex.IsMatch(ip, 0);
        }

        /// <summary>
        /// Used for logging messages in the Unity console
        /// </summary>
        /// <param name="msg">the string message</param>
        protected void Log(string msg)
        {
            if (_consoleLogsEnabled)
                Debug.Log($"UDP HOST: {msg}");
        }
        #endregion

        public virtual void OnApplicationQuit()
        {
            Close();
        }


    }
}
