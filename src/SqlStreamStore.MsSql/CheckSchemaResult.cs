﻿namespace SqlStreamStore
{
    /// <summary>
    ///     Represents the result of a schema check.
    /// </summary>
    public class CheckSchemaResult
    {
        /// <summary>
        ///     The version of the schema checked.
        /// </summary>
        public int CurrentVersion { get; }

        /// <summary>
        ///     The expected version for this version of MsSqlStreamStore to be compatible with.
        /// </summary>
        public int ExpectedVersion { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="CheckSchemaResult"/>
        /// </summary>
        /// <param name="currentVersion">The current version of the schema.</param>
        /// <param name="expectedVersion">The expected version of the schema.</param>
        public CheckSchemaResult(int currentVersion, int expectedVersion)
        {
            CurrentVersion = currentVersion;
            ExpectedVersion = expectedVersion;
        }

        /// <summary>
        ///     Checks to see if the schema version matches.
        /// </summary>
        /// <returns>True if the version match, otherwise False.</returns>
        public bool IsMatch()
        {
            return CurrentVersion == ExpectedVersion;
        }
    }
}