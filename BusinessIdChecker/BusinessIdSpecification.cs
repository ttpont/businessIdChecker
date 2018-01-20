using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessIdChecker
{
    public class BusinessIdSpecification : ISpecification<string>
    {
        private List<string> _reasonsForDissatisfaction;
        private bool _isValidBusinessId;
        private const string RegexPattern = @"^([0-9]{6,7})-([0-9]{1})";
        private readonly int[] CheckSumMultiplyArray = new int[] { 7, 9, 10, 5, 8, 4, 2 };
        private List<Action<string>> _checkList = new List<Action<string>>();

        public BusinessIdSpecification()
        {
            _checkList.Add(LengthCheck);
            _checkList.Add(RegexCheck);
            _checkList.Add(ValidateCheckSum);
        }

        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get
            {
                return _reasonsForDissatisfaction;
            }
        }

        public bool IsSatisfiedBy(string entity)
        {
            _isValidBusinessId = true;
            _reasonsForDissatisfaction = new List<string>();

            entity = entity ?? "";


            foreach (var check in _checkList)
            {
                check(entity);
            }

            return _isValidBusinessId;
        }

        private void LengthCheck(string entity)
        {
            if (entity.Length < 8)
            {
                InvalidateBusinessId("Entity is too short.");
                return;
            }
            else if (entity.Length > 9)
            {
                InvalidateBusinessId("Entity is too long.");
                return;
            }
        }

        private void RegexCheck(string entity)
        {
            var regex = new Regex(RegexPattern);
            if (!regex.IsMatch(entity))
            {
                InvalidateBusinessId("Entity has invalid format.");
            }
        }

        private void ValidateCheckSum(string entity)
        {
            try
            {
                if (entity.Length == 0)
                {
                    InvalidateBusinessId("Cannot calculate check sum for empty business ids.");
                    return;
                }

                if (entity.Length > 9)
                {
                    InvalidateBusinessId("Cannot calculate check sum for too long business ids.");
                    return;
                }

                var checkSum = (int)char.GetNumericValue(entity, entity.Length - 1);
                int calculatedSum = 0;

                // BusinessId can be 8 or 9 chars long. Add 0 prefix for too short businessIds
                while (entity.Length < 8)
                    entity = "0" + entity;

                for (int i = 0; i < CheckSumMultiplyArray.Length; i++)
                {
                    calculatedSum += (int)char.GetNumericValue(entity, i) * CheckSumMultiplyArray[i];
                }

                var calculatedCheckDigit = calculatedSum % 11;

                if (calculatedCheckDigit == 1)
                {
                    InvalidateBusinessId("Entity has invalid check sum. Check sum module cannot be 1.");
                }

                if (calculatedCheckDigit == 0 && checkSum != 0)
                {
                    InvalidateBusinessId("Entity has invalid check sum.");
                }

                if (calculatedCheckDigit > 2 && calculatedCheckDigit < 10 && checkSum != (11 - calculatedCheckDigit))
                {
                    InvalidateBusinessId("Entity has invalid check sum.");
                }
            }
            catch
            {
                InvalidateBusinessId("Unspecified error while calculating check sum.");
            }
        }

        private void InvalidateBusinessId(string message)
        {
            _reasonsForDissatisfaction.Add(message);
            _isValidBusinessId = false;
        }

    }
}
