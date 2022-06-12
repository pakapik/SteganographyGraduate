using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Models.Analyzers
{
    public struct RSGroupResult
    {
        public int AllVariations { get; set; }
        public int PositiveVariations { get; set; }
        public int NegativeVariations { get; set; }

        public int CountPositiveRegular { get; set; }
        public int CountPositiveSingular { get; set; }
        public int CountPositiveUnusable { get; set; }

        public int CommonPositiveCount => CountPositiveRegular + CountPositiveSingular + CountPositiveUnusable;

        public int CountNegativeRegular { get; set; }
        public int CountNegativeSingular { get; set; }
        public int CountNegativeUnusable { get; set; }

        public int CommonNegativeCount => CountNegativeRegular + CountNegativeSingular + CountNegativeUnusable;

        public void UpdateGroups()
        {
            if (PositiveVariations > AllVariations)
            {
                CountPositiveRegular++;
            }
            if (PositiveVariations < AllVariations)
            {
                CountPositiveSingular++;
            }
            if (PositiveVariations == AllVariations)
            {
                CountPositiveUnusable++;
            }

            if (NegativeVariations > AllVariations)
            {
                CountNegativeRegular++;
            }
            if (NegativeVariations < AllVariations)
            {
                CountNegativeSingular++;
            }
            if (NegativeVariations == AllVariations)
            {
                CountNegativeUnusable++;
            }
        }

        public static RSGroupResult operator +(RSGroupResult rs1, RSGroupResult rs2)
        {
            return new RSGroupResult()
            {
                AllVariations = rs1.AllVariations + rs2.AllVariations,
                PositiveVariations = rs1.PositiveVariations + rs2.PositiveVariations,
                NegativeVariations = rs1.NegativeVariations + rs2.NegativeVariations,
                CountPositiveRegular = rs1.CountPositiveRegular + rs2.CountPositiveRegular,
                CountPositiveSingular = rs1.CountPositiveSingular + rs2.CountPositiveSingular,
                CountPositiveUnusable = rs1.CountPositiveUnusable + rs2.CountPositiveUnusable,
                CountNegativeRegular = rs1.CountNegativeRegular + rs2.CountNegativeRegular,
                CountNegativeSingular = rs1.CountNegativeSingular + rs2.CountNegativeSingular,
                CountNegativeUnusable = rs1.CountNegativeUnusable + rs2.CountNegativeUnusable
            };
        }     

        public static RSGroupResult operator /(RSGroupResult rs1, int divider)
        {
            return new RSGroupResult()
            {
                AllVariations = rs1.AllVariations / divider,
                PositiveVariations = rs1.PositiveVariations / divider,
                NegativeVariations = rs1.NegativeVariations / divider,
                CountPositiveRegular = rs1.CountPositiveRegular / divider,
                CountPositiveSingular = rs1.CountPositiveSingular / divider,
                CountPositiveUnusable = rs1.CountPositiveUnusable / divider,
                CountNegativeRegular = rs1.CountNegativeRegular / divider,
                CountNegativeSingular = rs1.CountNegativeSingular / divider,
                CountNegativeUnusable = rs1.CountNegativeUnusable / divider
            };
        }
    }
}
