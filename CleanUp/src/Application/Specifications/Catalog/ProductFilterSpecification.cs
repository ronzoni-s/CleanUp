using CleanUp.Application.Specifications.Base;
using CleanUp.Domain.Entities.Catalog;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CleanUp.Application.Specifications.Catalog
{
    public class ProductFilterSpecification : HeroSpecification<Product>
    {
        public ProductFilterSpecification(string searchString, bool hideNotActive = false)
        {
            //Criteria = p => true;

            //if (orderId.HasValue)
            //{
            //    Includes.Add(a => a.OrderProducts);
            //    Criteria = p => p.OrderProducts.Any(op => op.OrderId == orderId);
            //}


            if (!string.IsNullOrEmpty(searchString))
            {
                //Expression<Func<Product, bool>> seachStringExpression = (p => p.Code != null && (p.Name.Contains(searchString) || p.Code.Contains(searchString)));
                //if (orderId.HasValue)
                //{
                //    Criteria = Expression.Lambda<Func<Product, bool>>(Expression.And(Criteria, seachStringExpression));
                //}
                //else
                //{
                //    Criteria = seachStringExpression;
                //}
                Criteria = (p => p.Code != null && (p.Name.Contains(searchString) || p.Code.Contains(searchString))
                            && (!hideNotActive || p.IsActive)
                            );
            }
            else if (hideNotActive)
            {
                Criteria = p => p.IsActive;
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}