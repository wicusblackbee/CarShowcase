# 🚗 Car Showcase - Blazor Application

A modern, fully-tested Blazor Server application for showcasing premium vehicles with comprehensive test coverage and automated deployment.

[![Build and Test](https://github.com/yourusername/car-showcase/actions/workflows/deploy.yml/badge.svg)](https://github.com/yourusername/car-showcase/actions/workflows/deploy.yml)
[![Coverage](https://img.shields.io/badge/coverage-75%25-brightgreen)](https://yourusername.github.io/car-showcase/coverage/)
[![Tests](https://img.shields.io/badge/tests-71%20passing-brightgreen)](#testing)

## 🌟 Features

### 🚗 Car Management
- **Complete CRUD Operations**: Add, view, update, and delete cars
- **Advanced Search & Filtering**: Filter by make, model, year, and price
- **Responsive Design**: Works seamlessly on desktop and mobile devices
- **Rich Car Details**: Comprehensive car information with specifications

### 🎨 User Interface
- **Modern Bootstrap Design**: Clean, professional interface
- **Interactive Components**: Smooth user interactions and transitions
- **Hero Section**: Eye-catching landing page with featured cars
- **Card-based Layout**: Organized display of car information

### 🧪 Quality Assurance
- **75%+ Code Coverage**: Comprehensive test suite ensuring code quality
- **71 Passing Tests**: Thorough testing of all components and services
- **Automated Testing**: Continuous integration with GitHub Actions
- **Multiple Test Types**: Unit tests, component tests, and integration tests

## 🏗️ Architecture

### Technology Stack
- **Frontend**: Blazor Server (.NET 8)
- **Styling**: Bootstrap 5 with custom CSS
- **Testing**: xUnit, bUnit, Moq, Coverlet
- **CI/CD**: GitHub Actions
- **Deployment**: GitHub Pages

### Project Structure
```
CarShowcase/
├── Models/
│   └── Car.cs                 # Car data model
├── Services/
│   ├── ICarService.cs         # Service interface
│   └── CarService.cs          # Car business logic
├── Pages/
│   ├── Index.razor            # Home page with featured cars
│   ├── Cars.razor             # Car listing with filters
│   └── CarDetails.razor       # Individual car details
├── Shared/
│   ├── MainLayout.razor       # Application layout
│   └── NavMenu.razor          # Navigation component
└── Program.cs                 # Application configuration

CarShowcase.Tests/
├── Models/
│   └── CarTests.cs            # Car model tests
├── Services/
│   └── CarServiceTests.cs     # Service layer tests
└── Components/
    ├── IndexPageTests.cs      # Home page tests
    ├── CarsPageTests.cs       # Car listing tests
    ├── CarDetailsPageTests.cs # Car details tests
    ├── NavMenuTests.cs        # Navigation tests
    ├── CounterPageTests.cs    # Counter component tests
    └── SurveyPromptTests.cs   # Survey prompt tests
```

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/car-showcase.git
   cd car-showcase
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the application**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project CarShowcase
   ```

5. **Open your browser**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate coverage report
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html
```

### Test Coverage
The project maintains **75%+ code coverage** across:

- **Model Tests**: Car entity validation and property testing
- **Service Tests**: Business logic and CRUD operations
- **Component Tests**: Blazor component rendering and interaction
- **Integration Tests**: End-to-end functionality testing

### Test Categories
- ✅ **71 Total Tests**
- ✅ **Model Tests**: 8 tests
- ✅ **Service Tests**: 25 tests  
- ✅ **Component Tests**: 38 tests

## 🚀 Deployment

### GitHub Actions CI/CD
The project includes automated deployment to GitHub Pages:

1. **Continuous Integration**
   - Builds the application
   - Runs all tests
   - Generates coverage reports
   - Validates 70%+ coverage threshold

2. **Continuous Deployment**
   - Deploys to GitHub Pages on main branch
   - Includes coverage reports
   - Creates project showcase page

### Manual Deployment
```bash
# Build for production
dotnet publish CarShowcase -c Release -o publish

# Deploy to your hosting provider
# (Copy contents of publish/ directory)
```

## 📊 Code Quality

### Coverage Metrics
- **Line Coverage**: 75.37%
- **Branch Coverage**: 67.18%
- **Total Lines**: 337
- **Covered Lines**: 254

### Quality Gates
- ✅ Minimum 70% code coverage
- ✅ All tests must pass
- ✅ No build warnings in Release mode
- ✅ Automated quality checks

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Maintain test coverage above 70%
- Write tests for new features
- Follow existing code style
- Update documentation as needed

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with [Blazor](https://blazor.net/) and [.NET 8](https://dotnet.microsoft.com/)
- UI components from [Bootstrap](https://getbootstrap.com/)
- Testing with [xUnit](https://xunit.net/), [bUnit](https://bunit.dev/), and [Moq](https://github.com/moq/moq4)
- Icons from [Open Iconic](https://useiconic.com/open)

---

**Live Demo**: [View on GitHub Pages](https://yourusername.github.io/car-showcase/)  
**Coverage Report**: [View Test Coverage](https://yourusername.github.io/car-showcase/coverage/)
