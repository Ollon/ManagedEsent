//-----------------------------------------------------------------------
// <copyright file="Windows8BasicDdlTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.
// </copyright>
//-----------------------------------------------------------------------

namespace InteropApiTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Isam.Esent.Interop;
    using Microsoft.Isam.Esent.Interop.Vista;
    using Microsoft.Isam.Esent.Interop.Windows8;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Basic DDL tests
    /// </summary>
    [TestClass]
    public class Windows8BasicDdlTests
    {
        /// <summary>
        /// The directory being used for the database and its files.
        /// </summary>
        private string directory;

        /// <summary>
        /// The path to the database being used by the test.
        /// </summary>
        private string database;

        /// <summary>
        /// The name of the table.
        /// </summary>
        private string table;

        /// <summary>
        /// The instance used by the test.
        /// </summary>
        private JET_INSTANCE instance;

        /// <summary>
        /// The session used by the test.
        /// </summary>
        private JET_SESID sesid;

        /// <summary>
        /// Identifies the database used by the test.
        /// </summary>
        private JET_DBID dbid;

        /// <summary>
        /// The tableid being used by the test.
        /// </summary>
        private JET_TABLEID tableid;

        /// <summary>
        /// Columnid of the column in the table.
        /// </summary>
        private JET_COLUMNID testColumnid;

        #region Setup/Teardown

        /// <summary>
        /// Initialization method. Called once when the tests are started.
        /// All DDL should be done in this method.
        /// </summary>
        [TestInitialize]
        [Description("Setup for BasicDDLTests")]
        public void Setup()
        {
            this.directory = SetupHelper.CreateRandomDirectory();
            this.database = Path.Combine(this.directory, "database.edb");
            this.table = "table";
            this.instance = SetupHelper.CreateNewInstance(this.directory);

            Api.JetSetSystemParameter(this.instance, JET_SESID.Nil, JET_param.Recovery, 0, "off");
            Api.JetInit(ref this.instance);
            Api.JetBeginSession(this.instance, out this.sesid, string.Empty, string.Empty);
            Api.JetCreateDatabase(this.sesid, this.database, string.Empty, out this.dbid, CreateDatabaseGrbit.None);
            Api.JetBeginTransaction(this.sesid);
            Api.JetCreateTable(this.sesid, this.dbid, this.table, 0, 100, out this.tableid);

            var columndef = new JET_COLUMNDEF()
            {
                cp = JET_CP.Unicode,
                coltyp = JET_coltyp.LongText,
            };
            Api.JetAddColumn(this.sesid, this.tableid, "TestColumn", columndef, null, 0, out this.testColumnid);

            Api.JetCloseTable(this.sesid, this.tableid);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);
            Api.JetOpenTable(this.sesid, this.dbid, this.table, null, 0, OpenTableGrbit.None, out this.tableid);
        }

        /// <summary>
        /// Cleanup after all tests have run.
        /// </summary>
        [TestCleanup]
        [Description("Cleanup for BasicDDLTests")]
        public void Teardown()
        {
            Api.JetCloseTable(this.sesid, this.tableid);
            Api.JetEndSession(this.sesid, EndSessionGrbit.None);
            Api.JetTerm(this.instance);
            Cleanup.DeleteDirectoryWithRetry(this.directory);
        }

        /// <summary>
        /// Verify that BasicDDLTests has setup the test fixture properly.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Verify that BasicDDLTests has setup the test fixture properly")]
        public void VerifyFixtureSetup()
        {
            Assert.IsNotNull(this.table);
            Assert.AreNotEqual(JET_INSTANCE.Nil, this.instance);
            Assert.AreNotEqual(JET_SESID.Nil, this.sesid);
            Assert.AreNotEqual(JET_DBID.Nil, this.dbid);
            Assert.AreNotEqual(JET_TABLEID.Nil, this.tableid);
            Assert.AreNotEqual(JET_COLUMNID.Nil, this.testColumnid);

            JET_COLUMNDEF columndef;
            Api.JetGetTableColumnInfo(this.sesid, this.tableid, this.testColumnid, out columndef);
            Assert.AreEqual(JET_coltyp.LongText, columndef.coltyp);
        }

        #endregion Setup/Teardown

        #region DDL Tests

        /// <summary>
        /// Creates an index with JetCreateIndex4.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates an index with JetCreateIndex4")]
        public void JetCreateIndex4()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            Api.JetBeginTransaction(this.sesid);

            const string IndexName = "another_index";
            const string IndexDescription = "-TestColumn\0\0";

            var indexcreate = new JET_INDEXCREATE
            {
                szIndexName = IndexName,
                szKey = IndexDescription,
                cbKey = IndexDescription.Length,
                grbit = CreateIndexGrbit.IndexIgnoreAnyNull,
                ulDensity = 100,
            };
            Windows8Api.JetCreateIndex4(this.sesid, this.tableid, new[] { indexcreate }, 1);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);

            Api.JetSetCurrentIndex(this.sesid, this.tableid, IndexName);
        }

        /// <summary>
        /// Creates an index with JetCreateIndex4 and set Jet_UNICODEINDEX2.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates an index with JetCreateIndex4 and set JET_UNICODEINDEX2.")]
        public void JetCreateIndex4UnicodeIndex2()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            Api.JetBeginTransaction(this.sesid);

            const string IndexName = "another_index";
            const string IndexDescription = "-TestColumn\0\0";
            const string LocaleName = "en-US";

            var unicode = new JET_UNICODEINDEX()
            {
                szLocaleName = LocaleName, 
                dwMapFlags = Conversions.LCMapFlagsFromCompareOptions(CompareOptions.None),  
            };

            var indexcreate = new JET_INDEXCREATE
            {
                szIndexName = IndexName,
                szKey = IndexDescription,
                cbKey = IndexDescription.Length,
                pidxUnicode = unicode,
                grbit = CreateIndexGrbit.IndexIgnoreAnyNull,
                ulDensity = 100,
            };
            Windows8Api.JetCreateIndex4(this.sesid, this.tableid, new[] { indexcreate }, 1);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);

            Api.JetSetCurrentIndex(this.sesid, this.tableid, IndexName);
        }

        /// <summary>
        /// Verifies the locale set with JET_UNICODEINDEX2 is correct with JetGetIndexInfo and JetGetTableIndexInfo.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates an index with JetCreateIndex4 and set JET_UNICODEINDEX2.")]
        public void VerifyJetIndexInfoLocaleName()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            Api.JetBeginTransaction(this.sesid);

            const string IndexName = "localizedIndex";
            const string IndexDescription = "-TestColumn\0\0";
            const string LocaleName = "pt-bR";

            var unicode = new JET_UNICODEINDEX()
            {
                szLocaleName = LocaleName,
                dwMapFlags = Conversions.LCMapFlagsFromCompareOptions(CompareOptions.None),
            };

            var indexcreate = new JET_INDEXCREATE
            {
                szIndexName = IndexName,
                szKey = IndexDescription,
                cbKey = IndexDescription.Length,
                pidxUnicode = unicode,
                grbit = CreateIndexGrbit.IndexIgnoreAnyNull,
                ulDensity = 100,
            };
            Windows8Api.JetCreateIndex4(this.sesid, this.tableid, new[] { indexcreate }, 1);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);

            string localeNameOut;
            Api.JetGetIndexInfo(this.sesid, this.dbid, this.table, IndexName, out localeNameOut, Windows8IdxInfo.LocaleName);
            Assert.IsNotNull(localeNameOut);
            Assert.AreEqual(LocaleName, localeNameOut, true);

            localeNameOut = null;
            Api.JetGetTableIndexInfo(this.sesid, this.tableid, IndexName, out localeNameOut, Windows8IdxInfo.LocaleName);
            Assert.IsNotNull(localeNameOut);
            Assert.AreEqual(LocaleName, localeNameOut, true);

            Api.JetSetCurrentIndex(this.sesid, this.tableid, IndexName);
        }

        /// <summary>
        /// Creates an index with JetCreateIndex4 and space hints.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates an index with JetCreateIndex4 and space hints.")]
        public void JetCreateIndex4SpaceHints()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            Api.JetBeginTransaction(this.sesid);

            const string IndexName = "another_index";
            const string IndexDescription = "-TestColumn\0\0";

            var spacehintsIndex = new JET_SPACEHINTS()
            {
                ulInitialDensity = 33,
                cbInitial = 4096,
                grbit = SpaceHintsGrbit.CreateHintAppendSequential | SpaceHintsGrbit.RetrieveHintTableScanForward,
                ulMaintDensity = 44,
                ulGrowth = 144,
                cbMinExtent = 1024 * 1024,
                cbMaxExtent = 3 * 1024 * 1024,
            };

            var indexcreate = new JET_INDEXCREATE
            {
                szIndexName = IndexName,
                szKey = IndexDescription,
                cbKey = IndexDescription.Length,
                grbit = CreateIndexGrbit.IndexIgnoreAnyNull,
                pSpaceHints = spacehintsIndex,
            };
            Windows8Api.JetCreateIndex4(this.sesid, this.tableid, new[] { indexcreate }, 1);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);

            Api.JetSetCurrentIndex(this.sesid, this.tableid, IndexName);
        }

        /// <summary>
        /// Creates an index with JetCreateIndex4 and space hints.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates an index with JetCreateIndex4 and space hints and unicodeindex2.")]
        public void JetCreateIndex4SpaceHintsUnicodeIndex2()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            Api.JetBeginTransaction(this.sesid);

            const string IndexName = "another_index";
            const string IndexDescription = "-TestColumn\0\0";
            const string LocaleName = "en-US";

            var spacehintsIndex = new JET_SPACEHINTS()
            {
                ulInitialDensity = 33,
                cbInitial = 4096,
                grbit = SpaceHintsGrbit.CreateHintAppendSequential | SpaceHintsGrbit.RetrieveHintTableScanForward,
                ulMaintDensity = 44,
                ulGrowth = 144,
                cbMinExtent = 1024 * 1024,
                cbMaxExtent = 3 * 1024 * 1024,
            };

            var unicode = new JET_UNICODEINDEX()
            {
                szLocaleName = LocaleName, 
                dwMapFlags = Conversions.LCMapFlagsFromCompareOptions(CompareOptions.None),  
            };
            
            var indexcreate = new JET_INDEXCREATE
            {
                szIndexName = IndexName,
                szKey = IndexDescription,
                cbKey = IndexDescription.Length,
                pidxUnicode = unicode,
                grbit = CreateIndexGrbit.IndexIgnoreAnyNull,
                pSpaceHints = spacehintsIndex,
            };
            Windows8Api.JetCreateIndex4(this.sesid, this.tableid, new[] { indexcreate }, 1);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);

            Api.JetSetCurrentIndex(this.sesid, this.tableid, IndexName);
        }

        /// <summary>
        /// Creates two indexes using JetCreateIndex4.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates two indexes using JetCreateIndex4")]
        public void CreateTwoIndexesUsingJetCreateIndex4()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            JET_TABLEID tableToIndex;

            Api.JetBeginTransaction(this.sesid);
            Api.JetCreateTable(this.sesid, this.dbid, "tabletoindex", 1, 100, out tableToIndex);

            var columndef = new JET_COLUMNDEF()
            {
                cp = JET_CP.Unicode,
                coltyp = JET_coltyp.LongText,
            };
            Api.JetAddColumn(this.sesid, tableToIndex, "column", columndef, null, 0, out this.testColumnid);

            Api.JetCloseTable(this.sesid, tableToIndex);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);

            Api.JetOpenTable(this.sesid, this.dbid, "tabletoindex", null, 0, OpenTableGrbit.DenyRead, out tableToIndex);
            const string Index1Name = "firstIndex";
            const string Index1Description = "-column\0\0";

            const string Index2Name = "secondIndex";
            const string Index2Description = "+column\0\0";

            var indexcreates = new[]
            {
                new JET_INDEXCREATE
                {
                    szIndexName = Index1Name,
                    szKey = Index1Description,
                    cbKey = Index1Description.Length,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 100,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index2Name,
                    szKey = Index2Description,
                    cbKey = Index2Description.Length,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 100,
                },
            };
            Windows8Api.JetCreateIndex4(this.sesid, tableToIndex, indexcreates, indexcreates.Length);

            Api.JetSetCurrentIndex(this.sesid, tableToIndex, Index1Name);
            Api.JetSetCurrentIndex(this.sesid, tableToIndex, Index2Name);
            Api.JetSetCurrentIndex(this.sesid, tableToIndex, null);
            Api.JetCloseTable(this.sesid, tableToIndex);
        }

        /// <summary>
        /// Creates a table, two columns, and two indexes using JetCreateTableColumnIndex4.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates a table, two columns, and two indexes using JetCreateTableColumnIndex4")]
        public void JetCreateTableColumnIndex4()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var columncreates = new JET_COLUMNCREATE[]
            {
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col1_short",
                    coltyp = JET_coltyp.Short,
                    cbMax = 2,
                },
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col2_longtext",
                    coltyp = JET_coltyp.LongText,
                    cp = JET_CP.Unicode,
                },
            };

            const string Index1Name = "firstIndex";
            const string Index1Description = "+col1_short\0-col2_longtext\0";

            const string Index2Name = "secondIndex";
            const string Index2Description = "+col2_longtext\0-col1_short\0";

            var indexcreates = new JET_INDEXCREATE[]
            {
                  new JET_INDEXCREATE
                {
                    szIndexName = Index1Name,
                    szKey = Index1Description,
                    cbKey = Index1Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index2Name,
                    szKey = Index2Description,
                    cbKey = Index2Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 79,
                },
            };

            var tablecreate = new JET_TABLECREATE()
            {
                szTableName = "tableBigBang",
                ulPages = 23,
                ulDensity = 75,
                cColumns = columncreates.Length,
                rgcolumncreate = columncreates,
                rgindexcreate = indexcreates,
                cIndexes = indexcreates.Length,
                cbSeparateLV = 100,
                cbtyp = JET_cbtyp.Null,
                grbit = CreateTableColumnIndexGrbit.None,
            };

            Api.JetBeginTransaction(this.sesid);
            Windows8Api.JetCreateTableColumnIndex4(this.sesid, this.dbid, tablecreate);

            var tableCreated = new JET_TABLEID()
            {
                Value = tablecreate.tableid.Value
            };

            Assert.AreNotEqual<JET_TABLEID>(JET_TABLEID.Nil, tableCreated);

            // 1 table, 2 columns, 2 indices = 5 objects.
            Assert.AreEqual<int>(tablecreate.cCreated, 5);

            Assert.AreNotEqual(tablecreate.rgcolumncreate[0].columnid, JET_COLUMNID.Nil);
            Assert.AreNotEqual(tablecreate.rgcolumncreate[1].columnid, JET_COLUMNID.Nil);

            Api.JetCloseTable(this.sesid, tableCreated);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);
        }

        /// <summary>
        /// Creates a table, two columns, and two indexes using JetCreateTableColumnIndex4.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates a table, two columns, and two indexes using JetCreateTableColumnIndex4 and set UnicodeIndex2")]
        public void JetCreateTableColumnIndex4UnicodeIndex2()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            const string LocaleName = "en-US";

            var unicode = new JET_UNICODEINDEX()
            {
                szLocaleName = LocaleName, 
                dwMapFlags = Conversions.LCMapFlagsFromCompareOptions(CompareOptions.None),  
            };

            var columncreates = new JET_COLUMNCREATE[]
            {
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col1_short",
                    coltyp = JET_coltyp.Short,
                    cbMax = 2,
                },
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col2_longtext",
                    coltyp = JET_coltyp.LongText,
                    cp = JET_CP.Unicode,
                },
            };

            const string Index1Name = "firstIndex";
            const string Index1Description = "+col1_short\0-col2_longtext\0";

            const string Index2Name = "secondIndex";
            const string Index2Description = "+col2_longtext\0-col1_short\0";

            var indexcreates = new JET_INDEXCREATE[]
            {
                  new JET_INDEXCREATE
                {
                    szIndexName = Index1Name,
                    szKey = Index1Description,
                    cbKey = Index1Description.Length + 1,
                    pidxUnicode = unicode,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index2Name,
                    szKey = Index2Description,
                    cbKey = Index2Description.Length + 1,
                    pidxUnicode = unicode,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 79,
                },
            };

            var tablecreate = new JET_TABLECREATE()
            {
                szTableName = "tableBigBang",
                ulPages = 23,
                ulDensity = 75,
                cColumns = columncreates.Length,
                rgcolumncreate = columncreates,
                rgindexcreate = indexcreates,
                cIndexes = indexcreates.Length,
                cbSeparateLV = 100,
                cbtyp = JET_cbtyp.Null,
                grbit = CreateTableColumnIndexGrbit.None,
            };

            Api.JetBeginTransaction(this.sesid);
            Windows8Api.JetCreateTableColumnIndex4(this.sesid, this.dbid, tablecreate);

            var tableCreated = new JET_TABLEID()
            {
                Value = tablecreate.tableid.Value
            };

            Assert.AreNotEqual<JET_TABLEID>(JET_TABLEID.Nil, tableCreated);

            // 1 table, 2 columns, 2 indices = 5 objects.
            Assert.AreEqual<int>(tablecreate.cCreated, 5);

            Assert.AreNotEqual(tablecreate.rgcolumncreate[0].columnid, JET_COLUMNID.Nil);
            Assert.AreNotEqual(tablecreate.rgcolumncreate[1].columnid, JET_COLUMNID.Nil);

            Api.JetCloseTable(this.sesid, tableCreated);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);
        }

        /// <summary>
        /// Creates a template table, two columns, and two indexes using JetCreateTableColumnIndex4.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates a template table, two columns, and two indexes using JetCreateTableColumnIndex4")]
        public void JetCreateTemplateTableColumnIndex4()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var columncreates = new JET_COLUMNCREATE[]
            {
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col1_short",
                    coltyp = JET_coltyp.Short,
                    cbMax = 2,
                },
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col2_longtext",
                    coltyp = JET_coltyp.LongText,
                    cp = JET_CP.Unicode,
                },
            };

            const string Index1Name = "firstIndex";
            const string Index1Description = "+col1_short\0-col2_longtext\0";

            const string Index2Name = "secondIndex";
            const string Index2Description = "+col2_longtext\0-col1_short\0";

            var indexcreates = new JET_INDEXCREATE[]
            {
                  new JET_INDEXCREATE
                {
                    szIndexName = Index1Name,
                    szKey = Index1Description,
                    cbKey = Index1Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index2Name,
                    szKey = Index2Description,
                    cbKey = Index2Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 79,
                },
            };

            var tablecreateTemplate = new JET_TABLECREATE()
            {
                szTableName = "tableOld",
                ulPages = 23,
                ulDensity = 75,
                cColumns = columncreates.Length,
                rgcolumncreate = columncreates,
                rgindexcreate = indexcreates,
                cIndexes = indexcreates.Length,
                cbSeparateLV = 100,
                cbtyp = JET_cbtyp.Null,
                grbit = CreateTableColumnIndexGrbit.TemplateTable,
            };

            Api.JetBeginTransaction(this.sesid);
            Windows8Api.JetCreateTableColumnIndex4(this.sesid, this.dbid, tablecreateTemplate);

            var tableCreated = new JET_TABLEID()
            {
                Value = tablecreateTemplate.tableid.Value
            };

            Assert.AreNotEqual<JET_TABLEID>(JET_TABLEID.Nil, tableCreated);

            // 1 table, 2 columns, 2 indices = 5 objects.
            Assert.AreEqual<int>(tablecreateTemplate.cCreated, 5);

            Assert.AreNotEqual(tablecreateTemplate.rgcolumncreate[0].columnid, JET_COLUMNID.Nil);
            Assert.AreNotEqual(tablecreateTemplate.rgcolumncreate[1].columnid, JET_COLUMNID.Nil);

            var tablecreateChild = new JET_TABLECREATE()
            {
                szTableName = "tableNew",
                szTemplateTableName = "tableOld",
                ulPages = 23,
                ulDensity = 75,
                rgcolumncreate = null,
                cColumns = 0,
                rgindexcreate = null,
                cIndexes = 0,
                cbSeparateLV = 100,
                cbtyp = JET_cbtyp.Null,
                grbit = CreateTableColumnIndexGrbit.None,
            };

            Windows8Api.JetCreateTableColumnIndex4(this.sesid, this.dbid, tablecreateChild);

            var tableidChild = new JET_TABLEID()
            {
                Value = tablecreateChild.tableid.Value
            };

            Assert.AreNotEqual<JET_TABLEID>(JET_TABLEID.Nil, tableidChild);

            // 1 table = 1 object
            Assert.AreEqual<int>(tablecreateChild.cCreated, 1);

            Api.JetCloseTable(this.sesid, tableCreated);
            Api.JetCloseTable(this.sesid, tableidChild);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);
        }

        /// <summary>
        /// Creates a table, two columns, and two indexes using JetCreateTableColumnIndex4 and Space Hints.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates a table, two columns, and two indexes using JetCreateTableColumnIndex4 and Space Hints.")]
        public void JetCreateTableColumnIndex4SpaceHints()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var columncreates = new[]
            {
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col1_short",
                    coltyp = JET_coltyp.Short,
                    cbMax = 2,
                    pvDefault = BitConverter.GetBytes((short)37),
                    cbDefault = 2,
                },
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col2_longtext",
                    coltyp = JET_coltyp.LongText,
                    cp = JET_CP.Unicode,
                },
            };

            const string Index1Name = "firstIndex";
            const string Index1Description = "+col1_short\0-col2_longtext\0";

            const string Index2Name = "secondIndex";
            const string Index2Description = "+col2_longtext\0-col1_short\0";

            var spacehintsIndex = new JET_SPACEHINTS()
            {
                ulInitialDensity = 33,
                cbInitial = 4096,
                grbit = SpaceHintsGrbit.CreateHintAppendSequential | SpaceHintsGrbit.RetrieveHintTableScanForward,
                ulMaintDensity = 44,
                ulGrowth = 144,
                cbMinExtent = 1024 * 1024,
                cbMaxExtent = 3 * 1024 * 1024,
            };

            var spacehintsSeq = new JET_SPACEHINTS()
            {
                ulInitialDensity = 33,
                cbInitial = 4096,
                grbit = SpaceHintsGrbit.CreateHintAppendSequential | SpaceHintsGrbit.RetrieveHintTableScanForward,
                ulMaintDensity = 44,
                ulGrowth = 144,
                cbMinExtent = 1024 * 1024,
                cbMaxExtent = 3 * 1024 * 1024,
            };

            var spacehintsLv = new JET_SPACEHINTS()
            {
                ulInitialDensity = 33,
                cbInitial = 4096,
                grbit = SpaceHintsGrbit.CreateHintAppendSequential | SpaceHintsGrbit.RetrieveHintTableScanForward,
                ulMaintDensity = 44,
                ulGrowth = 144,
                cbMinExtent = 1024 * 1024,
                cbMaxExtent = 3 * 1024 * 1024,
            };

            var indexcreates = new JET_INDEXCREATE[]
            {
                new JET_INDEXCREATE
                {
                    szIndexName = Index1Name,
                    szKey = Index1Description,
                    cbKey = Index1Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                    pSpaceHints = spacehintsIndex,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index2Name,
                    szKey = Index2Description,
                    cbKey = Index2Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 79,
                },
            };

            var tablecreate = new JET_TABLECREATE()
            {
                szTableName = "tableBigBang",
                ulPages = 23,
                ulDensity = 75,
                cColumns = columncreates.Length,
                rgcolumncreate = columncreates,
                rgindexcreate = indexcreates,
                cIndexes = indexcreates.Length,
                cbSeparateLV = 100,
                cbtyp = JET_cbtyp.Null,
                grbit = CreateTableColumnIndexGrbit.None,
                pSeqSpacehints = spacehintsSeq,
                pLVSpacehints = spacehintsLv,
            };

            Api.JetBeginTransaction(this.sesid);
            Windows8Api.JetCreateTableColumnIndex4(this.sesid, this.dbid, tablecreate);

            var tableCreated = new JET_TABLEID()
            {
                Value = tablecreate.tableid.Value
            };

            Assert.AreNotEqual<JET_TABLEID>(JET_TABLEID.Nil, tableCreated);

            // 1 table, 2 columns, 2 indices = 5 objects.
            Assert.AreEqual<int>(tablecreate.cCreated, 5);

            Api.JetCloseTable(this.sesid, tableCreated);
            Api.JetCommitTransaction(this.sesid, CommitTransactionGrbit.LazyFlush);
        }

        /// <summary>
        /// Creates a table, two columns, and three indexes (one with an invalid name)  using JetCreateTableColumnIndex4.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [Description("Creates a table, two columns, and three indexes (one with an invalid name) using JetCreateTableColumnIndex4")]
        public void JetCreateTableColumnIndex4WithInvalidIndexName()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            var columncreates = new JET_COLUMNCREATE[]
            {
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col1_short",
                    coltyp = JET_coltyp.Short,
                    cbMax = 2,
                },
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col2_longtext",
                    coltyp = JET_coltyp.LongText,
                    cp = JET_CP.Unicode,
                },
                new JET_COLUMNCREATE()
                {
                    szColumnName = "col1_short2",
                    coltyp = JET_coltyp.Short,
                    cbMax = 2,
                },
            };

            const string Index1Name = "firstIndex";
            const string Index1Description = "+col1_short\0-col2_longtext\0";

            const string Index2Name = "secondIndex";
            const string Index2Description = "+col2_longtext\0-col1_short\0";

            const string Index3Name = "[BAD!NAME]";
            const string Index3Description = "+col1_short2\0-col2_longtext\0";

            var indexcreates = new JET_INDEXCREATE[]
            {
                new JET_INDEXCREATE
                {
                    szIndexName = Index1Name,
                    szKey = Index1Description,
                    cbKey = Index1Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index2Name,
                    szKey = Index2Description,
                    cbKey = Index2Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                },
                new JET_INDEXCREATE
                {
                    szIndexName = Index3Name,
                    szKey = Index3Description,
                    cbKey = Index3Description.Length + 1,
                    grbit = CreateIndexGrbit.None,
                    ulDensity = 99,
                },
            };

            var tablecreate = new JET_TABLECREATE()
            {
                szTableName = "tableBigBang",
                ulPages = 23,
                ulDensity = 75,
                cColumns = columncreates.Length,
                rgcolumncreate = columncreates,
                rgindexcreate = indexcreates,
                cIndexes = indexcreates.Length,
                cbSeparateLV = 100,
                cbtyp = JET_cbtyp.Null,
                grbit = CreateTableColumnIndexGrbit.None,
            };

            Api.JetBeginTransaction(this.sesid);

            bool hitException = false;

            try
            {
                Windows8Api.JetCreateTableColumnIndex4(this.sesid, this.dbid, tablecreate);
            }
            catch (EsentInvalidNameException)
            {
                hitException = true;
            }
            
            Assert.IsTrue(hitException);
            Assert.AreEqual(JET_err.Success, tablecreate.rgindexcreate[0].err);
            Assert.AreEqual(JET_err.Success, tablecreate.rgindexcreate[1].err);
            Assert.AreEqual(JET_err.InvalidName, tablecreate.rgindexcreate[2].err);

            Api.JetRollback(this.sesid, RollbackTransactionGrbit.None);
        }

        /// <summary>
        /// Sort data with a temporary table
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [Description("Sort case-sensitive with JetOpenTemporaryTable2")]
        public void SortDataCaseSensitiveWithJetOpenTemporaryTable2()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            const string LocaleName = "fr-FR";

            var columns = new[]
            {
                new JET_COLUMNDEF { coltyp = JET_coltyp.Text, cp = JET_CP.Unicode, grbit = ColumndefGrbit.TTKey },
            };
            var columnids = new JET_COLUMNID[columns.Length];

            var idxunicode = new JET_UNICODEINDEX
            {
                dwMapFlags = Conversions.LCMapFlagsFromCompareOptions(CompareOptions.None),
                szLocaleName = LocaleName, 
            };

            var opentemporarytable = new JET_OPENTEMPORARYTABLE
            {
                cbKeyMost = SystemParameters.KeyMost,
                ccolumn = columns.Length,
                grbit = TempTableGrbit.Scrollable,
                pidxunicode = idxunicode,
                prgcolumndef = columns,
                prgcolumnid = columnids,
            };
            Windows8Api.JetOpenTemporaryTable2(this.sesid, opentemporarytable);

            var data = new[] { "g", "a", "A", "aa", "x", "b", "X" };
            foreach (string s in data)
            {
                using (var update = new Update(this.sesid, opentemporarytable.tableid, JET_prep.Insert))
                {
                    Api.SetColumn(this.sesid, opentemporarytable.tableid, columnids[0], s, Encoding.Unicode);
                    update.Save();
                }
            }

            Array.Sort(data, new CultureInfo(LocaleName).CompareInfo.Compare);
            CollectionAssert.AreEqual(
                data, this.RetrieveAllRecordsAsString(opentemporarytable.tableid, columnids[0]).ToArray());
            Api.JetCloseTable(this.sesid, opentemporarytable.tableid);
        }

        /// <summary>
        /// Sort with different locales with JetOpenTemporaryTable2.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [Description("Sort with different locales with JetOpenTemporaryTable2")]
        public void SortDataDifferentLocalesWithJetOpenTemporaryTable2()
        {
            if (!EsentVersion.SupportsWindows8Features)
            {
                return;
            }

            CultureInfo cultureInfo = new CultureInfo("es-ES_tradnl");
            string localeName = cultureInfo.CompareInfo.Name;
            Assert.AreEqual("es-ES_tradnl", localeName);

            var columns = new[]
            {
                new JET_COLUMNDEF { coltyp = JET_coltyp.Text, cp = JET_CP.Unicode, grbit = ColumndefGrbit.TTKey },
            };
            var columnids = new JET_COLUMNID[columns.Length];

            var idxunicode = new JET_UNICODEINDEX
            {
                szLocaleName = localeName,
            };

            var opentemporarytable = new JET_OPENTEMPORARYTABLE
            {
                cbKeyMost = SystemParameters.KeyMost,
                ccolumn = columns.Length,
                grbit = TempTableGrbit.Scrollable,
                pidxunicode = idxunicode,
                prgcolumndef = columns,
                prgcolumnid = columnids,
            };
            Windows8Api.JetOpenTemporaryTable2(this.sesid, opentemporarytable);

            // Note that es-ES_tradnl sorts differently than English.
            var data = new[] { "canary", "cocoa", "chicken", "bad!" };
            foreach (string s in data)
            {
                using (var update = new Update(this.sesid, opentemporarytable.tableid, JET_prep.Insert))
                {
                    Api.SetColumn(this.sesid, opentemporarytable.tableid, columnids[0], s, Encoding.Unicode);
                    update.Save();
                }
            }

            Array.Sort(data, new CultureInfo(localeName).CompareInfo.Compare);

            CollectionAssert.AreEqual(data, this.RetrieveAllRecordsAsString(opentemporarytable.tableid, columnids[0]).ToArray());
            Api.JetCloseTable(this.sesid, opentemporarytable.tableid);
        }
        #endregion DDL Tests

        #region Helper Methods

        /// <summary>
        /// Update the cursor and goto the returned bookmark.
        /// </summary>
        private void UpdateAndGotoBookmark()
        {
            var bookmark = new byte[SystemParameters.BookmarkMost];
            int bookmarkSize;
            Api.JetUpdate(this.sesid, this.tableid, bookmark, bookmark.Length, out bookmarkSize);
            Api.JetGotoBookmark(this.sesid, this.tableid, bookmark, bookmarkSize);
        }

        /// <summary>
        /// Enumerate all records and retrieve the specified column as a string.
        /// </summary>
        /// <param name="tableid">The table to enumerate.</param>
        /// <param name="columnid">The column to retrieve.</param>
        /// <returns>An enumeration of the column in all the records.</returns>
        private IEnumerable<string> RetrieveAllRecordsAsString(JET_TABLEID tableid, JET_COLUMNID columnid)
        {
            Api.MoveBeforeFirst(this.sesid, tableid);
            while (Api.TryMoveNext(this.sesid, tableid))
            {
                yield return Api.RetrieveColumnAsString(this.sesid, tableid, columnid);
            }
        }

        #endregion HelperMethods
    }
}