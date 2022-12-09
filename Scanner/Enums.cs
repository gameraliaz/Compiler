namespace Scanner
{
    public enum Tokens
    {
        Literal = 1,
        ID = 2,
        Const = 3,
        KW_procedure = 11,
        KW_division = 12,
        KW_end = 13,
        KW_set = 14,
        KW_to = 15,
        KW_get = 16,
        KW_put = 17,
        OP_div = 20, // /
        OP_sub = 21, // -
        OP_mul = 22, // *
        OP_add = 23, // +
        OP_sim = 24, // ;
        OP_cam = 25, // ,
        OP_lpa = 26, // (
        OP_rpa = 27, // )
    }
    public enum ClassLex
    {
        Error = 0,
        Accept = 1,
        AcceptStar = 2,
        OnWork = 3,
    }
}
