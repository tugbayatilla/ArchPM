using ArchPM.QueryBuilder.ExpressionHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ArchPM.Core.Extensions;
using ArchPM.Core.Exceptions;
using ArchPM.QueryBuilder.MethodCalls;
using ArchPM.QueryBuilder.Contents;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class QBuilder
    {
        #region Members
        
        readonly List<IExpressionHandler> registeredExpressions;
        readonly DecisionMaker decisionMaker;
        readonly TableInfoContainer tableInfoContainer; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QBuilder"/> class.
        /// </summary>
        public QBuilder()
            : this(new TSqlQueryGenerator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QBuilder"/> class.
        /// </summary>
        /// <param name="queryGenerator">The query generator.</param>
        public QBuilder(IQueryGenerator queryGenerator)
        {
            queryGenerator.NotNull("queryGenerator must be implemented!");

            this.QueryGenerator = queryGenerator;


            this.QueryType = QueryTypes.Select;

            this.registeredExpressions = new List<IExpressionHandler>();
            this.RegisteredMethodCalls = new List<IMethodCall>();
            this.registerExpressions();
            this.registerMethodCalls();

            this.decisionMaker = new DecisionMaker(this.registeredExpressions, this.RegisteredMethodCalls);
            this.tableInfoContainer = new TableInfoContainer();

            this.SelectContents = new List<SelectContent>();
            this.WhereContent = new WhereContent();
            this.JoinContents = new List<JoinContent>();
            this.UpdateContent = new UpdateContent();
            this.InsertContent = new InsertContent();
            this.PagingContent = new PagingContent();
            this.OrderByContents = new List<OrderByContent>();
            this.CreateContent = new CreateContent();
        } 
        #endregion

        
        
        #region Properties

        /// <summary>
        /// Gets or sets the query generator.
        /// </summary>
        /// <value>
        /// The query generator.
        /// </value>
        public IQueryGenerator QueryGenerator { get; set; }
        /// <summary>
        /// Gets or sets the type of the query.
        /// </summary>
        /// <value>
        /// The type of the query.
        /// </value>
        public QueryTypes QueryType { get; set; }
        /// <summary>
        /// Gets or sets the registered method calls.
        /// </summary>
        /// <value>
        /// The registered method calls.
        /// </value>
        public List<IMethodCall> RegisteredMethodCalls { get; set; }

        /// <summary>
        /// Gets or sets the select contents.
        /// </summary>
        /// <value>
        /// The select contents.
        /// </value>
        public List<SelectContent> SelectContents { get; set; }
        /// <summary>
        /// Gets or sets the content of the where.
        /// </summary>
        /// <value>
        /// The content of the where.
        /// </value>
        public WhereContent WhereContent { get; set; }
        /// <summary>
        /// Gets or sets the content of the update.
        /// </summary>
        /// <value>
        /// The content of the update.
        /// </value>
        public UpdateContent UpdateContent { get; set; }
        /// <summary>
        /// Gets or sets the content of the insert.
        /// </summary>
        /// <value>
        /// The content of the insert.
        /// </value>
        public InsertContent InsertContent { get; set; }
        /// <summary>
        /// Gets or sets the join contents.
        /// </summary>
        /// <value>
        /// The join contents.
        /// </value>
        public List<JoinContent> JoinContents { get; set; }
        /// <summary>
        /// Gets or sets the content of the paging.
        /// </summary>
        /// <value>
        /// The content of the paging.
        /// </value>
        public PagingContent PagingContent { get; set; }
        /// <summary>
        /// Gets or sets the order by contents.
        /// </summary>
        /// <value>
        /// The order by contents.
        /// </value>
        public List<OrderByContent> OrderByContents { get; set; }
        /// <summary>
        /// Gets or sets the content of the create.
        /// </summary>
        /// <value>
        /// The content of the create.
        /// </value>
        public CreateContent CreateContent { get; set; } 

        #endregion

        #region COUNT

        /// <summary>
        /// Counts the specified table name.
        /// </summary>
        /// <typeparam name="T">The type of the 1.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Count<T>(Expression<Func<T, Object>> predicate = null) where T : new()
        {
            var selectContent = createSelectContent(predicate);
            selectContent.IsUsingCountAggreate = true;

            this.SelectContents.Add(selectContent);
            return this;
        }

        #endregion

        #region SELECT
        /// <summary>
        /// Selects the specified table name.
        /// </summary>
        /// <typeparam name="T">The type of the 1.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Select<T>(Expression<Func<T, Object>> predicate = null) where T : new()
        {
            var selectContent = createSelectContent(predicate);

            this.SelectContents.Add(selectContent);
            return this;
        }

        /// <summary>
        /// Creates the content of the select.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        SelectContent createSelectContent<T>(Expression<Func<T, Object>> predicate = null) where T : new()
        {
            this.QueryType = QueryTypes.Select;

            SelectContent selectContent = new SelectContent();
            selectContent.TableInfo = tableInfoContainer.SetAlias(typeof(T).Name, "");

            this.decisionMaker.ExpressionHandle(selectContent.Items, predicate);

            UpdateTableInfo(selectContent.Items);

            //handling { p => null } senario
            if (selectContent.Items.Count == 1 && selectContent.Items.First() is ValueContentItem)
            {
                var nullValueContentItem = selectContent.Items.First() as ValueContentItem;
                if (nullValueContentItem.Value == null)
                    selectContent.Items.Remove(nullValueContentItem);
            }

            return selectContent;
        }


        #endregion

        #region UTILS

        /// <summary>
        /// Changes the name of the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newTableName">New name of the table.</param>
        /// <returns></returns>
        public QBuilder ChangeTableName<T>(String newTableName)
        {
            String currentTableName = typeof(T).Name;
            tableInfoContainer.ChangeName(currentTableName, newTableName);

            return this;
        }

        /// <summary>
        /// Sets the table alias.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public QBuilder SetTableAlias<T>(String alias)
        {
            tableInfoContainer.SetAlias(typeof(T).Name, alias);

            return this;
        }

        /// <summary>
        /// Registers the method calls. If exist, overrides
        /// </summary>
        /// <param name="methodCall">The method call.</param>
        public void RegisterMethodCalls(IMethodCall methodCall)
        {
            var exist = RegisteredMethodCalls.FirstOrDefault(p => p.Name == methodCall.Name);
            if (exist != null) //exist
            {
                RegisteredMethodCalls.Remove(exist);
            }
            RegisteredMethodCalls.Add(methodCall);
        }


        /// <summary>
        /// Updates given list the table info
        /// </summary>
        /// <param name="list">The list.</param>
        void UpdateTableInfo(IList<IContentItem> list)
        {
            list.ModifyEach(p =>
            {
                if (p is FieldContentItem)
                {
                    var item = p as FieldContentItem;
                    item.TableInfo = this.tableInfoContainer.Get(item.TableInfo.Name);
                }
                return p;
            });
        }


        /// <summary>
        /// Registers the expressions.
        /// </summary>
        void registerExpressions()
        {
            registeredExpressions.Clear();

            Assembly currentAssembly = this.GetType().Assembly;
            //collect all objects implementing IExpression and create new instance
            registeredExpressions.AddRange(currentAssembly.GetProvider<IExpressionHandler>());
        }
        /// <summary>
        /// Registers the method calls.
        /// </summary>
        void registerMethodCalls()
        {
            RegisteredMethodCalls.Clear();

            Assembly currentAssembly = this.GetType().Assembly;
            //collect all objects implementing IExpression and create new instance
            RegisteredMethodCalls.AddRange(currentAssembly.GetProvider<IMethodCall>());
        }

        #endregion
        
        #region JOIN

        //public ArchPmQueryBuilder OuterJoin<T1, T2>(Expression<Func<T1, T2, Object>> predicate, JoinDirections directon = JoinDirections.Left)
        //{
        //    return Join(predicate, directon, JoinTypes.Outer);
        //}


        /// <summary>
        /// Joins the specified predicate.
        /// </summary>
        /// <typeparam name="LeftTable">The type of the eft table.</typeparam>
        /// <typeparam name="RightTable">The type of the ight table.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="joinDirection">The join direction.</param>
        /// <param name="joinType">Type of the join.</param>
        /// <returns></returns>
        public QBuilder Join<LeftTable, RightTable>(Expression<Func<LeftTable, RightTable, Object>> predicate, JoinDirections joinDirection = JoinDirections.Left, JoinTypes joinType = JoinTypes.Inner)
        {
            JoinContent joinContent = new JoinContent();

            this.decisionMaker.ExpressionHandle(joinContent.Items, predicate);

            //update items table Info
            this.UpdateTableInfo(joinContent.Items);

            joinContent.LeftTableInfo = tableInfoContainer.Get(typeof(LeftTable).Name);
            joinContent.RightTableInfo = tableInfoContainer.Get(typeof(RightTable).Name);
            joinContent.JoinType = joinType;
            joinContent.JoinDirection = joinDirection;

            this.JoinContents.Add(joinContent);
            return this;
        }


        #endregion
        
        #region WHERE

        /// <summary>
        /// Where Operator can be used between Where Methods to be able to define operator between wheres
        /// </summary>
        /// <param name="whereOperator">The where operator.</param>
        /// <returns></returns>
        public QBuilder InsertOperatorBetweenWhereStatements(Operators whereOperator)
        {
            this.WhereContent.Items.Add(new OperatorContentItem() { Value = whereOperator.GetDescription() });

            return this;
        }
        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Where<T1>(Expression<Func<T1, Boolean>> predicate)
        {
            return whereAction(predicate);
        }
        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Where<T1, T2>(Expression<Func<T1, T2, Boolean>> predicate)
        {
            return whereAction(predicate);
        }
        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Where<T1, T2, T3>(Expression<Func<T1, T2, T3, Boolean>> predicate)
        {
            return whereAction(predicate);
        }
        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Where<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, Boolean>> predicate)
        {
            return whereAction(predicate);
        }
        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public QBuilder Where<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, Boolean>> predicate)
        {
            return whereAction(predicate);
        }
        /// <summary>
        /// Wheres the specified expression.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns></returns>
        QBuilder whereAction(Expression predicate)
        {
            if (predicate == null)
                return this;

            this.decisionMaker.ExpressionHandle(this.WhereContent.Items, predicate);

            //handling remove first and last parantesess
            if (this.WhereContent.Items.Count >= 2)
            {
                var first = this.WhereContent.Items.First();
                var last = this.WhereContent.Items.Last();
                if (first is BlockContentItem && last is BlockContentItem)
                {
                    this.WhereContent.Items.Remove(first);
                    this.WhereContent.Items.Remove(last);
                }
            }

            //handling and correcting table info
            UpdateTableInfo(this.WhereContent.Items);

            //handling Boolean fields - {p => p.IsFriendly}
            //get only field and boolean fields
            var onlyBooleanFieldItems = this.WhereContent.Items.Where(p => p is FieldContentItem && p.Value != null && ((FieldContentItem)p).Type == typeof(Boolean)).ToList();
            foreach (var booleanFieldItem in onlyBooleanFieldItems)
            {
                //next Item
                OperatorContentItem nextItem = this.WhereContent.Items.NextItemIfExist(booleanFieldItem) as OperatorContentItem;
                if (nextItem == null || (nextItem != null && !nextItem.ValueStr.Contains("=")))
                {
                    //previous Item
                    NotContentItem notItem = this.WhereContent.Items.PreviousItemIfExist(booleanFieldItem) as NotContentItem;

                    Int32 currentIndex = this.WhereContent.Items.IndexOf(booleanFieldItem);
                    this.WhereContent.Items.Insert(currentIndex + 1, new OperatorContentItem() { Value = (notItem != null ? "!" : "") + "=" });
                    this.WhereContent.Items.Insert(currentIndex + 2, new ValueContentItem() { Value = true });

                    //needs to be stay till items insertion completed
                    if (notItem != null)
                    {
                        this.WhereContent.Items.Remove(notItem);
                    }

                }
            }

            //handling replace equal(=) to IS when value NULL
            var nullValues = this.WhereContent.Items.Where(p => p is ValueContentItem && p.Value == null).ToList();
            foreach (var nullValue in nullValues)
            {
                //next Item
                OperatorContentItem previousItem = this.WhereContent.Items.PreviousItemIfExist(nullValue) as OperatorContentItem;
                if (previousItem != null && previousItem.ValueStr.Contains(Operators.EQUALS.GetDescription()))
                {
                    previousItem.Value = previousItem.ValueStr == Operators.NOTEQUALS.GetDescription() ? Operators.ISNOT.GetDescription() : Operators.IS.GetDescription();
                }
            }

            return this;
        }

        #endregion
        
        #region UPDATE



        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public QBuilder Update<T>(T entity)
        {
            this.QueryType = QueryTypes.Update;

            this.UpdateContent.Fields = entity.Properties().ToList();
            this.UpdateContent.TableInfo = tableInfoContainer.Get(typeof(T).Name);

            return this;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeOrExclude">The include or exclude.</param>
        /// <returns></returns>
        public QBuilder Update<T>(T entity, Expression<Func<T, Object>> predicate, IncludeExclude includeOrExclude = IncludeExclude.Include)
        {
            var that = Update(entity);

            var result = includeExclude(that.UpdateContent.Fields, predicate, includeOrExclude);

            return result;
        }

        /// <summary>
        /// Includes the exclude.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields">The fields.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeOrExclude">The include or exclude.</param>
        /// <returns></returns>
        internal QBuilder includeExclude<T>(List<PropertyDTO> fields, Expression<Func<T, Object>> predicate, IncludeExclude includeOrExclude)
        {
            var list = new List<IContentItem>();

            this.decisionMaker.ExpressionHandle(list, predicate);

            if (list.HasRecords())
            {
                switch (includeOrExclude)
                {
                    case IncludeExclude.Include:
                        //remove excluded items from properties
                        fields.RemoveAll(p => !list.Select(x => x.Value).Contains(p.Name));
                        break;
                    case IncludeExclude.Exclude:
                        //remove excluded items from properties
                        fields.RemoveAll(p => list.Select(x => x.Value).Contains(p.Name));
                        break;
                }
            }

            return this;
        }

        #endregion
        
        #region INSERT

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">include or exclude specified fields</param>
        /// <returns></returns>
        public QBuilder Insert<T>(T entity)
        {
            this.QueryType = QueryTypes.Insert;

            this.InsertContent.Fields = entity.Properties().ToList();
            this.InsertContent.TableInfo = tableInfoContainer.Get(typeof(T).Name);

            return this;
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">include or exclude specified fields</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeOrExclude">The include or exclude.</param>
        /// <returns></returns>
        public QBuilder Insert<T>(T entity, Expression<Func<T, Object>> predicate, IncludeExclude includeOrExclude = IncludeExclude.Include)
        {
            var that = Insert(entity);

            var result = includeExclude(that.InsertContent.Fields, predicate, includeOrExclude);

            return result;
        }

        /// <summary>
        /// Returns the scope identity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exclude">The predicate. Exclude selector</param>
        /// <returns></returns>
        public QBuilder ReturnScopeIdentity<T>(Expression<Func<T, Object>> exclude)
        {
            SelectContent content = new SelectContent();
            this.decisionMaker.ExpressionHandle(content.Items, exclude);

            var names = content.Items.Where(p => p is FieldContentItem).Cast<FieldContentItem>().Select(p => p.ValueStr);

            //remove given names from insert content
            this.InsertContent.Fields.RemoveAll(p => names.Contains(p.Name));
            this.InsertContent.ReturningScopeIdentity = true;

            return this;
        }

        #endregion
        
        #region PAGING

        /// <summary>
        /// Pagings the specified page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// <param name="orderByPredicate">The order by predicate.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public QBuilder Paging<T>(Int32 page, Int32 perPage, Expression<Func<T, Object>> orderByPredicate, OrderByDirections direction = OrderByDirections.Asc) where T : new()
        {
            this.PagingContent.TableInfo = tableInfoContainer.SetAlias(typeof(T).Name, "");
            this.decisionMaker.ExpressionHandle(this.PagingContent.Items, orderByPredicate);
            UpdateTableInfo(this.PagingContent.Items);

            //add into list
            this.PagingContent.Page = page;
            this.PagingContent.PerPage = perPage;
            this.PagingContent.Direction = direction;

            return this;
        }

        #endregion

        #region ORDER BY


        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <typeparam name="T">The type of the 1.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public QBuilder OrderBy<T>(Expression<Func<T, Object>> predicate, OrderByDirections direction = OrderByDirections.Asc) where T : new()
        {
            OrderByContent orderByContent = new OrderByContent();

            orderByContent.TableInfo = tableInfoContainer.SetAlias(typeof(T).Name, "");
            this.decisionMaker.ExpressionHandle(orderByContent.Items, predicate);
            UpdateTableInfo(orderByContent.Items);
            orderByContent.Direction = direction;

            //add into list
            this.OrderByContents.Add(orderByContent);

            return this;
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Creates the specified table name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public QBuilder Create<T>() where T : new()
        {
            this.QueryType = QueryTypes.Create;
            T entity = new T();

            this.CreateContent.Fields = entity.Properties().ToList();
            this.CreateContent.TableInfo = tableInfoContainer.Get(typeof(T).Name);

            return this;
        }

        #endregion


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <exception cref="QueryBuilderException">You must provide a querygenerator such as SqlQueryGenerator, OracleQueryGenerator</exception>
        public override string ToString()
        {
            if (this.QueryGenerator == null)
                throw new QueryBuilderException("You must provide a querygenerator such as SqlQueryGenerator, OracleQueryGenerator");

            return QueryGenerator.Execute(this);
        }


    }
}

