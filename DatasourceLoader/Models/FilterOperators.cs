namespace Dma.DatasourceLoader.Models
{
    public class FilterOperators : IEquatable<FilterOperators>
    {
        private string _value;
        public string Value => _value;
        
        public static readonly FilterOperators Eq = new FilterOperators("equals");
        public static readonly FilterOperators NotEq = new FilterOperators("not_equals");
        public static readonly FilterOperators Contains = new FilterOperators("contains");
        public static readonly FilterOperators NotContains = new FilterOperators("not_contains");
        public static readonly FilterOperators StartsWith = new FilterOperators("starts_with");
        public static readonly FilterOperators EndsWith = new FilterOperators("ends_with");
        public static readonly FilterOperators In = new FilterOperators("in");
        public static readonly FilterOperators NotIn = new FilterOperators("not_in");
        public static readonly FilterOperators Gt = new FilterOperators("gt");
        public static readonly FilterOperators Gte = new FilterOperators("gte");
        public static readonly FilterOperators Lt = new FilterOperators("lt");
        public static readonly FilterOperators Lte = new FilterOperators("lte");
        public static readonly FilterOperators Null = new FilterOperators("null");
        public static readonly FilterOperators NotNull = new FilterOperators("not_null");

        public static readonly FilterOperators[] ComplexFilters = new FilterOperators[]
        {
            In, NotIn
        };


        private FilterOperators(string value) {
            _value = value;
        }

        public static implicit operator string(FilterOperators op) => op.Value;
        //public static implicit operator FilterOperators(string op) => new(op);

        public bool Equals(FilterOperators? other)
        {
            return other?.Value == _value;
        }
    }
}
