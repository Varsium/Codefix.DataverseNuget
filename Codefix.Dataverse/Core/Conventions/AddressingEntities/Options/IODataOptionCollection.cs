﻿using Codefix.Dataverse.Core.Conventions.Functions;
using Codefix.Dataverse.Core.Conventions.Operators;
using System;
using System.Linq.Expressions;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Options
{
    public interface IODataOptionCollection<TODataOption, TEntity> : IODataOption<TODataOption, TEntity>
    {
        TODataOption Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false);

        TODataOption Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false);

        TODataOption Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false);

        TODataOption OrderBy(Expression<Func<TEntity, object>> orderBy);

        TODataOption OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy);

        TODataOption OrderByDescending(Expression<Func<TEntity, object>> orderByDescending);

        TODataOption Top(int number);

        TODataOption Skip(int number);

        TODataOption Count(bool value = true);
    }
}
