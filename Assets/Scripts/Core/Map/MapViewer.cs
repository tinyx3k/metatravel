using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static Core.Map.MercatorProjection;

namespace Core.Map
{
    public class MapViewer : MonoBehaviour
    {
        [SerializeField] private string apiKey;
        [SerializeField] private double lat, lng;
        [SerializeField] private RenderTexture mapTexture;

        private void Start()
        {
            var staticMapUrl = $"https://maps.googleapis.com/maps/api/staticmap?key={apiKey}&zoom={Zoom}&scale=2&format=png&maptype=satellite&size={TileSize}x{TileSize}";
            StartCoroutine(LoadMapImage($"{staticMapUrl}&center={lat}%2c{lng}"));
        }
        IEnumerator LoadMapImage(string mediaUrl)
        {   
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError) 
                Debug.Log(request.error);
            else
            {
                var texture2D = ((DownloadHandlerTexture) request.downloadHandler).texture;
                RenderTexture.active = mapTexture;
                Graphics.Blit(texture2D, mapTexture);
            }
            Debug.Log($"Downloaded {mediaUrl}");
        } 
    }
}
