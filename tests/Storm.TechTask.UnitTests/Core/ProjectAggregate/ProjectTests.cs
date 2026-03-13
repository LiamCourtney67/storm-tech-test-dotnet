using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Core.ProjectAggregate.Services;
using Storm.TechTask.UnitTests.Utilities.Comparison;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate
{
    public class ProjectTests : BaseFixture
    {
        public ProjectTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ChangesStateWhenUpdated()
        {
            // Arrange
            var actual = NewProject().Set(p => p.Name, "original-name").Build();
            var expected = NewProject().BuildFrom(actual).Set(p => p.Name, "new-name").Set(p => p.Category, ProjectCategory.Consultancy).Set(p => p.InternalOnly, false).Build();
            var uniquenessChecker = new Mock<IProjectUniquenessChecker>(MockBehavior.Strict);
            uniquenessChecker.Setup(uc => uc.VerifyNameUnique(actual, expected.Name, default)).Returns(Task.CompletedTask);

            // Act
            await actual.Update(expected.Name, expected.Category, expected.InternalOnly, uniquenessChecker.Object, default);

            // Assert
            actual.ShouldHaveSameStateAs(expected);
        }

        [Fact]
        public void ChangesStateWhenPaused()
        {
            // Arrange
            var actual = NewProject().Set(p => p.Status, ProjectStatus.Open).Build();
            var expected = NewProject().BuildFrom(actual).Set(p => p.Status, ProjectStatus.Paused).Build();

            // Act
            actual.Pause();

            // Assert
            actual.ShouldHaveSameStateAs(expected);
        }

        [Fact]
        public void ChangesStateWhenResumed()
        {
            // Arrange
            var actual = NewProject().Set(p => p.Status, ProjectStatus.Paused).Build();
            var expected = NewProject().BuildFrom(actual).Set(p => p.Status, ProjectStatus.Open).Build();

            // Act
            actual.Resume();

            // Assert
            actual.ShouldHaveSameStateAs(expected);
        }

        // Task 3 - Fix a bug 
        // Bug found: ProjectStatus is set to Paused instead of Closed when Close() is called.
        [Fact]
        public void ChangesStateWhenClosed()
        {
            // Arrange
            var actual = NewProject().Set(p => p.Status, ProjectStatus.Open).Build();
            var expected = NewProject().BuildFrom(actual).Set(p => p.Status, ProjectStatus.Closed).Build();

            // Act
            actual.Close();

            // Assert
            actual.ShouldHaveSameStateAs(expected);
        }
    }
}
