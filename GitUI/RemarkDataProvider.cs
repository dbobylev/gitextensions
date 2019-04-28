using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace GitUI
{
    public interface IRemarkDataProvider
    {
        IDictionary<string, string> GetData();
        string GetBranchRemark(string branchName);
        void UpdateBranch(string branchName, string remark);
    }

    [Serializable]
    public class RemarkItem
    {
        [XmlAttribute]
        public string key;
        [XmlAttribute]
        public string value;
    }

    public class RemarkDataProvider : IRemarkDataProvider
    {
        private readonly string _filePath = Path.Combine(Environment.CurrentDirectory, "BranchRemarkData.xml");

        [CanBeNull]
        private Dictionary<string, string> Data { get; set; }

        public RemarkDataProvider()
        {
            Load();
        }

        private void Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(RemarkItem[]));
            if (File.Exists(_filePath))
            {
                using (Stream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Data = ((RemarkItem[])formatter.Deserialize(stream)).ToDictionary(x => x.key, x => x.value);
                }
            }
            else
            {
                Data = new Dictionary<string, string>();
            }
        }

        private void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(RemarkItem[]));
            using (Stream stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, Data.Select(kv => new RemarkItem() { key = kv.Key, value = kv.Value }).ToArray());
            }
        }

        public IDictionary<string, string> GetData()
        {
            return Data;
        }

        public string GetBranchRemark(string branchName)
        {
            if (Data.ContainsKey(branchName))
            {
                return Data[branchName];
            }

            return null;
        }

        public void UpdateBranch(string branchName, string remark)
        {
            if (Data.ContainsKey(branchName))
            {
                if (string.IsNullOrEmpty(remark))
                {
                    Data.Remove(branchName);
                }
                else
                {
                    Data[branchName] = remark;
                }
            }
            else if (!string.IsNullOrEmpty(remark))
            {
                Data.Add(branchName, remark);
            }

            Save();
        }
    }
}