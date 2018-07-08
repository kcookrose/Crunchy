using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Crunchy.Models {

    // TODO: Remove join table when efcore supports many-to-many
    public class ProjectOwner {

        public long UserId { get; set; }
        public User User { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public bool SameAs(ProjectOwner other) {
            return UserId == other.UserId && ProjectId == other.ProjectId;
        }


        public static void OnModelCreating(TodoContext context, ModelBuilder builder) {
            var entity = builder.Entity<ProjectOwner>();
            entity.HasKey(projOwner => new {projOwner.UserId, projOwner.ProjectId});

            entity.HasOne(projOwner => projOwner.User)
                .WithMany(user => user.OwnedProjects)
                .HasForeignKey(projOwner => projOwner.UserId);
            
            entity.HasOne(projOwner => projOwner.Project)
                .WithMany(project => project.OwnerUsers)
                .HasForeignKey(projOwner => projOwner.ProjectId);
        }


        public static void Join(TodoContext context, Project project, User user) {
            var joined = new ProjectOwner { Project = project, User = user };
            context.ChangeTracker.TrackGraph(joined, (node => node.Entry.State = node.Entry.IsKeySet ? EntityState.Unchanged : EntityState.Added));
        }


        public bool Unjoin(TodoContext context) {
            context.EnsureDeepLoaded(context.Entry(this));
            context.EnsureDeepLoaded(context.Entry(Project));
            context.EnsureDeepLoaded(context.Entry(User));
            return Unjoin(context, Project, User);
        }


        public static bool Unjoin(TodoContext context, Project project, User user) {
            var joined = context.Find<ProjectOwner>(user.Uid, project.Pid);
            System.Console.WriteLine("Found joined: " + joined != null);
            if (joined == null) return false;
            ProjectOwner left = project.OwnerUsers.First(projOwner => projOwner.SameAs(joined));
            ProjectOwner right = user.OwnedProjects.First(projOwner => projOwner.SameAs(joined));
            if (left == null || right == null) return false;
            context.Remove(joined);
            project.OwnerUsers.Remove(left);
            user.OwnedProjects.Remove(right);
            return true;
        }
    }
}