using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public class CloudFile
    {
        public string path { get; set; }
        public string[] tags { get; set; }          // optional
        public string createdDate { get; set; }     // optional
        public int size { get; set; }               // optional
        public string name { get; set; }            // optional
        public string modifiedDate { get; set; }    // optional
        public string id { get; set; }              // optional
        public Boolean directory { get; set; }      // optional


        [Newtonsoft.Json.JsonIgnore]
        public Boolean HasTags { get { return ((tags != null) && (tags.Length > 0)); } }

        [Newtonsoft.Json.JsonIgnore]
        public Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType EntryType
        {
            get
            {
                Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType deType = CloudElementsConnector.DirectoryEntryType.File;
                if (directory) deType = CloudElementsConnector.DirectoryEntryType.Folder;
                return deType;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public Boolean IsCreatedValid { get { checkDates(); return (_CreatedIsValid); } }
        [Newtonsoft.Json.JsonIgnore]
        public Boolean IsModifiedValid { get { checkDates(); return (_ModifiedIsValid); } }

        private Boolean _CreatedIsValid;
        private Boolean _ModifiedIsValid;
        private Boolean _DatesChecked=false;
        private DateTime _WhenCreated;
        private DateTime _WhenModified;

        private void checkDates()
        {
            if (_DatesChecked) return;
            _CreatedIsValid = DateTime.TryParse(this.createdDate, out _WhenCreated);
            _ModifiedIsValid = DateTime.TryParse(this.modifiedDate, out _WhenModified);
            _DatesChecked = true;
        }

        public DateTime WhenCreated() 
        {
            checkDates();
            if (!_CreatedIsValid) throw new ApplicationException("createdDate could not be converted to internal date time - " + this.createdDate);
            return (_WhenCreated);
        }

        public DateTime WhenModified()
        {
            checkDates();
            if (!_ModifiedIsValid) throw new ApplicationException("modifiedDate could not be converted to internal date time - " + this.modifiedDate);
            return (_WhenModified);
        }


        /// <summary>
        /// Adds a tag to the tag collection with support for Key-Value pair tags (name=value)
        /// </summary>
        /// <param name="newTagValue"></param>
        /// <returns>true if the tag was added or updated</returns>
        public bool UpdateTag(string newTagValue)
        {
            // 
            bool isKVP = newTagValue.IndexOf("=") > 0;
            bool stored = false;
            string tagName = newTagValue;
            if (isKVP) tagName = tagName.Substring(0, tagName.IndexOf("=") - 1);
            int tagIdx = FindTag(tagName, isKVP);

            if (tagIdx >= 0)
            {
                if (this.tags[tagIdx] != newTagValue)
                {
                    this.tags[tagIdx] = newTagValue;
                    stored = true;
                }
            }
            else
            {
                List<string> tagList;
                if (HasTags) tagList = this.tags.ToList();
                else tagList = new List<string>();

                tagList.Add(newTagValue);
                this.tags = tagList.ToArray();
                stored = true;
            }

            return stored;
        }


        /// <summary>
        /// Finds the KVP tag with the specified name and returns its value
        /// </summary>
        /// <param name="newTagValue"></param>
        /// <returns>the tag value or nothing if the tag does not exist</returns>
        public string GetTagValue(string tagName  )  
        {
            // 
            bool isKVP = true;
            string stored = null;

            int tagIdx = FindTag(tagName, isKVP);
            if (tagIdx >= 0)
            {
                stored = this.tags[tagIdx];
                stored = stored.Substring(stored.IndexOf("=") + 1);
            }

            return stored;
       
        }

        /// <summary>
        /// Removes a tag from the tag collection; for a KVP tag, specify just name= 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns>true if the tag was removed</returns>
        public bool RemoveTag(string tagName)
        {
            // 
            bool isKVP = tagName.EndsWith("=");
            bool found = false;

            int tagIdx = FindTag(tagName, isKVP);
            if (tagIdx >= 0)
            {
                List<string> tagList = this.tags.ToList();
                tagList.RemoveAt(tagIdx);
                this.tags = tagList.ToArray();
                found = true;
            }

            return found;
        }


        /// <summary>
        /// Determines if tag is KVP and returns true if the tag currently exists
        /// </summary>
        /// <param name="whichTag"></param>
        /// <returns>index or -1</returns>
        public bool TagExists(string whichTag)
        {
            string tagName = whichTag;
            bool isKVP = whichTag.IndexOf("=") > 0;
            if (isKVP) tagName = tagName.Substring(0, tagName.IndexOf("=") - 1);
            return FindTag(tagName, isKVP) >= 0;
        }

        /// <summary>
        /// Determines if tag is KVP and returns index of matching tag or -1
        /// </summary>
        /// <param name="whichTag"></param>
        /// <returns>index or -1</returns>
        public int FindTag(string whichTag)
        {
            string tagName = whichTag;
            bool isKVP = whichTag.IndexOf("=") > 0;
            if (isKVP) tagName = tagName.Substring(0, tagName.IndexOf("=") - 1);
            return FindTag(tagName, isKVP);
        }



        /// <summary>
        /// Returns the index of the tag or -1 if the tag does not exist
        /// </summary>
        /// <param name="whichTag">The tag to be found</param>
        /// <param name="isKVP">tag is in KVP format (name=value), matches name</param>
        /// <returns>index of tag or -1</returns>
        public int FindTag(string whichTag, bool isKVP)
        {
            string thisTag;
            if (!HasTags) return -1;
            for (int i = 0; i < this.tags.Length; i++)
            {
                thisTag = this.tags[i];
                if (((isKVP) && (thisTag.StartsWith(whichTag))) || (!isKVP && thisTag == whichTag)) return i;
            }
            return -1;
        }

    }
}

