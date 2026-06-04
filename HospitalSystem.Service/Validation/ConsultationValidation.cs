namespace HospitalSystem.Service.Validation
{
    public class ConsultationValidation
    {
        public static bool ValidateConsultationId(int consultationId) => consultationId > 1;
    }
}
