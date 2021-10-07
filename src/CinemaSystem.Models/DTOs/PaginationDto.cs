namespace CinemaSystem.Models.DTOs
{
    public class PaginationDto
    {
        private int registersPerPageMaxQuantity = 50;

        public int Page { get; set; } = 1;

        private int registersPerPageQuantity = 10;
        public int RegistersPerPageQuantity
        {
            get => registersPerPageQuantity;
            set
            {
                if (value > this.registersPerPageMaxQuantity)
                {
                    value = this.registersPerPageMaxQuantity;
                }

                registersPerPageQuantity = value;
            }
        }
    }
}
