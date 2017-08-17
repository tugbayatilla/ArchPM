using ArchPM.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder
{
    public enum OrderByDirections
    {
        /// <summary>
        /// The asc
        /// </summary>
        [EnumDescription("ASC")]
        Asc,
        /// <summary>
        /// The desc
        /// </summary>
        [EnumDescription("DESC")]
        Desc
    }

    public enum JoinTypes
    {
        [EnumDescription("INNER")]
        Inner,
        [EnumDescription("OUTER")]
        Outer
    }

    public enum JoinDirections
    {
        [EnumDescription("LEFT")]
        Left,
        [EnumDescription("RIGHT")]
        Right
    }

    public enum Operators
    {
        [EnumDescription("AND")]
        AND,
        [EnumDescription("OR")]
        OR,
        [EnumDescription("=")]
        EQUALS,
        [EnumDescription("!=")]
        NOTEQUALS,
        [EnumDescription("IS")]
        IS,
        [EnumDescription("IS NOT")]
        ISNOT,
        [EnumDescription("IN")]
        IN,
        [EnumDescription("NOT IN")]
        NOTIN,
        [EnumDescription("LIKE")]
        LIKE,
        [EnumDescription("%")]
        PERCENT
    }

    public enum IncludeExclude
    {
        Include,
        Exclude
    }

    public enum QueryTypes
    {
        Select,
        Insert,
        Update,
        Create
    }

    public enum CreationTypes
    {
        WithValues,
        WithParameters
    }
}
