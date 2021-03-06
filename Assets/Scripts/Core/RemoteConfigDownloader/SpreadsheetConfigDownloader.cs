using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizFramework.Extensions;
using UnityEngine.Networking;

namespace QuizFramework.RemoteConfig
{
    public class SpreadsheetConfigDownloader : IRemoteConfigDownloader
    {
        private const string SpreadsheetsPath = "https://docs.google.com/spreadsheets/d/";
        private const string ExportRequestPath = "/export?format=csv&gid=";

        private async Task<List<string>> DownloadConfig(string configPath, string configId)
        {
            var result = new List<string>();

            var response = await SendRequest(configPath, configId);
            if (response.result != UnityWebRequest.Result.Success)
            {
                return result;
            }

            ReadTab(response.downloadHandler.text, result);
            return result;
        }

        private async Task<UnityWebRequest> SendRequest(string sheetId, string tabId)
        {
            var url = $"{SpreadsheetsPath}{sheetId}{ExportRequestPath}{tabId}";
            var request = UnityWebRequest.Get(url);
            await request.SendWebRequest();
            return request;
        }

        private void ReadTab(string tabText, List<string> tabValues)
        {
            var lineSeparator = Environment.NewLine;
            var tabRows = tabText.Split(new[] {lineSeparator}, StringSplitOptions.None);
            tabValues.AddRange(tabRows);
        }

        #region IRemoteConfigDownloader

        async Task<List<string>> IRemoteConfigDownloader.DownloadConfig(string configPath, string configId)
        {
            return await DownloadConfig(configPath, configId);
        }

        #endregion
    }
}