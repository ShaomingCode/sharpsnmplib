namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ValueAssignment : IConstruct
    {
        public ISmiType SmiType { get; set; }
        public ISmiValue SmiValue { get; set; }

        public ValueAssignment(ISmiType smiType, ISmiValue smiValue)
        {
            SmiType = smiType;
            SmiValue = smiValue;
        }

        public string Name { get; set; }
    }
}