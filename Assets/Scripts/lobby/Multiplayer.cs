using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
//using Utilities;


namespace Kart
{
    /* [System.Serializable]
     public enum EncryptionType
     {
         DTLS, // Datagram Transport Layer Security
         WSS  // Web Socket Secure
     }
     // Note: Also Udp and Ws are possible choices
    */
    public class Multiplayer : MonoBehaviour
    {
        [SerializeField] string lobbyName = "Lobby";
        [SerializeField] int maxPlayers = 4;
        //  [SerializeField] EncryptionType encryption = EncryptionType.DTLS;

        public static Multiplayer Instance { get; private set; }

        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }

        Lobby currentLobby;
        // string connectionType => encryption == EncryptionType.DTLS ? k_dtlsEncryption : k_wssEncryption;

        // const float k_lobbyHeartbeatInterval = 20f;
        // const float k_lobbyPollInterval = 65f;
        // const string k_keyJoinCode = "RelayJoinCode";
        // const string k_dtlsEncryption = "dtls"; // Datagram Transport Layer Security
        // const string k_wssEncryption = "wss"; // Web Socket Secure, use for WebGL builds

        // CountdownTimer heartbeatTimer = new CountdownTimer(k_lobbyHeartbeatInterval);
        // CountdownTimer pollForUpdatesTimer = new CountdownTimer(k_lobbyPollInterval);

        async void Start()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            await Authenticate();

            /*  heartbeatTimer.OnTimerStop += () =>
              {
                  HandleHeartbeatAsync();
                  heartbeatTimer.Start();
              };

              pollForUpdatesTimer.OnTimerStop += () =>
              {
                  HandlePollForUpdatesAsync();
                  pollForUpdatesTimer.Start();
              };*/
         }

            async Task Authenticate()
            {
                await Authenticate("Player" + Random.Range(0, 1000));
            }

            async Task Authenticate(string playerName)
            {
                if (UnityServices.State == ServicesInitializationState.Uninitialized)
                {
                    InitializationOptions options = new InitializationOptions();
                    options.SetProfile(playerName);

                    await UnityServices.InitializeAsync(options);
                }

                AuthenticationService.Instance.SignedIn += () =>
                {
                    Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);
                };

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    PlayerId = AuthenticationService.Instance.PlayerId;
                    PlayerName = playerName;
                }
            }
        }
    }
