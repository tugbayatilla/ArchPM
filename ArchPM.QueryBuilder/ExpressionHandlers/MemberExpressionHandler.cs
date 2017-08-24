using System;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using ArchPM.Core.Extensions;
using ArchPM.QueryBuilder.ContentItems;
using ArchPM.Core.Extensions;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class MemberExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(MemberExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            MemberExpression memberExp = expression as MemberExpression;
            if (memberExp == null)
                return;

            if (memberExp.NodeType == ExpressionType.MemberAccess)
            {
                if (memberExp.Member.Name == "HasValue")
                {

                    //get not expression and remove it
                    NotContentItem notItem = sb.FindLast(p => p is NotContentItem) as NotContentItem;
                    sb.Remove(notItem);

                    //fieldcontent comes here
                    this.DecisionMaker.ExpressionHandle(sb, memberExp.Expression);

                    var operatorItem = new OperatorContentItem();
                    operatorItem.Value = "IS" + (notItem == null ? " NOT" : "");
                    sb.Add(operatorItem);

                    var valueItem = new ValueContentItem();
                    valueItem.Value = null;
                    sb.Add(valueItem);

                }
                else if (memberExp.Type.IsList()) //List.Contains values
                {
                    var list = Expression.Lambda(memberExp).Compile().DynamicInvoke() as IEnumerable;
                    foreach (var item in list)
                    {
                        var valueItem = new ValueContentItem();
                        valueItem.Value = item;
                        sb.Add(valueItem);
                    }
                }
                else
                {
                    // {p => p.Id == id} (id is integer parameter)
                    if (memberExp.Expression != null
                        && (memberExp.Expression.NodeType == ExpressionType.Constant // { p => p.Id == id} (id is integer parameter)
                            || memberExp.Expression.NodeType == ExpressionType.MemberAccess)) // { p => p.Id == interfaceClass.Id } 
                    {
                        Object value = Expression.Lambda(memberExp).Compile().DynamicInvoke();
                        if (value == null || (value != null && value.GetType().IsDotNetPirimitive()))
                        {
                            sb.Add(new ValueContentItem() { Value = value });
                        }
                        else if (value is MemberExpression)
                        {
                            MemberExpression propertyExpression = value as MemberExpression;
                            var field = new FieldContentItem();
                            field.Value = propertyExpression.Member.Name;
                            field.Type = propertyExpression.Type;
                            field.TableInfo.Name = propertyExpression.Expression.Type.Name;
                            sb.Add(field);
                        }
                    }
                    else
                    {
                        var field = new FieldContentItem();
                        field.Value = memberExp.Member.Name;
                        field.Type = memberExp.Type;
                        field.TableInfo.Name = memberExp.Expression.Type.Name;
                        sb.Add(field);
                    }
                }
            }
        }
    }
}
