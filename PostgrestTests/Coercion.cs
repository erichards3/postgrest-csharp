﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Postgrest;
using PostgrestTests.Models;

namespace PostgrestTests
{
    [TestClass]
    public class Coercion
    {
        private static string baseUrl = "http://localhost:3000";

        [TestMethod]
        public async Task CanCoerceData()
        {

            // Check against already included case (inserted in `01-dummy-data.sql`
            var existingItem = await new Client(baseUrl).Table<KitchenSink>().Single();

            if (existingItem != null)
            {
                Assert.AreEqual(99999.0f, existingItem.FloatValue);
                Assert.AreEqual(99999.0d, existingItem.DoubleValue);
                CollectionAssert.AreEquivalent(new List<string> { "set", "of", "strings" }, existingItem.ListOfStrings);
                Assert.AreEqual(DateTime.MaxValue, existingItem.DateTimePosInfinity);
                Assert.AreEqual(DateTime.MinValue, existingItem.DateTimeNegInfinity);
                CollectionAssert.AreEquivalent(new List<float> { 10.0f, 12.0f }, existingItem.ListOfFloats);
                Assert.AreEqual(new IntRange(20, 50), existingItem.IntRange);
            }

            var stringValue = "test";
            var intValue = 1;
            var floatValue = 1.1f;
            var doubleValue = 1.1d;
            var dateTimeValue = DateTime.UtcNow;
            var listOfStrings = new List<string> { "test", "1", "2", "3" };
            var listOfDateTime = new List<DateTime> { new DateTime(2021, 12, 10), new DateTime(2021, 12, 11), new DateTime(2021, 12, 12) };
            var listOfInts = new List<int> { 1, 2, 3 };
            var listOfFloats = new List<float> { 1.1f, 1.2f, 1.3f };
            var intRange = new IntRange(0, 1);


            var model = new KitchenSink
            {
                StringValue = stringValue,
                IntValue = intValue,
                FloatValue = floatValue,
                DoubleValue = doubleValue,
                DateTimeValue = dateTimeValue,
                DateTimeNegInfinity = DateTime.MinValue,
                DateTimePosInfinity = DateTime.MaxValue,
                ListOfStrings = listOfStrings,
                ListOfDateTimes = listOfDateTime,
                ListOfInts = listOfInts,
                ListOfFloats = listOfFloats,
                IntRange = intRange
            };


            var insertedModel = await new Client(baseUrl).Table<KitchenSink>().Insert(model);
            var actual = insertedModel.Models.First();

            Assert.AreEqual(model.StringValue, actual.StringValue);
            Assert.AreEqual(model.IntValue, actual.IntValue);
            Assert.AreEqual(model.FloatValue, actual.FloatValue);
            Assert.AreEqual(model.DoubleValue, actual.DoubleValue);
            Assert.AreEqual(model.DateTimeValue.ToString(), actual.DateTimeValue.ToString());
            CollectionAssert.AreEquivalent(model.ListOfStrings, actual.ListOfStrings);
            CollectionAssert.AreEquivalent(model.ListOfDateTimes, actual.ListOfDateTimes);
            CollectionAssert.AreEquivalent(model.ListOfInts, actual.ListOfInts);
            CollectionAssert.AreEquivalent(model.ListOfFloats, actual.ListOfFloats);
            Assert.AreEqual(model.IntRange.Start, actual.IntRange!.Start);
            Assert.AreEqual(model.IntRange.End, actual.IntRange.End);
            
        }
    }
}
