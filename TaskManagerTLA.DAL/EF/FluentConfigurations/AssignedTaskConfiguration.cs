using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.EF.FluentConfigurations
{
    public class AssignedTaskConfiguration : IEntityTypeConfiguration<AssignedTComments>
    {
        public void Configure(EntityTypeBuilder<AssignedTComments> builder)
        {
            builder
                .HasOne(u => u.AssignedTask)
                .WithMany(s => s.Comments);
        }
    }
}
