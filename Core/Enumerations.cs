using System.ComponentModel;

namespace Core
{
    public class Enumerations
    {
        public enum DataType 
        {
            [Description("Hurleys Max Number")]
            HURLEYS_MAX_NUMBER = 1,

            [Description("Text")]
            TEXT = 2,

            [Description("Date Time")]
            DATE_TIME = 3,

            [Description("Number")]
            NUMBER = 4,

            [Description("Yes/No")]
            BOOLEAN = 5,
        }

        public enum SourceType
        {
            [Description("Azure FHIR API")]
            AZURE_FHIR_API = 1,

            [Description("Cerner EMR")]
            CERNER_EMR = 2

        }
        public enum QueueHandlerType
        {
            [Description("Analyze Patient")]
            ANALYZE_PATIENT = 1
        }



        public enum AssetType
        {
            [Description("Generic")]
            Generic = 1,

            [Description("Patient")]
            Patient = 2,

            [Description("Condition")]
            Condition = 3,

            [Description("Allergy")]
            Allergy = 4,

            [Description("Medication")]
            Medication = 5,

            [Description("Observation")]
            Observation = 6,

            [Description("Encounter")]
            Encounter = 7,

        }
    }
}