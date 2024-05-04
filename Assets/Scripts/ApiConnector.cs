using UnityEngine;
using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class APIConnector : MonoBehaviour
{
    private const string BASE_URL = "http://localhost:3000/api/";

    public enum HttpMethod
    {
        GET,
        POST,
        PATCH,
        DELETE
    }

    // Singleton instance
    public static APIConnector Instance { get; private set; }

    void Awake()
    {
        // Ensure only one instance of APIConnector exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async Task SendRequestAsync(string endpoint, HttpMethod method, Action<string> onSuccess, Action<string> onFailure, string requestBody = null)
    {
        string url = BASE_URL + endpoint;

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = null;

            try
            {
                switch (method)
                {
                    case HttpMethod.GET:
                        response = await client.GetAsync(url);
                        break;
                    case HttpMethod.POST:
                        if (requestBody != null)
                        {
                            HttpContent postContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(url, postContent);
                        }
                        else
                        {
                            onFailure?.Invoke("POST request body is empty");
                            return;
                        }
                        break;
                    case HttpMethod.DELETE:
                        response = await client.DeleteAsync(url);
                        break;
                    case HttpMethod.PATCH:
                        //response = await client.PatchAsync(url,'nik rouhek lina');
                        break;
                    default:
                        onFailure?.Invoke("Unsupported HTTP method");
                        return;
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    onSuccess?.Invoke(responseBody);
                }
                else
                {
                    onFailure?.Invoke(response != null ? $"Failed with status code: {response.StatusCode}" : "Failed to connect to the server");
                }
            }
            catch (Exception e)
            {
                onFailure?.Invoke("Error: " + e.Message);
            }
        }
    }
}
