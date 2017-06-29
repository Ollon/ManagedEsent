//-----------------------------------------------------------------------
// <copyright file="Windows8InstanceParameterTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.
// </copyright>
//-----------------------------------------------------------------------

namespace InteropApiTests
{
    using System;
    using Microsoft.Isam.Esent.Interop;
    using Microsoft.Isam.Esent.Interop.Windows7;
    using Microsoft.Isam.Esent.Interop.Windows8;
    using Microsoft.Isam.Esent.Interop.Windows81;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test the InstanceParameters class.
    /// </summary>
    public partial class InstanceParameterTests
    {
        /// <summary>
        /// Test the MaxTransactionSize property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the MaxTransactionSize property")]
        public void SetAndRetrieveInstanceParametersMaxTransactionSize()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var expected = 79;
            this.instanceParameters.MaxTransactionSize = expected;
            Assert.AreEqual(expected, this.instanceParameters.MaxTransactionSize);
        }

        /// <summary>
        /// Setting the MaxTransactionSize property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the MaxTransactionSize property should set the parameter on the instance")]
        public void VerifySetInstanceParametersMaxTransactionSize()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            int expected = 33;
            this.instanceParameters.MaxTransactionSize = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows8Param.MaxTransactionSize));
        }

        /// <summary>
        /// Test the EnableDbScanInRecovery property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the EnableDbScanInRecovery property")]
        public void SetAndRetrieveInstanceParametersEnableDbScanInRecovery()
        {
            var expected = Any.Int32Range(0, 2);
            this.instanceParameters.EnableDbScanInRecovery = expected;
            Assert.AreEqual(expected, this.instanceParameters.EnableDbScanInRecovery);
        }

        /// <summary>
        /// Setting the EnableDbScanInRecovery property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the EnableDbScanInRecovery property should set the parameter on the instance")]
        public void VerifySetInstanceParametersEnableDbScanInRecovery()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var expected = Any.Int32Range(0, 2);
            this.instanceParameters.EnableDbScanInRecovery = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows7Param.EnableDbScanInRecovery));
        }

        /// <summary>
        /// Test the DbScanThrottle property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the DbScanThrottle property")]
        public void SetAndRetrieveInstanceParametersDbScanThrottle()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var expected = 80;
            this.instanceParameters.DbScanThrottle = expected;
            Assert.AreEqual(expected, this.instanceParameters.DbScanThrottle);
        }

        /// <summary>
        /// Setting the DbScanThrottle property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the DbScanThrottle property should set the parameter on the instance")]
        public void VerifySetInstanceParametersDbScanThrottle()
        {
            int expected = 33;
            this.instanceParameters.DbScanThrottle = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows7Param.DbScanThrottle));
        }
        
        /// <summary>
        /// Test the DbScanIntervalMinSec property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the DbScanIntervalMinSec property")]
        public void SetAndRetrieveInstanceParametersDbScanIntervalMinSec()
        {
            var expected = 81;
            this.instanceParameters.DbScanIntervalMinSec = expected;
            Assert.AreEqual(expected, this.instanceParameters.DbScanIntervalMinSec);
        }

        /// <summary>
        /// Setting the DbScanIntervalMinSec property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the DbScanIntervalMinSec property should set the parameter on the instance")]
        public void VerifySetInstanceParametersDbScanIntervalMinSec()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            int expected = 34;
            this.instanceParameters.DbScanIntervalMinSec = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows7Param.DbScanIntervalMinSec));
        }

        /// <summary>
        /// Test the DbScanIntervalMaxSec property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the DbScanIntervalMaxSec property")]
        public void SetAndRetrieveInstanceParametersDbScanIntervalMaxSec()
        {
            var expected = 82;
            this.instanceParameters.DbScanIntervalMaxSec = expected;
            Assert.AreEqual(expected, this.instanceParameters.DbScanIntervalMaxSec);
        }

        /// <summary>
        /// Setting the DbScanIntervalMaxSec property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the DbScanIntervalMaxSec property should set the parameter on the instance")]
        public void VerifySetInstanceParametersDbScanIntervalMaxSec()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            int expected = 35;
            this.instanceParameters.DbScanIntervalMaxSec = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows7Param.DbScanIntervalMaxSec));
        }

        /// <summary>
        /// Test the CachePriority property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the CachePriority property")]
        public void SetAndRetrieveInstanceParametersCachePriority()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var expected = 83;
            this.instanceParameters.CachePriority = expected;
            Assert.AreEqual(expected, this.instanceParameters.CachePriority);
        }

        /// <summary>
        /// Setting the CachePriority property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the CachePriority property should set the parameter on the instance")]
        public void VerifySetInstanceParametersCachePriority()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            int expected = 36;
            this.instanceParameters.CachePriority = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows8Param.CachePriority));
        }

        /// <summary>
        /// Test the PrereadIOMax property.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test the PrereadIOMax property")]
        public void SetAndRetrieveInstanceParametersPrereadIOMax()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var expected = 84;
            this.instanceParameters.PrereadIOMax = expected;
            Assert.AreEqual(expected, this.instanceParameters.PrereadIOMax);
        }

        /// <summary>
        /// Setting the PrereadIOMax property should set the parameter
        /// on the instance.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the PrereadIOMax property should set the parameter on the instance")]
        public void VerifySetInstanceParametersPrereadIOMax()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            int expected = 37;
            this.instanceParameters.PrereadIOMax = expected;
            Assert.AreEqual(expected, this.GetIntegerParameter(Windows8Param.PrereadIOMax));
        }
        
        /// <summary>
        /// Test that EnableDBScanSerialization can be set and retrieved.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test that EnableDBScanSerialization can be set and retrieved")]
        public void VerifyGetAndSetEnableDBScanSerialization()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            bool expected = Any.Boolean;
            this.instanceParameters.EnableDBScanSerialization = expected;
            Assert.AreEqual(expected, this.GetBooleanParameter(Windows8Param.EnableDBScanSerialization));
            Assert.AreEqual(expected, this.instanceParameters.EnableDBScanSerialization);
        }

        /// <summary>
        /// Test that DurableCommitCallback can be set and retrieved.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test that DurableCommitCallback can be set and retrieved")]
        public void VerifyGetAndSetDurableCommitCallback()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            NATIVE_JET_PFNDURABLECOMMITCALLBACK savedCallback = this.instanceParameters.GetDurableCommitCallback();
            try
            {
                this.instanceParameters.SetDurableCommitCallback(this.NullDurableCommitCallback);
                Assert.AreEqual(this.NullDurableCommitCallback, this.instanceParameters.GetDurableCommitCallback());
            }
            finally
            {
                this.instanceParameters.SetDurableCommitCallback(savedCallback);
            }
        }

        /// <summary>
        /// Test that a null DurableCommitCallback can be set and retrieved.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test that a null DurableCommitCallback can be set and retrieved.")]
        public void VerifyGetAndSetNullDurableCommitCallback()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            NATIVE_JET_PFNDURABLECOMMITCALLBACK savedCallback = this.instanceParameters.GetDurableCommitCallback();
            try
            {
                this.instanceParameters.SetDurableCommitCallback(null);
                Assert.AreEqual(null, this.instanceParameters.GetDurableCommitCallback());
            }
            finally
            {
                this.instanceParameters.SetDurableCommitCallback(savedCallback);
            }
        }

        #region Windows 8.1
        /// <summary>
        /// Test the EnableShrinkDatabase property. Introduced in Windows 8.1.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Setting the EnableShrinkDatabase property should set the parameter.")]
        public void VerifySettingEnableDatabaseShrink()
        {
            if (!EsentVersion.SupportsWindows81Features)
            {
                return;
            }

            ShrinkDatabaseGrbit expected = ShrinkDatabaseGrbit.On | ShrinkDatabaseGrbit.Realtime;
            this.instanceParameters.EnableShrinkDatabase = expected;
            Assert.AreEqual((int)expected, this.GetIntegerParameter(Windows81Param.EnableShrinkDatabase));
            Assert.AreEqual(expected, this.instanceParameters.EnableShrinkDatabase);
        }
        #endregion
        /// <summary>
        /// A dummy function for the durable commit callback.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="commitIdSeen">
        /// The commit identifier.
        /// </param>
        /// <param name="grbit">
        /// A group of option bits.
        /// </param>
        /// <returns>An error code, equal to success.</returns>
        private JET_err NullDurableCommitCallback(
            IntPtr instance,
            ref NATIVE_COMMIT_ID commitIdSeen,
            uint grbit)
        {
            return JET_err.Success;
        }
    }
}
