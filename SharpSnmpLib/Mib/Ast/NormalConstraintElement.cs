using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NormalConstraintElement : ConstraintElement
    {
        public NormalConstraintElement(ConstraintElement constraintElement)
        {
            
        }

        public NormalConstraintElement()
        {

        }

        public void Add(ConstraintElement element)
        {

        }

        public ConstraintElement ConstraintElement
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ISmiType ConstraintType
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Includes
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Ellipsis
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IList<ConstraintElement> TypeConstraintList
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}