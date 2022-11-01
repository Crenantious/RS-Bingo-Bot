// <copyright file="EnumDict.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common
{
    /// <summary>
    /// Dictionary to link enum with fields instead of using constants.
    /// </summary>
    /// <typeparam name="TEntity">The enum to be used in the class.</typeparam>
    public class EnumDict<TEntity>
        where TEntity : Enum
    {
        private readonly Dictionary<TEntity, sbyte> enumToSbyte = new Dictionary<TEntity, sbyte>();
        private readonly Dictionary<sbyte, TEntity> sbyteToEnum = new Dictionary<sbyte, TEntity>();
        private readonly TEntity defaultGet;


        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDict{TEntity}"/> class.
        /// </summary>
        /// <param name="defaultValue">The enum to return if the value is not found.</param>
        public EnumDict(TEntity defaultValue)
        {
            defaultGet = defaultValue;
        }

        /// <summary>
        /// Add an entry and return self for linking.
        /// </summary>
        /// <param name="enumVal">The enum to add.</param>
        /// <param name="value">The matching value to add.</param>
        /// <returns>Self for linking.</returns>
        public EnumDict<TEntity> Add(TEntity enumVal, sbyte value)
        {
            enumToSbyte.Add(enumVal, value);
            sbyteToEnum.Add(value, enumVal);
            return this;
        }

        /// <summary>
        /// Retrieve the value from the enum.
        /// </summary>
        /// <param name="value">The enum to lookup the value for.</param>
        /// <returns>The value if found.</returns>
        public sbyte Get(TEntity value)
        {
            if (value != null && enumToSbyte.ContainsKey(value))
            { return enumToSbyte[value]; }
            return default;
        }

        /// <summary>
        /// Retrieve the enum from the value.
        /// </summary>
        /// <param name="value">The value to lookup the enum for.</param>
        /// <returns>The enum if found.</returns>
        public TEntity Get(sbyte value)
        {
            if (value != default && sbyteToEnum.ContainsKey(value))
            { return sbyteToEnum[value]; }
            return defaultGet;
        }

        /// <summary>
        /// Test if a given value is in the dictionary.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if the value tested is a valid entry for this enum.</returns>
        public bool ValidValue(sbyte value)
        {
            return value != default && sbyteToEnum.ContainsKey(value);
        }
    }
}