using SutoriProject.Sutori.Attributes;
using System;

namespace SutoriProject.Sutori.Enums
{
    public enum SutoriCulture
	{
		/// <summary>Default when not using multiple cultures.</summary>
		[Alternative("none")] None,

		/// <summary>All cultures.</summary>
		[Alternative("all")] All,

		/// <summary>English (United States)</summary>
		[Alternative("en-US")] EnUS,

		/// <summary>Chinese (simplified, PRC)</summary>
		[Alternative("zh-CN")] zhCN,

		/// <summary>Russian (Russia)</summary>
		[Alternative("ru-RU")] ruRU,

		/// <summary>French (France)</summary>
		[Alternative("fr-FR")] FrFR,

		/// <summary>Spanish (Spain)</summary>
		[Alternative("es-ES")] esES,

		/// <summary>English (United Kingdom)</summary>
		[Alternative("en-GB")] EnGB,

		/// <summary>German (Germany)</summary>
		[Alternative("de-DE")] deDE,

		/// <summary>Portuguese (Brazil)</summary>
		[Alternative("pt-BR")] ptBR,

		/// <summary>English (Canada)</summary>
		[Alternative("en-CA")] enCA,

		/// <summary>Spanish (Mexico)</summary>
		[Alternative("es-MX")] esMX,

		/// <summary>Italian (Italy)</summary>
		[Alternative("it-IT")] itIT,

		/// <summary>Japanese (Japan)</summary>
		[Alternative("ja-JP")] jaJP
	}


	public static class SutoriCultureHelper
    {
		public static SutoriCulture Parse(string value)
        {
			// deal with nulls and empty strings.
			if (string.IsNullOrWhiteSpace(value))
				return default(SutoriCulture);

			// test generic parsing first.
			if (Enum.TryParse(value, out SutoriCulture parsed))
				return parsed;

			// use alternative parsing if needed.
			switch (value)
            {
				case "none": return SutoriCulture.None;
				case "all": return SutoriCulture.All;
				case "en-US": return SutoriCulture.EnUS;
				case "zh-CN": return SutoriCulture.zhCN;
				case "ru-RU": return SutoriCulture.ruRU;
				case "fr-FR": return SutoriCulture.FrFR;
				case "es-ES": return SutoriCulture.esES;
				case "en-GB": return SutoriCulture.EnGB;
				case "de-DE": return SutoriCulture.deDE;
				case "pt-BR": return SutoriCulture.ptBR;
				case "en-CA": return SutoriCulture.enCA;
				case "es-MX": return SutoriCulture.esMX;
				case "it-IT": return SutoriCulture.itIT;
				case "ja-JP": return SutoriCulture.jaJP;
			}

			return default(SutoriCulture);
		}
	}
}