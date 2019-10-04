using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.DTOs
{
    public class NewMessageDTO
    {
        public string Subject { get; set; }
        public bool Replay { get; set; }
        public long Type { get; set; }
        public string Priority { get; set; }
        public long[] Selectedusers { get; set; }
        public string Content { get; set; }
        public SelectedOption SelectedOption { get; set; }
        public AttachmentUpload[] Files { get; set; }
    }
    public enum SelectedOption
    {
        All,
        Email,
        SMS,
        NONE
    }
    public class AttachmentUpload
    {
        public string FileName { get; set; }
        public string FileBase64 { get; set; }
        public string Type { get; set; }
    }
}
