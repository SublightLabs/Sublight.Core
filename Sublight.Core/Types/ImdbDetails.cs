namespace Sublight.Core.Types
{
    public class ImdbDetails
    {
        /// <summary>
        /// IMDb id, for example tt0944947.
        /// </summary>
        public string Id {get; set; }

        /// <summary>
        /// Title without accent characters.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Original title with accent characters.
        /// </summary>
        public string TitleDisplay { get; set; }

        public int? Year { get; set; }

        /// <summary>
        /// Filled for completed series.
        /// </summary>
        public int? YearTo { get; set; }

        /// <summary>
        /// Filled for series.
        /// </summary>
        public int? Season { get; set; }

        /// <summary>
        /// Filled for series.
        /// </summary>
        public int? Episode { get; set; }
    }
}
