using ArchPM.Core.Exceptions;
using ArchPM.QueryBuilder;
using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArchPM.Core.Extensions;

namespace ArchPM.QueryBuilder
{
    public class TSqlQueryGenerator : IQueryGenerator
    {

        /// <summary>
        /// Gets or sets a value indicating whether [use star].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use star]; otherwise, <c>false</c>.
        /// </value>
        public Boolean UseStar { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IQueryGenerator" /> is showing query as pretty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if pretty; otherwise, <c>false</c>.
        /// </value>
        public Boolean Pretty { get; set; }
        /// <summary>
        /// Gets or sets the type of the creation.
        /// </summary>
        /// <value>
        /// The type of the creation.
        /// </value>
        public CreationTypes CreationType { get; set; }

        /// <summary>
        /// Gets the seperator.
        /// </summary>
        /// <value>
        /// The seperator.
        /// </value>
        String SEPERATOR
        {
            get
            {
                return Pretty ? "\r\n" : " ";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlQueryGenerator"/> class.
        /// </summary>
        public TSqlQueryGenerator()
        {
            this.CreationType = CreationTypes.WithValues;
        }

        /// <summary>
        /// Executes the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public string Execute(QBuilder builder)
        {

            String result = String.Empty;
            switch (builder.QueryType)
            {
                case QueryTypes.Select:
                    result = this.GenerateSelectQuery(builder);
                    break;
                case QueryTypes.Insert:
                    result = this.GenerateInsertQuery(builder);
                    break;
                case QueryTypes.Update:
                    result = this.GenerateUpdateQuery(builder);
                    break;
                default:
                    break;
            }

            //if (IsTraceActive)
            //{
            //    TraceHelper.WriteLine(result);
            //}

            return result;
        }


        /// <summary>
        /// Generates the select query.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        String GenerateSelectQuery(QBuilder builder)
        {
            //result
            StringBuilder sb = new StringBuilder();


            #region paging code sample
            /*

           Class	
               Repository.cs
           Method	
               Search
           Parameters	
               Skip: 50,
               Take: 10,
               OrderBy: p.ID,
               Direction: Desc
           Query	
               SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.ID Desc) AS trow, 
               t0.*
               FROM SY_BASVURU AS t0
               WHERE
               t0.STATUS = 1 AND t0.BASVURU_TIPI IN (1)) AS t99 
               WHERE
               t99.trow BETWEEN 50 AND 59
               ORDER BY t99.ID Desc
            */

            #endregion

            Action removeLastCommaIfExist = () =>
            {

                //remove last comma and seperator if exist
                if (sb[sb.Length - 1].ToString() == this.SEPERATOR && sb[sb.Length - 2] == ',')
                {
                    sb.Replace(String.Format(",{0}", this.SEPERATOR), "", sb.Length - 2, 2);
                    sb.Append(this.SEPERATOR);
                }

            };

            Action<List<IContentItem>> setFieldsByCommaSeperated = (list) =>
            {

                foreach (var item in list)
                {
                    if (item is FieldContentItem)
                    {
                        var field = item as FieldContentItem;
                        sb.AppendFormat("{0}.{1}", field.TableInfo.Alias, field.Value);
                        //set alias
                        if (!String.IsNullOrEmpty(field.Alias))
                        {
                            sb.AppendFormat(" AS [{0}]", field.Alias); //tip: must be space before AS. dont change with Seperator
                        }

                        //SEPERATOR
                        sb.Append(",");
                        sb.Append(this.SEPERATOR);
                    }
                   
                  
                }
                //remove last comma and seperator if exist
                removeLastCommaIfExist();
            };

            if (builder.PagingContent.Items.HasRecords())
            {
                //SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id Asc) AS trow,  t0.Id FROM Person AS t0 WHERE t0.Id = 1) AS t99  WHERE t99.trow BETWEEN 5 AND 15 ORDER BY t99.Id Asc
                //SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {t0.Id} {Asc}) AS trow,  {t0.Id} FROM {Person} AS t0 WHERE {t0.Id = 1}) AS t99  
                // WHERE t99.trow BETWEEN {5} AND {15} ORDER BY {t99.Id} {Asc}

                sb.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ");

                setFieldsByCommaSeperated(builder.PagingContent.Items);

                sb.AppendFormat("{0}) AS trow,", builder.PagingContent.Direction.GetDescription());
                sb.Append(this.SEPERATOR);


                foreach (var selectContent in builder.SelectContents)
                {
                    if (selectContent.Items.Count == 0) //no predicate entered
                    {
                        sb.Append(selectContent.IsUsingCountAggreate ? "COUNT(*) AS [COUNT]" : String.Format("{0}.*", selectContent.TableInfo.Alias));
                        sb.Append(this.SEPERATOR);
                    }
                    else
                    {
                        setFieldsByCommaSeperated(selectContent.Items);
                        sb.Append(",");
                        sb.Append(this.SEPERATOR);
                    }
                }
                removeLastCommaIfExist();
                //sb.Remove(sb.Length - 1, 1);
                sb.AppendFormat("FROM {0} AS {1}", builder.PagingContent.TableInfo.Name, builder.PagingContent.TableInfo.Alias);

                Int32 betweenStart = builder.PagingContent.Page * builder.PagingContent.PerPage;
                Int32 betweenEnd = betweenStart + builder.PagingContent.PerPage;

                GenerateWhereQuery(sb, builder);
                removeLastCommaIfExist();

                sb.AppendFormat(") AS t99 WHERE t99.trow BETWEEN {0} AND {1} ORDER BY ",
                        betweenStart,
                        betweenEnd);

                foreach (var item in builder.PagingContent.Items)
                {
                    var field = item as FieldContentItem;
                    if (field != null)
                    {
                        sb.AppendFormat("t99.{0} {1}", field.Value, builder.PagingContent.Direction.GetDescription());
                        //SEPERATOR
                        sb.Append(",");
                        sb.Append(this.SEPERATOR);
                    }
                }
                //remove last comma and seperator if exist
                removeLastCommaIfExist();



                //String searchOrderbyFields = builder.PagingContent.Items.ToQueryString();
                //String searchOrderbyFieldsOuter = builder.PagingContent.QueryStrings.ToQueryString("", false);
                //queryBuilder = String.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {1} {6}) AS trow, {2}) AS {3} {8}WHERE{8}{3}.trow BETWEEN {4} AND {5}{8}ORDER BY {3}.{7} {6}",
                //    fieldNames, //0
                //    searchOrderbyFields, //1 
                //    queryBuilder.Substring(6), //2 
                //    builder.PagingContent.TableInfo.Alias, //3 
                //    builder.PagingContent.Page * builder.PagingContent.PerPage, //4
                //    builder.PagingContent.Page * builder.PagingContent.PerPage, //4
                //    builder.PagingContent.Direction, //6
                //    searchOrderbyFieldsOuter, //7
                //    Seperator); //8
            }
            else
            {
                //SQL COMMAND
                sb.Append("SELECT");
                sb.Append(this.SEPERATOR);

                #region FIELDS
                //Field Names
                foreach (var selectContent in builder.SelectContents)
                {
                    if (selectContent.Items.Count == 0) //no predicate entered
                    {
                        sb.Append(selectContent.IsUsingCountAggreate ? "COUNT(*) AS [COUNT]" : String.Format("{0}.*", selectContent.TableInfo.Alias));
                        sb.Append(this.SEPERATOR);
                    }
                    else
                    {
                        //all items must be FieldContentItem because it is select Content
                        IEnumerable<FieldContentItem> fields = selectContent.Items.Cast<FieldContentItem>();
                        foreach (var field in fields)
                        {
                            String fieldName = String.Format("{0}.{1}", field.TableInfo.Alias, field.Value);
                            //COUNT aggreage
                            if (selectContent.IsUsingCountAggreate)
                            {
                                fieldName = String.Format("COUNT({0})", fieldName);
                                if (String.IsNullOrEmpty(field.Alias))
                                {
                                    field.Alias = "COUNT"; //tip:default value for count
                                }
                            }

                            //set fieldname
                            sb.Append(fieldName);
                            //set alias
                            if (!String.IsNullOrEmpty(field.Alias))
                            {
                                sb.AppendFormat(" AS [{0}]", field.Alias); //tip: must be space before AS. dont change with Seperator
                            }

                            //SEPERATOR
                            sb.Append(",");
                            sb.Append(this.SEPERATOR);
                        }
                    }
                }
                //remove last comma and seperator if exist
                removeLastCommaIfExist();


                #endregion

                //SQL COMMAND
                sb.Append("FROM");
                sb.Append(this.SEPERATOR);

                #region TABLE NAMES
                //Table Names
                var uniqueTableInfos = builder.SelectContents.Select(p => p.TableInfo).Distinct().ToList();

                //if join used, remove from FROM section
                if (builder.JoinContents.Count > 0)
                {
                    builder.JoinContents.ForEach(content =>
                    {
                        uniqueTableInfos.RemoveAll(p => p.Name == content.RightTableInfo.Name);
                    });
                }


                foreach (var tableInfo in uniqueTableInfos)
                {
                    //tablename with alias
                    sb.AppendFormat("{0} AS {1}", tableInfo.Name, tableInfo.Alias);

                    //add comma if it is not last element
                    if (tableInfo != uniqueTableInfos.Last())
                    {
                        sb.Append(",");
                        //SEPERATOR
                        sb.Append(this.SEPERATOR);
                    }

                }
                #endregion

                #region JOIN

                if (builder.JoinContents.HasRecords())
                {
                    //validation
                    builder.JoinContents.ForEach(content =>
                    {
                        if (builder.SelectContents.Count(p => p.TableInfo.Name == content.LeftTableInfo.Name) == 0)
                            throw new QueryBuilderException(String.Format("Select[{0}](... must be used before use Join[{0},{1}](...",
                                content.LeftTableInfo.Name, content.RightTableInfo.Name));

                        String tableNameWithAlias = String.Format("{0} AS {1}", content.RightTableInfo.Name, content.RightTableInfo.Alias);
                        String joinFieldNames = String.Empty;

                        foreach (var item in content.Items)
                        {
                            if (item is FieldContentItem)
                            {
                                joinFieldNames += String.Format("{0}.{1}", (item as FieldContentItem).TableInfo.Alias, item.Value);
                            }
                            if (item is OperatorContentItem)
                            {
                                joinFieldNames += " ";
                                joinFieldNames += item.Value;
                                joinFieldNames += " ";
                            }
                        }

                        sb.Append(this.SEPERATOR);
                        sb.Append(content.JoinDirection == JoinDirections.Right ? "RIGHT " : "LEFT ");
                        sb.Append(content.JoinType == JoinTypes.Outer ? "OUTER " : "");
                        sb.Append(String.Format("JOIN {0} ON {1}", tableNameWithAlias, joinFieldNames));
                    });

                }

                #endregion




                //WHERE
                GenerateWhereQuery(sb, builder);


                //ORDER BY
                if (builder.OrderByContents.Count > 0)
                {
                    var orderByString = String.Join(" ,", builder.OrderByContents.Select(p => String.Format("{0} {1}",
                        String.Join(" ", p.Items.Select(x => x.Value)), p.Direction.ToString())));
                    sb.Append(this.SEPERATOR);
                    sb.AppendFormat("ORDER BY");
                    sb.Append(this.SEPERATOR);
                    sb.Append(orderByString);

                }
            }

            String result = sb.ToString().TrimEnd();
            return result;
        }

        /// <summary>
        /// Generates the where query.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="builder">The builder.</param>
        void GenerateWhereQuery(StringBuilder sb, QBuilder builder)
        {
            #region WHERE
            if (builder.WhereContent.Items.Count > 0)
            {
                sb.Append(this.SEPERATOR);  //tip: sperator required here. dont touch
                sb.Append("WHERE");
                sb.Append(this.SEPERATOR);

                builder.WhereContent.Items.ForEach(p =>
                {

                    Object value = p.Value;
                    if (p is ValueContentItem)
                    {
                        value = valueCorrelation(value);
                    }
                    else if (p is FieldContentItem)
                    {
                        var field = p as FieldContentItem;
                        value = String.Format("{0}.{1}", field.TableInfo.Alias, field.Value);
                        if (!String.IsNullOrEmpty(field.Alias))
                        {
                            value += String.Format("AS [{0}]", field.Alias);
                        }

                    }
                    sb.Append(value);

                    //if next item is also value, add comma
                    Int32 currentIndex = builder.WhereContent.Items.FindIndex(x => x == p);
                    Int32 nextItemIndex = currentIndex + 1;
                    if (builder.WhereContent.Items.Count > nextItemIndex) //next item exist
                    {
                        var nextItem = builder.WhereContent.Items[nextItemIndex]; //found next item
                        if (p is ValueContentItem && nextItem is ValueContentItem)
                        {
                            sb.Append(",");
                        }

                        //add space between these content items
                        var spaceCheck = (p is OperatorContentItem && nextItem is BlockContentItem)
                                      || (p is FieldContentItem && nextItem is OperatorContentItem)
                                      || (p is OperatorContentItem && nextItem is ValueContentItem)
                                      || (p is ValueContentItem && nextItem is OperatorContentItem)
                                      || (p is OperatorContentItem && nextItem is FieldContentItem);

                        if (spaceCheck)
                        {
                            sb.Append(" ");
                        }
                    }


                });

                //if last char is space, remove it
                if (sb[sb.Length - 1] == ' ')
                {
                    sb.Remove(sb.Length - 1, 1);
                }

            }
            #endregion
        }


        /// <summary>
        /// Generates the insert query.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        String GenerateInsertQuery(QBuilder builder)
        {
            List<String> fieldNameList = new List<String>();
            List<Object> tempFieldValueList = new List<Object>();

            builder.InsertContent.Fields.ForEach((p) =>
            {
                p.ConvertEnumValueToInt32OnPropertyValue();

                fieldNameList.Add(p.Name);

                if (this.CreationType == CreationTypes.WithValues)
                {
                    var val = valueCorrelation(p.Value);
                    tempFieldValueList.Add(val);
                }
                else //with parameters
                {
                    tempFieldValueList.Add(String.Format("@{0}", p.Name));
                }
            });


            String fieldNames = String.Join(",", fieldNameList);
            String fieldValues = String.Join(",", tempFieldValueList);
            String query = String.Format("INSERT INTO {0}{3}({1}){3}VALUES{3}({2});", builder.InsertContent.TableInfo.Name,
                fieldNames, fieldValues, this.SEPERATOR);

            if (builder.InsertContent.ReturningScopeIdentity)
            {
                query = String.Concat(query, SEPERATOR + "SELECT SCOPE_IDENTITY();");
            }

            return query;
        }


        /// <summary>
        /// Generates the update query.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        String GenerateUpdateQuery(QBuilder builder)
        {
            List<String> tempFieldAndValue = new List<String>();

            foreach (var prop in builder.UpdateContent.Fields)
            {
                if (!prop.IsPrimitive)
                    continue;

                if (this.CreationType == CreationTypes.WithValues)
                {
                    var val = String.Format("[{0}] = {1}", prop.Name, valueCorrelation(prop.Value));
                    tempFieldAndValue.Add(val);
                }
                else if (this.CreationType == CreationTypes.WithParameters)
                {
                    tempFieldAndValue.Add(String.Format("[{0}] = @{0}", prop.Name));
                }
            }
            String fieldAndValueString = String.Join(String.Format("{0},", this.SEPERATOR), tempFieldAndValue);
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE");
            sb.Append(this.SEPERATOR);
            sb.Append(builder.UpdateContent.TableInfo.Alias);
            sb.Append(this.SEPERATOR);
            sb.Append("SET");
            sb.Append(this.SEPERATOR);
            sb.Append(fieldAndValueString);
            sb.Append(this.SEPERATOR);
            sb.Append("FROM");
            sb.Append(this.SEPERATOR);
            sb.AppendFormat("{0} AS {1}", builder.UpdateContent.TableInfo.Name, builder.UpdateContent.TableInfo.Alias);
            GenerateWhereQuery(sb, builder);

            var query = sb.ToString();
            return query;

        }



        #region rammed
        //String GenerateWhereQuery(String query, Boolean withAlias = true)
        //    {
        //        if (builder.WhereContent.Items.HasRecords())
        //        {
        //            //builder.WhereContent.Items.ForEach(x =>
        //            //{
        //            //    if (x is ValueContentItem)
        //            //    {
        //            //        x.TableName = tableInfoList.correctTableName(x.TableName);
        //            //    }
        //            //});


        //            //var whereString = builder.WhereContent.Items.ToQueryString("", withAlias);
        //            //whereString = whereString.Replace("\"", "\'")
        //            //                         .Replace("!= null", "IS NOT NULL")
        //            //                         .Replace("!= NULL", "IS NOT NULL")
        //            //                         .Replace("= null", "IS NULL")
        //            //                         .Replace("= NULL", "IS NULL");

        //            //query += String.Format("{0}WHERE{0}", this.Seperator);
        //            //query += whereString;
        //        }

        //        return query;
        //    }

        //    /// <summary>
        //    /// Gets the select query.
        //    /// </summary>
        //    /// <returns></returns>
        //    String GenerateSelectQuery()
        //    {
        //        List<String> tempTableNameWithAliasForFROM = new List<String>();
        //        List<String> tempFieldNameList = new List<String>();

        //        //preparing field and table areas
        //        foreach (var selectContent in builder.SelectContents)
        //        {
        //            //tableName and tableAlias correletion
        //            selectContent.TableName = tableInfoList.correctTableName(selectContent.TableName);
        //            selectContent.TableAlias = tableInfoList.GetTableAlias(selectContent.TableName);
        //            selectContent.QueryStrings.ForEach(p =>
        //            {
        //                p.TableName = selectContent.TableName;
        //                p.TableAlias = selectContent.TableAlias;
        //                if (!String.IsNullOrEmpty(p.Alias) && p.Text != p.Alias)
        //                    p.Text = String.Format("{0} AS {1}", p.Text, p.Alias);
        //            });


        //            var tempTableName = String.Format("{0} AS {1}", selectContent.TableName, selectContent.TableAlias);
        //            if (!tempTableNameWithAliasForFROM.Contains(tempTableName))
        //                tempTableNameWithAliasForFROM.Add(tempTableName);

        //            //if join used, remove from FROM section
        //            if (this.JoinContents.HasRecords())
        //            {
        //                this.JoinContents.ForEach(content =>
        //                {
        //                    String rightTableAlias = this.tableInfoList.GetTableAlias(content.RightTableName);
        //                    tempTableNameWithAliasForFROM.RemoveAll(p => p == String.Format("{0} AS {1}", content.RightTableName, rightTableAlias));
        //                });
        //            }

        //            //field names
        //            var tempFieldNames = selectContent.QueryStrings.ToQueryString(" ,");
        //            if (selectContent.IsUsingCountAggregateFunction)
        //            {
        //                if (selectContent.QueryStrings.Any(p => p.Text == "*"))
        //                    tempFieldNames = selectContent.QueryStrings.ToQueryString(" ,", false);
        //                tempFieldNames = String.Format("COUNT({0}) AS [{1}]", tempFieldNames, _countAlias);
        //            }
        //            tempFieldNameList.Add(tempFieldNames);

        //        }

        //        //set comma between them
        //        String fieldNames = String.Join(" ,", tempFieldNameList);
        //        String tableNames = String.Join(" ,", tempTableNameWithAliasForFROM);

        //        String queryBuilder = String.Format("SELECT{0}{1}{0}FROM {2}", this.Seperator, fieldNames, tableNames);


        //        if (builder.JoinContents.HasRecords())
        //        {
        //            //validation
        //            builder.JoinContents.ForEach(content =>
        //            {
        //                if (builder.SelectContents.Count(p => p.TableName == content.LeftTableName) == 0)
        //                    throw new QueryBuilderException("Select[{0}](... must be used before use Join[{0},{1}](...", content.LeftTableName, content.RightTableName);

        //                String rightTableAlias = this.tableInfoList.GetTableAlias(content.RightTableName);
        //                String tableNameWithAlias = String.Format("{0} AS {1}", content.RightTableName, rightTableAlias);
        //                var joinFieldNames = content.QueryStrings.ToQueryString();

        //                queryBuilder += Seperator;
        //                queryBuilder += content.JoinDirection == JoinDirections.Right ? "RIGHT " : "LEFT ";
        //                queryBuilder += content.JoinType == JoinTypes.Outer ? "OUTER " : "";
        //                queryBuilder += String.Format("JOIN {0} ON {1}", tableNameWithAlias, joinFieldNames);
        //            });

        //        }

        //        //WHERE CLAUSE
        //        queryBuilder = GenerateWhereQuery(queryBuilder);

        //        if (builder.OrderByContents.HasRecords())
        //        {
        //            var orderByString = String.Join(" ,", builder.OrderByContents.Select(p => String.Format("{0} {1}", p.Content, p.Direction.ToString())));
        //            queryBuilder += Seperator + "ORDER BY " + orderByString;
        //        }

        //        /*

        //        Class	
        //            Repository.cs
        //        Method	
        //            Search
        //        Parameters	
        //            Skip: 50,
        //            Take: 10,
        //            OrderBy: p.ID,
        //            Direction: Desc
        //        Query	
        //            SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.ID Desc) AS trow, 
        //            t0.*
        //            FROM SY_BASVURU AS t0
        //            WHERE
        //            t0.STATUS = 1 AND t0.BASVURU_TIPI IN (1)) AS t99 
        //            WHERE
        //            t99.trow BETWEEN 50 AND 59
        //            ORDER BY t99.ID Desc
        //         */

        //        if (builder.PagingContent.QueryStrings.HasRecords())
        //        {

        //            String searchOrderbyFields = builder.PagingContent.QueryStrings.ToQueryString();
        //            String searchOrderbyFieldsOuter = builder.PagingContent.QueryStrings.ToQueryString("", false);
        //            queryBuilder = String.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {1} {6}) AS trow, {2}) AS {3} {8}WHERE{8}{3}.trow BETWEEN {4} AND {5}{8}ORDER BY {3}.{7} {6}",
        //                fieldNames, //0
        //                searchOrderbyFields, //1 
        //                queryBuilder.Substring(6), //2 
        //                builder.PagingContent.TableAlias, //3 
        //                builder.PagingContent.Skip, //4
        //                builder.PagingContent.Skip + builder.PagingContent.Take, //5 50 between 59
        //                builder.PagingContent.Direction, //6
        //                searchOrderbyFieldsOuter, //7
        //                Seperator); //8
        //        }

        //        var query = queryBuilder.ToString();

        //        return query;
        //    }

        //    /// <summary>
        //    /// Gets the create query.
        //    /// </summary>
        //    /// <returns></returns>
        //    String GenerateCreateQuery()
        //    {
        //        String query = String.Format("CREATE TABLE {0} ({1});", this.CreateContent.TableName, this.CreateContent.Content);

        //        return query;

        //    }

        //    /// <summary>
        //    /// Gets the insert query.
        //    /// </summary>
        //    /// <returns></returns>
        //    String GenerateInsertQuery()
        //    {
        //        List<String> fieldNameList = new List<String>();
        //        List<Object> tempFieldValueList = new List<Object>();

        //        builder.InsertContent.Fields.ForEach((p) =>
        //        {
        //            p = p.CorrectForDatabase();

        //            fieldNameList.Add(p.Name);

        //            if (this.CreationType == CreationTypes.WithValues)
        //            {
        //                var val = this.rightHandsideValueCorrelation(p.Value);
        //                tempFieldValueList.Add(val);
        //            }
        //            else if (this.CreationType == CreationTypes.WithParameters)
        //            {
        //                tempFieldValueList.Add(String.Format("@{0}", p.Name));
        //            }
        //        });


        //        String fieldNames = String.Join(",", fieldNameList);
        //        String fieldValues = String.Join(",", tempFieldValueList);
        //        String query = String.Format("INSERT INTO {0}{3}({1}){3}VALUES{3}({2});", builder.InsertContent.TableName, fieldNames, fieldValues, Seperator);

        //        if (builder.InsertContent.ReturningScopeIdentity)
        //        {
        //            query = String.Concat(query, Seperator + "SELECT SCOPE_IDENTITY();");
        //        }

        //        return query;
        //    }

        //     /// <summary>
        //    /// Gets the update query.
        //    /// </summary>
        //    /// <returns></returns>
        //    String GenerateUpdateQuery()
        //    {
        //        List<String> tempFieldAndValue = new List<String>();

        //        foreach (var prop in builder.UpdateContent.Fields)
        //        {
        //            if (!prop.IsPrimitive)
        //                continue;

        //            if (this.CreationType == CreationTypes.WithValues)
        //            {
        //                var val = String.Format("[{0}] = {1}", prop.Name, DecisionMaker.Instance.rightHandsideValueCorrelation(prop.Value));
        //                tempFieldAndValue.Add(val);
        //            }
        //            else if (this.CreationType == CreationTypes.WithParameters)
        //            {
        //                tempFieldAndValue.Add(String.Format("[{0}] = @{0}", prop.Name));
        //            }
        //        }
        //        String fieldAndValueString = String.Join(String.Format("{0},", this.Seperator), tempFieldAndValue);
        //        String query = String.Format("UPDATE {0}{2}SET{2}{1}", builder.UpdateContent.TableName, fieldAndValueString, Seperator);

        //        query = GenerateWhereQuery(query, false);

        //        return query;

        //    }

        //    /// <summary>
        //    /// Gets the update query.
        //    /// </summary>
        //    /// <returns></returns>
        //    String GenerateDeleteQuery()
        //    {

        //        String queryBuilder = String.Format("DELETE FROM {0}", this.UpdateContent.TableName);
        //        queryBuilder = GenerateWhereQuery(queryBuilder);

        //        var query = queryBuilder;
        //        return query;
        //    } 
        #endregion


        /// <summary>
        /// Values the correlation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal String valueCorrelation(Object value)
        {
            String result = "NULL";

            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    result = String.Format("'{0}'", ((DateTime)value).ToDbDefaultString());
                }
                else if (value.GetType() == typeof(String))
                {
                    result = String.Format("'{0}'", value);
                }
                else if (value.GetType() == typeof(Boolean))
                {
                    result = String.Format("{0}", (Boolean)value ? "1" : "0");
                }
                else if (value.GetType() == typeof(Decimal))
                {
                    result = String.Format("'{0}'", ((Decimal)value).ToString(CultureInfo.InvariantCulture));
                }
                else if (value.GetType().IsEnumOrIsBaseEnum())
                {
                    result = Convert.ToInt32(value).ToString();
                }
                else
                {
                    result = value.ToString();
                }
            }

            return result;
        }
    }
}
