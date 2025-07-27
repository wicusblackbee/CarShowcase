# Car Showcase - Blazor Application

This is a demonstration of a modern Blazor Server application with comprehensive testing.

## Features
- Car inventory management with advanced search and filtering
- Responsive design with multiple grid layouts
- Professional UI components (CarCard, SearchFilters, CarGrid, PageHeader, etc.)
- 75%+ test coverage with 161 passing tests
- Modern architecture with clean separation of concerns

## Technology Stack
- Blazor Server (.NET 8)
- xUnit + bUnit + Moq for testing
- Bootstrap 5 for responsive design
- GitHub Actions for CI/CD
- Coverlet for code coverage analysis

## Test Coverage
The application maintains high code quality with comprehensive test coverage.
View the detailed coverage report in the coverage/ directory.

## Components
- **CarCard**: Reusable car display component with multiple display options
- **SearchFilters**: Advanced search interface with multiple filter types
- **CarGrid**: Flexible grid layout with sorting and pagination
- **PageHeader**: Professional page headers with breadcrumbs and actions
- **HeroSection**: Customizable hero sections with call-to-action buttons
- **LoadingSpinner**: Configurable loading indicators

## Architecture
- Interface-based design with ICarService
- Dependency injection throughout
- Component isolation with CSS scoping
- Event-driven architecture with proper callback handling
- Type-safe configuration with enums and strongly-typed parameters
