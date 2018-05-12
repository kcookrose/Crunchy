namespace Crunchy.Models {
    public class FileRef {

        /// <summary>
        /// Unique ID for the file ref.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Url to be used in GET request to retrive file.
        /// </summary>
        public string RepoUrl { get; set; }
    }
}