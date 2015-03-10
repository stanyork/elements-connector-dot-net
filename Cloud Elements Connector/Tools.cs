using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public sealed class Tools
    {

        public static string TraceTimeFormat = "HH:mm:ss.ff";

        public static event ProgressEventHandler Progress;
        public delegate void ProgressEventHandler(object sender, int Pct);

        /// <summary>
        /// Copies potentially large stream, raising event as percent changes
        /// </summary>
        /// <param name="inStream">still open</param>
        /// <param name="outStream">flushed, but still open</param>
        /// <param name="inLength"></param>
        public static async Task StreamCopyWithProgress(System.IO.Stream inStream, System.IO.Stream outStream, long inLength)
        {
            byte[] zBuffer = new byte[16384];
            int zLen = 0;
            int pct = 0;
            int lastPct = 0;
            double totalToDo = inLength;
            if (totalToDo == 0) totalToDo = 1024 * 1024 * 1024;
            long doneSoFar = 0;
            Boolean Copying = true;
            while (Copying)
            {
                zLen = await inStream.ReadAsync(zBuffer, 0, zBuffer.Length);
                if (zLen == 0)
                {
                    Copying = false;
                }
                else
                {
                    await outStream.WriteAsync(zBuffer, 0, zLen);
                    doneSoFar += zLen;
                    if (totalToDo < (doneSoFar + zBuffer.Length)) totalToDo *= 2;
                    pct = Convert.ToInt32((doneSoFar / totalToDo) * 100);
                    if (pct > lastPct)
                    {
                        if (Progress != null) Progress(inStream, pct);
                        lastPct = pct;
                    }
                }
            }
            if (Progress != null) Progress(inStream, 100);
            outStream.Flush();

        }

        public static string SizeInBytesToString(Int64 sizeInBytes)
        {
            return SizeInBytesToString((UInt64)sizeInBytes);
        }



        /// <summary>
        /// Given a size in bytes, returns a string description with units in KB, MB, or GB
        /// </summary>
        /// <param name="sizeInBytes"></param>
        /// <returns></returns>
        public static string SizeInBytesToString(UInt64 sizeInBytes)
        {
            string result = string.Empty;
            if (sizeInBytes < 0)
            {
                result = string.Empty;
            }
            else if (sizeInBytes < 1024)
            {
                result = sizeInBytes.ToString();
            }
            else if (sizeInBytes < 1048576)
            {
                result = string.Format("{0:F0}KB", sizeInBytes / 1024);
            }
            else if (sizeInBytes < 1073741824)
            {
                result = string.Format("{0:F1}MB", sizeInBytes / 1048576);
            }
            else
            {
                result = string.Format("{0:N1}GB", sizeInBytes / 1073741824);
            }
            return result;
        }

        /// <summary>
        /// Converts .doc to application/msword, etc
        /// </summary>
        /// <param name="fileType">Extension of file, including leading .</param>
        /// <returns>MIME content type</returns>
        /// <remarks>Requires access to windows registry</remarks>
        public static string FileTypeToMimeContentType(string fileType)
        {
            if (!fileType.StartsWith(".")) fileType = "." + fileType;
            string ContentType = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\" + fileType, "Content Type", null);
            if (ContentType == null) {
                ContentType = GetContentTypeForExt(fileType);
            }

            return ContentType;
        }


        /// <summary>
        /// Converts a simple dictionary to a string containing JSON 
        /// </summary>
        /// <param name="info">Dictionary to convert</param>
        /// <returns>a string with properly formatted json</returns>
        /// <remarks>Consider using JSON.NET instead</remarks>
        public static string ToJson(Dictionary<string, object> info)
        {
            System.Text.StringBuilder responseBuilder = new System.Text.StringBuilder();
            responseBuilder.Append("{");
            bool IsFirst = true;
            foreach (KeyValuePair<string, object> kvp in info)
            {
                if ((kvp.Value != null))
                {
                    if (!IsFirst)
                    {
                        responseBuilder.Append(",");
                    }
                    else
                    {
                        IsFirst = false;
                    }
                    responseBuilder.AppendFormat("\"{0}\":", kvp.Key);
                    if (kvp.Value is string || kvp.Value is Guid)
                    {
                        responseBuilder.AppendFormat("\"{0}\"", kvp.Value);
                    }
                    else if (kvp.Value is int || kvp.Value is Int16 || kvp.Value is byte)
                    {
                        responseBuilder.AppendFormat("{0}", kvp.Value);
                    }
                    else if (kvp.Value is double || kvp.Value is decimal)
                    {
                        responseBuilder.AppendFormat("{0}", kvp.Value);
                    }
                    else if (kvp.Value is bool)
                    {
                        bool bv = (bool)kvp.Value;
                        responseBuilder.AppendFormat("{0}", bv.ToString().ToLower());
                    }
                    else
                    {
                        responseBuilder.AppendFormat("\"{0}\",\"{1}-type\":\"{2}\"", kvp.Value, kvp.Key, kvp.Value.GetType().ToString());
                    }
                }
            }
            responseBuilder.Append("}");
            return responseBuilder.ToString();
        }


        /// -----------------------------------------------------------------------------
        ///<summary>
        ///Receives a string and a file name as parameters and writes the contents of the string to that file
        ///</summary>
        ///<example><code>
        ///string lcString = "This is the line we want to insert in our file.";
        ///StrToFile(lcString, "c:\My Folders\MyFile.txt")</code>
        ///</example>
        ///<param name="cExpression">String containing source data </param>
        ///<param name="cFileName">String containing file specification </param>
        /// <remarks>Deletes the target file if it already exisits</remarks>
        ///-----------------------------------------------------------------------------
        public static void StringToFile(string cExpression, string cFileName)
        {
            //Check if the sepcified file exists
            if (System.IO.File.Exists(cFileName) == true)
            {
                //If so then Erase the file first as in this case we are overwriting
                System.IO.File.Delete(cFileName);
            }

            //Create the file if it does not exist and open it
            System.IO.FileStream oFs = new System.IO.FileStream(cFileName, System.IO.FileMode.CreateNew, System.IO.FileAccess.ReadWrite);

            //Create a writer for the file
            System.IO.StreamWriter oWriter = new System.IO.StreamWriter(oFs);

            //Write the contents
            oWriter.Write(cExpression);
            oWriter.Flush();
            oWriter.Close();

            oFs.Close();
        }


        /// -----------------------------------------------------------------------------
        ///<summary>
        ///Given a file name as a parameter, returns the contents of that file in a string.
        ///</summary>
        ///<example>
        /// <code>
        ///string lcFile = "c:\\my documents\\test.txt";
        ///string contents ; 
        ///contents = FileToString(lcFile)
        ///</example>
        /// <param name="fileName">A string containing the file specification</param>
        /// <returns>The contents of the file in a string</returns>
        /// <remarks></remarks>
        /// <exception cref="FileNotFoundException">If the file is not found</exception>
        ///-----------------------------------------------------------------------------
        public static string FileToString(string fileName)
        {
            //Create a StreamReader and open the file
            if (fileName == null || fileName == null) return "";
            System.IO.StreamReader oReader = System.IO.File.OpenText(fileName);
            //Read all the contents of the file in a string
            string lcString = oReader.ReadToEnd();

            //Close the StreamReader and return the string
            oReader.Close();
            oReader.Dispose();
            return lcString;
        }

        public static string TraceTimeNow()
        {
            return DateTime.Now.ToString(TraceTimeFormat);
        }


        /// <summary>
        /// Returns a digest (hash) of the stream.  
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="useCrypto">MD5,SHA1, SHA256, SHA512 (etc)</param>
        /// <returns></returns>
        public static string HashForBuffer(System.IO.Stream inStream, string useCrypto)
        {
            System.Security.Cryptography.HashAlgorithm viaCrypto = default(System.Security.Cryptography.HashAlgorithm);
            try
            {
                viaCrypto = (System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName(useCrypto.ToUpper());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("{0}: <#> {1}({3}) - {2}; Check crypto name", DateTime.Now.ToString(TraceTimeFormat), "HashForBuffer", ex.ToString(), useCrypto);
                throw ex;
            }
            Byte[] hashedBytes = viaCrypto.ComputeHash(inStream);
            string hashedText = BitConverter.ToString(hashedBytes).Replace("-", "");
            return hashedText;
        }


        private static string GetContentTypeForExt(string FileExt)
        {
            string ContentType = null;
            if (FileExt.StartsWith(".")) FileExt = FileExt.Substring(1);
            switch (FileExt.ToLower())
            {
                case "pdf":
                    ContentType = "application/pdf";

                    break;
                case "utf":
                    ContentType = "text/plain; charset=unicode-1-1";

                    break;
                case "zip":
                    ContentType = "application/zip";

                    break;
                case "htm":
                case "html":
                    ContentType = "text/html";

                    break;
                case "txt":
                    ContentType = "text/plain";

                    break;
                case "doc":
                    ContentType = "application/msword";

                    break;
                case "rtf":
                    ContentType = "application/rtf";

                    break;
                case "ppt":
                    ContentType = "application/vnd.ms-powerpoint";

                    break;
                case "pptx":
                case "ppts":
                    ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

                    break;
                case "csv":
                    ContentType = "application/vnd.ms-excel";

                    break;
                case "xls":
                    ContentType = "application/vnd.ms-excel";

                    break;
                case "xsl":
                case "xml":
                case "wsdl":
                    ContentType = "text/xml";

                    break;
                case "spm":
                case "spml":

                    ContentType = "text/html";

                    break;
                case "gif":
                    ContentType = "image/gif";

                    break;
                case "jpg":
                case "jpeg":
                    ContentType = "image/jpeg";

                    break;
                case "png":
                    ContentType = "image/png";

                    break;
                case "ps":
                    ContentType = "application/postscript";

                    break;
                case "tif":
                case "tiff":
                    ContentType = "image/tiff";

                    break;
                case "xbm":
                    ContentType = "image/x-xbitmap";

                    break;
                case "log":
                    ContentType = "text/plain";

                    break;
                case "dcr":
                    ContentType = "application/x-director";

                    break;
                case "wav":
                    ContentType = "audio/x-wav";

                    break;
                case "au":
                    ContentType = "audio/basic";

                    break;
                case "class":
                case "cls":
                case "exe":
                case "dll":
                    ContentType = "application/octet-stream";

                    break;
                case "pict":
                    ContentType = "application/image/x-macpict";

                    break;
                case "mp4":
                    ContentType = "video/mp4";

                    break;
                case "mpg":
                case "mpeg":
                    ContentType = "video/mpeg";

                    break;
                case "moov":
                case "mov":
                    ContentType = "video/quicktime";

                    break;
                case "svg":
                case "svgz":
                    ContentType = "image/svg+xml";

                    break;
                case "wml":
                    ContentType = "text/vnd.wap.wml";

                    break;
                case "wmlc":
                    ContentType = "application/vnd.wap.wmlc";

                    break;
                case "wmls":
                    ContentType = "text/vnd.wap.wmls";

                    break;
                case "wmlscriptc":
                    ContentType = "application/vnd.wap.wmlscriptc";

                    break;
                case "wbmp":
                    ContentType = "image/vnd.wap.wbmp";

                    break;
                case "jad":
                    ContentType = "text/vnd.sun.j2me.app-descriptor";

                    break;
                case "jar":
                    ContentType = "application/java-archive";

                    break;
                default:
                    ContentType = "application/binary";
                    break;
            }
            return ContentType;
        }
         

    }
}
