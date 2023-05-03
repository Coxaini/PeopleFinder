using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace PeopleFinder.Infrastructure.Persistence.Conventions;

public class DateConvention : IModelFinalizingConvention
{
    
    
    /*public DateConvention()
    {
        this.Properties<DateTime>()
            .Configure(c => c.HasColumnType("datetime2").HasPrecision(3));

        this.Properties<DateTime>()
            .Where(x => x.GetCustomAttributes(false).OfType<DataTypeAttribute>()
                .Any(a => a.DataType == DataType.Date))
            .Configure(c => c.HasColumnType("date"));
    }*/
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var property in modelBuilder.Metadata.GetEntityTypes()
                     .SelectMany(
                         entityType => entityType.GetDeclaredProperties()
                             .Where(property => property.ClrType == typeof(DateTime)|| property.ClrType == typeof(DateTime?))))
        {
            property.Builder.HasPrecision(2);
        }
        
        
        
    }
}