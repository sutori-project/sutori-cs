using SutoriProject.Sutori.Attributes;
using System;

namespace SutoriProject.Sutori.Enums
{
    public enum SutoriSolver
	{
		/// <summary>use this when no solver is required</summary>
		[Alternative("none")]
		None,

		/** use this when an option should be selected based on the index position of the option chosen */
		[Alternative("option_index")]
		OptionIndex,

		/** use this when an option should be selected based on a selected keyboard character */
		[Alternative("key_char_equality")]
		KeyCharEquality,

		/** use this when an option should be selected when text matches */
		[Alternative("text_equality")]
		TextEquality,

		/** use this if the custom callback should be used to determine when an option should be selected */
		[Alternative("custom")]
		Custom
	}


	public static class SutoriSolverHelper
    {
		public static SutoriSolver Parse(string value)
        {
			// deal with nulls and empty strings.
			if (string.IsNullOrWhiteSpace(value))
				return default(SutoriSolver);

			// test generic parsing first.
			if (Enum.TryParse(value, out SutoriSolver parsed))
				return parsed;

			// use alternative parsing if needed.
			switch (value)
			{
				case "none": return SutoriSolver.None;
				case "option_index": return SutoriSolver.OptionIndex;
				case "key_char_equality": return SutoriSolver.KeyCharEquality;
				case "text_equality": return SutoriSolver.TextEquality;
				case "custom": return SutoriSolver.Custom;
			}

			return default(SutoriSolver);
		}
    }
}