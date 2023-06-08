using BSN.Commons.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BSN.Commons.Utilities
{
    public static class PhoneNumberUtilities
	{
		public const string InternationalPrefixSymbol = "+";

		public static bool IsValidPhoneNumber(string value)
		{
			if (value == null)
				return false;

			return Regex.IsMatch(value, PHONE_NUMBER_PATTERN);
        }

		public static bool IsValidVirtualNumber(string value)
		{
			if (value == null)
				return false;

			return Regex.IsMatch(value, VIRTUAL_NUMBER_PATTERN);
		}

		/// <summary>
		/// Ascertains if a given string is a valid ICCID, which in our domain is the SerialNumber property.
		/// More information available at https://en.wikipedia.org/wiki/SIM_card#ICCID.
		/// </summary>
		public static bool IsValidIntegratedCircuitCardIdentifier(string value)
        {
			if (value == null)
				return false;

			return Regex.IsMatch(value, INTEGRATED_CIRCUIT_CARD_IDENTIFIER_PATTERN);
		}

		public static bool IsValidPhoneNumberPrefixes(List<string> values)
		{
			if (values.IsNullOrEmpty())
				return false;

			return values.TrueForAll(P => Regex.IsMatch(P, PHONE_NUMBER_PREFIX));
		}

		public static bool IsValidDisplayName(string value)
        {
			if (value == null)
				return true;

			return value.Length <= DISPLAY_NAME_MAXIMUM_LENGTH;
        }


		private const string PHONE_NUMBER_PATTERN = "^98[0-9]{3,10}$";
		private const string VIRTUAL_NUMBER_PATTERN = "^98[0-9]{10}$";
		private const string PHONE_NUMBER_PREFIX = "^98[0-9]{1,10}$";
		private const string INTEGRATED_CIRCUIT_CARD_IDENTIFIER_PATTERN = "^[0-9]{1,22}$";
		private const int DISPLAY_NAME_MAXIMUM_LENGTH = 16;
	}
}
